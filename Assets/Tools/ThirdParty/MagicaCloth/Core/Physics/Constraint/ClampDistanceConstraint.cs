﻿// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace MagicaCloth
{
    /// <summary>
    /// ルート原点からの最大最小距離拘束
    /// </summary>
    public class ClampDistanceConstraint : PhysicsManagerConstraint
    {
        /// <summary>
        /// 最大最小距離拘束データ
        /// todo:共有化可能
        /// </summary>
        [System.Serializable]
        public struct ClampDistanceData
        {
            /// <summary>
            /// 計算頂点インデックス
            /// </summary>
            public ushort vertexIndex;

            /// <summary>
            /// ターゲット頂点インデックス
            /// </summary>
            public ushort targetVertexIndex;

            /// <summary>
            /// パーティクル距離(v1.7.0)
            /// </summary>
            public float length;

            /// <summary>
            /// データが有効か判定する
            /// </summary>
            /// <returns></returns>
            public bool IsValid()
            {
                return vertexIndex > 0 || targetVertexIndex > 0;
            }
        }
        FixedChunkNativeArray<ClampDistanceData> dataList;

        /// <summary>
        /// 頂点インデックスごとの書き込みバッファ参照
        /// </summary>
        FixedChunkNativeArray<ReferenceDataIndex> refDataList;

        /// <summary>
        /// グループごとの拘束データ
        /// </summary>
        public struct GroupData
        {
            public int teamId;
            public int active;

            /// <summary>
            /// 最小距離割合
            /// </summary>
            public float minRatio;

            /// <summary>
            /// 最大距離割合
            /// </summary>
            public float maxRatio;

            /// <summary>
            /// 速度影響
            /// </summary>
            public float velocityInfluence;

            public ChunkData dataChunk;
            public ChunkData refChunk;
        }
        public FixedNativeList<GroupData> groupList;

        //=========================================================================================
        public override void Create()
        {
            dataList = new FixedChunkNativeArray<ClampDistanceData>();
            refDataList = new FixedChunkNativeArray<ReferenceDataIndex>();
            groupList = new FixedNativeList<GroupData>();
        }

        public override void Release()
        {
            dataList.Dispose();
            refDataList.Dispose();
            groupList.Dispose();
        }

        //=========================================================================================
        public int AddGroup(int teamId, bool active, float minRatio, float maxRatio, float velocityInfluence, ClampDistanceData[] dataArray, ReferenceDataIndex[] refDataArray)
        {
            if (dataArray == null || dataArray.Length == 0 || refDataArray == null || refDataArray.Length == 0)
                return -1;

            var teamData = MagicaPhysicsManager.Instance.Team.teamDataList[teamId];

            var gdata = new GroupData();
            gdata.teamId = teamId;
            gdata.active = active ? 1 : 0;
            gdata.minRatio = minRatio;
            gdata.maxRatio = maxRatio;
            gdata.velocityInfluence = velocityInfluence;
            gdata.dataChunk = dataList.AddChunk(dataArray.Length);
            gdata.refChunk = refDataList.AddChunk(refDataArray.Length);

            // チャンクデータコピー
            dataList.ToJobArray().CopyFromFast(gdata.dataChunk.startIndex, dataArray);
            refDataList.ToJobArray().CopyFromFast(gdata.refChunk.startIndex, refDataArray);

            int group = groupList.Add(gdata);
            return group;
        }

        public override void RemoveTeam(int teamId)
        {
            var teamData = MagicaPhysicsManager.Instance.Team.teamDataList[teamId];
            int group = teamData.clampDistanceGroupIndex;
            if (group < 0)
                return;

            var cdata = groupList[group];

            // チャンクデータ削除
            dataList.RemoveChunk(cdata.dataChunk);
            refDataList.RemoveChunk(cdata.refChunk);

            // データ削除
            groupList.Remove(group);
        }

        public void ChangeParam(int teamId, bool active, float minRatio, float maxRatio, float velocityInfluence)
        {
            var teamData = MagicaPhysicsManager.Instance.Team.teamDataList[teamId];
            int group = teamData.clampDistanceGroupIndex;
            if (group < 0)
                return;

            var gdata = groupList[group];
            gdata.active = active ? 1 : 0;
            gdata.minRatio = minRatio;
            gdata.maxRatio = maxRatio;
            gdata.velocityInfluence = velocityInfluence;
            groupList[group] = gdata;
        }

        //=========================================================================================
        /// <summary>
        /// 拘束の解決
        /// </summary>
        /// <param name="dtime"></param>
        /// <param name="jobHandle"></param>
        /// <returns></returns>
        public override JobHandle SolverConstraint(float dtime, float updatePower, int iteration, JobHandle jobHandle)
        {
            if (groupList.Count == 0)
                return jobHandle;

            // 最大最小距離拘束（パーティクルごとに実行する）
            var job1 = new ClampDistanceJob()
            {
                clampDistanceList = dataList.ToJobArray(),
                groupList = groupList.ToJobArray(),
                refDataList = refDataList.ToJobArray(),

                teamDataList = Manager.Team.teamDataList.ToJobArray(),
                teamIdList = Manager.Particle.teamIdList.ToJobArray(),

                flagList = Manager.Particle.flagList.ToJobArray(),
                basePosList = Manager.Particle.basePosList.ToJobArray(),
                nextPosList = Manager.Particle.InNextPosList.ToJobArray(),
                outNextPosList = Manager.Particle.OutNextPosList.ToJobArray(),

                posList = Manager.Particle.posList.ToJobArray(),
                frictionList = Manager.Particle.frictionList.ToJobArray(),
            };
            jobHandle = job1.Schedule(Manager.Particle.Length, 64, jobHandle);
            Manager.Particle.SwitchingNextPosList();

            return jobHandle;
        }

        /// <summary>
        /// 最大最小距離拘束ジョブ
        /// パーティクルごとに計算
        /// </summary>
        [BurstCompile]
        struct ClampDistanceJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<ClampDistanceData> clampDistanceList;
            [ReadOnly]
            public NativeArray<GroupData> groupList;
            [ReadOnly]
            public NativeArray<ReferenceDataIndex> refDataList;

            // チーム
            [ReadOnly]
            public NativeArray<PhysicsManagerTeamData.TeamData> teamDataList;
            [ReadOnly]
            public NativeArray<int> teamIdList;

            [ReadOnly]
            public NativeArray<PhysicsManagerParticleData.ParticleFlag> flagList;
            [ReadOnly]
            public NativeArray<float3> basePosList;
            [ReadOnly]
            public NativeArray<float3> nextPosList;
            [WriteOnly]
            public NativeArray<float3> outNextPosList;
            public NativeArray<float3> posList;
            [ReadOnly]
            public NativeArray<float> frictionList;

            // パーティクルごと
            public void Execute(int index)
            {
                // 初期化コピー
                var nextpos = nextPosList[index];
                outNextPosList[index] = nextpos;

                // 頂点フラグ
                var flag = flagList[index];
                if (flag.IsValid() == false || flag.IsFixed())
                    return;

                // チーム
                var team = teamDataList[teamIdList[index]];
                if (team.IsActive() == false || team.clampDistanceGroupIndex < 0)
                    return;

                // 更新確認
                if (team.IsUpdate() == false)
                    return;

                int pstart = team.particleChunk.startIndex;
                int vindex = index - pstart;

                // クロスごとの拘束データ
                var gdata = groupList[team.clampDistanceGroupIndex];
                if (gdata.active == 0)
                    return;

                // 参照データ情報
                var refdata = refDataList[gdata.refChunk.startIndex + vindex];
                if (refdata.count > 0)
                {
                    int dataIndex = gdata.dataChunk.startIndex + refdata.startIndex;
                    ClampDistanceData data = clampDistanceList[dataIndex];
                    if (data.IsValid() == false)
                        return;

                    // ターゲット
                    int pindex2 = pstart + data.targetVertexIndex;
                    float3 nextpos2 = nextPosList[pindex2];

                    // 現在のベクトル
                    float3 v = nextpos - nextpos2;

                    // 復元長さ
                    float length = data.length; // v1.7.0
                    //if (length == 0.0f)
                    //{
                    //    // 従来データ（basePosの長さから)
                    //    length = math.distance(basePosList[index], basePosList[pindex2]);
                    //}

                    // ベクトル長クランプ
                    v = MathUtility.ClampVector(v, length * gdata.minRatio, length * gdata.maxRatio);

                    // 位置
                    var opos = nextpos;
                    nextpos = nextpos2 + v;

                    // 摩擦係数から移動率を算出
                    float friction = frictionList[index];
                    float moveratio = math.saturate(1.0f - friction * Define.Compute.FrictionMoveRatio);
                    nextpos = math.lerp(opos, nextpos, moveratio);

                    // 書き出し
                    outNextPosList[index] = nextpos;

                    // 速度影響
                    var av = (nextpos - opos) * (1.0f - gdata.velocityInfluence);
                    posList[index] = posList[index] + av;
                }
            }
        }
    }
}

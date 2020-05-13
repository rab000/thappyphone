// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace MagicaCloth
{
    /// <summary>
    /// 浸透制限拘束
    /// </summary>
    public class PenetrationConstraint : PhysicsManagerConstraint
    {
        /// <summary>
        /// 浸透制限データ
        /// todo:共有可能
        /// </summary>
        [System.Serializable]
        public struct PenetrationData
        {
            /// <summary>
            /// 計算頂点インデックス
            /// </summary>
            public short vertexIndex;

            /// <summary>
            /// コライダー配列インデックス
            /// </summary>
            public short colliderIndex;

            /// <summary>
            /// コライダーローカル座標（中心軸）
            /// </summary>
            public float3 localPos;

            /// <summary>
            /// 押し出しローカル方向（単位ベクトル）
            /// </summary>
            public float3 localDir;

            /// <summary>
            /// パーティクルへの距離（オリジナル位置）
            /// </summary>
            public float distance;

            public bool IsValid()
            {
                return vertexIndex >= 0;
            }
        }
        FixedChunkNativeArray<PenetrationData> dataList;

        /// <summary>
        /// ローカルパーティクルインデックスごとのデータ参照情報
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
            /// (0=Surface, 1=Collider)
            /// </summary>
            public int mode;

            public ChunkData dataChunk;

            public ChunkData refDataChunk;

            public CurveParam radius;

            public CurveParam distance;
        }
        public FixedNativeList<GroupData> groupList;

        //=========================================================================================
        public override void Create()
        {
            groupList = new FixedNativeList<GroupData>();
            dataList = new FixedChunkNativeArray<PenetrationData>();
            refDataList = new FixedChunkNativeArray<ReferenceDataIndex>();
        }

        public override void Release()
        {
            groupList.Dispose();
            dataList.Dispose();
            refDataList.Dispose();
        }

        public int AddGroup(int teamId, bool active, ClothParams.PenetrationMode mode, BezierParam distance, BezierParam radius, PenetrationData[] moveLimitDataList, ReferenceDataIndex[] refDataArray)
        {
            var teamData = MagicaPhysicsManager.Instance.Team.teamDataList[teamId];

            var gdata = new GroupData();
            gdata.teamId = teamId;
            gdata.active = active ? 1 : 0;
            gdata.mode = (int)mode;
            gdata.distance.Setup(distance);
            gdata.radius.Setup(radius);
            gdata.dataChunk = dataList.AddChunk(moveLimitDataList.Length);
            gdata.refDataChunk = refDataList.AddChunk(refDataArray.Length);

            // チャンクデータコピー
            dataList.ToJobArray().CopyFromFast(gdata.dataChunk.startIndex, moveLimitDataList);
            refDataList.ToJobArray().CopyFromFast(gdata.refDataChunk.startIndex, refDataArray);

            int group = groupList.Add(gdata);
            return group;
        }


        public override void RemoveTeam(int teamId)
        {
            var teamData = MagicaPhysicsManager.Instance.Team.teamDataList[teamId];
            int group = teamData.penetrationGroupIndex;
            if (group < 0)
                return;

            var gdata = groupList[group];

            // チャンクデータ削除
            dataList.RemoveChunk(gdata.dataChunk);
            refDataList.RemoveChunk(gdata.refDataChunk);

            // データ削除
            groupList.Remove(group);
        }

        public void ChangeParam(int teamId, bool active, BezierParam distance, BezierParam radius)
        {
            var teamData = Manager.Team.teamDataList[teamId];
            int group = teamData.penetrationGroupIndex;
            if (group < 0)
                return;

            var gdata = groupList[group];
            gdata.active = active ? 1 : 0;
            gdata.distance.Setup(distance);
            gdata.radius.Setup(radius);
            groupList[group] = gdata;
        }

        //=========================================================================================
        public override JobHandle SolverConstraint(float dtime, float updatePower, int iteration, JobHandle jobHandle)
        {
            if (groupList.Count == 0)
                return jobHandle;

            // 移動制限拘束
            var job1 = new PenetrationJob()
            {
                groupList = groupList.ToJobArray(),
                dataList = dataList.ToJobArray(),
                refDataList = refDataList.ToJobArray(),

                flagList = Manager.Particle.flagList.ToJobArray(),
                teamIdList = Manager.Particle.teamIdList.ToJobArray(),
                nextPosList = Manager.Particle.InNextPosList.ToJobArray(),
                nextRotList = Manager.Particle.InNextRotList.ToJobArray(),
                transformIndexList = Manager.Particle.transformIndexList.ToJobArray(),
                depthList = Manager.Particle.depthList.ToJobArray(),
                basePosList = Manager.Particle.basePosList.ToJobArray(),
                baseRotList = Manager.Particle.baseRotList.ToJobArray(),

                colliderList = Manager.Team.colliderList.ToJobArray(),

                boneSclList = Manager.Bone.boneSclList.ToJobArray(),

                teamDataList = Manager.Team.teamDataList.ToJobArray(),

                outNextPosList = Manager.Particle.OutNextPosList.ToJobArray(),
                posList = Manager.Particle.posList.ToJobArray(),
            };
            jobHandle = job1.Schedule(Manager.Particle.Length, 64, jobHandle);
            Manager.Particle.SwitchingNextPosList();

            return jobHandle;
        }

        //=========================================================================================
        /// <summary>
        /// 浸透制限拘束ジョブ
        /// パーティクルごとに計算
        /// </summary>
        [BurstCompile]
        struct PenetrationJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<GroupData> groupList;
            [ReadOnly]
            public NativeArray<PenetrationData> dataList;
            [ReadOnly]
            public NativeArray<ReferenceDataIndex> refDataList;

            [ReadOnly]
            public NativeArray<PhysicsManagerParticleData.ParticleFlag> flagList;
            [ReadOnly]
            public NativeArray<int> teamIdList;
            [ReadOnly]
            public NativeArray<float3> nextPosList;
            [ReadOnly]
            public NativeArray<quaternion> nextRotList;
            [ReadOnly]
            public NativeArray<int> transformIndexList;
            [ReadOnly]
            public NativeArray<float> depthList;
            [ReadOnly]
            public NativeArray<float3> basePosList;
            [ReadOnly]
            public NativeArray<quaternion> baseRotList;

            [ReadOnly]
            public NativeArray<int> colliderList;

            [ReadOnly]
            public NativeArray<float3> boneSclList;

            [ReadOnly]
            public NativeArray<PhysicsManagerTeamData.TeamData> teamDataList;

            [WriteOnly]
            public NativeArray<float3> outNextPosList;
            public NativeArray<float3> posList;

            // パーティクルごと
            public void Execute(int index)
            {
                // 初期化コピー
                float3 nextpos = nextPosList[index];
                outNextPosList[index] = nextpos;

                var flag = flagList[index];
                if (flag.IsValid() == false || flag.IsFixed() || flag.IsCollider())
                    return;

                // チーム
                var team = teamIdList[index];
                var teamData = teamDataList[team];
                if (teamData.IsActive() == false)
                    return;
                if (teamData.penetrationGroupIndex < 0)
                    return;
                // 更新確認
                if (teamData.IsUpdate() == false)
                    return;

                // グループデータ
                var gdata = groupList[teamData.penetrationGroupIndex];
                if (gdata.active == 0)
                    return;

                // データ参照情報
                int vindex = index - teamData.particleChunk.startIndex;
                var refdata = refDataList[gdata.refDataChunk.startIndex + vindex];
                if (refdata.count == 0)
                    return;

                // depth
                var depth = depthList[index];

                // radius
                var radius = gdata.radius.Evaluate(depth);

                // 浸透距離
                float distance = gdata.distance.Evaluate(depth);


                // モード別処理
                var oldpos = nextpos;
                if (gdata.mode == 0)
                {
                    // Surface Penetration
                    // ベース位置から算出する
                    var bpos = basePosList[index];
                    var brot = baseRotList[index];
                    int dindex = refdata.startIndex;
                    var data = dataList[gdata.dataChunk.startIndex + dindex];

                    if (data.IsValid())
                    {
                        float3 n = math.mul(brot, data.localDir);

                        // 球の位置
                        var c = bpos + n * (distance - radius);

                        // 球内部制限
                        var v = nextpos - c;
                        var len = math.length(v);
                        if (len > radius)
                        {
                            v *= (radius / len);
                            nextpos = c + v;
                        }
                    }
                }
                else if (gdata.mode == 1)
                {
                    // Collider Penetration
                    float3 addv = 0;
                    int addcnt = 0;

                    int dindex = refdata.startIndex;
                    for (int i = 0; i < refdata.count; i++, dindex++)
                    {
                        var data = dataList[gdata.dataChunk.startIndex + dindex];
                        if (data.IsValid())
                        {
                            int cindex = colliderList[teamData.colliderChunk.startIndex + data.colliderIndex];
                            float3 opos;

                            // 球内部制限
                            bool ret = InverseSpherePenetration(ref data, distance, cindex, radius, nextpos, out opos);

                            // プレーン制限（今のところあまり有効性なし）
                            //bool ret = PlanePenetration(ref data, distance, cindex, nextpos, out opos);

                            // 押し出された場合はベクトル追加
                            if (ret)
                            {
                                addv += (opos - nextpos);
                                addcnt++;
                            }
                        }
                    }

                    // 平均化
                    if (addcnt > 0)
                    {
                        nextpos += addv / addcnt;
                    }
                }

                // 速度影響はなし
                posList[index] += (nextpos - oldpos);

                // 書き戻し
                outNextPosList[index] = nextpos;
            }

            //=====================================================================================
            /// <summary>
            /// 内球制限
            /// </summary>
            /// <param name="data"></param>
            /// <param name="distance"></param>
            /// <param name="cindex"></param>
            /// <param name="cr"></param>
            /// <param name="nextpos"></param>
            /// <param name="outpos"></param>
            /// <returns></returns>
            private bool InverseSpherePenetration(ref PenetrationData data, float distance, int cindex, float cr, float3 nextpos, out float3 outpos)
            {
                var cpos = nextPosList[cindex];
                var crot = nextRotList[cindex];

                // スケール
                var tindex = transformIndexList[cindex];
                var cscl = boneSclList[tindex];

                // 中心軸
                var d = math.mul(crot, data.localPos * cscl) + cpos;

                // 方向
                var n = math.mul(crot, data.localDir);

                // 球の位置
                var c = d + n * (data.distance - distance + cr);

                // 球内部制限
                var v = nextpos - c;
                var len = math.length(v);
                if (len > cr)
                {
                    v *= (cr / len);
                    outpos = c + v;
                    return true;
                }
                else
                {
                    outpos = nextpos;
                    return false;
                }
            }

            /// <summary>
            /// 平面制限
            /// </summary>
            /// <param name="data"></param>
            /// <param name="cindex"></param>
            /// <param name="nextpos"></param>
            /// <param name="outpos"></param>
            /// <returns></returns>
            private bool PlanePenetration(ref PenetrationData data, float distance, int cindex, float3 nextpos, out float3 outpos)
            {
                var cpos = nextPosList[cindex];
                var crot = nextRotList[cindex];

                // スケール
                var tindex = transformIndexList[cindex];
                var cscl = boneSclList[tindex];

                // 中心軸
                var d = math.mul(crot, data.localPos * cscl) + cpos;

                // 方向
                var n = math.mul(crot, data.localDir);

                // 押し出し平面を求める
                var c = d + n * (data.distance - distance);

                // c = 平面位置
                // n = 平面方向
                // 平面衝突判定と押し出し
                return MathUtility.IntersectPointPlane(c, n, nextpos, out outpos);
            }

#if false
            /// <summary>
            /// 角度制限
            /// </summary>
            /// <param name="data"></param>
            /// <param name="cindex"></param>
            /// <param name="nextpos"></param>
            /// <param name="outpos"></param>
            /// <param name="ang"></param>
            /// <returns></returns>
            private bool AnglePenetration(ref PenetrationData data, int cindex, float3 nextpos, out float3 outpos, float ang)
            {
                var cpos = nextPosList[cindex];
                var crot = nextRotList[cindex];

                // スケール
                var tindex = transformIndexList[cindex];
                var cscl = boneSclList[tindex];
                //float scl = cscl.x; // X軸を採用（基本的には均等スケールのみを想定）

                // 押し出し平面を求める
                var c = math.mul(crot, data.localPos * cscl) + cpos;
                var n = math.mul(crot, data.localDir);

                var v = nextpos - c;

                float3 v2;
                if (MathUtility.ClampAngle(v, n, ang, out v2))
                {
                    outpos = c + v2;
                    return true;
                }

                outpos = nextpos;
                return false;
            }
#endif
        }
    }
}

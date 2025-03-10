﻿// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MagicaCloth
{
    /// <summary>
    /// クロスのギズモ表示
    /// </summary>
    public class ClothGizmoDrawer
    {
        public static bool DrawClothGizmo(
            PhysicsTeam team,
            ClothData clothData,
            ClothParams param,
            ClothSetup setup,
            IEditorMesh editorMesh,
            IEditorCloth editorCloth
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawCloth == false)
                return false;

            if (ClothMonitorMenu.Monitor.UI.DrawClothVertex == false
                && ClothMonitorMenu.Monitor.UI.DrawClothDepth == false
                && ClothMonitorMenu.Monitor.UI.DrawClothBase == false
                && ClothMonitorMenu.Monitor.UI.DrawClothCollider == false
                && ClothMonitorMenu.Monitor.UI.DrawClothStructDistanceLine == false
                && ClothMonitorMenu.Monitor.UI.DrawClothBendDistanceLine == false
                && ClothMonitorMenu.Monitor.UI.DrawClothNearDistanceLine == false
                && ClothMonitorMenu.Monitor.UI.DrawClothRotationLine == false
                && ClothMonitorMenu.Monitor.UI.DrawClothTriangleBend == false
                && ClothMonitorMenu.Monitor.UI.DrawClothPenetration == false
                //&& ClothMonitorMenu.Monitor.UI.DrawClothBaseSkinning == false
                && ClothMonitorMenu.Monitor.UI.DrawClothAxis == false
                //&& ClothMonitorMenu.Monitor.UI.DrawClothVolume == false
#if MAGICACLOTH_DEBUG
                && ClothMonitorMenu.Monitor.UI.DrawClothVertexNumber == false
                && ClothMonitorMenu.Monitor.UI.DrawClothVertexIndex == false
                //&& ClothMonitorMenu.Monitor.UI.DrawAdjustRotationLine == false
#endif
                )
                return false;

            if (clothData == null)
                return false;

            if (Application.isPlaying)
            {
                if (clothData == null)
                    return false;

                if (team.IsActive() == false)
                    return false;

                // 頂点使用状態
                //var useList = editorCloth.GetUseList();
                var selList = editorCloth.GetSelectionList();

                // 頂点情報
                DrawVertexRuntime(team, clothData, param, setup, selList);

                // コライダー
                DrawCollider(team);

                // ライン
                DrawLineRuntime(team, clothData, setup, selList);

                // 回転ライン
                DrawRotationLineRuntime(team, clothData, setup, selList);

                // トライアングルベンド
                DrawTriangleBendRuntime(team, clothData, setup);

                // コライダー移動制限
                DrawPenetrationRuntime(team, param, clothData, selList);

                // ボリューム
                //DrawVolumeRuntime(team, clothData, setup);

                // 回転調整ライン
                //DrawAdjustRotationLineRuntime(team, clothData);
            }
            else
            {
                // メッシュ頂点法線接線
                List<Vector3> posList;
                List<Vector3> norList;
                List<Vector3> tanList;
                int vcnt = editorMesh.GetEditorPositionNormalTangent(out posList, out norList, out tanList);

                // 頂点使用状態
                //var useList = editorCloth.GetUseList();
                var selList = editorCloth.GetSelectionList();

                // 頂点情報
                DrawVertexClothData(clothData, param, vcnt, posList, norList, tanList, selList);

                // コライダー
                DrawCollider(team);

                // ライン
                DrawLineClothData(clothData, posList, selList);

                // 回転ライン
                DrawRotationLineClothData(clothData, posList, selList);

                // トライアングルベンド
                DrawTriangleBendClothData(clothData, posList);

                // コライダー移動制限
                DrawPenetrationClothData(team, param, clothData, posList, norList, tanList, selList);

                // ベーススキニング
                //DrawBaseSkinningClothData(team, clothData, posList, selList);

                // ボリューム
                //DrawVolumeClothData(clothData, posList);

                // 回転調整ライン
                //DrawAdjustRotationLineClothData(clothData, posList);
            }

            return true;
        }

        //=========================================================================================
        /// <summary>
        /// ランタイム状態での頂点表示
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="deformer"></param>
        /// <param name="clothData"></param>
        static void DrawVertexRuntime(
            PhysicsTeam team,
            ClothData clothData,
            ClothParams param,
            ClothSetup setup,
            List<int> selList
            )
        {
            bool drawVertex = ClothMonitorMenu.Monitor.UI.DrawClothVertex;
            bool drawRadius = ClothMonitorMenu.Monitor.UI.DrawClothRadius;
            bool drawDepth = ClothMonitorMenu.Monitor.UI.DrawClothDepth;
            bool drawBase = ClothMonitorMenu.Monitor.UI.DrawClothBase;
            bool drawAxis = ClothMonitorMenu.Monitor.UI.DrawClothAxis;
#if MAGICACLOTH_DEBUG
            bool number = ClothMonitorMenu.Monitor.UI.DrawClothVertexNumber;
            bool drawIndex = ClothMonitorMenu.Monitor.UI.DrawClothVertexIndex;
            bool drawFriction = ClothMonitorMenu.Monitor.UI.DrawClothFriction;
            bool drawDepthNumber = ClothMonitorMenu.Monitor.UI.DrawClothDepthNumber;
#else
            bool number = false;
            bool drawIndex = false;
            bool drawFriction = false;
            bool drawDepthNumber = false;
#endif

            if (!number && !drawVertex && !drawDepth && !drawBase && !drawAxis && !drawIndex && !drawFriction && !drawDepthNumber)
                return;

            int vcnt = clothData.useVertexList.Count;
            for (int i = 0; i < vcnt; i++)
            {
                int vindex = clothData.useVertexList[i];
                int pindex = team.ParticleChunk.startIndex + i;
                Vector3 pos = MagicaPhysicsManager.Instance.Particle.posList[pindex];
                float depth = MagicaPhysicsManager.Instance.Particle.depthList[pindex];
                //float radius = PhysicsManager.Instance.Particle.radiusList[pindex];
                float radius = drawRadius ? MagicaPhysicsManager.Instance.Particle.radiusList[pindex] : 0.001f;
                //float radius = param.GetRadius(depth);

                if (drawVertex || drawDepth || drawAxis)
                {
                    Quaternion rot = MagicaPhysicsManager.Instance.Particle.rotList[pindex];
                    Gizmos.color = GetVertexColor(vindex, depth, selList);
                    GizmoUtility.DrawWireSphere(pos, rot, Vector3.one, radius, drawVertex || drawDepth, drawAxis);
                }
                if (drawBase)
                {
                    Vector3 bpos = MagicaPhysicsManager.Instance.Particle.basePosList[pindex];
                    Quaternion brot = MagicaPhysicsManager.Instance.Particle.baseRotList[pindex];
                    Gizmos.color = GizmoUtility.ColorBasePosition;
                    GizmoUtility.DrawWireSphere(bpos, brot, Vector3.one, radius, true, false);
                }

                if (number)
                {
                    Handles.Label(pos, i.ToString());
                }
                if (drawIndex)
                {
                    Handles.Label(pos, pindex.ToString());
                }
                if (drawFriction)
                {
                    float friction = MagicaPhysicsManager.Instance.Particle.frictionList[pindex];
                    Handles.Label(pos, string.Format("{0:#.##}", friction));
                }
                //if (drawDepthNumber)
                if (drawDepth)
                {
                    float d = MagicaPhysicsManager.Instance.Particle.depthList[pindex];
                    Handles.Label(pos, string.Format("{0:#.##}", d));
                }
            }
        }

        /// <summary>
        /// エディタ状態での頂点表示
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="deformer"></param>
        /// <param name="clothData"></param>
        static void DrawVertexClothData(
            ClothData clothData,
            ClothParams param,

            int vcnt,
            List<Vector3> posList,
            List<Vector3> norList,
            List<Vector3> tanList,
            List<int> selList
            )
        {
            bool drawVertex = ClothMonitorMenu.Monitor.UI.DrawClothVertex;
            bool drawRadius = ClothMonitorMenu.Monitor.UI.DrawClothRadius;
            bool drawDepth = ClothMonitorMenu.Monitor.UI.DrawClothDepth;
            bool drawBase = ClothMonitorMenu.Monitor.UI.DrawClothBase;
            bool drawAxis = ClothMonitorMenu.Monitor.UI.DrawClothAxis;
#if MAGICACLOTH_DEBUG
            bool number = ClothMonitorMenu.Monitor.UI.DrawClothVertexNumber;
            bool drawDepthNumber = ClothMonitorMenu.Monitor.UI.DrawClothDepthNumber;
#else
            bool number = false;
            bool drawDepthNumber = false;
#endif

            if (!number && !drawVertex && !drawDepth && !drawBase && !drawAxis && !drawDepthNumber)
                return;

            for (int i = 0; i < clothData.VertexUseCount; i++)
            {
                int vindex = clothData.useVertexList[i];

                if (vindex >= posList.Count)
                    continue;

                Vector3 pos = posList[vindex];

                if (drawVertex || drawDepth || drawBase || drawAxis)
                {
                    Vector3 nor = norList[vindex];
                    Vector3 tan = tanList[vindex];
                    Quaternion rot = Quaternion.LookRotation(nor, tan);
                    float depth = clothData == null ? 0.0f : clothData.vertexDepthList[i];
                    //float radius = param.GetRadius(depth);
                    float radius = drawRadius ? param.GetRadius(depth) : 0.001f;

                    if (drawBase)
                    {
                        Gizmos.color = GizmoUtility.ColorBasePosition;
                        GizmoUtility.DrawWireSphere(pos, rot, Vector3.one, radius, true, false);
                    }
                    else
                    {
                        Gizmos.color = GetVertexColor(vindex, depth, selList);
                        GizmoUtility.DrawWireSphere(pos, rot, Vector3.one, radius, drawVertex || drawDepth, drawAxis);
                    }
                }

                if (number)
                {
                    Handles.Label(pos, i.ToString());
                }
                //if (drawDepthNumber)
                if (drawDepth)
                {
                    float depth = clothData == null ? 0.0f : clothData.vertexDepthList[i];
                    Handles.Label(pos, string.Format("{0:#.##}", depth));
                }
            }
        }

        /// <summary>
        /// クロス頂点カラー設定
        /// </summary>
        /// <param name="vindex"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        static Color GetVertexColor(int vindex, float depth, List<int> selList)
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothDepth)
            {
                return Color.Lerp(Color.red, Color.blue, depth);
            }
            else
            {
                if (selList == null || vindex >= selList.Count)
                    return GizmoUtility.ColorDeformerPoint;
                else if (selList[vindex] == SelectionData.Fixed)
                    return GizmoUtility.ColorKinematic;
                else if (selList[vindex] == SelectionData.Move)
                    return GizmoUtility.ColorDynamic;
                else
                    return GizmoUtility.ColorInvalid;
            }
        }

        static bool IsMove(int vindex, List<int> selList)
        {
            if (selList == null || vindex >= selList.Count)
                return false;

            return selList[vindex] == SelectionData.Move;
        }

        //=========================================================================================
        /// <summary>
        /// クロスに紐づくコライダーの表示
        /// </summary>
        /// <param name="scr"></param>
        public static void DrawCollider(PhysicsTeam team)
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothCollider == false)
                return;

            var colliderlist = team.TeamData.ColliderList;
            foreach (var collider in colliderlist)
            {
                if (collider == null)
                    continue;
                if (collider is MagicaSphereCollider)
                    MagicaSphereColliderGizmoDrawer.DrawGizmo(collider as MagicaSphereCollider, true);
                else if (collider is MagicaCapsuleCollider)
                    MagicaCapsuleColliderGizmoDrawer.DrawGizmo(collider as MagicaCapsuleCollider, true);
            }
        }

        //=========================================================================================
#if false
        /// <summary>
        /// エディタ状態でのベーススキニング表示
        /// </summary>
        /// <param name="team"></param>
        /// <param name="clothData"></param>
        /// <param name="posList"></param>
        /// <param name="selList"></param>
        static void DrawBaseSkinningClothData(
            PhysicsTeam team,
            ClothData clothData,
            List<Vector3> posList,
            List<int> selList
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothBaseSkinning == false)
                return;

            if (clothData.baseSkinningDataList == null)
                return;

            //var boneList = team.TeamData.SkinningBoneList;
            var boneList = team.TeamData.ColliderList;

            //Gizmos.color = GizmoUtility.ColorPenetration;
            for (int i = 0; i < clothData.VertexUseCount; i++)
            {
                int vindex = clothData.useVertexList[i];
                if (vindex >= posList.Count)
                    continue;
                if (IsMove(vindex, selList) == false)
                    continue;

                Vector3 pos = posList[vindex];

                for (int j = 0; j < Define.Compute.BaseSkinningWeightCount; j++)
                {
                    int dindex = i * Define.Compute.BaseSkinningWeightCount + j;
                    if (dindex >= clothData.baseSkinningDataList.Length)
                        return;

                    var data = clothData.baseSkinningDataList[dindex];
                    if (data.IsValid() == false)
                        continue;

                    int bindex = data.boneIndex;
                    if (bindex >= boneList.Count)
                        continue;

                    var bone = boneList[bindex];
                    if (bone == null)
                        continue;

                    //Gizmos.color = j == 0 ? Color.red : Color.yellow;
                    Gizmos.color = Color.gray;

#if true
                    Vector3 p, dir, d;
                    bone.CalcNearPoint(pos, out p, out dir, out d, true);
                    Gizmos.DrawLine(pos, p);
#else
                    //var cp = bone.TransformPoint(data.localPos);
                    Vector3 cp;
                    MeshUtility.ClosestPtBoneLine(pos, bone, 0.03f, out cp);
                    Gizmos.DrawLine(pos, cp);
#endif
                }
            }
        }
#endif


        //=========================================================================================
        /// <summary>
        /// ランタイム状態での浸透制限表示
        /// </summary>
        /// <param name="team"></param>
        /// <param name="clothData"></param>
        /// <param name="selList"></param>
        static void DrawPenetrationRuntime(
            PhysicsTeam team,
            ClothParams param,
            ClothData clothData,
            List<int> selList
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothPenetration == false)
                return;

            if (clothData.penetrationDataList == null)
                return;
            if (clothData.penetrationReferenceList == null)
                return;

            //var mode = param.GetPenetrationMode();
            var mode = clothData.penetrationMode;

            var colliderlist = team.TeamData.ColliderList;

            int vcnt = clothData.useVertexList.Count;
            for (int i = 0; i < vcnt; i++)
            {
                int vindex = clothData.useVertexList[i];
                if (IsMove(vindex, selList) == false)
                    continue;

                int pindex = team.ParticleChunk.startIndex + i;
                Vector3 pos = MagicaPhysicsManager.Instance.Particle.posList[pindex];

                if (i >= clothData.penetrationReferenceList.Length)
                    return;
                var refdata = clothData.penetrationReferenceList[i];
                for (int j = 0; j < refdata.count; j++)
                {
                    var dindex = refdata.startIndex + j;
                    var data = clothData.penetrationDataList[dindex];
                    if (data.IsValid() == false)
                        continue;

                    if (mode == ClothParams.PenetrationMode.SurfacePenetration)
                    {

                    }
                    else if (mode == ClothParams.PenetrationMode.ColliderPenetration)
                    {

                        int cindex = data.colliderIndex;
                        if (cindex >= colliderlist.Count)
                            continue;

                        var col = colliderlist[cindex];
                        if (col == null)
                            continue;

                        var cp = col.transform.TransformPoint(data.localPos);
                        //var pos = cp + col.transform.TransformDirection(data.localDir) * data.distance;

                        Gizmos.color = GizmoUtility.ColorPenetration;
                        Gizmos.DrawLine(pos, cp);
                    }
                }
            }
        }

        /// <summary>
        /// エディタ状態での浸透制限表示
        /// </summary>
        /// <param name="team"></param>
        /// <param name="clothData"></param>
        /// <param name="posList"></param>
        /// <param name="selList"></param>
        static void DrawPenetrationClothData(
            PhysicsTeam team,
            ClothParams param,
            ClothData clothData,
            List<Vector3> posList,
            List<Vector3> norList,
            List<Vector3> tanList,
            List<int> selList
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothPenetration == false)
                return;

            if (clothData.penetrationDataList == null)
                return;
            if (clothData.penetrationReferenceList == null)
                return;

            var colliderlist = team.TeamData.ColliderList;

            //var mode = param.GetPenetrationMode();
            var mode = clothData.penetrationMode;

            for (int i = 0; i < clothData.VertexUseCount; i++)
            {
                int vindex = clothData.useVertexList[i];
                if (vindex >= posList.Count)
                    continue;
                if (IsMove(vindex, selList) == false)
                    continue;

                //Vector3 pos = posList[vindex];

                if (i >= clothData.penetrationReferenceList.Length)
                    return;

                var refdata = clothData.penetrationReferenceList[i];
                for (int j = 0; j < refdata.count; j++)
                {
                    var dindex = refdata.startIndex + j;
                    var data = clothData.penetrationDataList[dindex];
                    if (data.IsValid() == false)
                        continue;

                    if (mode == ClothParams.PenetrationMode.SurfacePenetration)
                    {
                        var pos = posList[vindex];
                        var rot = Quaternion.LookRotation(norList[vindex], tanList[vindex]);
                        var dir = rot * data.localDir;
                        var depth = clothData.vertexDepthList[i];
                        var dist = param.GetPenetrationDistance().Evaluate(depth);

                        Gizmos.color = GizmoUtility.ColorPenetration;
                        Gizmos.DrawLine(pos, pos + dir * dist);
                        break;
                    }
                    else if (mode == ClothParams.PenetrationMode.ColliderPenetration)
                    {

                        int cindex = data.colliderIndex;
                        if (cindex >= colliderlist.Count)
                            continue;

                        var col = colliderlist[cindex];
                        if (col == null)
                            continue;

                        var cp = col.transform.TransformPoint(data.localPos);
                        var pos = cp + col.transform.TransformDirection(data.localDir) * data.distance;

                        Gizmos.color = GizmoUtility.ColorPenetration;
                        Gizmos.DrawLine(pos, cp);
                    }
                }
            }
        }

        //=========================================================================================
        /// <summary>
        /// ランタイム状態でのライン表示
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="deformer"></param>
        /// <param name="clothData"></param>
        static void DrawLineRuntime(
            PhysicsTeam team,
            ClothData clothData,
            ClothSetup setup,
            List<int> selList
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothStructDistanceLine)
            {
                DrawLineRuntimeSub(team, GizmoUtility.ColorStructLine, clothData.structDistanceDataList);
            }
            if (ClothMonitorMenu.Monitor.UI.DrawClothBendDistanceLine)
            {
                DrawLineRuntimeSub(team, GizmoUtility.ColorBendLine, clothData.bendDistanceDataList);
            }
            if (ClothMonitorMenu.Monitor.UI.DrawClothNearDistanceLine)
            {
                DrawLineRuntimeSub(team, GizmoUtility.ColorNearLine, clothData.nearDistanceDataList);
            }
        }

        static void DrawLineRuntimeSub(
            PhysicsTeam team,
            Color color,
            RestoreDistanceConstraint.RestoreDistanceData[] distanceDataList
            )
        {
            if (distanceDataList == null || distanceDataList.Length == 0)
                return;

            var manager = MagicaPhysicsManager.Instance;

            Gizmos.color = color;
            int cnt = distanceDataList.Length;
            for (int i = 0; i < cnt; i++)
            {
                var data = distanceDataList[i];
                int vindex0, vindex1;
                vindex0 = data.vertexIndex;
                vindex1 = data.targetVertexIndex;

                int pindex0 = team.ParticleChunk.startIndex + vindex0;
                int pindex1 = team.ParticleChunk.startIndex + vindex1;

                Vector3 pos0 = manager.Particle.posList[pindex0];
                Vector3 pos1 = manager.Particle.posList[pindex1];

                Gizmos.DrawLine(pos0, pos1);
            }
        }

        /// <summary>
        /// エディタ状態でのライン表示
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="deformer"></param>
        /// <param name="clothData"></param>
        static void DrawLineClothData(
            ClothData clothData,
            List<Vector3> posList,
            List<int> selList
            )
        {
            if (clothData == null)
                return;
            if (ClothMonitorMenu.Monitor.UI.DrawClothStructDistanceLine)
            {
                DrawLineClothDataSub(clothData, posList, GizmoUtility.ColorStructLine, clothData.structDistanceDataList);
            }
            if (ClothMonitorMenu.Monitor.UI.DrawClothBendDistanceLine)
            {
                DrawLineClothDataSub(clothData, posList, GizmoUtility.ColorBendLine, clothData.bendDistanceDataList);
            }
            if (ClothMonitorMenu.Monitor.UI.DrawClothNearDistanceLine)
            {
                DrawLineClothDataSub(clothData, posList, GizmoUtility.ColorNearLine, clothData.nearDistanceDataList);
            }
        }

        static void DrawLineClothDataSub(
            ClothData clothData,
            List<Vector3> posList,
            Color color,
            RestoreDistanceConstraint.RestoreDistanceData[] distanceDataList
            )
        {
            if (distanceDataList == null || distanceDataList.Length == 0)
                return;

            Gizmos.color = color;
            int cnt = distanceDataList.Length;
            for (int i = 0; i < cnt; i++)
            {
                var data = distanceDataList[i];
                int index0, index1;
                index0 = data.vertexIndex;
                index1 = data.targetVertexIndex;

                int vindex0 = clothData.useVertexList[index0];
                int vindex1 = clothData.useVertexList[index1];

                if (vindex0 >= posList.Count || vindex1 >= posList.Count)
                    continue;

                Vector3 pos0 = posList[vindex0];
                Vector3 pos1 = posList[vindex1];

                Gizmos.DrawLine(pos0, pos1);
            }
        }

        //=========================================================================================
        /// <summary>
        /// ランタイム状態での回転ライン表示
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="deformer"></param>
        /// <param name="clothData"></param>
        static void DrawRotationLineRuntime(
            PhysicsTeam team,
            ClothData clothData,
            ClothSetup setup,
            List<int> selList
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothRotationLine == false)
                return;
            if (clothData == null)
                return;
            if (clothData.parentList == null || clothData.parentList.Count != clothData.VertexUseCount)
                return;

            var manager = MagicaPhysicsManager.Instance;

            Gizmos.color = GizmoUtility.ColorRotationLine;

            for (int i = 0; i < clothData.VertexUseCount; i++)
            {
                int pi = clothData.parentList[i];
                if (pi < 0)
                    continue;

                int pindex0 = team.ParticleChunk.startIndex + i;
                int pindex1 = team.ParticleChunk.startIndex + pi;

                Vector3 pos0 = manager.Particle.posList[pindex0];
                Vector3 pos1 = manager.Particle.posList[pindex1];

                Gizmos.DrawLine(pos0, pos1);
            }
        }

        /// <summary>
        /// エディタ状態での回転ライン表示
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="deformer"></param>
        /// <param name="clothData"></param>
        static void DrawRotationLineClothData(
            ClothData clothData,
            List<Vector3> posList,
            List<int> selList
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothRotationLine == false)
                return;
            if (clothData == null)
                return;
            if (clothData.parentList == null || clothData.parentList.Count != clothData.VertexUseCount)
                return;

            Gizmos.color = GizmoUtility.ColorRotationLine;

            for (int i = 0; i < clothData.VertexUseCount; i++)
            {
                int pi = clothData.parentList[i];
                if (pi < 0)
                    continue;

                int vindex0 = clothData.useVertexList[i];
                int vindex1 = clothData.useVertexList[pi];

                if (vindex0 >= posList.Count || vindex1 >= posList.Count)
                    continue;

                Vector3 pos0 = posList[vindex0];
                Vector3 pos1 = posList[vindex1];

                Gizmos.DrawLine(pos0, pos1);
            }
        }

        //=========================================================================================
#if false
        static void DrawAdjustRotationLineRuntime(
            PhysicsTeam team,
            ClothData clothData
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawAdjustRotationLine == false)
                return;

            var manager = MagicaPhysicsManager.Instance;

            Gizmos.color = GizmoUtility.ColorAdjustLine;

            int cnt = clothData.AdjustRotationConstraintCount;
            for (int i = 0; i < cnt; i++)
            {
                var data = clothData.adjustRotationDataList[i];
                int tindex = data.targetIndex;
                if (tindex < 0)
                    tindex = -tindex - 1;

                int pindex0 = team.ParticleChunk.startIndex + data.keyIndex;
                int pindex1 = team.ParticleChunk.startIndex + tindex;

                Vector3 pos0 = manager.Particle.posList[pindex0];
                Vector3 pos1 = manager.Particle.posList[pindex1];

                Gizmos.DrawLine(pos0, pos1);
            }
        }

        static void DrawAdjustRotationLineClothData(
            ClothData clothData,
            List<Vector3> posList
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawAdjustRotationLine == false)
                return;
            if (clothData == null)
                return;

            Gizmos.color = GizmoUtility.ColorAdjustLine;

            int cnt = clothData.AdjustRotationConstraintCount;
            for (int i = 0; i < cnt; i++)
            {
                var data = clothData.adjustRotationDataList[i];

                int tindex = data.targetIndex;
                if (tindex < 0)
                    tindex = -tindex - 1;

                int vindex0 = clothData.useVertexList[data.keyIndex];
                int vindex1 = clothData.useVertexList[tindex];

                Vector3 pos0 = posList[vindex0];
                Vector3 pos1 = posList[vindex1];

                Gizmos.DrawLine(pos0, pos1);
            }
        }
#endif

        //=========================================================================================
        /// <summary>
        /// ランタイム状態でのトライアングルベンド表示
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="deformer"></param>
        /// <param name="clothData"></param>
        static void DrawTriangleBendRuntime(
            PhysicsTeam team,
            ClothData clothData,
            ClothSetup setup
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothTriangleBend == false)
                return;

            var manager = MagicaPhysicsManager.Instance;

            Gizmos.color = GizmoUtility.ColorTriangle;
            int cnt = clothData.TriangleBendConstraintCount;
            for (int i = 0; i < cnt; i++)
            {
                var data = clothData.triangleBendDataList[i];

                int pindex0 = team.ParticleChunk.startIndex + data.vindex0;
                int pindex1 = team.ParticleChunk.startIndex + data.vindex1;
                int pindex2 = team.ParticleChunk.startIndex + data.vindex2;
                int pindex3 = team.ParticleChunk.startIndex + data.vindex3;

                Vector3 pos0 = manager.Particle.posList[pindex0];
                Vector3 pos1 = manager.Particle.posList[pindex1];
                Vector3 pos2 = manager.Particle.posList[pindex2];
                Vector3 pos3 = manager.Particle.posList[pindex3];

                Gizmos.DrawLine(pos0, pos2);
                Gizmos.DrawLine(pos0, pos3);
                Gizmos.DrawLine(pos2, pos3);
                Gizmos.DrawLine(pos2, pos1);
                Gizmos.DrawLine(pos3, pos1);
            }
        }

        /// <summary>
        /// エディタ状態でのトライアングルベンド表示
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="deformer"></param>
        /// <param name="clothData"></param>
        static void DrawTriangleBendClothData(
            ClothData clothData,
            List<Vector3> posList
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothTriangleBend == false)
                return;
            if (clothData == null)
                return;

            Gizmos.color = GizmoUtility.ColorTriangle;
            int cnt = clothData.TriangleBendConstraintCount;
            for (int i = 0; i < cnt; i++)
            {
                var data = clothData.triangleBendDataList[i];

                int vindex0 = clothData.useVertexList[data.vindex0];
                int vindex1 = clothData.useVertexList[data.vindex1];
                int vindex2 = clothData.useVertexList[data.vindex2];
                int vindex3 = clothData.useVertexList[data.vindex3];

                Vector3 pos0 = posList[vindex0];
                Vector3 pos1 = posList[vindex1];
                Vector3 pos2 = posList[vindex2];
                Vector3 pos3 = posList[vindex3];

                Gizmos.DrawLine(pos0, pos2);
                Gizmos.DrawLine(pos0, pos3);
                Gizmos.DrawLine(pos2, pos3);
                Gizmos.DrawLine(pos2, pos1);
                Gizmos.DrawLine(pos3, pos1);
            }
        }

        //=========================================================================================
#if false
        /// <summary>
        /// ランタイム状態でのボリューム表示
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="deformer"></param>
        /// <param name="clothData"></param>
        static void DrawVolumeRuntime(
            PhysicsTeam team,
            ClothData clothData,
            ClothSetup setup
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothVolume == false)
                return;

            var manager = MagicaPhysicsManager.Instance;

            Gizmos.color = GizmoUtility.ColorTriangle;
            int cnt = clothData.VolumeConstraintCount;
            for (int i = 0; i < cnt; i++)
            {
                var data = clothData.volumeDataList[i];

                int pindex0 = team.ParticleChunk.startIndex + data.vindex0;
                int pindex1 = team.ParticleChunk.startIndex + data.vindex1;
                int pindex2 = team.ParticleChunk.startIndex + data.vindex2;
                int pindex3 = team.ParticleChunk.startIndex + data.vindex3;

                Vector3 pos0 = manager.Particle.posList[pindex0];
                Vector3 pos1 = manager.Particle.posList[pindex1];
                Vector3 pos2 = manager.Particle.posList[pindex2];
                Vector3 pos3 = manager.Particle.posList[pindex3];

                Gizmos.DrawLine(pos0, pos1);
                Gizmos.DrawLine(pos0, pos2);
                Gizmos.DrawLine(pos0, pos3);
                Gizmos.DrawLine(pos1, pos2);
                Gizmos.DrawLine(pos2, pos3);
                Gizmos.DrawLine(pos3, pos1);
            }
        }

        /// <summary>
        /// エディタ状態でのボリューム表示
        /// </summary>
        /// <param name="scr"></param>
        /// <param name="deformer"></param>
        /// <param name="clothData"></param>
        static void DrawVolumeClothData(
            ClothData clothData,
            List<Vector3> posList
            )
        {
            if (ClothMonitorMenu.Monitor.UI.DrawClothVolume == false)
                return;
            if (clothData == null)
                return;

            Gizmos.color = GizmoUtility.ColorTriangle;
            int cnt = clothData.VolumeConstraintCount;
            for (int i = 0; i < cnt; i++)
            {
                var data = clothData.volumeDataList[i];

                int vindex0 = clothData.useVertexList[data.vindex0];
                int vindex1 = clothData.useVertexList[data.vindex1];
                int vindex2 = clothData.useVertexList[data.vindex2];
                int vindex3 = clothData.useVertexList[data.vindex3];

                Vector3 pos0 = posList[vindex0];
                Vector3 pos1 = posList[vindex1];
                Vector3 pos2 = posList[vindex2];
                Vector3 pos3 = posList[vindex3];

                Gizmos.DrawLine(pos0, pos1);
                Gizmos.DrawLine(pos0, pos2);
                Gizmos.DrawLine(pos0, pos3);
                Gizmos.DrawLine(pos1, pos2);
                Gizmos.DrawLine(pos2, pos3);
                Gizmos.DrawLine(pos3, pos1);
            }
        }
#endif
    }
}

﻿// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp
using UnityEngine;

namespace MagicaCloth
{
    /// <summary>
    /// 面コライダー
    /// トランスフォームのY＋方向を面の法線として計算する
    /// </summary>
    [HelpURL("https://magicasoft.jp/magica-cloth-plane-collider/")]
    [AddComponentMenu("MagicaCloth/MagicaPlaneCollider")]
    public class MagicaPlaneCollider : ColliderComponent
    {
        //=========================================================================================
        protected override ChunkData CreateColliderParticleReal(int teamId)
        {
            uint flag = 0;
            flag |= PhysicsManagerParticleData.Flag_Kinematic;
            flag |= PhysicsManagerParticleData.Flag_Collider;
            flag |= PhysicsManagerParticleData.Flag_Transform_Read_Pos;
            flag |= PhysicsManagerParticleData.Flag_Transform_Read_Rot;
            flag |= PhysicsManagerParticleData.Flag_Transform_Read_Base;
            flag |= PhysicsManagerParticleData.Flag_Plane;
            flag |= PhysicsManagerParticleData.Flag_Reset_Position;

            var c = CreateParticle(
                flag,
                teamId, // team
                0.0f, // depth
                1.0f, // radius
                Vector3.zero
                );

            MagicaPhysicsManager.Instance.Team.AddCollider(teamId, c.startIndex);

            return c;
        }

        /// <summary>
        /// 指定座標に最も近い衝突点pと、中心軸からのpへの方向dirを返す。
        /// ※エディタ計算用
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="p"></param>
        /// <param name="dir"></param>
        public override bool CalcNearPoint(Vector3 pos, out Vector3 p, out Vector3 dir, out Vector3 d, bool skinning)
        {
            dir = Vector3.zero;

            // 平面姿勢
            var cpos = transform.position;

            // 平面法線
            var cdir = transform.up;

            // 平面法線に投影
            var v = pos - cpos;
            var gv = Vector3.Project(v, cdir);

            p = pos - gv;
            d = p;
            dir = gv.normalized;

            return Vector3.Dot(v, gv) <= 0.0f;
        }
    }
}

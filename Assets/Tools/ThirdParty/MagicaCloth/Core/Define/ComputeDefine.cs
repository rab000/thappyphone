// Magica Cloth.
// Copyright (c) MagicaSoft, 2020.
// https://magicasoft.jp

namespace MagicaCloth
{
    public static partial class Define
    {
        /// <summary>
        /// 計算用デファイン
        /// </summary>
        public static class Compute
        {
            /// <summary>
            /// 摩擦係数を計算するコライダーとの距離
            /// </summary>
            public const float CollisionFrictionRange = 0.01f; // 0.05(v1.6.1)

            /// <summary>
            /// 摩擦の減衰率
            /// </summary>
            public const float FrictionDampingRate = 0.6f; // 0.6(v1.6.1)

            /// <summary>
            /// 摩擦係数による移動率
            /// 0.9f = 最大摩擦でも10%は移動する
            /// </summary>
            public const float FrictionMoveRatio = 0.9f; // 0.5(v1.6.1)

            /// <summary>
            /// ClampPosition拘束でのパーティクル最大速度(m/s)
            /// </summary>
            public const float ClampPositionMaxVelocity = 1.0f;

            /// <summary>
            /// ClampRotation拘束でのパーティクル最大速度(m/s)
            /// </summary>
            public const float ClampRotationMaxVelocity = 1.0f;

            /// <summary>
            /// ベーススキニング最大ウエイト数
            /// </summary>
            public const int BaseSkinningWeightCount = 4; // 2?
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ntools
{
    /// <summary>
    /// 同步动画
    /// </summary>
    public class AnimationBase: MonoBehaviour
    {
        protected AnimationBase()
        {

        }
        /// <summary>
        /// 动画运行帧率
        /// </summary>
        internal static readonly int Fps = 60;
    }
}
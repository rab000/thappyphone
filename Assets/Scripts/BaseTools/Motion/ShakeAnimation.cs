
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ntools
{
    /// <summary>
    /// 重复晃动动画
    /// </summary>
    public class ShakeAnimation : AnimationBase
    {
        /// <summary>
        /// 结束回调事件
        /// </summary>
        public event Listener OnFinishedEvent;
        /// <summary>
        /// 速度
        /// </summary>
        private float _speed = 0.2f;
        /// <summary>
        /// 偏移度
        /// </summary>
        private float _offset = 10f;
        /// <summary>
        /// 是否是顺时针
        /// </summary>
        private bool _isCW = true;
        /// <summary>
        /// 原始旋转角度
        /// </summary>
        private Vector3 _orgRotation;
        /// <summary>
        /// 当前旋转的角度
        /// </summary>
        private float _curRotation = 0f;
        /// <summary>
        /// 结束回调
        /// </summary>
        private System.Action _callBack;
        /// <summary>
        /// 是否动画进行中
        /// </summary>
        private bool _isAnimation = false;
        /// <summary>
        /// 开始动画
        /// </summary>
        /// <param name="offect">偏移度</param>
        /// <param name="speed">速度</param>
        /// <param name="callBack">回调</param>
        /// <param name="isCW">是否是顺时针</param>
        public void NStart(float offect = 10f, float speed = 1.0f, System.Action callBack = null, bool isCW = true)
        {
            _callBack = callBack;
            _offset = offect;
            _orgRotation = transform.localEulerAngles;
            _speed = speed;
            _isCW = isCW;
            _curRotation = 0f;
            _isAnimation = true;
        }
        /// <summary>
        /// 开始动画
        /// </summary>
        /// <param name="offect">偏移度</param>
        /// <param name="time">时间</param>
        /// <param name="callBack">回调</param>
        /// <param name="isCW">是否是顺时针</param>
        public void FpsStart(float offect = 10f, float time = 2f, System.Action callBack = null, bool isCW = true)
        {
            this.NStart(offect, offect / (Fps * time), callBack, isCW);
        }
        /// <summary>
        /// 停止动画
        /// </summary>
        /// <param name="resetOrg">false不归位，true归位（回到原始状态）</param>
        public void NStop(bool resetOrg = false)
        {
            _isAnimation = false;
            if (resetOrg)
            {
                transform.localEulerAngles = _orgRotation;
            }
            if (_callBack != null)
            {
                _callBack();
            }
            if (OnFinishedEvent != null)
            {
                OnFinishedEvent();
            }
        }
        /// <inheritdoc>
        /// </inheritdoc>
        public void Update()
        {
            if (_isAnimation)
            {
                if (_isCW)
                {
                    _curRotation -= _speed;
                }
                else
                {
                    _curRotation += _speed;
                }
                transform.localEulerAngles = new Vector3(_orgRotation.x, _orgRotation.y, _orgRotation.z + _curRotation);
                if (System.Math.Abs(_curRotation) >= _offset)
                {
                    _isCW = !_isCW;
                }
            }
        }
    }
}
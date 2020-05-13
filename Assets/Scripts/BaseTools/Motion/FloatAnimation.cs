/********************************************************************************
** Auth:zhanghl
** Date:2017/9/8
** FileName:FloatAnimation
** Desc:漂浮动画（本地坐标）
** Ver:V1.0.0
*********************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ntools
{
    /// <summary>
    /// 漂浮动画（本地坐标）
    /// </summary>
    public class FloatAnimation : AnimationBase
    {
        /// <summary>
        /// 结束回调事件
        /// </summary>
        public event Listener OnFinishedEvent;
        /// <summary>
        /// 速度
        /// </summary>
        private float _speed = 0.01f;
        /// <summary>
        /// 偏移度
        /// </summary>
        private float _offset = 0.2f;
        /// <summary>
        /// 是否是先上浮
        /// </summary>
        private bool _isUp = true;
        /// <summary>
        /// 原始位置
        /// </summary>
        private Vector3 _orgPosition;
        /// <summary>
        /// 当前位置
        /// </summary>
        private float _curY = 0f;
        /// <summary>
        /// 是否动画进行中
        /// </summary>
        private bool _isAnimation = false;
        /// <summary>
        /// 回调
        /// </summary>
        private System.Action _callBack = null;
        /// <summary>
        /// 开始动画
        /// </summary>
        /// <param name="offset">漂浮的最大偏移</param>
        /// <param name="speed">速度</param>
        /// <param name="isUp">默认先向上票</param>
        public void NStart(float offset = 0.1f, float speed = 0.003f, bool isUp = true, System.Action callback = null)
        {
            _callBack = callback;
            _orgPosition = transform.localPosition;
            _offset = offset;
            _speed = speed;
            _isUp = isUp;
            _curY = 0f;
            _isAnimation = true;
        }
        public void FpsStart(float offect = 0.2f, float time = 2f, bool isUp = true)
        {
            this.NStart(offect, offect / (Fps * time), isUp);
        }
        /// <summary>
        /// 停止动画
        /// <param name="resetOrg">false不归位，true归位（回到原始状态）</param>
        public void NStop(bool resetOrg = false)
        {
            _isAnimation = false;
            if (resetOrg)
            {
                transform.localPosition = _orgPosition;
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
                if (System.Math.Abs(_curY) >= _offset)
                {
                    _isUp = !_isUp;
                }
                if (_isUp)
                {
                    _curY += _speed;
                }
                else
                {
                    _curY -= _speed;
                }
                transform.localPosition = new Vector3(_orgPosition.x, _orgPosition.y + _curY, _orgPosition.z);
            }
        }
    }
}
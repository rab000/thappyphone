
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ntools
{
    /// <summary>
    /// 缩放动画（2D所放的是x，y）
    /// </summary>
    public class ScaleAnimation : AnimationBase
    {
        /// <summary>
        /// 结束回调事件
        /// </summary>
        public event Listener OnFinishedEvent;
        /// <summary>
        /// 是否播放缩放动画
        /// </summary>
        private bool _isActionZoom = false;
        /// <summary>
        /// 速度
        /// </summary>
        private float _speed = 0.5f;
        /// <summary>
        /// 默认先放大false放大，true缩小
        /// </summary>
        private bool _zoomState = false;
        /// <summary>
        /// 是否重复
        /// </summary>
        private bool _isRepeat;
        /// <summary>
        /// 缩放到
        /// </summary>
        private float _scaleBy;
        /// <summary>
        /// 缩放辅助计算变量
        /// </summary>
        private float _scaleAdd;
        /// <summary>
        /// 原始缩放值
        /// </summary>
        private Vector3 _orgScale;
        /// <summary>
        /// 结束回调
        /// </summary>
        private System.Action _callBack;
        /// <summary>
        /// 开始动画
        /// </summary>
        /// <param name="scaleBy">缩放系数默认0.1（即扩大0.1倍）</param>
        /// <param name="isRepeat">是否重复</param>
        /// <param name="speed">速度</param>
        /// <param name="callBack">结束回调</param>
        public void NStart(float scaleBy = 0.1f, bool isRepeat = true, float speed = 0.5f, System.Action callBack = null)
        {
            _callBack = callBack;
            _orgScale = transform.localScale;
            _speed = speed;
            _isRepeat = isRepeat;
            _scaleBy = scaleBy;
            _scaleAdd = 0f;
            if (scaleBy > 0)
            {
                _zoomState = false;
            }
            else
            {
                _zoomState = true;
            }
            _isActionZoom = true;
        }
        /// <summary>
        /// 开始动画
        /// </summary>
        /// <param name="scaleBy">缩放系数默认0.1（即扩大0.1倍）</param>
        /// <param name="time">时间</param>
        public void FpsStart(float scaleBy = 0.1f, float time = 2f)
        {
            this.NStart(scaleBy, false, scaleBy / (Fps * time));
        }
        /// <summary>
        /// 停止动画
        /// </summary>
        /// <param name="_resetOrg">false不归位，true归位（回到原始状态）</param>
        public void NStop(bool _resetOrg = false)
        {
            _isActionZoom = false;
            if (_resetOrg)
            {
                transform.localScale = _orgScale;
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
            if (_isActionZoom)
            {
                if (_zoomState)
                {
                    _scaleAdd -= _speed;
                    transform.localScale = _orgScale + new Vector3(_scaleAdd, _scaleAdd);
                    if (_scaleAdd <= _scaleBy && !_isRepeat)
                    {
                        NStop();
                    }
                }
                else
                {
                    _scaleAdd += _speed;
                    transform.localScale = _orgScale + new Vector3(_scaleAdd, _scaleAdd);
                    if (_scaleAdd >= _scaleBy && !_isRepeat)
                    {
                        NStop();
                    }
                }
                if (transform.localScale.x > _orgScale.x * (_scaleBy > 0 ? 1 + _scaleBy : 1 - _scaleBy))
                {
                    _zoomState = true;
                }
                else if (transform.localScale.x < _orgScale.x * (_scaleBy < 0 ? 1 + _scaleBy : 1 - _scaleBy))
                {
                    _zoomState = false;
                }
            }
        }
    }
}
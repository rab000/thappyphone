
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ntools
{
    /// <summary>
    /// 自身旋转动画（沿z轴，本地坐标）
    /// </summary>
    public class RotateAnimation : AnimationBase
    {
        /// <summary>
        /// 结束回调事件
        /// </summary>
        public event Listener OnFinishedEvent;
        /// <summary>
        /// 是否播放动画
        /// </summary>
        private bool _isAction = false;
        /// <summary>
        /// 是否重复旋转
        /// </summary>
        private bool _isRepeat;
        /// <summary>
        /// 速度
        /// </summary>
        private float _speed = 0.5f;
        /// <summary>
        /// 顺时针还是逆时针（默认false顺时针，true逆时针）
        /// </summary>
        private bool _actionState = false;
        /// <summary>
        /// 旋转角度
        /// </summary>
        private float _rotateAngleBy;
        /// <summary>
        /// 计算角度辅助变量
        /// </summary>
        private float _addValue;
        /// <summary>
        /// 原始角度
        /// </summary>
        private Vector3 _org;
        /// <summary>
        /// 回调
        /// </summary>
        private System.Action _callBack = null;
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="repeat">是否重复旋转</param>
        /// <param name="actionState">false顺时针；true逆时针</param>
        /// <param name="speed">速度</param>
        /// <param name="angle">旋转的角度</param>
        public void NStart(bool repeat = true, bool actionState = false, float speed = 1f, float angle = 360f, System.Action callback = null)
        {
            _callBack = callback;
            _org = this.transform.localEulerAngles;
            _isAction = true;
            _speed = speed;
            _actionState = actionState;
            _rotateAngleBy = angle;
            _isRepeat = repeat;
            _addValue = 0f;
        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="actionState">false顺时针；true逆时针</param>
        /// <param name="angle">旋转的角度</param>
        /// <param name="time">时间</param>
        public void FpsStart(bool actionState = false, float angle = 360f, float time = 2f)
        {
            this.NStart(false, actionState, angle / (Fps * time), angle);
        }
        /// <summary>
        /// 停止动画
        /// </summary>
        /// <param name="resetOrg">false不归位，true归位（回到原始状态）</param>
        public void NStop(bool resetOrg = false)
        {
            _isAction = false;
            if (resetOrg)
            {
                transform.localEulerAngles = _org;
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
            if (_isAction)
            {
                _addValue += _speed;
                if (_actionState)
                {
                    transform.Rotate(new Vector3(0, 0, 1f) * _speed);
                    if (!_isRepeat && _addValue >= _rotateAngleBy)
                    {
                        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, _org.z + _rotateAngleBy);
                        NStop();
                    }
                }
                else
                {
                    transform.Rotate(new Vector3(0, 0, -1f) * _speed);
                    if (!_isRepeat && _addValue >= _rotateAngleBy)
                    {
                        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, _org.z - _rotateAngleBy);
                        NStop();
                    }
                }
            }
        }
    }
}
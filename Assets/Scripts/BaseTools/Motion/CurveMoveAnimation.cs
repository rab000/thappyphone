
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ntools
{
    /// <summary>
    /// 曲线动画（针对世界坐标）
    /// </summary>
    public class CurveMoveAnimation : AnimationBase
    {
        /// <summary>
        /// 结束回调事件
        /// </summary>
        public event Listener OnFinishedEvent;
        /// <summary>
        /// 速度（每次移动的点数）
        /// </summary>
        private int _speed = 1;
        /// <summary>
        /// 动画进行中true；false动画停止
        /// </summary>
        private bool _isAction = false;
        /// <summary>
        /// 原始坐标
        /// </summary>
        private Vector3 _org;
        /// <summary>
        /// 移动到
        /// </summary>
        private Vector3 _moveTo;
        /// <summary>
        /// 移动到哪个点
        /// </summary>
        private int _index;
        /// <summary>
        /// 移动点
        /// </summary>
        private List<Vector3> _point;
        /// <summary>
        /// 每个点之间的点分个数
        /// </summary>
        private float _unitNum = 100;
        /// <summary>
        /// 回调
        /// </summary>
        private System.Action _callBack = null;
        private void calculate(List<Vector3> controlPoint = null)
        {
            for (int j = 0; j < controlPoint.Count - 1; j++)
            {
                for (int i = 0; i < _unitNum; i++)
                {
                    circle(controlPoint[j], controlPoint[j + 1], i / _unitNum);
                }
            }
        }
        private void circle(Vector3 start, Vector3 end, float controlNum)
        {
            _point.Add(Vector3.Lerp(start, end, controlNum));
        }
        private int calculateUnitNum(float time)
        {
            int totalFps = (int)Math.Ceiling(Fps * time);
            int everyPoint = totalFps;
            while (everyPoint % 2 == 0)
            {
                everyPoint = everyPoint / 2;
            }
            while (everyPoint % 3 == 0)
            {
                everyPoint = everyPoint / 3;
            }
            while (everyPoint % 5 == 0)
            {
                everyPoint = everyPoint / 5;
            }
            while (everyPoint % 7 == 0)
            {
                everyPoint = everyPoint / 7;
            }
            _unitNum = totalFps * everyPoint;
            return everyPoint;
        }
        /// <summary>
        /// 开始动画
        /// </summary>
        /// <param name="moveTo">移动到哪（世界坐标）</param>
        /// <param name="speed">移动所需时间</param>
        /// <param name="controlPoint">控制点（贝塞尔曲线控制点）</param>
        /// <param name="callback">回调</param>
        public void FpsStart(Vector3 moveTo, float time = 2f, List<Vector3> controlPoint = null, System.Action callback = null)
        {
            this.NStart(moveTo, calculateUnitNum(time), controlPoint, callback);
        }
        /// <summary>
        /// 开始动画
        /// </summary>
        /// <param name="moveTo">移动到哪（世界坐标）</param>
        /// <param name="speed">速度</param>
        /// <param name="controlPoint">控制点（贝塞尔曲线控制点）</param>
        /// <param name="callback">回调</param>
        public void NStart(Vector3 moveTo, int speed = 1, List<Vector3> controlPoint = null, System.Action callback = null)
        {
            _callBack = callback;
            _org = transform.position;
            _moveTo = moveTo;
            _speed = speed;
            _speed = Mathf.Min(10, speed);
            if (controlPoint == null)
            {
                controlPoint = new List<Vector3>();
            }
            controlPoint.Insert(0, _org);
            controlPoint.Add(_moveTo);
            _point = new List<Vector3>();
            calculate(controlPoint);
            _index = 0;
            _isAction = true;
        }
        /// <summary>
        /// 停止动画
        /// </summary>
        public void NStop()
        {
            _isAction = false;
            transform.position = _moveTo;
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
                if (_index < _point.Count - 1)
                {
                    transform.position = Vector3.Lerp(_point[_index], _point[_index + 1], 1);
                    _index += _speed;
                }
                else
                {
                    NStop();
                }
            }
        }
    }
}
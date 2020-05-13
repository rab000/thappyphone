
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ntools
{
    /// <summary>
    /// 弧线运动(针对世界坐标)
    /// </summary>
    public class ArcAnimation : AnimationBase
    {
        /// <summary>
        /// 动画结束回调事件
        /// </summary>
        public event Listener OnFinishedEvent;
        /// <summary>
        /// 速度（每次移动的点数）
        /// </summary>
        private int _speed = 1;
        /// <summary>
        /// 曲线弧度（数越小弧度越大）
        /// </summary>
        private float _arc;
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
        private void calculate()
        {
            Vector3 centor = (_org + _moveTo) * 0.5f;
            Vector3 centorProject = Vector3.Project(centor, _org - _moveTo);
            centor = Vector3.MoveTowards(centor, centorProject, _arc);
            for (int i = 0; i <= _unitNum; ++i)
            {
                Vector3 drawVec = Vector3.Slerp(_org - centor, _moveTo - centor, (1 / _unitNum) * i);
                drawVec += centor;
                _point.Add(drawVec);
            }
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
        /// <summary>
        /// 开始动画
        /// </summary>
        /// <param name="moveTo">移动到</param>
        /// <param name="speed">速度</param>
        /// <param name="arc">弧度（【0,1】）</param>
        /// <param name="callback">结束后的回调</param>
        public void NStart(Vector3 moveTo, int speed = 2, float arc = 0.5f, System.Action callback = null)
        {
            _callBack = callback;
            _org = transform.position;
            _moveTo = moveTo;
            _speed = speed;
            _arc = arc;
            _point = new List<Vector3>();
            calculate();
            _index = 0;
            _isAction = true;
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
        /// <param name="moveTo">移动到</param>
        /// <param name="time">经过多长时间</param>
        /// <param name="arc">弧度（【0,1】）</param>
        /// <param name="callback">结束后的回调</param>
        public void FpsStart(Vector3 moveTo, float time = 2f, float arc = 0.5f, System.Action callback = null)
        {
            this.NStart(moveTo, calculateUnitNum(time), arc, callback);
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
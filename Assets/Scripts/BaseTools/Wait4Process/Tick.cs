namespace ZLib
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using UnityEngine;

	public static class Tick
	{
		private static List<CallBackData> _cbdList = new List<CallBackData>();
		private static List<Action> _lateUpdateList = new List<Action>();
		private static float _realDeltaTime = 0f;
		private static float _realtimeSinceStartup = 0f;
		private static List<TweenFloatData> _tfdList = new List<TweenFloatData>();
		private static List<Action> _updateList = new List<Action>();
		public static bool useRealtimeSinceStartup = true;

		public static int AddCallback(Action __fun, float __interval = 0f, uint __repeat = 1)
		{
			CallBackData item = new CallBackData {
				fun = __fun,
				interval = __interval,
				repeat = __repeat,
				lastTime = realTime
			};
			_cbdList.Add(item);
			return item.id;
		}

		public static int AddDelayCallback(Action __fun, float __interval = 0f)
		{
			return AddCallback(__fun, __interval, 1);
		}

		public static int AddDelayCallback(float __interval, Action __fun)
		{
			return AddCallback(__fun, __interval, 1);
		}

		public static void AddLateUpdate(Action __fun)
		{
			if (!_lateUpdateList.Contains(__fun))
			{
				_lateUpdateList.Add(__fun);
			}
		}

		public static void AddUpdate(Action __fun)
		{
			if (!_updateList.Contains(__fun))
			{
				_updateList.Add(__fun);
			}
		}

		public static void KillTweenFloat(int __tweenID)
		{
			for (int i = _tfdList.Count - 1; i >= 0; i--)
			{
				TweenFloatData data = _tfdList[i];
				if (data.tweenID == __tweenID)
				{
					_tfdList.RemoveAt(i);
					break;
				}
			}
		}

		public static void LateUpdate()
		{
			if (_lateUpdateList.Count > 0)
			{
				Action[] actionArray = _lateUpdateList.ToArray();
				for (int i = 0; i < actionArray.Length; i++)
				{
					Action action = actionArray[i];
					action();
				}
				actionArray = null;
			}
		}

		public static void RemoveCallback(Action __fun)
		{
			for (int i = _cbdList.Count - 1; i >= 0; i--)
			{
				CallBackData data = _cbdList[i];
				if (data.fun == __fun)
				{
					_cbdList.RemoveAt(i);
				}
			}
		}

		public static void RemoveCallback(int __id)
		{
			for (int i = _cbdList.Count - 1; i >= 0; i--)
			{
				CallBackData data = _cbdList[i];
				if (data.id == __id)
				{
					_cbdList.RemoveAt(i);
					break;
				}
			}
		}

		public static void RemoveLateUpdate(Action __fun)
		{
			for (int i = _lateUpdateList.Count - 1; i >= 0; i--)
			{
				if (_lateUpdateList[i] == __fun)
				{
					_lateUpdateList.RemoveAt(i);
					break;
				}
			}
		}

		public static void RemoveUpdate(Action __fun)
		{
			for (int i = _updateList.Count - 1; i >= 0; i--)
			{
				if (_updateList[i] == __fun)
				{
					_updateList.RemoveAt(i);
					break;
				}
			}
		}

		public static int StartTweenFloat(float __from, float __to, float __duration, Action<float> __onUpdate = null, Action __onComplete = null, float __interval = 0f)
		{
			TweenFloatData item = new TweenFloatData {
				from = __from,
				to = __to,
				duration = __duration,
				onUpdate = __onUpdate,
				onComplete = __onComplete,
				interval = __interval,
				startTime = realTime,
				lastValue = __from,
				curValue = __from
			};
			_tfdList.Add(item);
			return item.tweenID;
		}

		public static void Update()
		{
			int num;
			_realDeltaTime = Time.realtimeSinceStartup - _realtimeSinceStartup;
			_realtimeSinceStartup = Time.realtimeSinceStartup;
			if (_updateList.Count > 0)
			{
				Action[] actionArray = _updateList.ToArray();
				for (num = 0; num < actionArray.Length; num++)
				{
					Action action = actionArray[num];
					action();
				}
				actionArray = null;
			}
			if (_cbdList.Count > 0)
			{
				CallBackData[] dataArray = _cbdList.ToArray();
				for (num = 0; num < dataArray.Length; num++)
				{
					CallBackData item = dataArray[num];
					if ((item.interval == 0f) || ((realTime - item.lastTime) >= item.interval))
					{
						item.curRepeat++;
						if ((item.repeat != 0) && (item.curRepeat >= item.repeat))
						{
							_cbdList.Remove(item);
						}
						else
						{
							item.lastTime = realTime;
						}
						item.fun();
					}
				}
				dataArray = null;
			}
			if (_tfdList.Count > 0)
			{
				TweenFloatData[] dataArray2 = _tfdList.ToArray();
				for (num = 0; num < dataArray2.Length; num++)
				{
					TweenFloatData data2 = dataArray2[num];
					if ((realTime - data2.startTime) >= data2.duration)
					{
						if (data2.onUpdate != null)
						{
							data2.onUpdate(data2.to);
						}
						if (data2.onComplete != null)
						{
							data2.onComplete();
						}
						_tfdList.Remove(data2);
					}
					else
					{
						data2.curValue = data2.from + (((realTime - data2.startTime) / data2.duration) * (data2.to - data2.from));
						if ((data2.interval == 0f) || ((data2.curValue - data2.lastValue) >= data2.interval))
						{
							data2.lastTime = realTime;
							data2.lastValue = data2.curValue;
							if (data2.onUpdate != null)
							{
								data2.onUpdate(data2.curValue);
							}
						}
					}
				}
				dataArray2 = null;
			}
		}

		public static float realDeltaTime
		{
			get
			{
				if (useRealtimeSinceStartup)
				{
					return _realDeltaTime;
				}
				return Time.deltaTime;
			}
		}

		public static float realTime
		{
			get
			{
				if (useRealtimeSinceStartup)
				{
					return Time.realtimeSinceStartup;
				}
				return Time.time;
			}
		}

		private class CallBackData
		{
			public uint curRepeat = 0;
			public Action fun;
			public int id = ID++;
			private static int ID;
			public float interval = 0f;
			public float lastTime = 0f;
			public uint repeat = 0;
		}

		private class TweenFloatData
		{
			public float curValue = 0f;
			public float duration;
			public float from;
			public float interval;
			public float lastTime = 0f;
			public float lastValue = 0f;
			public Action onComplete;
			public Action<float> onUpdate;
			public float startTime = 0f;
			public float to;
			public static int TWEEN_ID = 0;
			public int tweenID = ++TWEEN_ID;
		}
	}
}

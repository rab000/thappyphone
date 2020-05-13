namespace ZLib
{
	using System;
	using UnityEngine;

	public static class Utils
	{
		public static bool FloatEqual(float __a, float __b, int __precision)
		{
			return (((int) (__a * __precision)) == ((int) (__b * __precision)));
		}

		public static float Get360Degree(float __degree)
		{
			return (((__degree % 360f) + 360f) % 360f);
		}

		public static Vector3 Lerp(Vector3 __v1, Vector3 __v2, float __time, Action<float> __onUpdate, Action __onComplete)
		{
			Vector3 zero = Vector3.zero;
			if (__time < 1f)
			{
				if (__time < 0f)
				{
					__time = 0f;
				}
				zero = Vector3.Lerp(__v1, __v2, __time);
				if (__onUpdate != null)
				{
					__onUpdate(__time);
				}
				return zero;
			}
			if (__time >= 1f)
			{
				zero = __v2;
				if (__onComplete != null)
				{
					__onComplete();
				}
			}
			return zero;
		}
	}
}

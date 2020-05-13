namespace ZEngine
{
	using System;
	using System.Runtime.InteropServices;
	using UnityEngine;
	using ZLib;

	public static class EngineUtils
	{
		public static Material GetMaterial(Renderer __render)
		{
			return __render.sharedMaterial;
		}

		public static void SetMaterial(Renderer __render, Material __material)
		{
			__render.sharedMaterial = __material;
		}

		public static float XZDistance(Vector3 __a, Vector3 __b)
		{
			float num = __b.x - __a.x;
			float num2 = __b.z - __a.z;
			return Mathf.Sqrt((num * num) + (num2 * num2));
		}

		public static bool XZIsGridEqual(Vector3 __a, Vector3 __b, int __precision = 1)
		{
			return (Utils.FloatEqual(__a.x, __b.x, __precision) && Utils.FloatEqual(__a.z, __b.z, __precision));
		}

		public static bool XZIsInRadius(Vector3 __curvp, Vector3 __desvp, float __radius)
		{
			return ((__radius * __radius) >= XZSqrMagnitude(__curvp, __desvp));
		}

		public static bool XZIsInRadius(float __curpox, float __curpoy, float __despox, float __despoy, float __radius)
		{
			Vector3 vector = new Vector3(__curpox, 0f, __curpoy);
			Vector3 vector2 = new Vector3(__despox, 0f, __despoy);
			return XZIsInRadius(vector, vector2, __radius);
		}

		public static float XZSqrMagnitude(Vector3 __a, Vector3 __b)
		{
			float num = __b.x - __a.x;
			float num2 = __b.z - __a.z;
			return ((num * num) + (num2 * num2));
		}
	}
}

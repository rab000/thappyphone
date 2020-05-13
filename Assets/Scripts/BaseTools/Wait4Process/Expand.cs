using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class Expand
{
	public static List<T> Clone<T>(this List<T> __ts)
	{
		return __ts.GetRange(0, __ts.Count);
	}

	public static void ForEach<T>(this T[] __ts, Action<T> __action)
	{
		for (int i = 0; i < __ts.Length; i++)
		{
			__action(__ts[i]);
		}
	}

	public static TKey GetKey<TKey, TValue>(this Dictionary<TKey, TValue> __dictionary, TValue __value)
	{
		foreach (TKey local in __dictionary.Keys)
		{
			TValue local3 = __dictionary[local];
			if (local3.Equals(__value))
			{
				return local;
			}
		}
		return default(TKey);
	}

	public static int IndexOf<T>(this T[] __ts, T __t)
	{
		for (int i = 0; i < __ts.Length; i++)
		{
			if (__ts[i].Equals(__t))
			{
				return i;
			}
		}
		return -1;
	}

	public static bool IsInstance<T>(this object __obj)
	{
		return __obj.IsInstance(typeof(T));
	}

	public static bool IsInstance(this object __obj, Type __t)
	{
		return (__obj.GetType() == __t);
	}

	public static void SafeForEach<T>(this List<T> __ts, Action<T> __action)
	{
		T[] localArray = __ts.ToArray();
		for (int i = 0; i < localArray.Length; i++)
		{
			__action(localArray[i]);
		}
	}

	public static void SafeForEach<T>(this T[] __ts, Action<T> __action)
	{
		__ts.ForEach<T>(p => __action(p));
	}
}

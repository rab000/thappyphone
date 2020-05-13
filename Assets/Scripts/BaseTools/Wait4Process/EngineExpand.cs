using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public static class EngineExpand
{
	public static void EnableAllCollider(GameObject __go, bool __enable)
	{
		Collider[] componentsInChildren = __go.GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			collider.enabled = __enable;
		}
	}

	public static GameObject GetChildByName(this GameObject __go, string __name, bool __seachChildren = true)
	{
		Transform transform = __go.transform.GetChildByName(__name, __seachChildren);
		if (transform != null)
		{
			return transform.gameObject;
		}
		return null;
	}

	public static Transform GetChildByName(this Transform __ts, string __name, bool __seachChildren = true)
	{
		if (__ts.name == __name)
		{
			return __ts;
		}
		foreach (Transform transform in __ts)
		{
			if (__seachChildren)
			{
				Transform transform2 = transform.GetChildByName(__name, __seachChildren);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			else if (transform.name == __name)
			{
				return transform;
			}
		}
		return null;
	}

	public static GameObject[] GetChildren(this GameObject __go, bool __seachChildren = true)
	{
		List<GameObject> list = new List<GameObject>();
		__go.transform.GetChildren(__seachChildren).ForEach<Transform>(delegate (Transform p) {
			list.Add(p.gameObject);
		});
		return list.ToArray();
	}

	public static Transform[] GetChildren(this Transform __ts, bool __seachChildren = true)
	{
		if (__seachChildren)
		{
			return __ts.GetComponentsInChildren<Transform>();
		}
		List<Transform> list = new List<Transform> {
			__ts
		};
		foreach (Transform transform in __ts)
		{
			list.Add(transform);
		}
		return list.ToArray();
	}

	public static GameObject[] GetChildrenByName(this GameObject __go, string __name, bool __seachChildren = true)
	{
		List<GameObject> list = new List<GameObject>();
		__go.transform.GetChildrenByName(__name, __seachChildren).ForEach<Transform>(delegate (Transform p) {
			list.Add(p.gameObject);
		});
		return list.ToArray();
	}

	public static Transform[] GetChildrenByName(this Transform __ts, string __name, bool __seachChildren = true)
	{
		List<Transform> list = new List<Transform>();
		__ts.GetChildren(__seachChildren).ForEach<Transform>(delegate (Transform p) {
			if (p.name == __name)
			{
				list.Add(p);
			}
		});
		return list.ToArray();
	}

	public static GameObject[] GetChildrenTrueForAll(this GameObject __go, bool __seachChildren = true, Func<GameObject, bool> testFunction = null)
	{
		List<GameObject> list = new List<GameObject>();
		__go.transform.GetChildren(__seachChildren).ForEach<Transform>(delegate (Transform p) {
			if (testFunction != null)
			{
				GameObject gameObject = p.gameObject;
				if (testFunction(gameObject))
				{
					list.Add(p.gameObject);
				}
			}
			else
			{
				list.Add(p.gameObject);
			}
		});
		return list.ToArray();
	}

	public static T GetComponent<T>(this GameObject __go, bool __creat) where T: Component
	{
		T component = __go.GetComponent<T>();
		if ((component == null) && __creat)
		{
			component = __go.AddComponent<T>();
		}
		return component;
	}

	public static T GetComponent<T>(this Transform __ts, bool __creat) where T: Component
	{
		return __ts.gameObject.GetComponent<T>(__creat);
	}

	public static void RemoveAllCollider(this GameObject __go)
	{
		Collider[] componentsInChildren = __go.GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			UnityEngine.Object.Destroy(collider);
		}
	}

	public static void ReplaceShader(this Transform __ts, string __oldShaderName, Shader __newShader)
	{
		if (__ts != null)
		{
			if (__ts.gameObject.GetComponent<Renderer>() != null)
			{
				foreach (Material material in __ts.gameObject.GetComponent<Renderer>().materials)
				{
					if (material.shader.name == __oldShaderName)
					{
						material.shader = __newShader;
					}
				}
			}
			foreach (Transform transform in __ts)
			{
				transform.ReplaceShader(__oldShaderName, __newShader);
			}
		}
	}

	public static void ReplaceShader(this Transform __ts, Shader __oldShader, Shader __newShader)
	{
		if (__ts != null)
		{
			if (__ts.gameObject.GetComponent<Renderer>() != null)
			{
				foreach (Material material in __ts.gameObject.GetComponent<Renderer>().materials)
				{
					if (material.shader == __oldShader)
					{
						material.shader = __newShader;
					}
				}
			}
			foreach (Transform transform in __ts)
			{
				transform.ReplaceShader(__oldShader, __newShader);
			}
		}
	}

	public static void Reset(this Transform __ts)
	{
		__ts.localPosition = Vector3.zero;
		__ts.localRotation = Quaternion.identity;
		__ts.localScale = Vector3.one;
	}

	public static void SetAlpha(this Transform __ts, string __propertyName, float __alpha)
	{
		if (__ts != null)
		{
			if (__ts.gameObject.GetComponent<Renderer>() != null)
			{
				foreach (Material material in __ts.gameObject.GetComponent<Renderer>().materials)
				{
					if (material.HasProperty(__propertyName))
					{
						Color color = material.GetColor(__propertyName);
						material.SetColor(__propertyName, new Color(color.r, color.g, color.b, __alpha));
					}
				}
			}
			foreach (Transform transform in __ts)
			{
				transform.SetAlpha(__propertyName, __alpha);
			}
		}
	}

	public static void SetColor(this Transform __ts, string __propertyName, Color __color)
	{
		if (__ts != null)
		{
			if (__ts.gameObject.GetComponent<Renderer>() != null)
			{
				foreach (Material material in __ts.gameObject.GetComponent<Renderer>().materials)
				{
					material.SetColor(__propertyName, __color);
				}
			}
			foreach (Transform transform in __ts)
			{
				transform.SetColor(__propertyName, __color);
			}
		}
	}

	public static void SetParent(this GameObject __go, GameObject __parent)
	{
		__go.transform.SetParent(__parent.transform);
	}

	public static void SetParent(this Transform __ts, Transform __parent)
	{
		Vector3 position = __ts.position;
		Quaternion rotation = __ts.rotation;
		Vector3 lossyScale = __ts.lossyScale;
		__ts.parent = __parent;
		__ts.localScale = lossyScale;
		__ts.localPosition = position;
		__ts.localRotation = rotation;
	}

	public static void SetRender(this GameObject __go, bool __enable)
	{
		Renderer[] componentsInChildren = __go.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer renderer in componentsInChildren)
		{
			renderer.enabled = __enable;
		}
	}

	public static void SetShader(this Transform __ts, Shader __shader)
	{
		if (__ts != null)
		{
			if (__ts.gameObject.GetComponent<Renderer>() != null)
			{
				foreach (Material material in __ts.gameObject.GetComponent<Renderer>().materials)
				{
					material.shader = __shader;
				}
			}
			foreach (Transform transform in __ts)
			{
				transform.SetShader(__shader);
			}
		}
	}

	public static void SetTexture(this Transform __ts, string __propertyName, Texture __texture)
	{
		if (__ts != null)
		{
			if (__ts.gameObject.GetComponent<Renderer>() != null)
			{
				foreach (Material material in __ts.gameObject.GetComponent<Renderer>().materials)
				{
					material.SetTexture(__propertyName, __texture);
				}
			}
			foreach (Transform transform in __ts)
			{
				transform.SetTexture(__propertyName, __texture);
			}
		}
	}
}

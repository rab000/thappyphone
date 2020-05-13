using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScriptableHelper : MonoBehaviour {

	ScriptTemplate st;

	private void SaveToAsset()
	{
		#if UNITY_EDITOR
		st.i = 3;
		CreateOrReplaceAsset(st, "Assets/tfolder/t.asset");
		AssetDatabase.SaveAssets();
		#endif
	}

	private T CreateOrReplaceAsset<T>(T asset, string path) where T : ScriptableObject
	{
		#if UNITY_EDITOR
		T existingAsset = AssetDatabase.LoadAssetAtPath<T>(path);

		if (existingAsset == null)
		{
			AssetDatabase.CreateAsset(asset, path);
			existingAsset = asset;
		}
		else
		{
			EditorUtility.CopySerialized(asset, existingAsset);
		}

		return existingAsset;
		#else
		return null;
		#endif
	}
}

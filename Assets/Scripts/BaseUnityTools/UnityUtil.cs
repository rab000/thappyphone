using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnityUtil : MonoBehaviour
{
    #region unity platform

    public enum PlatformEnum
    {
        windows,
        mac,
        android,
        ios,
        linux,
    }

#if UNITY_STANDALONE_WIN
    public static PlatformEnum CurPlatform = PlatformEnum.windows;
#elif UNITY_IOS
    public static PlatformEnum CurPlatform = PlatformEnum.ios;
#elif UNITY_ANDROID
    public static PlatformEnum CurPlatform = PlatformEnum.android;
#elif UNITY_STANDALONE_OSX
    public static PlatformEnum CurPlatform = PlatformEnum.mac;
#elif UNITY_STANDALONE_LINUX
    public static PlatformEnum CurPlatform = PlatformEnum.linux;
#endif


    /// <summary>
	/// iPhone X 和 iPhone Xs 宽高比例一致,可使用同样适配方案
	/// </summary>
	/// <returns></returns>
	public static bool isIPhoneXOrXS()
    {
        return Camera.main.pixelWidth == 1125 && Camera.main.pixelHeight == 2436;
    }

    /// <summary>
    /// iPhone XR 和 iPhone XR Max 宽高比例一致,可使用同样适配方案
    /// </summary>
    /// <returns></returns>
    public static bool isIPhoneXROrXRMax()
    {
        return (Camera.main.pixelWidth == 828 && Camera.main.pixelHeight == 1792) ||
            (Camera.main.pixelWidth == 1242 && Camera.main.pixelHeight == 2688);
    }

    public static bool isIpad()
    {
        if (Camera.main.pixelWidth == 768 && Camera.main.pixelHeight == 1024)
        {
            return true;
        }

        if (Camera.main.pixelWidth == 1536 && Camera.main.pixelHeight == 2048)
        {
            return true;
        }

        if (Camera.main.pixelWidth == 2048 && Camera.main.pixelHeight == 2732)
        {
            return true;
        }

        if (Camera.main.pixelWidth == 1668 && Camera.main.pixelHeight == 2388)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region unity path

    public static string AssetPath
    {
        get
        {
            return Application.dataPath;
        }
    }

    public static string StreamingAssetsPath
    {
        get
        {
#if UNITY_ANDROID
                return Application.streamingAssetsPath;
#elif UNITY_IOS
                return "file:///" + Application.streamingAssetsPath;
#elif UNITY_EDITOR_OSX
                return "file:///" + Application.streamingAssetsPath;
#elif UNITY_EDITOR_WIN
            return "file:///" + Application.streamingAssetsPath;
#endif

        }
    }

    public static string PersistentPath
    {
        get
        {
            return Application.persistentDataPath;
        }
    }

    /// <summary>
    /// 切换到相对路径
    /// 比如绝对路径为D:/SVN/NEditor/trunk/Assets/NEDITOR/RoleEditor/Res/Female/AnimationClip/common/walk.anim
    /// 转换为相对路径为Assets/NEDITOR/RoleEditor/Res/Female/AnimationClip/common/walk.anim
    /// 
    /// AssetDatabase.LoadAssetAtPath要使用相对路径
    /// 
    /// </summary>
    /// <returns>The relative path.</returns>
    /// <param name="absolutePath">Absolute path.</param>
    public static string ChangeToRelativePath(string absolutePath)
    {
        string s = "Assets" + absolutePath.Substring(AssetPath.Length);
        return s;
    }

    /// <summary>
    /// 切换到绝对路径
    /// </summary>
    /// <returns>The absolute path.</returns>
    /// <param name="relativePath">Relative path.</param>
    public static string ChangeToAbsolutePath(string relativePath)
    {
        string s = AssetPath + relativePath.Substring("Assets".Length);
        return s;
    }

    #endregion


    /// <summary>
    /// 从对象身上获取此组件，没有则添加
    /// </summary>
    /// <typeparam name="T">要获取的组件类型</typeparam>
    /// <param name="obj">要获取此组件的对象</param>
    /// <returns>返回此组件</returns>
    public static T Get<T>(GameObject obj) where T : Component
    {
        T t = obj.GetComponent<T>();
        if (t == null)
        {
            t = obj.AddComponent<T>();
        }
        return t;
    }
    /// <summary>
    /// 创建对象
    /// </summary>
    /// <param name="name">对象名字</param>
    /// <param name="components">想添加的组件</param>
    /// <returns>创建后的对象</returns>
    public static GameObject CreateGameObject(string name, params Type[] components)
    {
        GameObject obj = new GameObject(name, components);
        return obj;
    }
    /// <summary>
    /// 加载对象并获取相应组件
    /// </summary>
    /// <typeparam name="T">想加载的类型</typeparam>
    /// <param name="objPath">相对于Resources的路径</param>
    /// <param name="parent">被加载对象放到哪个对象下</param>
    /// <returns></returns>
    public static T ResourcesLoad<T>(string objPath, Transform parent = null) where T : Component
    {
        GameObject m = Resources.Load<GameObject>(objPath);
        GameObject obj = UnityEngine.Object.Instantiate(m) as GameObject;
        T t = obj.GetComponent<T>();
        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        return t;
    }
    /// <summary>
    /// 加载对象并获取相应组件
    /// </summary>
    /// <typeparam name="T">想加载的类型</typeparam>
    /// <param name="objPath">相对于Resources的路径</param>
    /// <param name="parent">被加载对象放到哪个对象下</param>
    /// <returns></returns>
    public static T ResourcesUILoad<T>(string objPath, Transform parent = null) where T : Component
    {
        GameObject m = Resources.Load<GameObject>(objPath);
        GameObject obj = UnityEngine.Object.Instantiate(m) as GameObject;
        T t = obj.GetComponent<T>();
        obj.transform.SetParent(parent);
        obj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        obj.GetComponent<RectTransform>().localRotation = Quaternion.identity;
        obj.GetComponent<RectTransform>().localScale = Vector3.one;
        return t;
    }

}

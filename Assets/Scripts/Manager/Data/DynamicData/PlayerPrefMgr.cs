﻿using System;
using UnityEngine;
using System.Reflection;

/// <summary>
/// 本地数据管理类 
/// </summary>
public class PlayerPrefMgr
{

    #region value
    /// <summary>
    /// 储存Bool
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetString(key + "Bool", value.ToString());
    }

    /// <summary>
    /// 取Bool
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static bool GetBool(string key)
    {
        try
        {
            return bool.Parse(PlayerPrefs.GetString(key + "Bool"));
        }
        catch (Exception e)
        {
            return false;
        }

    }


    /// <summary>
    /// 储存String
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    /// <summary>
    /// 取String
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    /// <summary>
    /// 储存Float
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    /// <summary>
    /// 取Float
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    /// <summary>
    /// 储存Int
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }


    /// <summary>
    /// 取Int
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    #endregion

    #region struct
    /// <summary>
    /// 储存IntArray
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void SetIntArray(string key, int[] value)
    {

        for (int i = 0; i < value.Length; i++)
        {
            PlayerPrefs.SetInt(key + "IntArray" + i, value[i]);
        }
        PlayerPrefs.SetInt(key + "IntArray", value.Length);
    }

    /// <summary>
    /// 取IntArray
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static int[] GetIntArray(string key)
    {
        int[] intArr = new int[1];
        if (PlayerPrefs.GetInt(key + "IntArray") != 0)
        {
            intArr = new int[PlayerPrefs.GetInt(key + "IntArray")];
            for (int i = 0; i < intArr.Length; i++)
            {
                intArr[i] = PlayerPrefs.GetInt(key + "IntArray" + i);
            }
        }
        return intArr;
    }

    /// <summary>
    /// 储存FloatArray
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void SetFloatArray(string key, float[] value)
    {

        for (int i = 0; i < value.Length; i++)
        {
            PlayerPrefs.SetFloat(key + "FloatArray" + i, value[i]);
        }
        PlayerPrefs.SetInt(key + "FloatArray", value.Length);
    }

    /// <summary>
    /// 取FloatArray
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static float[] GetFloatArray(string key)
    {
        float[] floatArr = new float[1];
        if (PlayerPrefs.GetInt(key + "FloatArray") != 0)
        {
            floatArr = new float[PlayerPrefs.GetInt(key + "FloatArray")];
            for (int i = 0; i < floatArr.Length; i++)
            {
                floatArr[i] = PlayerPrefs.GetFloat(key + "FloatArray" + i);
            }
        }
        return floatArr;
    }


    /// <summary>
    /// 储存StringArray
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void SetStringArray(string key, string[] value)
    {

        for (int i = 0; i < value.Length; i++)
        {
            PlayerPrefs.SetString(key + "StringArray" + i, value[i]);
        }
        PlayerPrefs.SetInt(key + "StringArray", value.Length);
    }

    /// <summary>
    /// 取StringArray
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static string[] GetStringArray(string key)
    {
        string[] stringArr = new string[1];
        if (PlayerPrefs.GetInt(key + "StringArray") != 0)
        {
            stringArr = new string[PlayerPrefs.GetInt(key + "StringArray")];
            for (int i = 0; i < stringArr.Length; i++)
            {
                stringArr[i] = PlayerPrefs.GetString(key + "StringArray" + i);
            }
        }
        return stringArr;
    }


    /// <summary>
    /// 储存Vector2
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void SetVector2(string key, Vector2 value)
    {
        PlayerPrefs.SetFloat(key + "Vector2X", value.x);
        PlayerPrefs.SetFloat(key + "Vector2Y", value.y);

    }

    /// <summary>
    /// 取Vector2
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static Vector2 GetVector2(string key)
    {
        Vector2 Vector2;
        Vector2.x = PlayerPrefs.GetFloat(key + "Vector2X");
        Vector2.y = PlayerPrefs.GetFloat(key + "Vector2Y");
        return Vector2;
    }


    /// <summary>
    /// 储存Vector3
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void SetVector3(string key, Vector3 value)
    {
        PlayerPrefs.SetFloat(key + "Vector3X", value.x);
        PlayerPrefs.SetFloat(key + "Vector3Y", value.y);
        PlayerPrefs.SetFloat(key + "Vector3Z", value.z);
    }

    /// <summary>
    /// 取Vector3
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static Vector3 GetVector3(string key)
    {
        Vector3 vector3;
        vector3.x = PlayerPrefs.GetFloat(key + "Vector3X");
        vector3.y = PlayerPrefs.GetFloat(key + "Vector3Y");
        vector3.z = PlayerPrefs.GetFloat(key + "Vector3Z");
        return vector3;
    }


    /// <summary>
    /// 储存Quaternion
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public static void SetQuaternion(string key, Quaternion value)
    {
        PlayerPrefs.SetFloat(key + "QuaternionX", value.x);
        PlayerPrefs.SetFloat(key + "QuaternionY", value.y);
        PlayerPrefs.SetFloat(key + "QuaternionZ", value.z);
        PlayerPrefs.SetFloat(key + "QuaternionW", value.w);
    }

    /// <summary>
    /// 取Quaternion
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static Quaternion GetQuaternion(string key)
    {
        Quaternion quaternion;
        quaternion.x = PlayerPrefs.GetFloat(key + "QuaternionX");
        quaternion.y = PlayerPrefs.GetFloat(key + "QuaternionY");
        quaternion.z = PlayerPrefs.GetFloat(key + "QuaternionZ");
        quaternion.w = PlayerPrefs.GetFloat(key + "QuaternionW");
        return quaternion;
    }

    #endregion

    #region class

    /// <summary>
    /// 注意这里不能保存嵌套类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="o"></param>
    public static void SaveClass<T>(string name, T o)
    {
        Type t = o.GetType();
        FieldInfo[] fiedls = t.GetFields();
        for (int i = 0; i < fiedls.Length; i++)
        {
            string saveName = name + "." + fiedls[i].Name;
            if (fiedls[i].GetValue(o) != null)
            {
                switch (fiedls[i].FieldType.Name)
                {
                    case "String":
                        PlayerPrefs.SetString(saveName, fiedls[i].GetValue(o).ToString());
                        break;
                    case "Int32":
                    case "Int64":
                    case "Int":
                    case "uInt":
                        PlayerPrefs.SetInt(saveName, (int)fiedls[i].GetValue(o));
                        break;
                    case "Float":
                        PlayerPrefs.SetFloat(saveName, (float)fiedls[i].GetValue(o));
                        break;
                }
            }

        }
    }

    public static void ClearClass<T>(string name, T o)
    {
        Type t = o.GetType();
        FieldInfo[] fiedls = t.GetFields();
        for (int i = 0; i < fiedls.Length; i++)
        {
            string saveName = name + "." + fiedls[i].Name;
            switch (fiedls[i].FieldType.Name)
            {
                case "String":
                    PlayerPrefs.SetString(saveName, "");
                    break;
                case "Int32":
                case "Int64":
                case "Int":
                case "uInt":
                    PlayerPrefs.SetInt(saveName, 0);
                    break;
                case "Float":
                    PlayerPrefs.SetFloat(saveName, 0);
                    break;
            }
        }
    }

    public static T GetClassValue<T>(string name) where T : new()
    {
        T newObj = new T();

        Type t = newObj.GetType();
        FieldInfo[] fiedls = t.GetFields();
        for (int i = 0; i < fiedls.Length; i++)
        {
            string saveName = name + "." + fiedls[i].Name;
            switch (fiedls[i].FieldType.Name)
            {
                case "String":
                    fiedls[i].SetValue(newObj, PlayerPrefs.GetString(saveName));
                    break;
                case "Int32":
                case "Int64":
                case "Int":
                case "uInt":
                    fiedls[i].SetValue(newObj, PlayerPrefs.GetInt(saveName));
                    break;
                case "Float":
                    fiedls[i].SetValue(newObj, PlayerPrefs.GetFloat(saveName));
                    break;
            }
        }

        return newObj;
    }

    #endregion

}
using System;
using UnityEngine;
/// <summary>
/// 用于每次改变值立刻自动存储的变量 (eg:设置声音开关)
/// 要避免这种变量频繁修改，因为每次修改都会存储
/// </summary>
/// <typeparam name="T"></typeparam>
public class AutosaveProperty<T>
{
    public string ID;

    private T _value;
    public T Value
    {
        get
        {
            Type type = typeof(T);

            if (type.Equals(typeof(int)))
            {
                int i = PlayerPrefMgr.GetInt(ID);
                _value = (T)Convert.ChangeType(i, typeof(T));
            }
            else if (type.Equals(typeof(float)))
            {
                float f = PlayerPrefMgr.GetFloat(ID);
                _value = (T)Convert.ChangeType(f, typeof(T));
            }
            else if (type.Equals(typeof(bool)))
            {
                bool b = PlayerPrefMgr.GetBool(ID);
                _value = (T)Convert.ChangeType(b, typeof(T));
            }
            else if (type.Equals(typeof(string)))
            {
                string s = PlayerPrefMgr.GetString(ID);
                _value = (T)Convert.ChangeType(s, typeof(T));
            }
            else if (type.Equals(typeof(Vector3)))
            {
                string s = PlayerPrefMgr.GetString(ID);
                _value = (T)Convert.ChangeType(s, typeof(T));
            }
            return _value;
        }
        
    }
    public void Set(T v)
    {
        if (Equals(_value, v))
        {
            return;
        }

        if (v is int)
        {
            int i = (int)Convert.ChangeType(v, typeof(int));
            PlayerPrefMgr.SetInt(ID, i);
        }
        else if (v is float)
        {
            string s = (string)Convert.ChangeType(v, typeof(string));
            PlayerPrefMgr.SetString(ID, s);

        }
        else if (v is bool)
        {
            bool b = (bool)Convert.ChangeType(v, typeof(bool));
            PlayerPrefMgr.SetBool(ID, b);
        }
        else if (v is string)
        {
            string s = (string)Convert.ChangeType(v, typeof(string));
            PlayerPrefMgr.SetString(ID, s);
        }

    }

}

using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

/// <summary>
/// 这个类的功能已经转到PlayerPrefMgr种，可以在合适的时候丢弃掉
/// </summary>
public static class PlayerPrefsExtend
{
    public static void Save(string name, object o)
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

    public static void Clear(string name, object o)
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

    public static T GetValue<T>(string name) where T : new()
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
}

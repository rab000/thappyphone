using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用于从txt中导入一些配置信息
/// </summary>
public class TxtConfig
{
    public static Dictionary<string, string> Load(string txtContent, string fileRelativeRoot = "")
    {
        Dictionary<string, string> _data = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(txtContent))
        {
            Debug.LogError("Load Error!");
            return _data;
        }
        string[] line = txtContent.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        string[][] data = new string[line.Length][];
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i].StartsWith("#"))
            {
                continue;
            }
            data[i] = line[i].Split(new string[] { "=" }, System.StringSplitOptions.None);
        }
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == null)
            {
                continue;
            }
            
            _data.Add(data[i][0], data[i][1]);
        }
        return _data;
    }
}

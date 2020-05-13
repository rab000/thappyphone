using System.Collections.Generic;

public class StaticDataTable
{
    /// <summary>
    /// 存字段名和对应的列
    /// </summary>
    public Dictionary<string, int> FieldNameDic;

    /// <summary>
    /// 存id及对应的行号
    /// </summary>
    public Dictionary<string, int> IDDic;

    /// <summary>
    /// 存储excel数据表中所有数据
    /// </summary>
    public string[,] TableDataArray;

    /// <summary>
    /// 表名
    /// </summary>
    public string TableName;

    /// <summary>
    /// 行
    /// </summary>
    public int Row;

    /// <summary>
    /// 列
    /// </summary>
    public int Column;

    public StaticDataTable(string name)
    {
        TableName = name;        
        FieldNameDic = new Dictionary<string, int>();
        IDDic = new Dictionary<string, int>();
    }

}

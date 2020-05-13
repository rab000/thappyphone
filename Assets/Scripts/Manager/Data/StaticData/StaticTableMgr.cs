using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用于存储所有导入的配表
/// </summary>
public class StaticTableMgr
{
    static IoBuffer buffer = new IoBuffer(1024000);

    public static Dictionary<string, StaticDataTable> TableDic = new Dictionary<string, StaticDataTable>();

    public static void CreateTable(string tableName)
    {       
        if (!TableDic.ContainsKey(tableName))
        {
            StaticDataTable table = new StaticDataTable(tableName);
        }
    }

    public static void LoadDataTable(string tablePath)
    {
        byte[] bs = FileHelper.ReadBytesFromFile(tablePath);
       
        buffer.PutBytes(bs);

        string tableName = buffer.GetString();

        StaticDataTable dataTable = new StaticDataTable(tableName);

        if (TableDic.ContainsKey(tableName))
        {
            Debug.LogError("TableMgr.LoadDataTable table：" + tableName + "已存在，导入失败");
            return;
        }

        dataTable.Column = buffer.GetInt();

        dataTable.Row = buffer.GetInt();

        Debug.LogError("TableMgr.LoadDataTable tableName:" + tableName + " row:" + dataTable.Row + " colomn:" + dataTable.Column);

        dataTable.TableDataArray = new string[dataTable.Row, dataTable.Column];

        for (int i = 0; i < dataTable.Row; i++)
        {
            for (int j = 0; j < dataTable.Column; j++)
            {
                string fieldContent = buffer.GetString();

                dataTable.TableDataArray[i, j] = fieldContent;

                Debug.Log("TableMgr.LoadDataTable i:" + i + " j:" + j + " fieldContent:" + fieldContent);
            }
        }

        for (int i = 0; i < dataTable.Column; i++)
        {
            //Debug.LogError("nani-->i:"+i+" "+ dataTable.TableDataArray[0, i]);
            dataTable.FieldNameDic.Add(dataTable.TableDataArray[0,i], i);
        }

        for (int i = 0; i < dataTable.Row; i++)
        {
            dataTable.IDDic.Add(dataTable.TableDataArray[i, 0], i);
        }

        TableDic.Add(tableName, dataTable);

        buffer.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"> 数据id，每行第一个字段，也是主键</param>
    /// <param name="field"> 数据字段名,第一行每列的名称</param>
    public static string Get(string tableName, string id, string fieldName)
    {
        StaticDataTable table = TableDic[tableName];

        int column = table.FieldNameDic[fieldName];

        int row = table.IDDic[id];

        return TableDic[tableName].TableDataArray[row, column];
                       
    }

}

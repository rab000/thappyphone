using System.Collections.Generic;

///// <summary>
///// 资源信息表
///// 存储(来自xml的)资源信息，被资源信息总索引表使用
///// 包括资源id，依赖关系，资源名，asset名等，md5验证码
///// </summary>
public class ResInfoFromXml
{
    /// <summary>
    /// 资源id(资源相对资源根路径的相对地址)
    /// </summary>
    public string ResID;

    public int ResHashCode;

    //public ResEnum.RES_PACK_TYPE ResPackType;

    /// <summary>
    /// 依赖资源表
    /// </summary>
    public List<string> DependResIDList;
   
}
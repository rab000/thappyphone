/// <summary>
/// 可直接存储类
/// 嵌套类需要自行做deep copy
/// </summary>
/// <typeparam name="T"></typeparam>
public class SerializableClass<T> where T:new()
{
    protected string ID;

    public virtual void Save(T classData)
    {
        PlayerPrefMgr.SaveClass(ID, classData);
    }

    public virtual T Get(T t)
    {
        return PlayerPrefMgr.GetClassValue<T>(ID);
    }

}

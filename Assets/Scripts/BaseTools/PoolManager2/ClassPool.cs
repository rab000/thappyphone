using System.Collections.Generic;

public class ClassPool<T> where T :class,new()
{

    public int CurSize = 0;

    /// <summary>
    /// 最大保留数
    /// </summary>
    private int MaxSize = 100;

    private List<T> objectList = new List<T>();

    public void SetMaxSize(int maxSize)
    {
        MaxSize = maxSize;
    }

    public T Get()
    {
        if (objectList.Count > 0)
        {
            T temp = objectList[0];
            objectList.RemoveAt(0);
            return temp;
        }
        
        T t = new T();
        return t;

    }
    
    public void Put(T t)
    {
        if (CurSize >= MaxSize) return;
        objectList.Add(t);
        CurSize++;
    }

    /// <summary>
    /// 清空池对象
    /// </summary>
    public void Clear()
    {
        objectList.Clear();
        CurSize = 0;
    }

}

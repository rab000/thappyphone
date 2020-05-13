
using UnityEngine;

/// <summary>
/// 注意，放在prefab上，动态创建的脚本，不要继承这个类，容易创建出两个go，导致错误
/// 当脚本没创建过时，这个类会为脚本单独创建go并挂载脚本
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonBehaviour<T> : MonoBehaviour where T : Component
{

    private static readonly object m_Syslock = new object();

    private static T _Ins;

    public static T GetIns()
    {
        if (_Ins == null)
        {
            lock (m_Syslock)
            {
                _Ins = FindObjectOfType<T>();
                if (_Ins == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    go.hideFlags = HideFlags.HideAndDontSave;
                    _Ins = go.AddComponent<T>();
                }

                if (_Ins != null)
                {
                    DontDestroyOnLoad(_Ins.gameObject);
                }
            }

        }
        return _Ins;
    }
  
    protected virtual void Awake()
    {
        if (_Ins != null && _Ins != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _Ins = this as T;

        DontDestroyOnLoad(gameObject);
       
    }

    public void Dispose()
    {
        GameObject.Destroy(gameObject);
        _Ins = null;
    }
}


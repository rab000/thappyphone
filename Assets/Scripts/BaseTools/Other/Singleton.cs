using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new()
{
    //private static object Locker { get; } = new object();

    private static T _Ins;

    public static T GetIns()
    {
        if (_Ins == null)
        {
            //lock (Locker)
            //{
                if (_Ins == null)
                {
                    _Ins = new T();
                    _Ins.Init();
                }
            //}
        }
        return _Ins;
    }

    public virtual void Init() { }

}


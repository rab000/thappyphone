using UnityEngine;
using PathologicalGames;
public class PoolManagerDemo : MonoBehaviour
{

    SpawnPool spawnPool;

    PrefabPool refabPool;

    void Start()
    {
        //方法1 直接在场景物体上挂SpawnPool
        spawnPool = PoolManager.Pools["Shapes"];
        //方法2 直接代码新建池
        refabPool = new PrefabPool(Resources.Load<Transform>("momo"));
    }

    void OnGUI()
    {
        if (GUILayout.Button("初始化内存池"))
        {
            if (!spawnPool._perPrefabPoolOptions.Contains(refabPool))
            {
                refabPool = new PrefabPool(Resources.Load<Transform>("momo"));
                //默认初始化两个Prefab
                refabPool.preloadAmount = 2;
                //开启限制
                refabPool.limitInstances = true;
                //关闭无限取Prefab
                refabPool.limitFIFO = false;
                //限制池子里最大的Prefab数量
                refabPool.limitAmount = 5;
                //开启自动清理池子
                refabPool.cullDespawned = true;
                //最终保留
                refabPool.cullAbove = 10;
                //多久清理一次
                refabPool.cullDelay = 5;
                //每次清理几个
                refabPool.cullMaxPerPass = 5;
                //初始化内存池
                spawnPool._perPrefabPoolOptions.Add(refabPool);
                spawnPool.CreatePrefabPool(spawnPool._perPrefabPoolOptions[spawnPool.Count]);
            }
        }

        if (GUILayout.Button("从内存池里面取对象"))
        {
            ///从内存池里面取一个GameObjcet
            Transform momo = spawnPool.Spawn("momo");

            //存入
            //spawnPool.Despawn(momo);
        }


        if (GUILayout.Button("清空内存池"))
        {
            //清空池子
            spawnPool.DespawnAll();
        }
    }
}
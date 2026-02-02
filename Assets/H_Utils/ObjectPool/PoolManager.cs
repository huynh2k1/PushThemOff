using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager I;

    [SerializeField] Transform poolRoot;
    [SerializeField] int defautPoolSize = 10;

    Dictionary<PooledObject, ObjectPool> pools = new Dictionary<PooledObject, ObjectPool>();

    private void Awake()
    {
        I = this;

        if(poolRoot == null)
        {
            poolRoot = transform;
        }
    }

    public T Spawn<T>(T prefab, Vector3 pos, Quaternion rot) where T : PooledObject
    {
        if(!pools.TryGetValue(prefab, out ObjectPool pool))
        {
            pool = new ObjectPool(prefab, defautPoolSize, poolRoot);
            pools.Add(prefab, pool);
        }

        T obj = pool.Get() as T;
        obj.transform.SetPositionAndRotation(pos, rot);
        return obj;
    }

}

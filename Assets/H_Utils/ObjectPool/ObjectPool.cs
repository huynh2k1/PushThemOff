using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
public class ObjectPool
{
    Queue<PooledObject> objects = new Queue<PooledObject>();
    PooledObject prefab;
    Transform parent;

    public ObjectPool(PooledObject prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
            CreateNew();
    }

    void CreateNew()
    {
        var obj = GameObject.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        obj.OnRequestDespawn += ReturnToPool;   
        objects.Enqueue(obj);   //trả về giá trị của đối tượng vị trí đầu tiên nhưng không xóa khỏi queue
    }

    public PooledObject Get()
    {
        if (objects.Count == 0)
            CreateNew();

        var obj = objects.Dequeue(); //trả về giá trị của đối tượng vị trí đầu tiên và xóa khỏi queue
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
        return obj;
    }

    void ReturnToPool(PooledObject obj)
    {
        obj.OnDespawn();
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }
}

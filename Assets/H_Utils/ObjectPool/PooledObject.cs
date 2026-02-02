using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public event Action<PooledObject> OnRequestDespawn;

    protected void RequestDespawn()
    {
        OnRequestDespawn?.Invoke(this);
    }

    public virtual void OnSpawn() { }

    public virtual void OnDespawn() { }
}

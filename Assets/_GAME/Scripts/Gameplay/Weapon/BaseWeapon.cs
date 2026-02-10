using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public abstract class BaseWeapon : PooledObject
{
    protected WeaponData data;
    protected Transform owner;
    protected Vector3 direction;

    [SerializeField] ParticleSystem _trail;
    public static Action<float, float> OnWeaponHitAction;

    protected bool _isStart = false;

    public virtual void Init(WeaponData newData,Transform ownerTf, Vector3 dir)
    {
        data = newData;
        owner = ownerTf;
        direction = dir;
        _isStart = true;
    }

    protected virtual void OnHitEnemy(E1 enemy, Vector3 hitDir)
    {
        enemy.TakeDamage(data.Damage);
    }

    protected virtual void OnDestroyed()
    {
        _isStart = false;
        gameObject.SetActive(false);    
        RequestDespawn();
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        _trail?.gameObject.SetActive(true);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        _trail?.gameObject.SetActive(false);
    }
}

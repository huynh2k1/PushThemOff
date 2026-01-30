using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    protected WeaponData data;
    protected Transform owner;
    protected Vector3 direction;

    public static Action OnWeaponHitAction;

    public virtual void Init(WeaponData newData,Transform ownerTf, Vector3 dir)
    {
        data = newData;
        owner = ownerTf;
        direction = dir;
    }

    protected virtual void OnHitEnemy(Enemy enemy, Vector3 hitDir)
    {
        enemy.TakeDamage(data.Damage);
    }
}

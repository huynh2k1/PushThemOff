
using System;
using UnityEngine;

[Serializable]
public abstract class WeaponData : ScriptableObject
{
    [Header("Info")]
    public int Id;
    public string Name;
    public int Price;
    public Sprite Icon;


    [Header("Gameplay")]
    public float Speed;
    public int MaxDistance;
    public float Damage;
}

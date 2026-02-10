
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
    public float Damage;
    public int BonusHealth;
    public int MaxDistance;
    public string Skill;
    public float Speed;

    [Header("Effects")]
    public float shakeDuration;
    public float shakeMagnitude;
}

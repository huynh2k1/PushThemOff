using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledEffect : MonoBehaviour
{
    public EffectType Type { get; private set;}
    ParticleSystem ps;

    public static Action<PooledEffect> OnEffectCompleteAction;

    public void Init(EffectType type)
    {
        Type = type;
        ps = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        ps.Play();
        StartCoroutine(DisableWhenDone());
    }

    IEnumerator DisableWhenDone()
    {
        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        
        OnEffectCompleteAction?.Invoke(this);   
    }
}

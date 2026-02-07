using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    public event Action OnAttackAction;
    public event Action OnAttack2Action;
    public event Action OnEndAttackAction;

    public void HandleAttack()
    {
        OnAttackAction?.Invoke();
    }

    public void HandleAttack2()
    {
        OnAttack2Action?.Invoke();
    }

    public void HandleEndAttack()
    {
        OnEndAttackAction?.Invoke();
    }

}

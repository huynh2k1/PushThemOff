using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    public event Action OnEventAnimAction;

    public void HandleEvent()
    {
        OnEventAnimAction?.Invoke();
    }
}

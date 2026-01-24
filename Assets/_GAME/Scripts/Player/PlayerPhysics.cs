using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{

    public event Action<Collider> OnTriggerEnterAction;
    public event Action<Collider> OnTriggerExitAction;


    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterAction?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitAction?.Invoke(other); 
    }
}

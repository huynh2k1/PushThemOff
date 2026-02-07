using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{
    public int idDoor;

    public static Action<int> OnPlayMeetNPC;

    bool isMeet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isMeet)
                return;
            isMeet = true;  
            OnPlayMeetNPC?.Invoke(idDoor);
        }
    }
}

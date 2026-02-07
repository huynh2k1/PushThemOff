using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] int DoorID;    //0 = mở khi clear area 0

    [SerializeField] GameObject _model;
    [SerializeField] Collider _collider;


    private void OnEnable()
    {
        LevelCtrl.OnAreaClearedGlobal += HandleAreaCleared;
        npc.OnPlayMeetNPC += HandleAreaCleared;
    }

    private void OnDisable()
    {
        LevelCtrl.OnAreaClearedGlobal -= HandleAreaCleared;
        npc.OnPlayMeetNPC -= HandleAreaCleared;
    }

    void HandleAreaCleared(int areaIndex)
    {
        if(areaIndex == DoorID)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        _model.SetActive(false);
        _collider.enabled = false;
    }
}

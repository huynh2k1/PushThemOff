using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] int DoorID;    //0 = mở khi clear area 0

    private void OnEnable()
    {
        LevelCtrl.OnAreaClearedGlobal += HandleAreaCleared;
    }

    private void OnDisable()
    {
        LevelCtrl.OnAreaClearedGlobal -= HandleAreaCleared;
    }

    void HandleAreaCleared(int areaIndex)
    {
        if(areaIndex == DoorID)
        {

        }
    }

    void OpenDoor()
    {

    }
}

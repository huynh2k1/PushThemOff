using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using H_Utils;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseDoor : MonoBehaviour
{
    [SerializeField] protected int DoorID;    //0 = mở khi clear area 0
    [SerializeField] protected UnlockType type;
    
    [SerializeField] protected GameObject _virtualCam;
    [SerializeField] protected Collider _collider;

    protected virtual void OnEnable()
    {
        LevelCtrl.OnAreaClearedGlobal += HandleAreaCleared;
        npc.OnTalkEndAction += HandleKeyUnlock;

        InitDoor();
    }

    private void OnDisable()
    {
        LevelCtrl.OnAreaClearedGlobal -= HandleAreaCleared;
        npc.OnTalkEndAction -= HandleKeyUnlock;
    }

    void HandleAreaCleared(int areaIndex)
    {
        if (type == UnlockType.Key)
            return;
        if(areaIndex == DoorID)
        {
            OpenDoor();
        }
    }

    void HandleKeyUnlock(int index)
    {
        if (index == DoorID)
        {
            OpenDoor();
        }
    }

    protected abstract void InitDoor();
    protected abstract void OpenDoor();

    public enum UnlockType
    {
        ClearArea,
        Key,
    }
}

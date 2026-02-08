using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using H_Utils;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] int DoorID;    //0 = mở khi clear area 0
    [SerializeField] UnlockType type;
    [SerializeField] GameObject _model;
    [SerializeField] Collider _collider;
    [SerializeField] GameObject _virtualCam;

    private void OnEnable()
    {
        LevelCtrl.OnAreaClearedGlobal += HandleAreaCleared;
        npc.OnTalkEndAction += HandleKeyUnlock;
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

    void OpenDoor()
    {
        _virtualCam.SetActive(true);
        GameController.I.ChangeState(GameState.NONE);
        DOVirtual.DelayedCall(1f, () =>
        {
            _model.transform.DOKill();
            _model.transform.DOMoveY(-2, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _model.SetActive(false);
                _collider.enabled = false;
                _virtualCam.SetActive(false);
                DOVirtual.DelayedCall(1f, () =>
                {
                    GameController.I.GameResume();
                });
            });

        });
    }

    public enum UnlockType
    {
        ClearArea,
        Key,
    }
}

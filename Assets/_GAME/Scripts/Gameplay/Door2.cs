using DG.Tweening;
using H_Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2 : BaseDoor
{
    [SerializeField] Transform _leftDoor, _rightDoor;

    protected override void InitDoor()
    {
        _leftDoor.eulerAngles = Vector3.zero;   
        _rightDoor.eulerAngles = Vector3.zero;
    }

    protected override void OpenDoor()
    {
        _virtualCam.SetActive(true);
        GameController.I.ChangeState(GameState.NONE);
        DOVirtual.DelayedCall(1f, () =>
        {
            _leftDoor.DOKill();
            _rightDoor.DOKill();

            _leftDoor.DOLocalRotate(new Vector3(0, -90, 0), 1f).SetEase(Ease.Linear);
            _rightDoor.DOLocalRotate(new Vector3(0, 90, 0), 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    _virtualCam.SetActive(false);
                    GameController.I.GameResume();
                });
            });
        });
    }
}

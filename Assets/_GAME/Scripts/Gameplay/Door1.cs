using DG.Tweening;
using H_Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1 : BaseDoor
{
    [SerializeField] GameObject _model;

    protected override void InitDoor()
    {
        Vector3 pos = _model.transform.position;
        pos.y = 0;
        _model.transform.position = pos;

        _model.SetActive(true);
        _collider.enabled = true;
    }

    protected override void OpenDoor()
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
}

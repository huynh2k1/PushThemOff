using System;
using System.Collections;
using System.Collections.Generic;
using H_Utils;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : BasePopup
{
    public override UIType Type => UIType.PAUSE;

    [SerializeField] Button _btnHome;
    [SerializeField] Button _btnReplay;

    public static Action OnClickHomeAction;
    public static Action OnClickReplayAction;

    protected override void Awake()
    {
        base.Awake();
        _btnHome.onClick.AddListener(OnClickHome);
        _btnReplay.onClick.AddListener(OnClickReplay);
    }

    public override void OnClickClose()
    {
        Hide(() =>
        {
            GameController.I.GameResume();
        });

    }

    void OnClickHome()
    {
        Hide(() =>
        {
            OnClickHomeAction?.Invoke();
        });
    }

    void OnClickReplay()
    {
        Hide(() =>
        {
            OnClickReplayAction?.Invoke();
        });
    }
}

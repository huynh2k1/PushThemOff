using H_Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseUI : BasePopup
{
    public override UIType Type => UIType.LOSE;

    [SerializeField] Button _btnHome;
    [SerializeField] Button _btnReplay;

    public static Action OnClickHomeAction;
    public static Action<bool> OnClickReplayAction;

    protected override void Awake()
    {
        base.Awake();
        _btnHome.onClick.AddListener(OnClickHome);  
        _btnReplay.onClick.AddListener(OnClickReplay);
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
            OnClickReplayAction?.Invoke(false);
        });
    }
}

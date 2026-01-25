using System;
using System.Collections;
using System.Collections.Generic;
using H_Utils;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    public override UIType Type => UIType.HOME;

    [SerializeField] PanelCoin _panelCoin;
    [SerializeField] Button _btnPlay;
    [SerializeField] Button _btnSetting;
    [SerializeField] Button _btnShop;

    public static Action OnClickPlayAction;
    public static Action OnClickSettingAction;
    public static Action OnClickShopAction;

    private void Awake()
    {
        _btnPlay.onClick.AddListener(OnClickPlay);
        _btnSetting.onClick.AddListener(OnClickSetting);
        _btnShop.onClick.AddListener(OnClickShop);  
    }

    void OnClickPlay()
    {
        Hide();
        OnClickPlayAction();
    }

    void OnClickSetting()
    {
        OnClickSettingAction?.Invoke();
    }

    void OnClickShop()
    {
        OnClickShopAction?.Invoke();
    }
}

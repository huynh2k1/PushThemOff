using System;
using System.Collections;
using System.Collections.Generic;
using H_Utils;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    public override UIType Type => UIType.HOME;     
    [SerializeField] Button _btnPlay;

    public static Action OnClickPlayAction;
    public static Action OnClickHowToPlayAction;

    private void Awake()
    {
        _btnPlay.onClick.AddListener(OnClickPlay);
    }

    void OnClickPlay()
    {
        Hide();
        OnClickPlayAction();
    }

    void OnClickHowToPlay()
    {
        OnClickHowToPlayAction?.Invoke();
    }

}

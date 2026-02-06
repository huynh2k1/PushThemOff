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
    [SerializeField] Button _btnResume;

    public static Action OnClickHomeAction;
    public static Action OnClickResumeAction;

    protected override void Awake()
    {
        base.Awake();
        _btnHome.onClick.AddListener(OnClickHome);
        _btnResume.onClick.AddListener(OnClickResume);
    }

    void OnClickHome()
    {
        Hide(() =>
        {
            OnClickHomeAction?.Invoke();
        });
    }

    void OnClickResume()
    {
        Hide(() =>
        {
            OnClickResumeAction?.Invoke();
        });
    }
}

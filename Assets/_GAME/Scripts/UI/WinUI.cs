using H_Utils;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WinUI : BasePopup
{
    public override UIType Type => UIType.WIN;

    [SerializeField] Button _btnHome;
    [SerializeField] Button _btnReplay;
    [SerializeField] Button _btnNext;

    public static Action OnClickHomeAction;
    public static Action<bool> OnClickReplayAction;
    public static Action OnClickNextAction;

    protected override void Awake()
    {
        base.Awake();
        _btnHome.onClick.AddListener(OnClickHome);
        _btnReplay.onClick.AddListener(OnClickReplay);
        _btnNext.onClick.AddListener(OnClickNext);  
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
            OnClickReplayAction?.Invoke(true);
        });
    }

    void OnClickNext()
    {
        Hide(() =>
        {
            OnClickNextAction?.Invoke();
        });
    }
}

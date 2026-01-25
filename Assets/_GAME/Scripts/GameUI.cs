using System;
using System.Collections;
using System.Collections.Generic;
using H_Utils;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    public override UIType Type => UIType.GAME; 
    [SerializeField] Button _btnAttack;
    [SerializeField] Button _btnPause;

    public static Action OnClickAttackAction;
    public static Action OnClickPauseAction;

    private void Awake()
    {
        _btnAttack.onClick.AddListener(OnClickAttack);
        _btnPause.onClick.AddListener(OnClickPause);
    }

    void OnClickAttack()
    {

        OnClickAttackAction?.Invoke();
    }

    void OnClickPause()
    {
        OnClickPauseAction?.Invoke();   
    }
}

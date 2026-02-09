using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using H_Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : BaseUICtrl
{
    [SerializeField] DialogueUI _dialogueUI;
    [SerializeField] CanvasGroup _splash;

    [Header("Setting Splash")]
    [SerializeField] float _timeFade = 0.5f;
    [SerializeField] float _timeLoading = 1f;

    private void OnEnable()
    {
        HomeUI.OnClickSettingAction += Setting;
        HomeUI.OnClickShopAction += Shop;

        npc.OnPlayMeetNPC += ShowDialogue;
        Tutorial.OnTutorialPlay += ShowDialogue;
    }

    private void OnDestroy()
    {
        HomeUI.OnClickSettingAction -= Setting;
        HomeUI.OnClickShopAction -= Shop;

        npc.OnPlayMeetNPC -= ShowDialogue;
        Tutorial.OnTutorialPlay -= ShowDialogue;
    }

    public void OnInit()
    {
        Hide(UIType.HOME);
        Hide(UIType.GAME);
    }

    public void OnGameHome()
    {
        Show(UIType.HOME);
        Hide(UIType.GAME);
    }

    public void OnStartGame()
    {
        Show(UIType.GAME);
        Hide(UIType.HOME);
    }

    public void OnGameWin()
    {
        Show(UIType.WIN);
    }

    public void OnGameLose()
    {
        Show(UIType.LOSE);
    }

    public void OnGamePause()
    {
        Show(UIType.PAUSE);
    }

    public void Setting()
    {
        Show(UIType.SETTINGS);
    }

    public void Shop()
    {
        Show(UIType.SHOP);
    }

    public void ShowDialogue(DialogueData data)
    {
        Debug.Log("Show");
        _dialogueUI.Show(data);
    }

    public void LoadingSplash(Action action1 = default, Action action2 = default)
    {
        _splash.DOKill();

        _splash.interactable = true;

        _splash.DOFade(1, _timeFade).From(0).SetEase(Ease.Linear).OnComplete(() =>
        {
            action1?.Invoke();   
            _splash.DOFade(0, _timeFade).SetDelay(_timeLoading).SetEase(Ease.Linear).OnComplete(() =>
            {
                action2?.Invoke();
                _splash.interactable = false;
            });
        });
    }
}

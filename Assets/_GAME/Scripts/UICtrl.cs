using System.Collections;
using System.Collections.Generic;
using H_Utils;
using Unity.VisualScripting;
using UnityEngine;

public class UICtrl : BaseUICtrl
{
    private void OnEnable()
    {
        HomeUI.OnClickSettingAction += Setting;
        HomeUI.OnClickShopAction += Shop;

    }

    private void OnDestroy()
    {
        HomeUI.OnClickSettingAction -= Setting;
        HomeUI.OnClickShopAction -= Shop;
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
}

using System.Collections;
using System.Collections.Generic;
using H_Utils;
using Unity.VisualScripting;
using UnityEngine;

public class UICtrl : BaseUICtrl
{
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

    }

    public void OnGameLose()
    {

    }

    public void OnGamePause()
    {

    }
}

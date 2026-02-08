using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using H_Utils;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController I;
    [SerializeField] UICtrl _uiCtrl;
    [SerializeField] LevelCtrl _levelCtrl;
    [SerializeField] Tutorial _tutorial;

    public GameState CurState;
    private void Awake()
    {
        Application.targetFrameRate = 120;
        I = this;
    }

    private void OnEnable()
    {
        _levelCtrl.OnLevelCompletedEvent += GameWin;
        PlayerCtrl.OnPlayerDeadAction += GameLose;

        HomeUI.OnClickPlayAction += GameStart;

        GameUI.OnClickPauseAction += GamePause; 

        PauseUI.OnClickHomeAction += GameHome;
        PauseUI.OnClickResumeAction += GameResume;



        WinUI.OnClickHomeAction += GameHome;
        WinUI.OnClickReplayAction += GameReplay;
        WinUI.OnClickNextAction += GameStart;

        LoseUI.OnClickHomeAction += GameHome;
        LoseUI.OnClickReplayAction += GameReplay;

        Tutorial.OnTutorialEnd += GameHome;
    }

    private void OnDestroy()
    {
        _levelCtrl.OnLevelCompletedEvent -= GameWin;
        PlayerCtrl.OnPlayerDeadAction -= GameLose;

        HomeUI.OnClickPlayAction -= GameStart;

        GameUI.OnClickPauseAction -= GamePause;

        PauseUI.OnClickHomeAction -= GameHome;
        PauseUI.OnClickResumeAction -= GameResume;



        WinUI.OnClickHomeAction -= GameHome;
        WinUI.OnClickReplayAction -= GameReplay;
        WinUI.OnClickNextAction -= GameStart;

        LoseUI.OnClickHomeAction -= GameHome;
        LoseUI.OnClickReplayAction -= GameReplay;

        Tutorial.OnTutorialEnd -= GameHome;    
    }

    private void Start()
    {
        if (GameDatas.IsFirstPlayGame)
        {
            GameDatas.IsFirstPlayGame = false;
            _tutorial.Play();
            return;
        }
        GameHome();
    }

    public void ChangeState(GameState newState)
    {
        CurState = newState;
    }

    public void GameHome()
    {
        ChangeState(GameState.NONE);
        _uiCtrl.OnGameHome();
        _levelCtrl.ClearLevel();    
    }


    void SetUpGame()
    {
        _uiCtrl.OnStartGame();
        _levelCtrl.OnStartGame(GameDatas.CurrentLevel);
        ChangeState(GameState.PLAYING);
    }
    public void GameStart()
    {
        SetUpGame();
    }
    
    public void GamePause()
    {
        ChangeState(GameState.NONE);
        _uiCtrl.OnGamePause();
    }

    public void GameResume()
    {
        ChangeState(GameState.PLAYING);
    }

    public void GameReplay(bool isWin)
    {
        _levelCtrl.OnReplayGame(isWin);
        SetUpGame();
    }

    public void GameWin()
    {
        if (CurState != GameState.PLAYING)
            return;
        ChangeState(GameState.NONE);
        _levelCtrl.OnGameWin();
        _uiCtrl.Hide(UIType.GAME);
        DOVirtual.DelayedCall(1f, () =>
        {
            _uiCtrl.OnGameWin();
        });
    }

    public void GameLose()
    {
        if (CurState != GameState.PLAYING)
            return;
        ChangeState(GameState.NONE);
        _uiCtrl.Hide(UIType.GAME);
        DOVirtual.DelayedCall(1f, () =>
        {
            _uiCtrl.OnGameLose();
        });
    }

}

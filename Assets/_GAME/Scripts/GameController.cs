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

    public bool IsPlaying => CurState == GameState.PLAYING;

    private void Awake()
    {
        Application.targetFrameRate = 120;
        I = this;
        _uiCtrl.OnInit();
    }

    private void OnEnable()
    {
        _levelCtrl.OnLevelCompletedEvent += GameWin;
        PlayerCtrl.OnPlayerDeadAction += GameLose;

        HomeUI.OnClickPlayAction += GameStart;

        GameUI.OnClickPauseAction += GamePause; 

        PauseUI.OnClickHomeAction += BackToHome;
        PauseUI.OnClickResumeAction += GameResume;



        WinUI.OnClickHomeAction += GameHome;
        WinUI.OnClickReplayAction += GameReplay;
        WinUI.OnClickNextAction += GameStart;

        LoseUI.OnClickHomeAction += BackToHome;
        LoseUI.OnClickReplayAction += GameReplay;

        Tutorial.OnTutorialEnd += HandleEndTutorial;
    }

    private void OnDestroy()
    {
        _levelCtrl.OnLevelCompletedEvent -= GameWin;
        PlayerCtrl.OnPlayerDeadAction -= GameLose;

        HomeUI.OnClickPlayAction -= GameStart;

        GameUI.OnClickPauseAction -= GamePause;

        PauseUI.OnClickHomeAction -= BackToHome;
        PauseUI.OnClickResumeAction -= GameResume;



        WinUI.OnClickHomeAction -= GameHome;
        WinUI.OnClickReplayAction -= GameReplay;
        WinUI.OnClickNextAction -= GameStart;

        LoseUI.OnClickHomeAction -= BackToHome;
        LoseUI.OnClickReplayAction -= GameReplay;

        Tutorial.OnTutorialEnd -= HandleEndTutorial;    
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

    public void HandleEndTutorial()
    {
        _uiCtrl.LoadingSplash(() =>
        {
            SetUpGame();
        });
    }

    public void GameHome()
    {
        ChangeState(GameState.NONE);
        _uiCtrl.OnGameHome();
        _levelCtrl.ClearLevel();    
    }

    public void BackToHome()
    {
        _uiCtrl.LoadingSplash(() =>
        {
            GameHome();
        });
    }

    void SetUpGame()
    {
        _uiCtrl.OnStartGame();
        _levelCtrl.OnStartGame(GameDatas.CurrentLevel);
        ChangeState(GameState.PLAYING);
    }
    public void GameStart()
    {
        _uiCtrl.LoadingSplash(() => SetUpGame());
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

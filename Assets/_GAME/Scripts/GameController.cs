using System.Collections;
using System.Collections.Generic;
using H_Utils;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController I;
    [SerializeField] UICtrl _uiCtrl;
    [SerializeField] LevelCtrl _levelCtrl;

    public GameState CurState;
    private void Awake()
    {
        I = this;
    }

    private void OnEnable()
    {
        HomeUI.OnClickPlayAction += GameStart;

        GameUI.OnClickPauseAction += GamePause; 

        PauseUI.OnClickHomeAction += GameHome;
        PauseUI.OnClickReplayAction += GameReplay;
       
    }

    private void OnDestroy()
    {
        HomeUI.OnClickPlayAction -= GameStart;

        GameUI.OnClickPauseAction -= GamePause;
    }

    private void Start()
    {
        GameHome();
    }

    void ChangeState(GameState newState)
    {
        CurState = newState;
    }

    public void GameHome()
    {
        ChangeState(GameState.NONE);
        _uiCtrl.OnGameHome();
    }

    public void GameStart()
    {
        ChangeState(GameState.PLAYING);
        _uiCtrl.OnStartGame();
        _levelCtrl.OnStartGame();
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

    public void GameReplay()
    {
        ChangeState(GameState.PLAYING);
        _uiCtrl.OnStartGame();
    }

    public void GameWin()
    {

    }

    public void GameLose()
    {

    }

}

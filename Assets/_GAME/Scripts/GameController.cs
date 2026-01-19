using System.Collections;
using System.Collections.Generic;
using H_Utils;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController I;
    [SerializeField] UICtrl _uiCtrl;
    [SerializeField] CharacterCtrl _character;

    public GameState CurState;
    private void Awake()
    {
        I = this;
    }

    private void OnEnable()
    {
        HomeUI.OnClickPlayAction += GameStart;
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
        _character.OnGameHome();
    }

    public void GameStart()
    {
        ChangeState(GameState.PLAYING);
        _uiCtrl.OnStartGame();
        _character.OnStartGame();

    }
    
    public void GamePause()
    {

    }

    public void GameResume()
    {

    }

    public void GameWin()
    {

    }

    public void GameLose()
    {

    }

}

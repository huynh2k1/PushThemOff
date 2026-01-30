using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    [SerializeField] Transform _gameplayParent;
    [SerializeField] PlayerCtrl _player;

    public void OnStartGame()
    {
        InitPlayer();
    }

    public void InitPlayer()
    {
        _player.OnInitGame();
    }
}

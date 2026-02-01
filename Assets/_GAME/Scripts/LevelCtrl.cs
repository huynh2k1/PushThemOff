using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
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

    [Button("Save Data")]
    public void Save()
    {

    }

    [Button("Load Data")]
    public void LoadData()
    {

    }
}

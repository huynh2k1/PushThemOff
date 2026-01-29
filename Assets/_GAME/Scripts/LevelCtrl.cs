using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;

    public void OnStartGame()
    {
        InitPlayer();
    }

    public void InitPlayer()
    {
        GameObject obj = Instantiate(_playerPrefab, transform);
        obj.transform.position = Vector3.zero;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaContainer : MonoBehaviour
{
    public int AreaId;

    public event Action<AreaContainer> OnAreaCleared;

    int _aliveCount;

    List<BaseEnemy> _enemies = new();

    public void Init()
    {
        _enemies.Clear();

        _enemies.AddRange(GetComponentsInChildren<BaseEnemy>());

        _aliveCount = _enemies.Count;

        for (int i = 0; i < _enemies.Count; i++)
        {
            BaseEnemy e = _enemies[i];
            e.OnEnemyDeadAction += OnEnemyDead;
        }
    }

    void OnEnemyDead(BaseEnemy enemy)
    {
        enemy.OnEnemyDeadAction -= OnEnemyDead;

        _aliveCount--;

        if (_aliveCount <= 0)
        {
            OnAreaCleared?.Invoke(this);
        }
    }

    private void OnDisable()
    {
        // an toàn: tránh leak event
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] != null)
                _enemies[i].OnEnemyDeadAction -= OnEnemyDead;
        }
    }
}

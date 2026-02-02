using GameConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTextSpawner : MonoBehaviour
{
    public static PopupTextSpawner I;

    [SerializeField] PopupText popupTextPrefab;

    [SerializeField] PopupTextConfig coinConfig;
    [SerializeField] PopupTextConfig damageConfig;
    [SerializeField] PopupTextConfig healConfig;

    private void Awake()
    {
        I = this;
    }

    public void Spawn(PopupTextType type, Vector3 pos, int value)
    {
        var popup = PoolManager.I.Spawn(popupTextPrefab, pos, Quaternion.identity);

        popup.Init(pos, value, GetConfig(type));
    }

    PopupTextConfig GetConfig(PopupTextType type)
    {
        switch (type)
        {
            case PopupTextType.COIN: return coinConfig; 
            case PopupTextType.DAMAGE: return damageConfig;
            case PopupTextType.HEAL: return healConfig; 
        }

        return damageConfig;
    }
}

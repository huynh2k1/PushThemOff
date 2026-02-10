using H_Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWrapper : MonoBehaviour
{
    [SerializeField] GameObject[] weapons;

    private void Start()
    {
        ActiveWeaponUsed();
    }

    void ActiveWeaponUsed()
    {
        foreach(var w in weapons)
        {
            w.SetActive(false); 
        }

        weapons[GameDatas.CurWeapon].SetActive(true);
    }
}

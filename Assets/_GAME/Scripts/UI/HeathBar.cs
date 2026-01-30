using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeathBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TMP_Text _text;

    float curHP;
    float maxHP;
    Coroutine _lerpHPCoroutine;

    public void Init(float hp)
    {
        Show(true);
        curHP = hp; 
        maxHP = hp;
        UpdateUI(curHP, maxHP);
    }

    public void UpdateHealthBar(float currentHP)
    {
        curHP = currentHP;
        if(curHP <= 0)
        {
            Show(false);
            return;
        }
        UpdateUI(curHP, maxHP);
    }

    void UpdateUI(float cur, float max)
    {
        _slider.value = cur / max;
        UpdateText(cur, max);
    }

    void UpdateText(float cur, float max)
    {
        _text.text = $"{curHP}/{maxHP}";
    }

    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}

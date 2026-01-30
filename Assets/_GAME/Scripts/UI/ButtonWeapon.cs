using H_Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWeapon : MonoBehaviour
{
    public static ButtonWeapon Current;

    public int ID;
    [SerializeField] Button _btn;

    [SerializeField] Image _iconWrapon;
    [SerializeField] GameObject _hover;

    public static Action<int> OnClickThisAction;

    private void Awake()
    {
        _btn.onClick.AddListener(OnClickThis);

        Hover(false);
        if(ID == GameDatas.CurWeapon)
        {
            Hover(true);
            Current = this;
        }
    }

    void OnClickThis()
    {
        if(Current != null)
        {
            Current.Hover(false);
        }

        Hover(true);
        Current = this;
        OnClickThisAction?.Invoke(ID);
    }

    public void Hover(bool isShow) => _hover.SetActive(isShow);
}

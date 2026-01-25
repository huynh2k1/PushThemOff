using System;
using System.Collections;
using System.Collections.Generic;
using H_Utils;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    [SerializeField] Button _btnBye;
    [SerializeField] Button _btnEquip;
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _titleText;
    [SerializeField] TMP_Text _priceText;

    public event Action<int> OnBuySuccessAction;
    public event Action OnBuyFailAction;


    //Element Datas
    int ID;
    int _price;

    private void Awake()
    {
        _btnBye.onClick.AddListener(OnClickBuy);
    }

    void OnClickBuy()
    {
        bool canBuy = GameDatas.Coin >= _price ? true : false;

        if (canBuy)
        {
            GameDatas.Coin -= _price;
            OnBuySuccessAction?.Invoke(ID);
        }
        else
        {
            OnBuyFailAction?.Invoke();
        }
    }

    void OnClickEquip()
    {

    }

    public void LoadData(int id, int price, Sprite newIcon, string title)
    {
        ID = id;

        SetPrice(price);
        SetTitle(title);
        SetIcon(newIcon);
    }


    void SetIcon(Sprite newIcon)
    {
        _icon.sprite = newIcon;
        _icon.SetNativeSize();
    }

    void SetTitle(string title)
    {
        _titleText.text = title.ToString();
    }

    void SetPrice(int price)
    {
        _price = price;
        _priceText.text = price.ToString();
    }

    void CheckUnlocked()
    {
        if (GameDatas.GetWeaponUnlock(ID))
        {
            Unlock();
        }
    }

    void Unlock()
    {

    }

    void Lock()
    {

    }

    void Equip()
    {

    }

    void ShowBtnBuy(bool isShow) => _btnBye.gameObject.SetActive(isShow);   
    void ShowBtnEquip(bool isShow) => _btnEquip.gameObject.SetActive(isShow);
}

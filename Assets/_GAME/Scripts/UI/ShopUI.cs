using System.Collections;
using System.Collections.Generic;
using H_Utils;
using UnityEngine;

public class ShopUI : BasePopup
{
    public override UIType Type => UIType.SHOP;

    [SerializeField] PanelCoin _panelCoin;
    [SerializeField] Transform _contentParent;
    [SerializeField] ShopElement _shopElementPrefab;

    List<ShopElement> _listElement;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public override void Show()
    {
        base.Show();
        _panelCoin.UpdateText(GameDatas.Coin);
    }

    void Init()
    {
        int count = 5;

        _listElement = new List<ShopElement>();

        for(int i = 0; i < count; i++)
        {
            ShopElement s = Instantiate(_shopElementPrefab, _contentParent);
            _listElement.Add(s);
        }
    }
}

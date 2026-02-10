using System.Collections;
using System.Collections.Generic;
using H_Utils;
using TMPro;
using UnityEngine;

public class ShopUI : BasePopup
{
    public override UIType Type => UIType.SHOP;

    [SerializeField] SwipeMenu _swipeMenu;
    [SerializeField] PanelCoin _panelCoin;
    [SerializeField] Transform _contentParent;
    [SerializeField] ShopElement _shopElementPrefab;

    [SerializeField]
    WeaponData[] _listWeaponData;

    List<ShopElement> _listElement;

    [Header("Stat Weapons")]
    [SerializeField] TMP_Text txtName;
    [SerializeField] TMP_Text txtHealth;
    [SerializeField] TMP_Text txtDamage;
    [SerializeField] TMP_Text txtRange;
    [SerializeField] TMP_Text txtBonusSkill;

    protected override void Awake()
    {
        base.Awake();
        _swipeMenu.OnPageChanged += HandleOnPageChangedEvent;
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        _swipeMenu.OnPageChanged -= HandleOnPageChangedEvent;
    }

    public override void Show()
    {
        base.Show();
        _panelCoin.UpdateText(GameDatas.Coin);
    }

    void Init()
    {
        int count = _listWeaponData.Length;

        _listElement = new List<ShopElement>();

        for(int i = 0; i < count; i++)
        {
            ShopElement s = Instantiate(_shopElementPrefab, _contentParent);

            WeaponData data = _listWeaponData[i];   
            s.LoadData(i, data.Price, data.Icon, data.Name);
            _listElement.Add(s);

            s.OnEquipAction += ReloadUI;
            s.OnBuySuccessAction += (id) =>
            {
                _panelCoin.UpdateText(GameDatas.Coin);
            };
        }

        _swipeMenu.Init(_listElement);

    }

    public void ReloadUI()
    {
        foreach(var e in _listElement)
        {
            e.ReloadUI();
        }
    }

    void HandleOnPageChangedEvent(int id)
    {
        WeaponData data = _listWeaponData[id - 1];
        UpdateStatsWeapon(data.Name, data.BonusHealth, (int)data.Damage, data.MaxDistance, data.Skill);
    }

    void UpdateStatsWeapon(string NameWeapon, int Health, int Damage, int Range, string Skill)
    {
        txtName.text = NameWeapon;
        txtDamage.text = $"- Damage: {Damage}";
        txtHealth.text = $"- Health: {Health}";  
        txtRange.text = $"- Range: {Range}";
        txtBonusSkill.text = "- Skill: " + Skill.ToString();
    }
}

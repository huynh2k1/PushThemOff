using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWeapon : MonoBehaviour
{
    public int ID { get; set; }
    [SerializeField] Button _btn;

    [SerializeField] Image _iconWrapon;
    [SerializeField] GameObject _hover;

    public event Action<int> OnClickThisAction;

    private void Awake()
    {
        _btn.onClick.AddListener(OnClickThis);
    }

    void OnClickThis()
    {
        OnClickThisAction?.Invoke(ID);
    }


}

using H_Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseUICtrl : MonoBehaviour
{
    public BaseUI[] _uis;
    protected Dictionary<UIType, BaseUI> _dictUIs = new Dictionary<UIType, BaseUI>();

    protected virtual void Awake()
    {
        foreach (BaseUI ui in _uis)
        {
            if (_dictUIs.ContainsKey(ui.Type) == false)
            {
                _dictUIs.Add(ui.Type, ui);
            }
        }
    }

    public void Show(UIType type)
    {
        if (_dictUIs.ContainsKey(type) == false)
        {
            return;
        }

        _dictUIs[type].Show();
    }

    public void Hide(UIType type)
    {
        if (_dictUIs.ContainsKey(type) == false)
        {
            return;
        }

        _dictUIs[type].Hide();
    }
}

using DG.Tweening;
using H_Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundElement : MonoBehaviour
{
    [SerializeField] SoundType _type;
    [SerializeField] Button _btn;
    [SerializeField] RectTransform _handle;
    [SerializeField] float _timeTween = 0.2f;

    [SerializeField] GameObject _onTxtObj;
    [SerializeField] GameObject _offTxtObj;

    bool _isOn;

    float _handlePosX_On = 50f;

    private void Awake()
    {
        _btn.onClick.AddListener(OnClickThis);
    }

    private void OnEnable()
    {
        LoadData();
    }

    public void OnClickThis()
    {
        _isOn = !_isOn;
        UpdateUI(_isOn);
        ShowTextOnOff(_isOn);
        Save();

    }

    public void LoadData()
    {
        switch (_type)
        {
            case SoundType.MUSIC:
                _isOn = GameDatas.IsMusicOn;
                break;
            case SoundType.SOUND:
                _isOn = GameDatas.IsSoundOn;
                break;
            case SoundType.VIBRATION:
                _isOn = GameDatas.IsVibrationOn;    
                break;
        }

        ShowTextOnOff(_isOn);
        _handle.anchoredPosition = _isOn ? new Vector2(_handlePosX_On, _handle.anchoredPosition.y) : new Vector2(-_handlePosX_On, _handle.anchoredPosition.y);
    }

    public void Save()
    {
        switch (_type)
        {
            case SoundType.SOUND:
                GameDatas.IsSoundOn = _isOn;
                break;
            case SoundType.MUSIC:
                GameDatas.IsMusicOn = _isOn;
                break;
            case SoundType.VIBRATION:
                GameDatas.IsVibrationOn = _isOn;
                break;
        }
    }

    public void UpdateUI(bool IsOn)
    {
        _handle.DOKill();
        _handle.DOAnchorPosX(IsOn ? _handlePosX_On : -_handlePosX_On, _timeTween).SetEase(Ease.Linear);
    }

    void ShowTextOnOff(bool isOn)
    {
        _onTxtObj.SetActive(isOn);
        _offTxtObj.SetActive(!isOn);    
    }
}

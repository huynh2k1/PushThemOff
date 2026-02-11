using H_Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : BasePopup
{
    public override UIType Type => UIType.LEVEL_SELECT;
    [SerializeField] Button _btnPlay;

    [SerializeField] LevelMenu _levelMenu;
    [SerializeField]
    LevelElement[] _levelElements;

    [SerializeField] TMP_Text _txtChapter;

    public static Action OnClickPlayAction;

    public int idLevel;

    protected override void Awake()
    {
        base.Awake();
        _btnPlay.onClick.AddListener(OnClickPlay);
        _levelMenu.OnPageChanged += HandleOnPageChange;
        _levelMenu.Init();
        Init();
    }

    private void OnDestroy()
    {
        _levelMenu.OnPageChanged -= HandleOnPageChange;
    }

    void Init()
    {
        for(int i = 0; i < _levelElements.Length; i++)
        {
            _levelElements[i].Init(i);
        }
    }

    void HandleOnPageChange(int id)
    {
        UpdateTxtChapter(id);
        idLevel = id - 1;
        if(GameDatas.LevelUnlock >= (id - 1))
        {
            _btnPlay.gameObject.SetActive(true);
        }else
        {
            _btnPlay.gameObject.SetActive(false);
        }
    }

    void OnClickPlay()
    {
        GameDatas.CurrentLevel = idLevel;
        OnClickPlayAction?.Invoke();
    }

    void UpdateTxtChapter(int chapter)
    {
        _txtChapter.text = $"CHAPTER - {chapter}";
    }
}

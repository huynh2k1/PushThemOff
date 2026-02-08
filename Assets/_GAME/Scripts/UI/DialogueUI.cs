using System;
using System.Collections;
using System.Collections.Generic;
using H_Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : BasePopup
{
    public override UIType Type => UIType.DIALOGUE;

    [SerializeField] Image _characterUI;
    [SerializeField] TMP_Text _contentText;

    [SerializeField] Button _btnNext;
    [SerializeField] Button _btnPrev;
    [SerializeField] Button _btnSkip;

    public static Action OnEndShowAction;

    DialogueData current;

    int index;
    protected override void Awake()
    {
        base.Awake();
        _btnNext.onClick.AddListener(Next);
        _btnSkip.onClick.AddListener(Skip);
    }

    public void Show(DialogueData data)
    {
        current = data;
        index = 0;

        Show();

        ShowLine();
    }

    public void Next()
    {
        index++;

        if(index >= current.listData.Count)
        {
            End();
            return;
        }

        ShowLine();
    }

    public void Previous()
    {
        if (index <= 0)
            return;
        index--;
        ShowLine();
    }
   
    void ShowLine()
    {
        var data = current.listData[index];

        _characterUI.sprite = data.characterUI;
        _characterUI.SetNativeSize();
        _contentText.text = data.content;
    }

    void Skip()
    {
        End();
    }

    void End()
    {
        Hide(() =>
        {
            OnEndShowAction?.Invoke();
        });
    }
}

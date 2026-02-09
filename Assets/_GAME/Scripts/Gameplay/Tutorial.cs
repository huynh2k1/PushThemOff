using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Action<DialogueData> OnTutorialPlay;
    public static Action OnTutorialEnd;

    [SerializeField] CinemachineBrain _blendCam;
    [SerializeField] GameObject _virtualCam;

    [SerializeField] DialogueData _data;

    private void OnEnable()
    {
        DialogueUI.OnEndShowAction += HandleTutorialEnd;
    }

    private void OnDisable()
    {
        DialogueUI.OnEndShowAction -= HandleTutorialEnd;
    }

    public void Play()
    {
        _blendCam.m_DefaultBlend.m_Time = 2f;
        gameObject.SetActive(true);
        _virtualCam.SetActive(true);

        DOVirtual.DelayedCall(2f, () =>
        {
            OnTutorialPlay?.Invoke(_data);
        });
    }

    void HandleTutorialEnd()
    {
        _blendCam.m_DefaultBlend.m_Time = 1f;
        _virtualCam?.SetActive(false);
        OnTutorialEnd?.Invoke();
        DOVirtual.DelayedCall(1f, () =>
        {
            gameObject.SetActive(false);
        });
    }
}

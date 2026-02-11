using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBattleTrigger : MonoBehaviour
{

    [SerializeField] BoxCollider _boxCollider;
    [SerializeField] DialogueData _dialogueData;

    public static Action<DialogueData> OnPlayerTriggerAction;
    public static Action OnTalkEndAction;

    private void OnEnable()
    {
        _boxCollider.enabled = true;

        DialogueUI.OnEndShowAction += HandleTalkEndAction;  
    }

    private void OnDisable()
    {
        DialogueUI.OnEndShowAction -= HandleTalkEndAction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _boxCollider.enabled = false;
            OnPlayerTriggerAction?.Invoke(_dialogueData);
            GameController.I.ChangeState(H_Utils.GameState.NONE);
        }
    }

    void HandleTalkEndAction()
    {
        OnTalkEndAction?.Invoke();  
        GameController.I.GameResume();
    }
}

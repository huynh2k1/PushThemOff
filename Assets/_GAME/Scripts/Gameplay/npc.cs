using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{
    public int idDoor;

    public static Action<DialogueData> OnPlayMeetNPC;
    public static Action<int> OnTalkEndAction;
    [SerializeField] DialogueData data;
    bool isMeet = false;

    private void OnEnable()
    {
        DialogueUI.OnEndShowAction += HandleTalkEndAction;
        if(isMeet)
            isMeet = false;
    }

    private void OnDisable()
    {
        DialogueUI.OnEndShowAction -= HandleTalkEndAction;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isMeet)
                return;
            isMeet = true;

            Debug.Log("Meet Player");
            GameController.I.ChangeState(H_Utils.GameState.NONE);
            OnPlayMeetNPC?.Invoke(data);
        }
    }

    void HandleTalkEndAction()
    {
        OnTalkEndAction?.Invoke(idDoor);
        GameController.I.GameResume();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> listData;
}

[System.Serializable]
public class DialogueLine
{
    public Sprite characterUI;     // NPC, Boss, Player...
    [TextArea(2, 5)]
    public string content;
}
using GameConfig;
using UnityEngine;

[CreateAssetMenu(menuName = "GameConfig/PopupTextConfig", fileName = "PopupTextConfig")]
public class PopupTextConfig : ScriptableObject
{
    public PopupTextType type;
    public Color textColor = Color.white;
}

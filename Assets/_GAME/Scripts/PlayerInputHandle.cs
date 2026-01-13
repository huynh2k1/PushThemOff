using System;
using UnityEngine;

public class PlayerInputHandle : MonoBehaviour, IPlayerInput
{
    public Vector2 MoveInput { get; private set; }
    public bool AttackPressed { get; private set; }
    [Header("Joystick (Mobile)")]
    [SerializeField] private Joystick joystick;

    private void Update()
    {
        if (joystick != null)
        {
            MoveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        else
        {
            MoveInput = Vector2.zero;
        }

        AttackPressed = Input.GetKeyDown(KeyCode.Space);
    }

}

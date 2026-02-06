using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickCtrl : MonoBehaviour
{
    public static Action<Vector2> OnJoystickMove;

    [SerializeField] Joystick _joystick;
    Vector2 direction;

    private void Update()
    {
        if (GameController.I.CurState != H_Utils.GameState.PLAYING)
        {
            if(direction != Vector2.zero)
            {
                direction = Vector2.zero;
                OnJoystickMove?.Invoke(direction);
            }
            return;
        }
        direction = new Vector2(-_joystick.Horizontal, -_joystick.Vertical);
        direction.Normalize();
        OnJoystickMove?.Invoke(direction);
    }

    //private void FixedUpdate()
    //{
    //    JoystickMove(direction);
    //}

    //public void JoystickMove(Vector2 direction)
    //{
    //    direction.Normalize();
    //    OnJoystickMove?.Invoke(direction);
    //}
}

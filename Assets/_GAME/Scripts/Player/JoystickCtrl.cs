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

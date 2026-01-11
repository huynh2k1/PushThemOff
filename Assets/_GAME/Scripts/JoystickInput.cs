using System;
using UnityEngine;

public class JoystickInput : MonoBehaviour
{
    public static JoystickInput Instance;

    [Header("Joystick (Mobile)")]
    [SerializeField] private Joystick joystick;

    [Header("Keyboard Fallback (Editor / PC)")]
    [SerializeField] private bool useKeyboardInEditor = true;

    public Vector2 Direction { get; private set; }

    public static Action<Vector2> OnInputChanged;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        ReadInput();

        // Chỉ bắn event khi input thay đổi
        if (Direction != Vector2.zero)
        {
            OnInputChanged?.Invoke(Direction);
        }
    }

    private void ReadInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (useKeyboardInEditor)
        {
            Direction = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );
            return;
        }
#endif

        if (joystick != null)
        {
            Direction = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        else
        {
            Direction = Vector2.zero;
        }
    }
}

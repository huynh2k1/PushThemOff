using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] Transform _body;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 10f;

    private Vector2 _input;

    private void OnEnable()
    {
        JoystickInput.OnInputChanged += Move;
    }

    private void OnDisable()
    {
        JoystickInput.OnInputChanged -= Move;
    }

    private void Update()
    {
        Rotate();
    }

    private void Move(Vector2 input)
    {
        _input = input;

        Vector3 moveDir = new Vector3(_input.x, 0, _input.y);

        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
    }

    private void Rotate()
    {
        Vector3 lookDir;

        // Nếu có target thì nhìn về target
        if (_target != null)
        {
            lookDir = _target.position - _body.position;
        }
        else
        {
            // Nếu không có target thì nhìn theo hướng di chuyển
            lookDir = new Vector3(_input.x, 0, _input.y);
        }

        if (lookDir.sqrMagnitude < 0.001f) return;

        lookDir.y = 0; // Giữ nhân vật không ngửa lên cúi xuống

        Quaternion targetRot = Quaternion.LookRotation(lookDir);

        _body.rotation = Quaternion.Slerp(
            _body.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }

    private void OnMoveInput(Vector2 input)
    {
        _input = input;

        Vector3 moveDir = new Vector3(_input.x, 0, _input.y);

        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
    }
}

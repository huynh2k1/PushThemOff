using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 input)
    {
        Vector3 velocity = new Vector3(input.x, rb.velocity.y, input.y) * moveSpeed;
        rb.velocity = velocity;
    }

    public void Stop()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }
}
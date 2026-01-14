using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] bool _physicMovement = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 input)
    {
        if(_physicMovement)
        {
            PhysicMove(input);
        }
        else
        {
            TransformMove(input);
        }
    }

    public void TransformMove(Vector2 input) {
        Vector3 moveDir = new Vector3(input.x, 0, input.y); 
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
    }

    public void PhysicMove(Vector2 input)
    {
        Vector3 velocity = new Vector3(input.x, rb.velocity.y, input.y) * moveSpeed;
        rb.velocity = velocity;
    }
    public void Stop()
    {
        rb.isKinematic = false;
    }
}
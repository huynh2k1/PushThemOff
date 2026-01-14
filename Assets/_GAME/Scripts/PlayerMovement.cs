using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Move(Vector2 input)
    {
        Vector3 moveDir = new Vector3(input.x, 0, input.y); 
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
    }

    public void Stop()
    {
        rb.isKinematic = false;
    }
}
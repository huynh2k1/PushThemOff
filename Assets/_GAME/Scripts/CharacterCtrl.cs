using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    [SerializeField] bool _physicMovement = false;

    [SerializeField] private Joystick joystick;
    [SerializeField] Transform body;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 10f;

    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayer;

    public bool isGrounded;
    public bool isFalling;
    public bool isDead;

    //Variables
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerAnimator animator;

    Vector2 MoveInput;


    private void Update()
    {
        if (isDead || isFalling)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        MoveInput.x = joystick.Horizontal;
        MoveInput.y = joystick.Vertical;

        if (!_physicMovement)
            TransformMove(MoveInput);

        Vector3 lookDir = new Vector3(MoveInput.x, 0, MoveInput.y);
        Rotate(lookDir);
    }

    private void FixedUpdate()
    {
        CheckGround();

        if (isFalling)
        {
            Fall();// nếu bạn có animation rơi
        }
    }

    void Attack()
    {
        animator.Attack();
    }

    void Fall()
    {
        //isDead = true;
        animator.Idle();
    }

    void Dead()
    {
        animator.Dead();
        rb.isKinematic = true;
    }

    public void TransformMove(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            animator.Move();
        }
        else
        {
            animator.Idle();
        }
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
    }

    public void Rotate(Vector3 lookDir)
    {
        if (lookDir.sqrMagnitude < 0.001f) return;

        lookDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookDir);
        body.rotation = Quaternion.Slerp(body.rotation, rot, rotateSpeed * Time.deltaTime);
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );

        isFalling = !isGrounded && rb.velocity.y < -0.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Dead();
        }
    }
}

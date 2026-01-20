using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
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

    [SerializeField] CinemachineVirtualCamera _camZoom;

    Vector2 MoveInput;
    Vector3 _initPos;
    Quaternion _initRotation;

    public static Action OnPlayerAttackAction;

    private void Awake()
    {
        _initPos = transform.position;
        _initRotation = body.rotation;
    }

    private void OnEnable()
    {
        GameUI.OnClickAttackAction += Attack;
    }

    private void OnDestroy()
    {
        GameUI.OnClickAttackAction -= Attack;
    }


    [Button("Init Player")]
    public void OnGameHome()
    {
        if (_camZoom != null)
        {
            ActiveCamZoom(true);
        }

        transform.position = _initPos;
        body.rotation = _initRotation;

        isDead = false;
        rb.isKinematic = false;

        animator.RebineAnim();
        animator.Idle();
    }

    [Button("Player Ready")]
    public void OnStartGame()
    {
        ActiveCamZoom(false);
    }


    private void Update()
    {
        if (isDead || isFalling)
            return;
        MoveInput.x = -joystick.Horizontal;
        MoveInput.y = -joystick.Vertical;

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
        OnPlayerAttackAction?.Invoke();
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

    public void ActiveCamZoom(bool isActive)
    {
        _camZoom.enabled = isActive;
    }
}

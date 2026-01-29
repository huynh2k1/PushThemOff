using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PlayerCtrl : BaseCharacter
{
    [SerializeField] bool _physicMovement = false;

    [SerializeField] Transform body;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 10f;

    public bool isDead;

    //Variables
    [SerializeField] PlayerAnimator animator;
    [SerializeField] PlayerFireRange fireRange;

    [SerializeField] CinemachineBrain _camBrain;
    [SerializeField] CinemachineVirtualCamera _camDeco;

    Vector2 MoveInput;
    Vector3 _initPos;
    Quaternion _initRotation;
    float maxInputMagnitude = 1f;

    public static Action OnPlayerAttackAction;

    protected override void Awake()
    {
        base.Awake();
        _initPos = transform.position;
        _initRotation = body.rotation;

        animator.Idle();
    }

    protected void OnEnable()
    {
        GameUI.OnClickAttackAction += Attack;
        JoystickCtrl.OnJoystickMove += Movement;
    }

    protected void OnDestroy()
    {
        GameUI.OnClickAttackAction -= Attack;
        JoystickCtrl.OnJoystickMove -= Movement;
    }


    [Button("Init Player")]
    public void OnGameHome()
    {
        if (_camDeco != null)
        {
            ActiveCamZoom(true);
        }

        transform.position = _initPos;
        body.rotation = _initRotation;

        isDead = false;
        rb.isKinematic = false;

        animator.RebineAnim();
    }

    [Button("Player Ready")]
    public void OnStartGame()
    {
        ActiveCamZoom(false);
    }

    public void Movement(Vector2 MoveInput)
    {
        if (isDead)
            return;
        if (!_physicMovement)
            TransformMove(MoveInput);

        if (fireRange != null && fireRange.CurrentTarget != null)
        {
            RotateToEnemy(fireRange.CurrentTarget);
        }
        else
        {
            Vector3 lookDir = new Vector3(MoveInput.x, 0, MoveInput.y);
            Rotate(lookDir);
        }
    }

    void RotateToEnemy(Transform enemy)
    {
        if (enemy == null) return;

        Vector3 dir = enemy.position - body.position;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        body.rotation = Quaternion.Slerp(body.rotation, rot, rotateSpeed * Time.deltaTime);
    }

    

    protected override void Attack()
    {
        if (isDead)
            return;
            
        animator.Attack();
        DOVirtual.DelayedCall(0.2f, () =>
        {
            OnPlayerAttackAction?.Invoke();
        });
    }

   

    protected override void Dead()
    {
        isDead = true;
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



    public void ActiveCamZoom(bool isActive)
    {
        _camDeco.enabled = isActive;
        _camBrain.m_DefaultBlend.m_Time = isActive ? 0f : 1f;

    }

    public override void TakeDamage(float damage)
    {
    }
}

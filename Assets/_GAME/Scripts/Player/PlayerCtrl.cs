using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using GameConfig;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerCtrl : BaseCharacter
{
    [SerializeField] bool _physicMovement = false;

    [SerializeField] Transform body;
    [SerializeField] Collider _collider;
    [SerializeField] ParticleSystem _hitEffect;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 10f;

    //Variables
    [SerializeField] PlayerAnimator animator;
    [SerializeField] PlayerFireRange fireRange;
    [SerializeField] PlayerAttack playerAttack;

    Vector2 MoveInput;
    Quaternion _initRotation;
    float maxInputMagnitude = 1f;
    float groundY;

    public static Action OnPlayerAttackAction;
    public static Action OnPlayerBeTakeDamage;
    public static Action OnPlayerDeadAction;

    protected void OnDestroy()
    {
        GameUI.OnClickAttackAction -= Attack;
        JoystickCtrl.OnJoystickMove -= Movement;
    }

    public override void OnInit()
    {
        base.OnInit();
        groundY = transform.position.y;
        //_initRotation = body.rotation;

        isDead = false;
       
        _collider.enabled = true;

        //animator.RebineAnim();
        animator.Idle();

        playerAttack.OnInit();

        GameUI.OnClickAttackAction += Attack;
        JoystickCtrl.OnJoystickMove += Movement;
    }

    public void Movement(Vector2 MoveInput)
    {
        if (isDead)
            return;
        if (!_physicMovement)
            TransformMove(MoveInput);
        else
            MoveWithRigidbody(new Vector3(MoveInput.x, 0, MoveInput.y));

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

    public override void TakeDamage(float damage)
    {
        OnPlayerBeTakeDamage?.Invoke();
        PopupTextSpawner.I.Spawn(PopupTextType.DAMAGE, transform.position, (int)damage);
        _hitEffect.Play();
        base.TakeDamage(damage);
    }

    protected override void Dead()
    {
        isDead = true;
        animator.Dead();
        rb.isKinematic = true;
        _collider.enabled = false;

        OnPlayerDeadAction?.Invoke();
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
        Vector3 pos = transform.position;
        pos += moveDir * moveSpeed * Time.deltaTime;
        pos.y = groundY;
        transform.position = pos;
    }

    void MoveWithRigidbody(Vector3 dir)
    {
        if (rb == null) return;

        if (dir != Vector3.zero)
        {
            animator.Move();
        }
        else
        {
            animator.Idle();
        }
        Vector3 velocity = dir.normalized * moveSpeed;
        velocity.y = rb.velocity.y; // giữ gravity

        rb.velocity = velocity;
    }

    public void Rotate(Vector3 lookDir)
    {
        if (lookDir.sqrMagnitude < 0.001f) return;

        lookDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookDir);
        body.rotation = Quaternion.Slerp(body.rotation, rot, rotateSpeed * Time.deltaTime);
    }
}

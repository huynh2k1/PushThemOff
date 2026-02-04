using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : BaseCharacter
{
    public EnemyType enemyType;
    [Header("Rotate/Anim")]
    [SerializeField] protected Transform _rotater;
    [SerializeField] protected Animator _anim;
    [SerializeField] protected AnimEvent _animEvent;

    [SerializeField] protected float detectRange;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected LayerMask layerTarget;
    protected float attackTimer;

    public event Action<BaseEnemy> OnEnemyDeadAction;
    protected bool _isAttacking;

    protected virtual void OnEnable()
    {
        _animEvent.OnAttackAction += HandleEventAttack;
        _animEvent.OnEndAnimAction += HandleEventEndAttack;
    }

    protected virtual void OnDestroy()
    {
        _animEvent.OnAttackAction -= HandleEventAttack;
        _animEvent.OnEndAnimAction -= HandleEventEndAttack;
    }

    public abstract void HandleEventAttack();
    public virtual void HandleEventEndAttack()
    {
        _isAttacking = false;
    }

    protected virtual void HandleAttack()
    {
        if (!_isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackDelay;
            }
        }
    }

    protected override void Attack()
    {
        if (_isAttacking) return;

        _isAttacking = true;
        _anim.SetTrigger("Attack");
    }

    protected override void Dead()
    {
        isDead = true;
        Destroy(gameObject);
        EffectPool.I.Spawn(
            EffectType.EXPLOSION,
            transform.position,
            Quaternion.identity);

        OnEnemyDeadAction?.Invoke(this);
    }

    protected virtual void LookAtTarget(Vector3 pos)
    {
        Vector3 dir = pos - _rotater.position;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion lookRot = Quaternion.LookRotation(dir.normalized);
        _rotater.rotation = Quaternion.Slerp(_rotater.rotation, lookRot, Time.deltaTime * 10f);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}

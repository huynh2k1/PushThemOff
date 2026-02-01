using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemy : BaseCharacter
{
    [Header("Rotate / Anim")]
    [SerializeField] Transform _rotater;
    [SerializeField] Animator _anim;
    [SerializeField] AnimEvent _animEvent;

    [Header("Heal")]
    [SerializeField] float detectRange = 6f;
    [SerializeField] float healInterval = 5f;
    [SerializeField] float healAmount = 20f;
    [SerializeField] LayerMask enemyLayer;

    float healTimer;

    private void OnEnable()
    {
        if (_animEvent != null)
            _animEvent.OnEventAnimAction += CastHeal;
    }

    private void OnDisable()
    {
        if (_animEvent != null)
            _animEvent.OnEventAnimAction -= CastHeal;
    }

    protected override void OnInit()
    {
        base.OnInit();

        healTimer = healInterval;
    }

    void Update()
    {
        healTimer -= Time.deltaTime;

        if (healTimer <= 0f)
        {
            Attack();
            healTimer = healInterval;
        }
    }

    public void CastHeal()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            detectRange,
            enemyLayer);

        foreach (var hit in hits)
        {
            BaseCharacter enemy = hit.GetComponent<BaseCharacter>();

            // tránh tự heal nếu bạn không muốn
            if (enemy == null || enemy == this)
                continue;

            enemy.Heal(healAmount);
        }
    }

    protected override void Attack()
    {
        if (_anim != null)
            _anim.SetTrigger("Attack");
    }

    protected override void Dead()
    {
        EffectPool.I.Spawn(
            EffectType.EXPLOSION,
            transform.position,
            Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemy : BaseEnemy
{
    [Header("Heal")]
    [SerializeField] float healAmount = 20f;
    [SerializeField] ParticleSystem healNova;


    protected override void OnInit()
    {
        base.OnInit();

        attackTimer = attackDelay;
    }

    void Update()
    {
        HandleAttack();
    }

    public override void HandleEventAttack()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            detectRange,
            layerTarget);

        healNova.Play();

        foreach (var hit in hits)
        {
            BaseCharacter enemy = hit.GetComponent<BaseCharacter>();

            // tránh tự heal nếu bạn không muốn
            if (enemy == null || enemy.isDead)
                continue;

            enemy.Heal(healAmount);
        }
    }
}

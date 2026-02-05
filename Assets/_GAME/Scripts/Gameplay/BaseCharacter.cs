using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected HeathBar _heathBar;
    [SerializeField] protected float maxHealth = 100f;
    public bool isDead;
    public float damage;
    protected float _curHP;


    protected virtual void Awake()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();
    }

    public virtual void OnInit()
    {
        _curHP = maxHealth;
        _heathBar.Init(_curHP);
    }

    protected abstract void Attack();
    protected abstract void Dead();

    public virtual void TakeDamage(float damage)
    {
        if (damage >= _curHP)
        {
            _curHP = 0;
            _heathBar.UpdateHealthBar(_curHP);
            Dead();
            return;
        }

        _curHP -= damage;
        _heathBar.UpdateHealthBar(_curHP);
    }

    public virtual void Heal(float amount)
    {
        //if (isDead) return;

        _curHP += amount;
        _curHP = Mathf.Min(_curHP, maxHealth);

        _heathBar.UpdateHealthBar(_curHP);
    }

}

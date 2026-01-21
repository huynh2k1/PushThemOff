using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseCharacter
{
    protected override void Attack()
    {
    }

    protected override void Dead()
    {
        Destroy(gameObject);
    }
}

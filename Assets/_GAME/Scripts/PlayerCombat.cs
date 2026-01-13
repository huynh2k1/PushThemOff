using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    IPlayerAnimator animator;

    private void Awake()
    {
        animator = GetComponent<IPlayerAnimator>();
    }

    public void Attack()
    {
        animator.Attack();
    }
}
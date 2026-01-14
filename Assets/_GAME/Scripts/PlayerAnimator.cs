using UnityEngine;

public interface IPlayerAnimator
{
    void Idle();
    void Move();
    void Attack();
    void Dead();
    void Stun();
}

public class PlayerAnimator : MonoBehaviour, IPlayerAnimator
{
    [SerializeField] Animator animator;

    public void EnableAnimator(bool isEnable)
    {
        animator.enabled = isEnable;
    }

    public void Idle() => animator.SetBool("Move", false);
    public void Move() => animator.SetBool("Move", true);
    public void Attack() => animator.SetTrigger("Attack");
    public void Dead() => animator.SetBool("Dead", true);
    public void Stun() => animator.SetTrigger("Stunk");
}
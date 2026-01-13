using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    bool isDead;
    IPlayerAnimator animator;

    public bool IsDead => isDead;

    private void Awake()
    {
        animator = GetComponent<IPlayerAnimator>();
    }

    public void Kill()
    {
        if (isDead) return;

        isDead = true;
        animator.Dead();
    }
}

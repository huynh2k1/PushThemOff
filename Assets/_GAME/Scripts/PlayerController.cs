using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerRotation rotation;
    [SerializeField] PlayerCombat combat;
    PlayerGroundChecker groundChecker;
    PlayerLife life;

    IPlayerInput input;
    IPlayerAnimator animator;

    public bool isDead;

    private void Awake()
    {
        input = GetComponent<IPlayerInput>();
        groundChecker = GetComponent<PlayerGroundChecker>();
        life = GetComponent<PlayerLife>();
        //movement = GetComponent<PlayerMovement>();
        //rotation = GetComponent<PlayerRotation>();
        //combat = GetComponent<PlayerCombat>();
        animator = GetComponent<IPlayerAnimator>();
    }

    private void OnEnable()
    {
        groundChecker.OnFallOutGround += OnFallOutGround;
    }

    private void OnDisable()
    {
        groundChecker.OnFallOutGround -= OnFallOutGround;
    }

    private void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleCombat();
    }

    void HandleMovement()
    {
        if (input.MoveInput.magnitude > 0.1f)
        {
            movement.Move(input.MoveInput);
            animator.Move();

            Vector3 lookDir = new Vector3(input.MoveInput.x, 0, input.MoveInput.y);
            rotation.Rotate(lookDir);
        }
        else
        {
            animator.Idle();
        }
    }

    void HandleCombat()
    {
        if (input.AttackPressed)
        {
            combat.Attack();
        }
    }

    void OnFallOutGround()
    {
        Dead();
        life.Kill();
        movement.Stop();
    }

    public void Dead()
    {
        isDead = true;
    }
}

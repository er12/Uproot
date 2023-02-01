using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 16f;
    public float jumpSpeed = 3f;

    public float maxHealth = 3f;

    public float currentHealth;

    // State machine

    private PlayerBaseState currentState;
    public PlayerBaseState CurrentState
    {
        get { return currentState; }
    }
    public readonly PlayerIdleState IdleState = new PlayerIdleState();
    public readonly PlayerMovingState MovingState = new PlayerMovingState();
    public readonly PlayerAttackingState AttackingState = new PlayerAttackingState();

    //
    // TODO: Root attack
    // public readonly PlayerAttackingState AttackingState = new PlayerAttackingState();




    
    //TODO: Hurt
    // public readonly PlayerAttackingState AttackingState = new PlayerAttackingState();
    // TODO: DEAD
    // public readonly PlayerAttackingState AttackingState = new PlayerAttackingState();


    // For animations
    private string currentAnimaton;


    private Rigidbody2D rb;
    public Rigidbody2D Rigidbody
    {
        get { return rb; }
    }

    static bool playerExists;

    // Events
    public delegate void PlayerStatus(float value);

    // other vars
    public bool isGrounded;

    public bool isPlayerFacingRight = true;  // For determining which way the player is currently facing.

    public Vector2 respawnPoint;


    public Animator animator;

    public SpriteRenderer sprite;

    public BoxCollider2D AttackCheck;


    bool isAgainstBoulder;

    public EnemyController lastAttackedFrom;

    void OnEnable()
    {
        PlayerMovingState.PlayerFlipped += Flip;
    }


    void OnDisable()
    {
        PlayerMovingState.PlayerFlipped -= Flip;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
            Destroy(gameObject);

        animator = GetComponentInChildren(typeof(Animator)) as Animator;

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        // Status Bar
        currentHealth = maxHealth;

        TransitionToState(IdleState);
    }

    void Update()
    {
        if (CurrentState == null) // For when reloading unity
        {
            TransitionToState(IdleState);
        }

        currentState?.Update(this);
    }

    void FixedUpdate()
    {
        currentState?.FixUpdate(this);
    }

    public void TransitionToState(PlayerBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    private void setGrounded(bool value)
    {
        isGrounded = value;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isPlayerFacingRight = !isPlayerFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // public void takeDamage(EnemyController enemy)
    // {
    //     if (currentState == TakingDamageState || currentState == DisabledState)
    //         return;
    //     lastAttackedFrom = enemy;
    //     TransitionToState(TakingDamageState);
    // }


    // public void die()
    // {
    //     if (currentState == DisabledState)
    //         return;
    //     StopAllCoroutines();
    //     TransitionToState(DisabledState);
    // }


    private void OnTriggerEnter2D(Collider2D other)
    {

        /*
                switch (other.transform.tag)
                {
                    case "Checkpoint":
                        respawnPoint = transform.position;
                        break;

                    default:
                        break;
                }
                */
    }





    public void ChangeAnimationState(string newAnimation)
    {
        /*
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;*/
    }


}










using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Gamelogic.Extensions;

public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerState> stateMachine;
    public enum PlayerState
    {
        Idle,
        Walking,
        Attacking,
        TakingDamage,
        Rooting,
        Dodging,
        Talking,
        Dead
    }

    public static event System.Action OnTakeDamage;

    private static bool playerExists;
    private string currentAnimaton;
    private Vector3 dodgeDirection;
    private Vector2 movement;
    private Vector2 rootsPosition;
    private Vector2 lastDirection = new Vector2(0f, -1f);
    private Vector2 lastPlantPosition = new Vector2(0f, -1f);
    public bool grabbedSomething = false;
    public bool isAttacking;
    public bool isPlayerFacingRight = true;  // For determining which way the player is currently facing.
    public float strength = 2f;
    public float moveSpeed = 16f;
    public float jumpSpeed = 3f;
    public float maxHealth = 4f;
    public float currentHealth;
    public float dodgeSpeed = 3f;
    public Enemy lastAttackedFrom;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D col;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    public Animator animator;

    public AudioClip swordAudio;

    public ParticleSystem ps
    {
        get
        {
            if (_CachedSystem == null)
                _CachedSystem = GetComponentInChildren<ParticleSystem>();
            ;
            return _CachedSystem;
        }
    }
    private ParticleSystem _CachedSystem;
    public bool includeChildren = true;


    void OnEnable()
    {
        ps.Stop(includeChildren, ParticleSystemStopBehavior.StopEmitting);
        RootController.OnRootPlantWarpGrab += PlayerGrabPlantWithRoot;
        Enemy.OnRootFinishedEnemyGrab += TransitionToIdle;
        //RootController.OnRootItemGrab += PlayerGrabPlantWithRoot;
        RootController.OnRootNothingGrab += TransitionToIdle;
    }

    void OnDisable()
    {
        RootController.OnRootPlantWarpGrab -= PlayerGrabPlantWithRoot;
        RootController.OnRootNothingGrab -= TransitionToIdle;
    }

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
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Status Bar
        currentHealth = maxHealth;

        stateMachine = new StateMachine<PlayerState>();
        stateMachine.AddState(PlayerState.Idle, IdleEnter, IdleUpdate);
        stateMachine.AddState(PlayerState.Walking, WalkingEnter, WalkingUpdate);
        stateMachine.AddState(PlayerState.Attacking, AttackingEnter, AttackingUpdate);
        stateMachine.AddState(PlayerState.TakingDamage, TakingDamageEnter);
        stateMachine.AddState(PlayerState.Rooting, RootingEnter, RootingUpdate);
        stateMachine.AddState(PlayerState.Dodging, DodgingEnter, DodgingUpdate);
        stateMachine.AddState(PlayerState.Talking, TalkingEnter);
        stateMachine.AddState(PlayerState.Dead, DeadEnter);
        stateMachine.CurrentState = PlayerState.Idle;
    }

    void Update()
    {
        if (stateMachine == null) Start();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (stateMachine.CurrentState == PlayerState.Walking)
        {
            // player.Rigidbody.MovePosition(player.Rigidbody.position + movement * player.moveSpeed * Time.deltaTime);
            rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);

            lastDirection = rb.velocity;
        }
        if (stateMachine.CurrentState == PlayerState.Dodging)
        {
            rb.velocity = dodgeDirection * dodgeSpeed;
        }
    }

    private void FinishAttacking() //USED BY ANIMATION
    {
        isAttacking = false;
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

    public void TakeDamage(Enemy enemy)
    {
        if (stateMachine.CurrentState == PlayerState.TakingDamage || stateMachine.CurrentState == PlayerState.Talking)
        {
            return;
        }

        lastAttackedFrom = enemy;
        currentHealth -= 1;
        stateMachine.CurrentState = PlayerState.TakingDamage;
    }

    // public void Die()
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
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    public void PlayerGrabPlantWithRoot(Vector2 plantPosition, float sproutForce)
    {
        jumpForce = sproutForce;

        ChangeAnimationState("GoingUnderground");
        ps.Play(true);

        col.enabled = false;
        //spriteRenderer.sortingOrder = 1;

        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Horizontal", lastDirection.normalized.x);
        animator.SetFloat("Vertical", lastDirection.normalized.y);

        lastPlantPosition = plantPosition;
    }

    public void TransitionToIdle()
    {
        stateMachine.CurrentState = PlayerState.Idle;
        col.enabled = true;
    }

    public void CreateDust() //USED BY ANIMATION
    {
        var dust = Instantiate(Resources.Load("Prefabs/Dust") as GameObject, transform.position, Quaternion.identity);
        ChangeAnimationState("Player_CrawlingUnderground");
        StartCoroutine(GoToPlant());
    }

    private float jumpForce = 0f;
    IEnumerator GoToPlant()
    {
        float lerpTime = 2.5f;
        float t = 0f;

        while ((Vector2)transform.position != lastPlantPosition)
        {
            transform.position = Vector3.Lerp(transform.position, lastPlantPosition, lerpTime * Time.deltaTime);

            t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

            if (t > 0.9f)
            {
                //TODO: @fidel, hacer que en el salto se avancen 2 tiles
                ChangeAnimationState("Player_PoppingOut");
                animator.SetFloat("Horizontal", lastDirection.x);
                animator.SetFloat("Vertical", lastDirection.y);
                ps.Stop(includeChildren, ParticleSystemStopBehavior.StopEmitting);
                rb.AddForce(lastDirection * jumpForce);
                break;
            }
            yield return null;
        }
    }

    public void IdleEnter()
    {
        ChangeAnimationState("Idle");

        //This code is for making the player idle at the position he was going
        Vector2 direction = lastDirection;
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        rb.velocity = Vector2.zero;
    }

    public void IdleUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            stateMachine.CurrentState = PlayerState.Walking;
        }
        if (Input.GetButtonDown("Attack"))
        {
            stateMachine.CurrentState = PlayerState.Attacking;
        }
        else if (Input.GetButtonDown("Root"))
        {
            stateMachine.CurrentState = PlayerState.Rooting;
        }
    }

    public void WalkingEnter()
    {
        ChangeAnimationState("Walking");

        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

    public void WalkingUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);

            if (movement.x > 0 && !isPlayerFacingRight)
            {
                Flip();
            }
            else if (movement.x < 0 && isPlayerFacingRight)
            {
                Flip();
            }
        }
        else
        {
            stateMachine.CurrentState = PlayerState.Idle;
        }

        if (Input.GetButtonDown("Attack"))
        {
            stateMachine.CurrentState = PlayerState.Attacking;
        }
        else if (Input.GetButtonDown("Root"))
        {
            stateMachine.CurrentState = PlayerState.Rooting;
        }
    }

    public void AttackingEnter()
    {
        ChangeAnimationState("Attacking");
        isAttacking = true;

        animator.SetFloat("Horizontal", lastDirection.x);
        animator.SetFloat("Vertical", lastDirection.y);

        rb.velocity = Vector2.zero;

        audioSource.clip = swordAudio;
        audioSource.Play();
    }

    public void AttackingUpdate()
    {
        if (!isAttacking)
        {
            stateMachine.CurrentState = PlayerState.Idle;
        }
    }

    public void TakingDamageEnter()
    {
        ChangeAnimationState("TakingDamage");

        rb.velocity = Vector2.zero;

        StartCoroutine(Recoil(lastAttackedFrom));
        OnTakeDamage?.Invoke();
    }

    IEnumerator Recoil(Enemy enemy)
    {
        Vector2 recoilDirection = transform.position - enemy.transform.position;
        rb.AddForce(recoilDirection * strength, ForceMode2D.Impulse);

        yield return new WaitForSeconds(.5f);
        stateMachine.CurrentState = PlayerState.Idle;
    }

    public void RootingEnter()
    {
        ChangeAnimationState("RootAttack");
        isAttacking = true;

        animator.SetFloat("Horizontal", lastDirection.x);
        animator.SetFloat("Vertical", lastDirection.y);

        rb.velocity = Vector2.zero;

        //TODO: Remove when finished root
        StartCoroutine(RootAttack());
    }

    IEnumerator RootAttack()
    {
        Vector2 playersPosition = transform.position;
        Vector2 direction = lastDirection.normalized;

        // Down root is a little off apart, this code puts it near
        if (direction.x == 0 && direction.y == -1)
        {
            direction.y = -0.5f;
        }

        rootsPosition = playersPosition + direction;

        yield return new WaitForSeconds(.25f);
        var root = Object.Instantiate(Resources.Load("Prefabs/Root") as GameObject, rootsPosition, Quaternion.identity);
        var rootController = root.GetComponent<RootController>();
        rootController.Init(lastDirection.normalized);
    }

    public void RootingUpdate()
    {
        if (!isAttacking)
        {
            stateMachine.CurrentState = PlayerState.Idle;
        }
    }

    public void DodgingEnter()
    {
        ChangeAnimationState("Idle");

        dodgeDirection = lastDirection;
    }

    public void DodgingUpdate()
    {
        float dodgeSpeedDropMultiplier = 7f;
        dodgeSpeed -= dodgeSpeed * dodgeSpeedDropMultiplier * Time.deltaTime;

        float dodgeSpeedMinimun = 7f;

        //Finished dodgeing
        if (dodgeSpeed < dodgeSpeedMinimun)
        {
            stateMachine.CurrentState = PlayerState.Walking;
        }
    }

    public void TalkingEnter()
    {
        rb.velocity = Vector2.zero;
    }

    public void DeadEnter()
    {
        ChangeAnimationState("Dead");

        // TODO: ANIMATE Dead state

        spriteRenderer.enabled = false;

        //Maybe checkpoint

    }
}










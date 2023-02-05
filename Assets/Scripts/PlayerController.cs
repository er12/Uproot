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
    public readonly PlayerWalkingState WalkingState = new PlayerWalkingState();
    public readonly PlayerAttackingState AttackingState = new PlayerAttackingState();
    public readonly PlayerRootAttackState RootAttackState = new PlayerRootAttackState();
    public readonly PlayerDodgingState DodgingState = new PlayerDodgingState();

    //TODO: Hurt
    // public readonly PlayerAttackingState AttackingState = new PlayerAttackingState();
    // TODO: DEAD
    // public readonly PlayerAttackingState AttackingState = new PlayerAttackingState();

    // For animations
    private string currentAnimaton;

    public AudioSource audioSource;
    public AudioClip swordAudio;

    private Rigidbody2D rb;
    public Rigidbody2D Rigidbody
    {
        get { return rb; }
    }

    static bool playerExists;

    // Events
    public delegate void PlayerStatus(float value);

    // other vars
    public bool isAttacking;

    public bool isPlayerFacingRight = true;  // For determining which way the player is currently facing.

    public Vector2 respawnPoint;

    public Animator animator;

    public SpriteRenderer spriteRenderer;

    public BoxCollider2D collider2D;

    bool isAgainstBoulder;

    // Records of events
    public Vector2 lastDirection = new Vector2(0f, -1f);
    public Vector2 lastPlantPosition = new Vector2(0f, -1f);

    public EnemyController lastAttackedFrom;

    public ParticleSystem particleSystem
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
        particleSystem.Stop(includeChildren, ParticleSystemStopBehavior.StopEmitting);
        PlayerWalkingState.PlayerFlipped += Flip;
        RootController.OnRootPlantWarpGrab += PlayerGrabPlantWithRoot;
        //RootController.OnRootEnemyGrab += PlayerGrabPlantWithRoot;
        //RootController.OnRootItemGrab += PlayerGrabPlantWithRoot;
        RootController.OnRootNothingGrab += TransitionToIdle;
    }

    void OnDisable()
    {
        PlayerWalkingState.PlayerFlipped -= Flip;
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
        collider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

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

    private void finishAttaking()
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
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    public void PlayerGrabPlantWithRoot(Vector2 plantPosition)
    {
        ChangeAnimationState("GoingUnderground");
        particleSystem.Play(true);

        collider2D.enabled = false;
        //spriteRenderer.sortingOrder = 1;

        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Horizontal", lastDirection.normalized.x);
        animator.SetFloat("Vertical", lastDirection.normalized.y);

        lastPlantPosition = plantPosition;
    }

    public void TransitionToIdle()
    {
        TransitionToState(IdleState);
        collider2D.enabled = true;
        //spriteRenderer.sortingOrder = 4;
    }

    //Used by anim
    public void CreateDust()
    {
        var dust = Instantiate(Resources.Load("Prefabs/Dust") as GameObject, transform.position, Quaternion.identity);
        ChangeAnimationState("Player_CrawlingUnderground");
        StartCoroutine(goToPlant());
    }

    IEnumerator goToPlant()
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
                particleSystem.Stop(includeChildren, ParticleSystemStopBehavior.StopEmitting);

                break;
            }
            yield return null;
        }

    }
}










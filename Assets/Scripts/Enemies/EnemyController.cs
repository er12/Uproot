using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Vector2 currentTile = Vector2.zero;
    public Vector2 destinationTile = Vector2.zero;
    public Vector2 currentPosition = Vector2.zero;
    public Vector2 destinationPosition = Vector2.zero;

    public Animator animator;
    private string currentAnimaton;
    public bool enemyFacingRight = false;



    private EnemyBaseState currentState;
    public EnemyBaseState CurrentState
    {
        get { return currentState; }
    }
    private Rigidbody2D rb;
    public Rigidbody2D Rigidbody
    {
        get { return rb; }
    }

    public bool isEnemyFacingRight = true;  // For determining which way the enemy is currently facing.

    public readonly EnemyRoamingState RoamingState = new EnemyRoamingState();

    public SpriteRenderer spriteRenderer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        ObtainNewDestination();

        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        TransitionToState(RoamingState);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentState == null) // For when reloading unity
        {
            TransitionToState(RoamingState);
        }

        currentState?.Update(this);
    }

    void FixedUpdate()
    {
        currentState?.FixUpdate(this);
    }

    public void ObtainNewDestination()
    {
        currentPosition = transform.position;
        while (currentTile == destinationTile)
        {
            bool moveDirection = (Random.Range(0, 2) == 0);
            if (moveDirection)
            {
                destinationTile.x = Random.Range(-1, 2);
            }
            else
            {
                destinationTile.y = Random.Range(-1, 2);
            }
        }
        destinationPosition = currentPosition + currentTile - destinationTile;
    }

    // Start is called before the first frame update


    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    void gotHitByRoot()
    {
        Debug.Log("gotHitByRoot");

        // StartFlipping animation
    }

}

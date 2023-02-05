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

    private Rigidbody2D rb;
    public Animator animator;

    private void Awake()
	{
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

	private void Start()
    {
        //Debug.Log(currentTile.x + " " + currentTile.y);
        ObtainNewDestination();
        //Debug.Log(destinationTile.x + " " + destinationTile.y);
        //Debug.Log(destinationPosition);
        //Debug.Log(currentPosition);
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

    private void Update()
    {
        float threshold = 0.15f;
        //Debug.Log(Vector3.Distance(transform.position, destinationPosition));
        if (Vector3.Distance(transform.position, destinationPosition) < threshold)
        {
            ///Debug.Log("Destination reached!");
            rb.velocity = Vector2.zero;
            currentTile = destinationTile;
            ObtainNewDestination();
        }
        else
        {
            rb.velocity = currentTile - destinationTile;
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 1f);
            animator.SetFloat("Horizontal", rb.velocity.x);
            animator.SetFloat("Vertical", rb.velocity.y);
            if (rb.velocity.x > 0) transform.localScale = new Vector3(-1f, 1f, 1f);
            else transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void ChangeAnimationState(string newAnimation)
    {
        animator.Play(newAnimation);
    }

    /*private EnemyBaseState currentState;
    public EnemyBaseState CurrentState
    {
        get { return currentState; }
    }
    private Rigidbody2D rb;
    public Rigidbody2D Rigidbody
    {
        get { return rb; }
    }

    public float moveSpeed = 1f;

    public bool isEnemyFacingRight = true;  // For determining which way the enemy is currently facing.

    public readonly EnemyRoamingState RoamingState = new EnemyRoamingState();


    // Start is called before the first frame update
    void Start()
    {
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

    void gotHitByRoot() {
        Debug.Log("gotHitByRoot");
        
        // StartFlipping animation
    }*/

}

using Gamelogic.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IGrabbable
{
    public static event System.Action OnRootFinishedEnemyGrab;

    private StateMachine<EnemyState> stateMachine;
    public enum EnemyState
    {
        Roaming,
        Flipped
    }
    
    public MainInstances mainInstances;

    private string currentAnimaton;
    public bool tilted = false;
    public float health = 5f;
    public float moveSpeed = 1f;
    public Vector2 currentTile = Vector2.zero;
    public Vector2 destinationTile = Vector2.zero;
    public Vector2 currentPosition = Vector2.zero;
    public Vector2 destinationPosition = Vector2.zero;
    public bool isEnemyFacingRight = false;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    PlayerController player;

    private void Start()
    {
        player = mainInstances.playerController;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        ObtainNewDestination();

        stateMachine = new StateMachine<EnemyState>();
        stateMachine.AddState(EnemyState.Roaming, RoamingStart, RoamingUpdate);
        stateMachine.AddState(EnemyState.Flipped, FlippedStart);
        stateMachine.CurrentState = EnemyState.Roaming;
    }

    void Update()
    {
        if (stateMachine == null) Start();
        stateMachine.Update();
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

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;
        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    void GotHitByRoot()
    {
        Debug.Log("gotHitByRoot");

        // StartFlipping animation
    }

    void BackToFlippedAnim()
    {
        if (stateMachine.CurrentState == EnemyState.Flipped)
            animator.Play("Turtle_OnItsBack");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            player.lastAttackedFrom = this;
            player.TakeDamage(this);
        }
    }

    public void AnimateGrabbedByRoot()
    {
        if (gameObject.name.StartsWith("Turtle"))
        {
            stateMachine.CurrentState = EnemyState.Flipped;
        }
    }

    public void InvokeFinishedGrabbing()
    {
        tilted = true;

        OnRootFinishedEnemyGrab?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player" && stateMachine.CurrentState != EnemyState.Flipped)
        {
            player.lastAttackedFrom = this;
            player.TakeDamage(this);
            return;
        }

        if (other.name == "AttackCheck")
        {
            if (stateMachine.CurrentState == EnemyState.Flipped)
            {
                TakeDamage();
            }
            else
            {
                player.lastAttackedFrom = this;
                player.stateMachine.CurrentState = PlayerController.PlayerState.TakingDamage;
            }
        }

    }

    public void FlippedStart()
    {
        animator.Play("FlippedByRoot");
        rb.velocity = Vector2.zero;

        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Vertical", rb.velocity.y);

        StartCoroutine(Struggle());
    }

    IEnumerator Struggle()
    {
        yield return new WaitForSeconds(1f);
        InvokeFinishedGrabbing();

        animator.Play("Turtle_OnItsBack");
        animator.SetFloat("Horizontal", rb.velocity.normalized.x);
        animator.SetFloat("Vertical", rb.velocity.normalized.y);

        yield return new WaitForSeconds(3f);
        // TODO:? Animate flip

        animator.Play("Roaming");
        stateMachine.CurrentState = EnemyState.Roaming;
    }

    public void TakeDamage()
    {
        animator.Play("Turtle_Flipped_TakingDamage");
        // animator.SetFloat("Horizontal", Rigidbody.velocity.normalized.x);
        // animator.SetFloat("Vertical", Rigidbody.velocity.normalized.y);

        if (health <= 1)
        {
            StartCoroutine(Die());
        }
        else
        {
            health--;
        }
    }

    IEnumerator Die()
    {
        {
            yield return new WaitForSeconds(0.3f);
            Destroy(gameObject);
        }
    }

    public void RoamingStart()
    {
        ChangeAnimationState("Roaming");
    }

    public void RoamingUpdate()
    {
        float threshold = 0.15f;
        //Debug.Log(Vector3.Distance(transform.position, destinationPosition));
        if (Vector3.Distance(transform.position, destinationPosition) < threshold)
        {
            //Debug.Log("Destination reached!");
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

            if (rb.velocity.x > 0 && !isEnemyFacingRight)
            {
                Flip();
            }
            else if (rb.velocity.x < 0 && isEnemyFacingRight)
            {
                Flip();
            }
        }
    }

    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        isEnemyFacingRight = !isEnemyFacingRight;
    }

	public void Grab()
	{
        AnimateGrabbedByRoot();
    }
}

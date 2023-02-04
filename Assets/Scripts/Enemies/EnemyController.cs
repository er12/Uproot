using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private string currentAnimaton;
    public Animator animator;

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
    }

}

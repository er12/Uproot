using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlippedState : EnemyBaseState
{

    EnemyController enemy;
    Vector2 direction;
    bool startedStruggling;
    public override void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        enemy.animator.Play("FlippedByRoot");
        direction = enemy.Rigidbody.velocity.normalized;
        enemy.Rigidbody.velocity = Vector2.zero;
        startedStruggling = false;

        enemy.animator.SetFloat("Horizontal", enemy.Rigidbody.velocity.x);
        enemy.animator.SetFloat("Vertical", enemy.Rigidbody.velocity.y);

        enemy.StartCoroutine(struggle());

    }

    public override void Update(EnemyController enemy)
    {

    }



    public override void FixUpdate(EnemyController enemy)
    {

    }

    IEnumerator struggle()
    {
        yield return new WaitForSeconds(1f);
        enemy.invokeFinishedGrabbing();
        
        enemy.animator.Play("Turtle_OnItsBack");

        enemy.animator.SetFloat("Horizontal", direction.normalized.x);
        enemy.animator.SetFloat("Vertical", direction.normalized.y);

        yield return new WaitForSeconds(3f);
        // TODO:? Animate flip


        enemy.animator.Play("Roaming");
        enemy.TransitionToRoaming();
    }


}





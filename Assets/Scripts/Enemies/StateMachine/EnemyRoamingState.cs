using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoamingState : EnemyBaseState
{
    static int N = 3, M = 3;
    int[,] movementGrid = new int[N, M];
    Vector2 inGridPosition = new Vector2(1, 1);
    Vector2 direction = new Vector2(0, -1); //looking down

    EnemyController enemy;
    public override void EnterState(EnemyController enemy)
    {
        this.enemy = enemy;
        enemy.ChangeAnimationState("Roaming");
    }

    public override void Update(EnemyController enemy)
    {
        float threshold = 0.15f;
        //Debug.Log(Vector3.Distance(transform.position, destinationPosition));
        if (Vector3.Distance(enemy.transform.position, enemy.destinationPosition) < threshold)
        {
            ///Debug.Log("Destination reached!");
            enemy.Rigidbody.velocity = Vector2.zero;
            enemy.currentTile = enemy.destinationTile;
            enemy.ObtainNewDestination();
        }
        else
        {
            enemy.Rigidbody.velocity = enemy.currentTile - enemy.destinationTile;
            enemy.Rigidbody.velocity = Vector2.ClampMagnitude(enemy.Rigidbody.velocity, 1f);
            enemy.animator.SetFloat("Horizontal", enemy.Rigidbody.velocity.x);
            enemy.animator.SetFloat("Vertical", enemy.Rigidbody.velocity.y);

            if (enemy.Rigidbody.velocity.x > 0 && !enemy.enemyFacingRight)
            {
                flipEnemy(enemy);
            }
            else if (enemy.Rigidbody.velocity.x < 0 && enemy.enemyFacingRight)
            {
                flipEnemy(enemy);
            }
        }

    }



    public override void FixUpdate(EnemyController enemy)
    {

    }


    public void flipEnemy(EnemyController enemy)
    {
        Vector3 theScale = enemy.transform.localScale;
        theScale.x *= -1;
        enemy.transform.localScale = theScale;
        enemy.enemyFacingRight = !enemy.enemyFacingRight;

    }
}





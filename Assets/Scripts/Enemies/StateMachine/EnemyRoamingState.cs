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
        enemy.animator.SetFloat("Horizontal", direction.x);
        enemy.animator.SetFloat("Vertical", direction.y); 

        enemy.StartCoroutine(move());
    }

    public override void Update(EnemyController enemy)
    {

    }


    public override void FixUpdate(EnemyController enemy)
    {

    }

    IEnumerator move()
    {
        Vector2 newVector = Vector2.zero;
        int breaker = 0;
        while (true)
        {
            do
            {
                float x = Random.Range(getInferiorLim((int)inGridPosition.x), getSuperiorLim((int)inGridPosition.x));
                Debug.Log("x = " + x);
                
                
                float y = Random.Range(getInferiorLim((int)inGridPosition.y), getSuperiorLim((int)inGridPosition.y)) * (x != 0f ? 0 : 1);
                newVector = new Vector2(x, y);
                
                breaker++;

            } while (breaker <= 20);
            breaker = 0;

            enemy.Rigidbody.velocity = new Vector2(newVector.x * enemy.moveSpeed, newVector.y * enemy.moveSpeed);

            //enemy.transform.position += (Vector3)(newVector);
            inGridPosition += newVector;

            yield return new WaitForSeconds(2f);
        }
    }

    int getSuperiorLim(int n)
    {
        int newN = n + 1;
        return newN >= N ? n : newN;
    }
    int getInferiorLim(int n)
    {
        int newN = n - 1;
        return newN < 0 ? n : newN;
    }
}





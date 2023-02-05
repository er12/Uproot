using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlippedState : EnemyBaseState
{

    EnemyController enemy;
    public override void EnterState(EnemyController enemy)
    {
        enemy.Rigidbody.velocity = Vector2.zero;
    }

    public override void Update(EnemyController enemy)
    {

    }



    public override void FixUpdate(EnemyController enemy)
    {

    }


}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakingDamageState : PlayerBaseState
{
    PlayerController player;
    EnemyController enemy;
    public float strength = 8f;
    public override void EnterState(PlayerController player)
    {
        player.ChangeAnimationState("TakingDamage");
        this.player = player;
        player.Rigidbody.velocity = Vector2.zero;
        this.enemy = player.lastAttackedFrom;

        if (enemy == null)
        {
            enemy = new EnemyController();
            enemy.transform.position = player.transform.position;
        }

        player.StartCoroutine(recoil());

    }

    public override void Update(PlayerController player)
    {

    }

    public override void FixUpdate(PlayerController player)
    {
    }

    IEnumerator recoil()
    {
        Vector2 recoilDirection = player.transform.position - enemy.transform.position;

        player.Rigidbody.AddForce(recoilDirection * strength, ForceMode2D.Impulse);

        yield return new WaitForSeconds(.5f);
        player.TransitionToState(player.IdleState);
    }


}

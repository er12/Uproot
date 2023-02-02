using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRootAttackState : PlayerBaseState
{
    private string animationName = "RootAttack";
    public override void EnterState(PlayerController player)
    {
        player.ChangeAnimationState(animationName);
        player.isAttacking = true;

        player.animator.SetFloat("Horizontal", player.lastDirection.x);
        player.animator.SetFloat("Vertical", player.lastDirection.y);

        player.Rigidbody.velocity = Vector2.zero;

        player.StartCoroutine(waitseconds(player));

    }

    public override void Update(PlayerController player)
    {
        if (!player.isAttacking)
        {
            player.TransitionToState(player.IdleState);
        }
    }

    public override void FixUpdate(PlayerController player)
    {
    }

    IEnumerator waitseconds(PlayerController player)
    {
        yield return new WaitForSeconds(1f);
            player.TransitionToState(player.IdleState);
    }

}

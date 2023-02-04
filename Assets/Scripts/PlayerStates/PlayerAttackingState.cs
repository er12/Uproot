using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private string animationName = "Attacking";
    public override void EnterState(PlayerController player)
    {
        player.ChangeAnimationState(animationName);
        player.isAttacking = true;

        player.animator.SetFloat("Horizontal", player.lastDirection.x);
        player.animator.SetFloat("Vertical", player.lastDirection.y);

        player.Rigidbody.velocity = Vector2.zero;

        player.audioSource.clip = player.swordAudio;
        player.audioSource.Play();
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

}

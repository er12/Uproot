using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        player.ChangeAnimationState("Idle");

        //This code is for making the player idle at the position he was going
        Vector2 direction = player.lastDirection;
        player.animator.SetFloat("Horizontal", direction.x);
        player.animator.SetFloat("Vertical", direction.y);

        player.Rigidbody.velocity = Vector2.zero;
    }

    public override void Update(PlayerController player)
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            player.TransitionToState(player.WalkingState);
        }

        if (Input.GetKey(KeyCode.X))
        {
            player.TransitionToState(player.AttackingState);
        }
    }


    public override void FixUpdate(PlayerController player)
    {
    }
}

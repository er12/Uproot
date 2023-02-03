using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private Vector3 dodgeDirection;
    public float dodgeSpeed = 3f;

    public override void EnterState(PlayerController player)
    {
        player.ChangeAnimationState("Idle");
        
        dodgeDirection = player.lastDirection;
        
    }

    public override void Update(PlayerController player)
    {

        float dodgeSpeedDropMultiplier = 7f;
        dodgeSpeed -= dodgeSpeed * dodgeSpeedDropMultiplier * Time.deltaTime;

        float dodgeSpeedMinimun = 7f;

        //Finished dodgeing
        if (dodgeSpeed < dodgeSpeedMinimun)
        {
            player.TransitionToState(player.WalkingState);
        }

    }

    public override void FixUpdate(PlayerController player)
    {
        player.Rigidbody.velocity = dodgeDirection * dodgeSpeed;
    }
}


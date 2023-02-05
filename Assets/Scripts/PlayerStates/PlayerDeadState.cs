using UnityEngine;
using System.Linq;

public class PlayerDeadState : PlayerBaseState
{

    public override void EnterState(PlayerController player)
    {
        player.ChangeAnimationState("Dead");

        // TODO: ANIMATE Dead state

        player.spriteRenderer.enabled = false;

        //Maybe checkpoint

    }

    public override void Update(PlayerController player)
    {

    }

    public override void FixUpdate(PlayerController player)
    {
      
    }

}

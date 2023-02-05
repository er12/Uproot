using UnityEngine;
using System.Linq;

public class PlayerTalkingState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        player.Rigidbody.velocity = Vector2.zero;
    }

    public override void Update(PlayerController player)
    {
       

    }

    public override void FixUpdate(PlayerController player)
    {
    
    }

}

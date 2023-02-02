using UnityEngine;
using System.Linq;

public class PlayerWalkingState : PlayerBaseState
{
    public delegate void PlayerStatus();
    public static event PlayerStatus PlayerFlipped;

    public override void EnterState(PlayerController player)
    {
        player.ChangeAnimationState("Walking");

        player.Rigidbody.velocity = new Vector2(movement.x * player.moveSpeed, player.Rigidbody.velocity.y);
    }

    public override void Update(PlayerController player)
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        player.animator.SetFloat("Horizontal", movement.x);
        player.animator.SetFloat("Vertical", movement.y);

        if (movement != Vector2.zero)
        {

            if (movement.x > 0 && !player.isPlayerFacingRight)
            {
                PlayerFlipped.Invoke();
            }
            else if (movement.x < 0 && player.isPlayerFacingRight)
            {
                PlayerFlipped.Invoke();
            }
        }
        else
        {
            player.TransitionToState(player.IdleState);
        }

        if (Input.GetKey(KeyCode.X))
        {
            player.TransitionToState(player.AttackingState);
        }

    }

    public override void FixUpdate(PlayerController player)
    {
        player.Rigidbody.velocity = new Vector2(movement.x * player.moveSpeed, movement.y * player.moveSpeed);
        player.lastDirection = player.Rigidbody.velocity;
    }

}

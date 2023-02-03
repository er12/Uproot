using UnityEngine;
using System.Linq;

public class PlayerWalkingState : PlayerBaseState
{
    public delegate void PlayerStatus();
    public static event PlayerStatus PlayerFlipped;

    public override void EnterState(PlayerController player)
    {
        player.ChangeAnimationState("Walking");
        Debug.Log("player");
        

        player.Rigidbody.velocity = new Vector2(movement.x * player.moveSpeed, player.Rigidbody.velocity.y);
    }

    public override void Update(PlayerController player)
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            player.animator.SetFloat("Horizontal", movement.x);
            player.animator.SetFloat("Vertical", movement.y);

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

        if (Input.GetKey(KeyCode.Z))
        {
            player.TransitionToState(player.RootAttackState);
        }
        if (Input.GetKey(KeyCode.X))
        {
            player.TransitionToState(player.AttackingState);

        }
        if (Input.GetKey(KeyCode.C))
        {
            player.TransitionToState(player.RootAttackState);
        }

    }

    public override void FixUpdate(PlayerController player)
    {
        // player.Rigidbody.MovePosition(player.Rigidbody.position + movement * player.moveSpeed * Time.deltaTime);
        player.Rigidbody.velocity = new Vector2(movement.x * player.moveSpeed, movement.y * player.moveSpeed);

        player.lastDirection = player.Rigidbody.velocity;
    }

}

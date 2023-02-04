using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRootAttackState : PlayerBaseState
{
    private string animationName = "RootAttack";
    private string rootRouteString = "Prefabs/RootIndicator";
    public bool grabbedSomething = false;
    Vector2 rootsPosition;

    public override void EnterState(PlayerController player)
    {
        player.ChangeAnimationState(animationName);
        player.isAttacking = true;

        player.animator.SetFloat("Horizontal", player.lastDirection.x);
        player.animator.SetFloat("Vertical", player.lastDirection.y);

        player.Rigidbody.velocity = Vector2.zero;

        //TODO: Remove when finished root
        player.StartCoroutine(RootAttack(player));

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

    IEnumerator RootAttack(PlayerController player)
    {
        Vector2 playersPosition = player.transform.position;
        Vector2 direction = player.lastDirection.normalized;

        // Down root is a little off apart, this code puts it near
        if (direction.x == 0 && direction.y == -1)
        {
            direction.y = -0.5f;
        }

        rootsPosition = playersPosition + direction;

        yield return new WaitForSeconds(.25f);
        var rootIndicator = Object.Instantiate(Resources.Load(rootRouteString) as GameObject, rootsPosition, Quaternion.identity);
        var rootIndicatorController = rootIndicator.GetComponent<RootIndicatorController>();
        rootIndicatorController.Init(player.lastDirection.normalized);
        //var rootController = Object.Instantiate(Resources.Load("Prefabs/Root") as GameObject, rootsPosition, Quaternion.identity).GetComponent<RootController>();

        //yield return new WaitForSeconds(player.maxRootLengthTime);



    }
}

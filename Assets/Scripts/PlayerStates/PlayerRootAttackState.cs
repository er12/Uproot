using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRootAttackState : PlayerBaseState
{
    private string animationName = "RootAttack";
    private string rootRouteString = "Prefabs/Root";

    Vector2 rootsPosition;


    public bool grabbedSomething = false;

    public override void EnterState(PlayerController player)
    {
        player.ChangeAnimationState(animationName);
        player.isAttacking = true;

        player.animator.SetFloat("Horizontal", player.lastDirection.x);
        player.animator.SetFloat("Vertical", player.lastDirection.y);

        player.Rigidbody.velocity = Vector2.zero;

        //TODO: Remove when finished root
        player.StartCoroutine(doRootAttack(player));

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

    IEnumerator doRootAttack(PlayerController player)
    {

        Vector2 playersPosition = player.transform.position;
        rootsPosition = playersPosition + player.lastDirection.normalized;
        
        yield return new WaitForSeconds(.25f);
        var root = Object.Instantiate(Resources.Load(rootRouteString) as GameObject, rootsPosition, Quaternion.identity);
        root.transform.parent = player.gameObject.transform;

        yield return new WaitForSeconds(player.maxRootLengthTime);
        if (!grabbedSomething)
        {
            player.TransitionToState(player.IdleState);

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    private string animBoolName;
    public Vector2 movement;

    public abstract void EnterState(PlayerController player);

    public abstract void Update(PlayerController player);

    public abstract void FixUpdate(PlayerController player);
}

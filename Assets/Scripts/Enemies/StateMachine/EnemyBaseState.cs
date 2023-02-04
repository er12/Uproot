using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    private string animBoolName;
    public Vector2 movement;

    public abstract void EnterState(EnemyController enemy);

    public abstract void Update(EnemyController enemy);

    public abstract void FixUpdate(EnemyController enemy);
}

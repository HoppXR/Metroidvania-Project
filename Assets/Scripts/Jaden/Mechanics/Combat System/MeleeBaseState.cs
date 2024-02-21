using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : State
{
    protected Animator animator;
    
    public float duration;
    protected bool shouldCombo;
    protected int attackIndex;

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);
        animator = GetComponent<Animator>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetMouseButtonDown(0))
        {
            shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

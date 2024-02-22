using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFinisherState : MeleeBaseState
{
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);

        attackIndex = 3;
        duration = 0.45f;
        animator.SetTrigger("Attack" + attackIndex);
        Debug.Log("Player Attack " + attackIndex);
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime >= duration)
        {
            StateMachine.SetNextStateToMain();
        }
    }
}

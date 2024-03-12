using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransition3 : StateMachineBehaviour
{
    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerCombat.Instance.isAttacking)
        {
            PlayerCombat.Instance.animator.SetTrigger("Attack1");
        }
    }
}
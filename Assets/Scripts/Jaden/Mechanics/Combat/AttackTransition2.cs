using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransition2 : StateMachineBehaviour
{
    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerCombat.Instance.isAttacking)
        {
            FindObjectOfType<AudioManager>().Play("PlayerAttack");
            PlayerCombat.Instance.animator.SetTrigger("Attack3");
        }
    }
}
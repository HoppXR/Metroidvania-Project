using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeBaseState : State
{
    protected Animator animator;
    
    public float duration;
    protected bool shouldCombo;
    protected int attackIndex;

    protected Collider2D hitCollider;
    private List<Collider2D> collidersDamaged;
    private GameObject hitEffectPrefab;

    private float AttackPressTimer = 0f;

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);
        animator = GetComponent<Animator>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        AttackPressTimer -= Time.deltaTime;

        if (animator.GetFloat("Weapon.Active") > 0f)
        {
            Attack();
        }

        if (Input.GetMouseButtonDown(0))
        {
            AttackPressTimer = 2;
        }

        if (animator.GetFloat("AttackWindow.Open") > 0f && AttackPressTimer > 0)
        {
            shouldCombo = true;
        }
    }

    protected void Attack()
    {
        Collider2D[] collidersToDamage = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int colliderCount = Physics2D.OverlapCollider(hitCollider, filter, collidersToDamage);

        for (int i = 0; i < colliderCount; i++)
        {
            if (!collidersDamaged.Contains(collidersToDamage[i]))
            {
                TeamComponent hitTeamComponent = collidersToDamage[i].GetComponentInChildren<TeamComponent>();

                // Checking if colliders hit enemy
                if (hitTeamComponent && hitTeamComponent.TeamIndex == TeamIndex.Enemy)
                {
                    GameObject.Instantiate(hitEffectPrefab, collidersToDamage[i].transform);
                    Debug.Log("Enemy Has Taken:" + attackIndex + "Damage");
                    collidersDamaged.Add(collidersToDamage[i]);
                }
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

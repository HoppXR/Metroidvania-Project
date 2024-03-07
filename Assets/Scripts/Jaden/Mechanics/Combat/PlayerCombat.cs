using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public bool isAttacking = false;
    public static PlayerCombat instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
        }
    }
}

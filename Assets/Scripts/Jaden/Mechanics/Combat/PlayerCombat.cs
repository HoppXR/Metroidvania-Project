using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public static PlayerCombat Instance;
    
    [HideInInspector] public bool isAttacking = false;

    private void Awake()
    {
        Instance = this;
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

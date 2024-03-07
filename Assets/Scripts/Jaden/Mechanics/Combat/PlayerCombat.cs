using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerMovement _player;
    public Animator animator;
    public bool isAttacking = false;
    public static PlayerCombat instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _player = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            
        }
    }
}

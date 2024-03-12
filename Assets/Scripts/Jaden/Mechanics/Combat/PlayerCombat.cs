using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public static PlayerCombat Instance;
    private PlayerMovement _player;
    
    public bool isAttacking = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        _player = GetComponent<PlayerMovement>();
    }

    public void Attack()
    {
        if (isAttacking)
        {
            return;
        }
        
        StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        _player.SlowDown();
        
        yield return new WaitForSeconds(0.5f);
        
        isAttacking = false;
        _player.SpeedUp();
    }
}

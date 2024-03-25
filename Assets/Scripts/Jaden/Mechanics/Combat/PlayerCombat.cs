using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public static PlayerCombat Instance;
    private PlayerMovement _player;
    
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool canAttack;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        _player = GetComponent<PlayerMovement>();

        canAttack = true;
    }

    public void Attack()
    {
        if (isAttacking || !canAttack || PauseMenu.isPaused)
            return;
        
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

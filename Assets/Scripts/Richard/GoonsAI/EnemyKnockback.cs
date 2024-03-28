using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private EnemyAI _enemy;
    
    private readonly float _force = 15;

    private void Start()
    {
        _enemy = GetComponent<EnemyAI>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MyWeapon")
        {
            Vector2 difference = transform.position - other.transform.position;
            transform.position = new Vector2(transform.position.x + difference.x * _force * (Time.deltaTime * 0.5f), transform.position.y + difference.y * _force * (Time.deltaTime * 0.5f));
            if (_enemy !=null) StartCoroutine(Knockback());
        }
    }

    IEnumerator Knockback()
    {
        _enemy.canMove = false;
        
        yield return new WaitForSeconds(1f);
        
        _enemy.canMove = true;
    }
}

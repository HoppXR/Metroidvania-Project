using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private float _force = 5;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MyWeapon")
        {
            Vector2 difference = transform.position - other.transform.position;
            transform.position = new Vector2(transform.position.x + difference.x * _force * Time.deltaTime, transform.position.y + difference.y * _force * Time.deltaTime);
        }
    }
}

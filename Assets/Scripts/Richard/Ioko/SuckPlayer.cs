using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckPlayer : MonoBehaviour
{
    public Transform player;
    public float influenceRange;
    public float intensity;
    private Vector2 pullForce;
    public IokoAttack iokoAttack;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss2");
        if (bossObject != null)
        {
            iokoAttack = bossObject.GetComponent<IokoAttack>();
            if (iokoAttack == null)
            {
                return;
            }
        }
        else
        {
            return;
        }
    }
    
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceToPlayer <= influenceRange)
        {
            pullForce = (transform.position - player.position).normalized * intensity;
            player.position += (Vector3)pullForce * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (iokoAttack != null)
        {
            iokoAttack.BlackHoleDestroyed();
        }
        else
        {
            return;
        }
    }
}
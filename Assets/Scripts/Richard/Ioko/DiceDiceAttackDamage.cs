using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceDiceAttackDamage : MonoBehaviour
{
    private IokoAttack _iokoAttack;
    
    void Start()
    {
        _iokoAttack = GetComponent<IokoAttack>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamage"))
        {
            GameManager.gameManager._playerHealth.TakeDamage(_iokoAttack.ranDamage);
            Debug.Log(GameManager.gameManager._playerHealth.Health);
        }
    }
}

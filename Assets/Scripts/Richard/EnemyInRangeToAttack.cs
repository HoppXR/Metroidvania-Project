using UnityEngine;

public class EnemyInRangeToAttack : MonoBehaviour
{
    public delegate void PlayerInRange(bool inRange);
    public static event PlayerInRange OnPlayerInRange;

    private EnemyAI enemyAI;
    
    private void Start()
    {
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerInRange?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerInRange?.Invoke(false);
        }
    }

    private void OnDestroy()
    {
        OnPlayerInRange = null;
    }

}
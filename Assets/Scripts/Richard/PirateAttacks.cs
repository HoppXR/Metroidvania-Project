using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateAttacks : MonoBehaviour
{
    public GameObject GhostShip;
    [SerializeField] private int travelSpeed = 4;
    public Transform boss;
    
    public GameObject SpinAttackIndicators;
    private PolygonCollider2D polygonCollider2D;
    
    public BossCannonAttack[] BossCannonAttack;
    
    public Vector3 teleportDestination;
    public Vector3 returnDestination;
    public GameObject Gooners;
    private int goonerCount;

    void Start()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        StartCoroutine(RandomAttackRoutine());
    }
    private IEnumerator RandomAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            int randomNumber = Random.Range(1, 14);

            if (randomNumber >= 1 && randomNumber <= 4)
            {
                foreach (var BossCannonAttack in BossCannonAttack)
                {
                    BossCannonAttack.ShootCannon();
                }
            }
            else if (randomNumber >= 5 && randomNumber <= 8)
            {
                SpawnSummonShip();
            }
            else if (randomNumber >= 9 && randomNumber <= 12)
            {
                StartCoroutine(SpinAttackCoroutine(gameObject));
            }
            else // randomNumber is 13
            {
                GoonsAttack();
            }
        }
    }
    
    void SpawnSummonShip()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 spawnPosition = new Vector2(Random.Range(-30, 30), 193);
        GameObject Ghostship = Instantiate(GhostShip, spawnPosition, Quaternion.identity);

        Vector3 directionToPlayer = (player.transform.position - Ghostship.transform.position).normalized;

        Rigidbody2D shipRb = Ghostship.GetComponent<Rigidbody2D>();
        shipRb.velocity = directionToPlayer * travelSpeed;

        // Calculate the time it takes to reach the player based on distance and speed
        float timeToReachPlayer = Vector2.Distance(player.transform.position, Ghostship.transform.position) / travelSpeed;
        Destroy(Ghostship, timeToReachPlayer+5f);
    }


    IEnumerator SpinAttackCoroutine(GameObject parent)
    {
        GameObject spinAttack = Instantiate(SpinAttackIndicators, boss.position, Quaternion.identity);
        spinAttack.transform.parent = parent.transform;
        Destroy(spinAttack, 1f);
        yield return new WaitForSeconds(1f);
        polygonCollider2D.enabled = true;
        yield return new WaitForSeconds(1f);
        polygonCollider2D.enabled = false;
        yield return new WaitForSeconds(1f);
    }
    
    
    void GoonsAttack()
    {
        boss.position = teleportDestination;
        
        Rigidbody2D bossRb = boss.GetComponent<Rigidbody2D>();
        bossRb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        // Define the spawn area
        float minX = -13.4f;
        float maxX = 16.9f;
        float minY = -7.1f;
        float maxY = 166.3f;

        // Spawn four minions
        for (int i = 0; i < 4; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            GameObject gooner = Instantiate(Gooners, spawnPosition, Quaternion.identity);
            //Destroy(gooner, 8f);
            PirateGooners goons = Gooners.GetComponent<PirateGooners>();
            goons.pirateAttacks = this;
            
            goonerCount++;
        }
    }

    void CheckGoonerCount()
    {
        if (boss != null && goonerCount <= 0)
        {
            Rigidbody2D bossRb = boss.GetComponent<Rigidbody2D>();

            boss.position = returnDestination;
            if (bossRb != null)
            {
                bossRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
    
    public void MinionDestroyed()
    {
        goonerCount--;
        CheckGoonerCount();
    }
}
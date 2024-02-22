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
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            foreach (var BossCannonAttack in BossCannonAttack)
            {
                BossCannonAttack.ShootCannon();
            }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SpawnSummonShip();
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(SpinAttackCoroutine());
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            GoonsAttack();
        }

    }

    void SpawnSummonShip()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 spawnPosition = new Vector2(Random.Range(-12, 12), 13);
        GameObject Ghostship = Instantiate(GhostShip, spawnPosition, Quaternion.identity);

        Vector3 directionToPlayer = (player.transform.position - Ghostship.transform.position).normalized;

        Rigidbody2D shipRb = Ghostship.GetComponent<Rigidbody2D>();
        shipRb.velocity = directionToPlayer * travelSpeed;

        // Calculate the time it takes to reach the player based on distance and speed
        float timeToReachPlayer = Vector2.Distance(player.transform.position, Ghostship.transform.position) / travelSpeed;

        // Destroy after reaching the player
        Destroy(Ghostship, timeToReachPlayer+0.2f);
    }


    IEnumerator SpinAttackCoroutine()
    {
            GameObject spinAttack = Instantiate(SpinAttackIndicators, boss.position, Quaternion.identity);
            Destroy(spinAttack, 1f);
            yield return new WaitForSeconds(1f);
            polygonCollider2D.enabled = true;
            yield return new WaitForSeconds(1f);
            polygonCollider2D.enabled = false;
            yield return new WaitForSeconds(1f);
    }
    
    
    void GoonsAttack()
    {
        // Teleport the boss to the specified destination
        boss.position = teleportDestination;

        // Define the spawn area
        float minX = -14.4f;
        float maxX = 13.71f;
        float minY = -7.1f;
        float maxY = 6.62f;

        // Spawn four minions
        for (int i = 0; i < 4; i++)
        {
            // Generate a random position within the spawn area
            Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            // Instantiate the minion prefab at the random position
            GameObject gooner = Instantiate(Gooners, spawnPosition, Quaternion.identity);
            Destroy(gooner,2f);
            
            // Assign PirateAttacks script to PirateGooners script
            PirateGooners goons = Gooners.GetComponent<PirateGooners>();
            goons.pirateAttacks = this;

            // Increase the minion count
            goonerCount++;
        }
    }

    void CheckGoonerCount()
    {
        if (goonerCount <= 0)
        {
            boss.position = returnDestination;
        }
    }
    
    public void MinionDestroyed()
    {
        goonerCount--;
        CheckGoonerCount();
    }
}
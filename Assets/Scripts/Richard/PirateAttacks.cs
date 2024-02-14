using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateAttacks : MonoBehaviour
{
    public GameObject GhostShip;
    [SerializeField] private int travelSpeed = 4;
    
    public BossCannonAttack[] BossCannonAttack;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            foreach (var BossCannonAttack in BossCannonAttack)
            {
                BossCannonAttack.ShootCannon();
            }
        
        if (Input.GetKeyDown(KeyCode.O))

            SpawnSummonShip();

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
        Destroy(Ghostship, timeToReachPlayer+0.5f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateAttacks : MonoBehaviour
{
    private Animator _animator;
    
    public GameObject GhostShip;
    [SerializeField] private int travelSpeed = 4;
    public Transform boss;
    
    public GameObject SpinAttackIndicators;
    public GameObject spinAttackHitbox;
    
    public BossCannonAttack[] BossCannonAttack;
    
    public Vector3 teleportDestination;
    public Vector3 returnDestination;
    public GameObject Gooners;
    private int goonerCount;
    
    [SerializeField] float minX = -12.9f;
    [SerializeField] float maxX = 14f;
    [SerializeField] float minY = 160f;
    [SerializeField] float maxY = 176f;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        
        StartCoroutine(RandomAttackRoutine());
    }
    private IEnumerator RandomAttackRoutine()
    {
        while (!BossHealthLastima.isDead)
        {
            yield return new WaitForSeconds(7f);

            int randomNumber = Random.Range(1, 9);

            if (randomNumber >= 2 && randomNumber <= 4)
            {
                foreach (var BossCannonAttack in BossCannonAttack)
                {
                    BossCannonAttack.ShootCannon();
                }
                FindObjectOfType<AudioManager>().Play("Cannon");
            }
            else
            {
                SpawnSummonShip();
            }
        }
    }
    
    void SpawnSummonShip()
    {
        if (BossHealthLastima.isDead)
            return;
        FindObjectOfType<AudioManager>().Play("Ghostship");
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 spawnPosition = new Vector2(Random.Range(-30, 30), 193);
        
        _animator.SetTrigger("ShipCall");
        
        GameObject Ghostship = Instantiate(GhostShip, spawnPosition, Quaternion.identity);

        Vector3 directionToPlayer = (player.transform.position - Ghostship.transform.position).normalized;

        Rigidbody2D shipRb = Ghostship.GetComponent<Rigidbody2D>();
        shipRb.velocity = directionToPlayer * travelSpeed;

        // Calculate the time it takes to reach the player based on distance and speed
        float timeToReachPlayer = Vector2.Distance(player.transform.position, Ghostship.transform.position) / travelSpeed;
        Destroy(Ghostship, timeToReachPlayer+5f);
    }


    public IEnumerator SpinAttackCoroutine(GameObject parent)
    {
        GameObject spinAttack = Instantiate(SpinAttackIndicators, boss.position, Quaternion.identity);
        spinAttack.transform.parent = parent.transform;
        Destroy(spinAttack, 2f);
        yield return new WaitForSeconds(2f);
        spinAttackHitbox.SetActive(true);
        yield return new WaitForSeconds(1f);
        spinAttackHitbox.SetActive(false);
        yield return new WaitForSeconds(1f);
    }
    
    
    public void GoonsAttack()
    {
        if (BossHealthLastima.isDead)
            return;
        FindObjectOfType<AudioManager>().Play("Goonsattack");
        boss.position = teleportDestination;
        
        Rigidbody2D bossRb = boss.GetComponent<Rigidbody2D>();
        bossRb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        
        _animator.SetTrigger("CallMinions");

        // Spawn four minions
        for (int i = 0; i < 4; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            GameObject gooner = Instantiate(Gooners, spawnPosition, Quaternion.identity);
            Destroy(gooner, 30f);
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
            if (bossRb != null)
            { 
                boss.position = returnDestination;
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
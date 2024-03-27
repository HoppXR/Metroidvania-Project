using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IokoAttack : MonoBehaviour
{
    public Transform boss;
    private Transform player;
    public Rigidbody2D rb;
    
    public GameObject SpinAttackIndicators;
    public GameObject spinAttackHitbox;
    
    public GameObject cardProjectile;
    
    public GameObject frontRNGIndicators;
    public GameObject frontRNGHitbox;
    
    
    public GameObject blackHole;
    public Vector3 blackHoleSpawnLocation;
    public Vector3 bossTeleportLocation;
    public Vector3 bossReturnTeleportLocation;
    public int blackHoleCount;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player")?.transform;
        StartCoroutine(RandomAttackRoutine());
    }
    
    private IEnumerator RandomAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(7f);

            int randomNumber = Random.Range(1, 6);

            if (randomNumber >= 2 && randomNumber <= 3)
            {
                StartCoroutine(LuckOfTheDraw());
            }
            else if (randomNumber >= 4 && randomNumber <= 5)
            {
                StartCoroutine(DieDice());
            }
        }
    }
    
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(SpinAttackCoroutine(gameObject));
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(LuckOfTheDraw());
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(DieDice());
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            FinalGambit();
        }
    }*/
    
    public IEnumerator SpinAttackCoroutine(GameObject parent)
    {
        GameObject spinAttack = Instantiate(SpinAttackIndicators, boss.position, Quaternion.identity);
        spinAttack.transform.parent = parent.transform;
        Destroy(spinAttack, 3f);
        yield return new WaitForSeconds(3f);
        spinAttackHitbox.SetActive(true);
        yield return new WaitForSeconds(2f);
        spinAttackHitbox.SetActive(false);
        yield return new WaitForSeconds(1f);
    }


    IEnumerator LuckOfTheDraw()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        int[] myArray = { 5, 10, 15, 20 };

        for (int i = 0; i < 5; i++)
        {
            int randomIndex = Random.Range(0, myArray.Length);
            int randomSpeed = myArray[randomIndex];

            if (player != null)
            {
                GameObject card = Instantiate(cardProjectile, transform.position, Quaternion.identity);
                Vector3 directionToPlayer = (player.transform.position - card.transform.position).normalized;
                Rigidbody2D cardRb = card.GetComponent<Rigidbody2D>();
                cardRb.velocity = directionToPlayer * randomSpeed;
                Destroy(card, 10);
            }

            yield return new WaitForSeconds(1f);
        }
    }
    

    IEnumerator DieDice()
    {
        frontRNGIndicators.SetActive(true);
        yield return new WaitForSeconds(3f);
        frontRNGIndicators.SetActive(false);
        frontRNGHitbox.SetActive(true);
        yield return new WaitForSeconds(2f);
        frontRNGHitbox.SetActive(false);
        yield return new WaitForSeconds(1f);
    }
    
    public void FinalGambit()
    {
        boss.position = bossTeleportLocation;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        GameObject suck = Instantiate(blackHole, blackHoleSpawnLocation, Quaternion.identity);
        Destroy(suck, 10);
        SuckPlayer BlackHole = suck.GetComponent<SuckPlayer>();
        BlackHole.iokoAttack = this;
        blackHoleCount++;
    }

    public void CheckHoleState()
    {
        if (boss != null && blackHoleCount <= 0)
        {
            if (rb != null)
            { 
                boss.position = bossReturnTeleportLocation;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    public void BlackHoleDestroyed()
    {
        blackHoleCount--;
        CheckHoleState();
    }
}

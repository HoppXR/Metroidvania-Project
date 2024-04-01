using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IokoAttack : MonoBehaviour
{
    private Animator _animator;
    public Transform boss;
    private Transform player;
    public Rigidbody2D rb;
    
    public GameObject SpinAttackIndicators;
    public GameObject spinAttackHitbox;
    
    public GameObject cardProjectile;
    
    public GameObject frontRNGIndicators;
    public GameObject frontRNGHitbox;
    public int ranDamage;
    
    
    public GameObject blackHole;
    public Vector3 blackHoleSpawnLocation;
    public Vector3 bossTeleportLocation;
    public Vector3 bossReturnTeleportLocation;
    public int blackHoleCount;
    
    
    void Start()
    {
        _animator = GetComponent<Animator>();
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
                _animator.SetTrigger("CardThrow");
                yield return new WaitForSeconds(2.2f);
                
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
        int randomIndex = Random.Range(0, 6);
        int randomDamage = randomIndex;
        if (randomDamage == 0)
        {
            //dice roll 1 animation
            _animator.SetFloat("DiceRoll", 1);
            ranDamage = 3;
        }
        if (randomDamage == 1)
        {
            //dice roll 2 animation
            _animator.SetFloat("DiceRoll", 2);
            ranDamage = 6;
        }
        if (randomDamage == 2)
        {
            //dice roll 3 animation
            _animator.SetFloat("DiceRoll", 3);
            ranDamage = 9;
        }
        if (randomDamage == 3)
        {
            //dice roll 4 animation
            _animator.SetFloat("DiceRoll", 4);
            ranDamage = 12;
        }
        if (randomDamage == 4)
        {
            //dice roll 5 animation
            _animator.SetFloat("DiceRoll", 5);
            ranDamage = 15;
        }
        if (randomDamage == 5)
        {
            //dice roll 6 animation
            _animator.SetFloat("DiceRoll", 6);
            ranDamage = 18;
        }
        
        _animator.SetTrigger("DiceAttack");
        yield return new WaitForSeconds(2f);
        
        frontRNGIndicators.SetActive(true);
        yield return new WaitForSeconds(3.1f);
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

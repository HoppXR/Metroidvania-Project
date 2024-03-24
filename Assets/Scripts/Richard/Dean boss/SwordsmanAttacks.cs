using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsmanAttacks : MonoBehaviour
{
    public GameObject attackHitbox;
    public GameObject bigHitBoxAttackIndicator;
    public GameObject bigHitBoxAttackHitbox;
    


    public Transform boss;
    private Transform player;
    public Rigidbody2D rb;
    public GameObject swordProjectile;
    [SerializeField] private float attackRange = 1.5f;

    [SerializeField] private float lungeDuration;
    [SerializeField] private float lungeSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float smallDashDuration;
    [SerializeField] private float smallDashSpeed;


    [SerializeField] private float swordProjectileSpeed;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player")?.transform;
        //StartCoroutine(RandomAttackRoutine());
    }
    
    private IEnumerator RandomAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.2f);

            int randomNumber = Random.Range(1, 4);

            if (randomNumber == 1)
            {
                StartCoroutine(TripleDashAttack(gameObject));
                yield return new WaitForSeconds(2f);
            }
            else if (randomNumber == 2)
            {
                StartCoroutine(SlashThenRanged());
                yield return new WaitForSeconds(1f);
            }
            else if (randomNumber == 3)
            {
                StartCoroutine(DoubleSlash());
                yield return new WaitForSeconds(1f);
            }
            else if (randomNumber == 4)
            {
                StartCoroutine(TripleDashAttack(gameObject));
                yield return new WaitForSeconds(1f);
            }
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(DoubleSlash());
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(SlashThenRanged());
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(TripleDashAttack(gameObject));
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(StompAttack());
        }
    }

    public IEnumerator DoubleSlash() //lunge forward a bit and attack x2
    {
        StartCoroutine(SmallLunge());
        yield return new WaitForSeconds(0.2f);
        Attacking();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SmallLunge());
        yield return new WaitForSeconds(0.2f);
        Attacking();
    }

    IEnumerator SlashThenRanged() // Slash then throw (sword projectile x2)
    {
        Attacking();
        yield return new WaitForSeconds(0.5f);
        ShootSword();
        yield return new WaitForSeconds(1f);
        ShootSword();
        yield return new WaitForSeconds(0.5f);

    }

    IEnumerator TripleDashAttack(GameObject parent) //Dashes like eye of chthulu and release a attack at the end x3
    {
        StartCoroutine(Dash());
        yield return new WaitForSeconds(0.2f);
        bigHitBoxAttackIndicator.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bigHitBoxAttackIndicator.SetActive(false);
        BigHitBoxAttack();
        
        yield return new WaitForSeconds(0.5f);
        
        StartCoroutine(SmallDash());
        yield return new WaitForSeconds(0.2f);
        bigHitBoxAttackIndicator.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bigHitBoxAttackIndicator.SetActive(false);
        BigHitBoxAttack();
        
        yield return new WaitForSeconds(0.5f);
        
        StartCoroutine(SmallDash());
        yield return new WaitForSeconds(0.2f);
        bigHitBoxAttackIndicator.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bigHitBoxAttackIndicator.SetActive(false);
        BigHitBoxAttack();
        
        yield return new WaitForSeconds(3f);
    }


private IEnumerator SmallLunge()
    {
        if (player != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            rb.velocity = directionToPlayer * lungeSpeed;
        }

        yield return new WaitForSeconds(lungeDuration);
        
        rb.velocity = Vector2.zero;
        
    }

    private IEnumerator Dash()
    {
        if (player != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            rb.velocity = directionToPlayer * dashSpeed;
        }

        yield return new WaitForSeconds(dashDuration);
        
        rb.velocity = Vector2.zero;

    }
    private IEnumerator SmallDash()
    {
        if (player != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            rb.velocity = directionToPlayer * smallDashSpeed;
        }

        yield return new WaitForSeconds(smallDashDuration);
        
        rb.velocity = Vector2.zero;

    }
    
    


    public void ShootSword()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {

            GameObject sword = Instantiate(swordProjectile, transform.position, Quaternion.identity);
            Vector3 directionToPlayer = (player.transform.position - sword.transform.position).normalized;
            Rigidbody2D bulletRb = sword.GetComponent<Rigidbody2D>();
            bulletRb.velocity = directionToPlayer * swordProjectileSpeed;
            Destroy(sword, 10f);

        }
    }
    
    IEnumerator StompAttack ()
    {
        bigHitBoxAttackIndicator.SetActive(true);
        yield return new WaitForSeconds(2f);
        bigHitBoxAttackIndicator.SetActive(false);
        BigHitBoxAttack();
        yield return new WaitForSeconds(1f);
    }


    void Attacking()
    {
        attackHitbox.SetActive(true);
        if (player != null)
        {
            Vector2 directionToPlayer = player.position - transform.position;
            directionToPlayer.Normalize();
            attackHitbox.transform.localPosition = directionToPlayer * attackRange;
        }
        StartCoroutine(DeactivateAttack());
    }
    
    private IEnumerator DeactivateAttack()
    {
        yield return new WaitForSeconds(0.3f);
        attackHitbox.SetActive(false);
    }

    void BigHitBoxAttack()
    {
        bigHitBoxAttackHitbox.SetActive(true);
        StartCoroutine(DeactivateBigHitBoxAttack());
    }

    private IEnumerator DeactivateBigHitBoxAttack()
    {
        yield return new WaitForSeconds(0.4f);
        bigHitBoxAttackHitbox.SetActive(false);
    }
}

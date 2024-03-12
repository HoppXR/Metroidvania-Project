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


    [SerializeField] private float swordProjectileSpeed;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player")?.transform;
    }

    // Update is called once per frame
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
    }

    IEnumerator DoubleSlash() //lunge forward a bit and attack x2
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
        GameObject spinAttack = Instantiate(bigHitBoxAttackIndicator, boss.position, Quaternion.identity);
        spinAttack.transform.parent = parent.transform;
        Destroy(spinAttack, 0.5f);
        yield return new WaitForSeconds(0.5f);
        BigHitBoxAttack();
        
        yield return new WaitForSeconds(0.5f);
        
        StartCoroutine(Dash());
        yield return new WaitForSeconds(0.2f);
        GameObject spinAttack2 = Instantiate(bigHitBoxAttackIndicator, boss.position, Quaternion.identity);
        spinAttack2.transform.parent = parent.transform;
        Destroy(spinAttack2, 0.5f);
        yield return new WaitForSeconds(0.5f);
        BigHitBoxAttack();
        
        yield return new WaitForSeconds(0.5f);
        
        StartCoroutine(Dash());
        yield return new WaitForSeconds(0.2f);
        GameObject spinAttack3 = Instantiate(bigHitBoxAttackIndicator, boss.position, Quaternion.identity);
        spinAttack3.transform.parent = parent.transform;
        Destroy(spinAttack3, 0.5f);
        yield return new WaitForSeconds(0.5f);
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

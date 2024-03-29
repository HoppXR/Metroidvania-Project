using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;

public class AvesAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform boss;
    private Transform player;
    public Rigidbody2D rb;
    public GameObject pulsarObject1;
    public GameObject pulsarObject2;
    [SerializeField] private float spinActiveTime = 8f;
    
    public GameObject flameBreathIndicatorL;
    public GameObject flameBreathAttackL;
    public GameObject flameBreathIndicatorR;
    public GameObject flameBreathAttackR;
    
    [SerializeField] private float lungeDuration = 1f;
    [SerializeField] private float lungeSpeed = 30f;
    
    [SerializeField] private float dashDuration = 1.5f;
    [SerializeField] private float dashSpeed = 25f;
    public GameObject dashCollider;

    public GameObject meteor;
    [SerializeField] private float travelSpeed = 4;
    
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
            yield return new WaitForSeconds(4f);

            int randomNumber = Random.Range(1, 6);

            if (randomNumber >= 2 && randomNumber <= 3)
            {
                PulsarAttack();
                yield return new WaitForSeconds(1f);
            }
            else if (randomNumber >= 4 && randomNumber <= 5)
            {
                FireBreath();
                yield return new WaitForSeconds(1f);
            }
            else
            {
                StartCoroutine(MeteorAttack());
                yield return new WaitForSeconds(1f);
            }
        }
    }
    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PulsarAttack();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            FireBreath();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(ChargeDash());
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(MeteorAttack());
        }
    }*/

    void FireBreath()
    {
        Vector3 playerDirection = player.position - boss.position;
        float position = Vector3.Dot(playerDirection, boss.right);

        if (position > 0) // Player is on the right side
        {
            StartCoroutine(FlameBreathR());
        }
        else // Player is on the left side
        {
            StartCoroutine(FlameBreathL());
        }
        
    }

    IEnumerator MeteorAttack()
    {
        for (int i = 0; i < 4; i++)
        {
            float randomNumber = Random.Range(-0.5f, 1f);
            Vector2 spawnPosition = new Vector2(player.position.x, 13.01f);

            GameObject Meteor = Instantiate(meteor, spawnPosition, Quaternion.identity);

            Vector3 directionToPlayer = (player.position - Meteor.transform.position).normalized;

            Rigidbody2D MeteorRb = Meteor.GetComponent<Rigidbody2D>();
            MeteorRb.velocity = directionToPlayer * travelSpeed;

            // Calculate the time it takes to reach the player based on distance and speed
            float timeToReachPlayer = Vector2.Distance(player.position, Meteor.transform.position) / travelSpeed;
            Destroy(Meteor, timeToReachPlayer + randomNumber);

            yield return new WaitForSeconds(1.5f);
        }
    }

    public IEnumerator ChargeDash()
    {
        StartCoroutine(SmallLunge());
        yield return new WaitForSeconds(1f);
        StartCoroutine(Dash());
    }

    void PulsarAttack()
    {
        pulsarObject1.SetActive(true);
        pulsarObject2.SetActive(true);
        
        Invoke("DisablePulsarAttack", spinActiveTime);
    }

    void DisablePulsarAttack()
    {
        pulsarObject1.SetActive(false);
        pulsarObject2.SetActive(false);
    }
    
    
    
    IEnumerator FlameBreathL()
    {
        flameBreathIndicatorL.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        flameBreathIndicatorL.SetActive(false);
        flameBreathAttackL.SetActive(true);
        StartCoroutine(DeactivateFlameBreathL());
    }

    private IEnumerator DeactivateFlameBreathL()
    {
        yield return new WaitForSeconds(2f);
        flameBreathAttackL.SetActive(false);
    }
    IEnumerator FlameBreathR()
    {
        flameBreathIndicatorR.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        flameBreathIndicatorR.SetActive(false);
        flameBreathAttackR.SetActive(true);
        StartCoroutine(DeactivateFlameBreathR());
    }

    private IEnumerator DeactivateFlameBreathR()
    {
        yield return new WaitForSeconds(2f);
        flameBreathAttackR.SetActive(false);
    }
    
    private IEnumerator SmallLunge()
    {
        if (player != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            rb.velocity = -directionToPlayer * lungeSpeed;
        }

        yield return new WaitForSeconds(lungeDuration);
        
        rb.velocity = Vector2.zero;
        
    }
    
    private IEnumerator Dash()
    {
        dashCollider.SetActive(true);
        if (player != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            rb.velocity = directionToPlayer * dashSpeed;
        }

        yield return new WaitForSeconds(dashDuration);
        dashCollider.SetActive(false);
        rb.velocity = Vector2.zero;

    }
}
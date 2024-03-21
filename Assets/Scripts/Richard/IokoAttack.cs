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

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player")?.transform;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(SpinAttackCoroutine(gameObject));
        }

        if (Input.GetKeyDown(KeyCode.O))
        {

        }

        if (Input.GetKeyDown(KeyCode.I))
        {

        }

        if (Input.GetKeyDown(KeyCode.U))
        {

        }
    }
    
    IEnumerator SpinAttackCoroutine(GameObject parent)
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
}

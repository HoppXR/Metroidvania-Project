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
    public GameObject pulsarObject3;
    [SerializeField] private float spinTime = 8f;
    
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
            PulsarAttack();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {

        }

        if (Input.GetKeyDown(KeyCode.I))
        {

        }

        if (Input.GetKeyDown(KeyCode.I))
        {

        }
    }

    IEnumerator FireBreath()
    {
        return null;
    }

    void MeteorAttack()
    {
        
    }

    void ChargeDash()
    {
        
    }

    void PulsarAttack()
    {
        pulsarObject1.SetActive(true);
        pulsarObject2.SetActive(true);
        pulsarObject3.SetActive(true);
        
        Invoke("DisablePulsarAttack", spinTime);
    }

    void DisablePulsarAttack()
    {
        pulsarObject1.SetActive(false);
        pulsarObject2.SetActive(false);
        pulsarObject3.SetActive(false);
    }
    
}
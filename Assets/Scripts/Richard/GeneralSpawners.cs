using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSpawners : MonoBehaviour
{
    public GameObject[] tutorialGoons;
    
    public Vector3[] spawnPositions;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialGoons.Length != spawnPositions.Length)
            {
                Debug.LogError("The number of prefabs and spawn positions must match!");
                return;
            }
            
            for (int i = 0; i < tutorialGoons.Length; i++)
            {
                GameObject goons = Instantiate(tutorialGoons[i], spawnPositions[i], Quaternion.identity);

                hasTriggered = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateGooners : MonoBehaviour
{
    // Reference to the boss's PirateAttacks script
    public PirateAttacks pirateAttacks;

    void Start()
    {
        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");
        if (bossObject != null)
        {
            pirateAttacks = bossObject.GetComponent<PirateAttacks>();
            if (pirateAttacks == null)
            {
                Debug.LogWarning("PirateAttacks component not found on object with tag 'Boss'.");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'Boss' found in the scene.");
        }
    }

    private void OnDestroy()
    {
        if (pirateAttacks != null)
        {
            pirateAttacks.MinionDestroyed();
        }
        else
        {
            Debug.LogWarning("PirateAttacks reference is null in PirateGooners script!");
        }
    }
}

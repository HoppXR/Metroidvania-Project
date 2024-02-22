using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateGooners : MonoBehaviour
{
    // Reference to the boss's PirateAttacks script
    public PirateAttacks pirateAttacks;

    private void OnDestroy()
    {
        pirateAttacks.MinionDestroyed();
    }
}
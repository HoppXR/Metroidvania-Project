using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    public EnableAfterDestroy enableAfterDestroy; // Reference to EnableAfterDestroy script

    private void OnDestroy()
    {
        if (enableAfterDestroy != null)
        {
            enableAfterDestroy.IncreaseCounter();
        }
    }
}

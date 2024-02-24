using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnabler : MonoBehaviour
{
    public GameObject objectToEnable1;
    public GameObject objectToEnable2;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Enables the first object
            if (objectToEnable1 != null)
            {
                objectToEnable1.SetActive(true);
            }

            // Enables the second object
            if (objectToEnable2 != null)
            {
                objectToEnable2.SetActive(true);
            }
        }
    }
}

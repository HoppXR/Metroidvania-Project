using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAfterDestroy : MonoBehaviour
{
    public int counter;
    public GameObject objectToDisable;

    private void Start()
    {
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(true);
        }
    }

    public void IncreaseCounter()
    {
        counter++;

        if (counter >= 2)
        {
            if (objectToDisable != null)
            {
                objectToDisable.SetActive(false);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveObject : MonoBehaviour
{
    public float interactionDistance = 3f; 
    public KeyCode interactionKey = KeyCode.E;
    public TextMeshProUGUI saveText;

    private DataPersistenceManager persistenceManager;
    private GameObject player;

    private void Start()
    {
        persistenceManager = DataPersistenceManager.instance;
        player = GameObject.FindGameObjectWithTag("Player"); 
        saveText.gameObject.SetActive(false); 
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= interactionDistance)
        {
            saveText.gameObject.SetActive(true);
            saveText.text = "Save (E)";

            if (Input.GetKeyDown(interactionKey))
            {
                if (persistenceManager != null)
                {
                    persistenceManager.ManualSave();
                }
                else
                {
                    Debug.LogWarning("DataPersistenceManager is not assigned.");
                }
            }
        }
        else
        {
            saveText.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}

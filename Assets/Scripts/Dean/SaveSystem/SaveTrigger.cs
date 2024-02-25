using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private DataPersistenceManager persistenceManager;
    public GameObject saveIcon; 
    public float displayDuration = 3.0f; 

    private void Start()
    {
        persistenceManager = DataPersistenceManager.instance;
        if (persistenceManager == null)
        {
            Debug.LogWarning("DataPersistenceManager is not assigned.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SaveGame();
        }
    }

    private void SaveGame()
    {
        if (persistenceManager != null)
        {
            persistenceManager.ManualSave();
            Debug.Log("Game saved!");

            if (saveIcon != null)
            {
                saveIcon.SetActive(true);
                Invoke("DisableSaveIcon", displayDuration);
            }
        }
    }

    private void DisableSaveIcon()
    {
        if (saveIcon != null)
        {
            saveIcon.SetActive(false);
        }
    }
}

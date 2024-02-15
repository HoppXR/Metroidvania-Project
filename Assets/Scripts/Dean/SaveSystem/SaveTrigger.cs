using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private DataPersistenceManager persistenceManager;

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
        }
    }
}

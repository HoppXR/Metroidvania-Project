using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private DataPersistenceManager _saveData;
    
    public GameObject deathScreenCanvas;

    private void Start()
    {
        _saveData = FindFirstObjectByType<DataPersistenceManager>();
        
        deathScreenCanvas.SetActive(false);
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void EnableGameOverMenu()
    {
        deathScreenCanvas.SetActive(true);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        _saveData.LoadGame();
        
        deathScreenCanvas.SetActive(false);
    }
    
    private void OnEnable()
    {
        HealthManager.OnPlayerDeath += EnableGameOverMenu;
    }

    private void OnDisable()
    {
        HealthManager.OnPlayerDeath -= EnableGameOverMenu;
    }
}

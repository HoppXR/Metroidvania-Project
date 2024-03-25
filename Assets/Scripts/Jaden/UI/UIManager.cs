using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private DataPersistenceManager _saveData;
    
    public GameObject deathScreenCanvas;
    
    [Header("First Selected")]
    [SerializeField] private GameObject retry;
    [SerializeField] private GameObject mainMenu;

    private void Start()
    {
        _saveData = FindFirstObjectByType<DataPersistenceManager>();
        
        deathScreenCanvas.SetActive(false);
    }
    
    public void MainMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        
        SceneManager.LoadScene("MainMenu");
    }
    
    public void EnableGameOverMenu()
    {
        deathScreenCanvas.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(retry);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        _saveData.LoadGame();
        
        deathScreenCanvas.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(null);
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

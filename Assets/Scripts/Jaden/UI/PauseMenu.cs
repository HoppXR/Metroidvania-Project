using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private DataPersistenceManager _saveData;
    
    public GameObject pauseMenuCanvas;

    public static bool isPaused;
    
    void Start()
    {
        _saveData = FindFirstObjectByType<DataPersistenceManager>();
        
        pauseMenuCanvas.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveGame()
    {
        _saveData.SaveGame();
    }
}

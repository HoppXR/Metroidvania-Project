using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private DataPersistenceManager _saveData;
    
    public GameObject pauseMenuCanvas;

    public static bool isPaused;
    
    [Header("First Selected")]
    [SerializeField] private GameObject resume;
    [SerializeField] private GameObject saveGame;
    [SerializeField] private GameObject mainMenu;
    
    
    void Start()
    {
        _saveData = FindFirstObjectByType<DataPersistenceManager>();
        
        pauseMenuCanvas.SetActive(false);
    }
    
    void Update()
    {
        if (InputReader.Instance.MenuOpenCloseInput)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        
        pauseMenuCanvas.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(resume);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        
        pauseMenuCanvas.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void GoToMainMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveGame()
    {
        _saveData.SaveGame();
    }
}

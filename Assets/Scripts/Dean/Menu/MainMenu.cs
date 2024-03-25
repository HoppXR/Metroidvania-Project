using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    
    public void PlayGame()
    {
        levelLoader.LoadNextLevel();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

}

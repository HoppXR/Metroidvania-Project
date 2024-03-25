using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    private float _transitionTime = 1.417f;

    public void LoadNextLevel()
    {
       StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
        
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}

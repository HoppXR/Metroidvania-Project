using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeIntroScene : MonoBehaviour
{
    [SerializeField] private float nextSceneTime;
    public Animator transition;

    [SerializeField] private float changeTime;

    private bool transitionStarted = false;

    private Dictionary<string, int> sceneIndexMap = new Dictionary<string, int>();

    private void Start()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            sceneIndexMap.Add(sceneName, i);
        }

        transition.SetTrigger("End");
    }

    private void Update()
    {
        changeTime -= Time.deltaTime;

        if (changeTime <= 0 && !transitionStarted)
        {
            transitionStarted = true;
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        LoadSceneByName("PortalRoom");
    }

    public void LoadSceneByName(string sceneName)
    {
        if (sceneIndexMap.ContainsKey(sceneName))
        {
            int buildIndex = sceneIndexMap[sceneName];
            StartCoroutine(ELoadLevel(buildIndex));
        }
        else
        {
            Debug.LogError("Scene " + sceneName + " not found in build settings.");
        }
    }

    IEnumerator ELoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(nextSceneTime);

        SceneManager.LoadScene(levelIndex);
    }
}

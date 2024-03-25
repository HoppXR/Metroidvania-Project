using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public string sceneToLoad; 
    public float interactRange = 1.5f; 
    public GameObject interactionUI;
    public LevelLoader levelLoader;
    private float _transitionTime = 1.417f;

    private bool playerInRange = false;

    private void Start()
    {
        interactionUI.SetActive(false); 
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) && playerInRange)
        {
            StartCoroutine(EnterPortal());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionUI.SetActive(true); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionUI.SetActive(false); 
        }
    }
   
    IEnumerator EnterPortal()
    {
        levelLoader.transition.SetTrigger("Start");

        yield return new WaitForSeconds(_transitionTime);

        SceneManager.LoadScene(sceneToLoad);
    }
}

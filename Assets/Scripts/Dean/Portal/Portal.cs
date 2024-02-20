using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneToLoad; 
    public float interactRange = 1.5f; 
    public GameObject interactionUI;

    private bool playerInRange = false;

    private void Start()
    {
        interactionUI.SetActive(false); 
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            EnterPortal();
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

    private void EnterPortal()
    {
        SceneManager.LoadScene(sceneToLoad); 
    }
}

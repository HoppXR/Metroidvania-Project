using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;

public class SaveObject : MonoBehaviour
{
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;
    public KeyCode proceedKey = KeyCode.Space; 
    public TextMeshProUGUI saveText;
    public GameObject saveCanvas;
    public AudioClip saveSound;

    private DataPersistenceManager persistenceManager;
    private GameObject player;
    private AudioSource audioSource;
    private bool isSaving = false;

    private void Start()
    {
        persistenceManager = DataPersistenceManager.instance;
        player = GameObject.FindGameObjectWithTag("Player");
        saveText.gameObject.SetActive(false);
        saveCanvas.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isSaving && Vector3.Distance(transform.position, player.transform.position) <= interactionDistance)
        {
            saveText.gameObject.SetActive(true);
            saveText.text = "Save (E)";

            if (Input.GetKeyDown(interactionKey) || Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                StartCoroutine(SaveGame());
            }
        }
        else
        {
            saveText.gameObject.SetActive(false);
        }

        if (isSaving && (Input.GetKeyDown(proceedKey) || Gamepad.current.buttonSouth.wasPressedThisFrame))
        {
            saveCanvas.SetActive(false);
            isSaving = false;
        }
    }

    private IEnumerator SaveGame()
    {
        isSaving = true;
        saveCanvas.SetActive(true);
        audioSource.PlayOneShot(saveSound);

        yield return new WaitUntil(() => Input.GetKeyDown(proceedKey) || Gamepad.current.buttonSouth.wasPressedThisFrame);

        saveCanvas.SetActive(false);
        isSaving = false;

        if (persistenceManager != null)
        {
            persistenceManager.ManualSave();
        }
        else
        {
            Debug.LogWarning("DataPersistenceManager is not assigned.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}

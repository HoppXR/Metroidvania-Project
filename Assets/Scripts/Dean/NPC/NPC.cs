using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    private PlayerMovement _thePlayer;

    [SerializeField] GameObject promptText;
    [SerializeField] GameObject particleEffect; 
    [SerializeField] bool shouldDestroyAfterDialogue = false; 

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;
    private bool isTyping;

    public Animator contButtonAnimator;
    public float wordSpeed;
    public bool playerIsClose;
    private Coroutine typingCoroutine;

    void Start()
    {
        _thePlayer = FindFirstObjectByType<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && !isTyping)
        {
            _thePlayer.CanMoveFalse();
            _thePlayer.GrappleHook.CanGrappleFalse();

            if (dialoguePanel.activeInHierarchy)
            {
                ZeroText();
                promptText.SetActive(true);
            }
            else
            {
                dialoguePanel.SetActive(true);
                promptText.SetActive(false);

                index = 0;

                typingCoroutine = StartCoroutine(Typing());

                contButtonAnimator.gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && dialoguePanel.activeInHierarchy && !isTyping)
        {
            NextLine();
        }
    }

    public void NextLine()
    {
        if (isTyping)
            return;

        contButtonAnimator.gameObject.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            typingCoroutine = StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
            promptText.SetActive(true);

            if (shouldDestroyAfterDialogue)
            {
                DestroyNPC();
            }
        }
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        isTyping = false;
        
        _thePlayer.CanMoveTrue();
        _thePlayer.GrappleHook.CanGrappleTrue();

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = null;
    }

    IEnumerator Typing()
    {
        isTyping = true;
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
        contButtonAnimator.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            if (!dialoguePanel.activeInHierarchy)
                promptText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            promptText.SetActive(false);
            if (dialoguePanel.activeInHierarchy)
            {
                ZeroText();
            }
        }
    }

    void DestroyNPC()
    {
        if (particleEffect != null)
        {
            GameObject particleInstance = Instantiate(particleEffect, transform.position, Quaternion.identity);

            Destroy(particleInstance, 2f);
        }

        Destroy(gameObject);
    }
}

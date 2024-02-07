using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    private PlayerMovement thePlayer;

    [SerializeField] GameObject promptText;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;
    private bool isTyping;

    public Animator contButtonAnimator; // Reference to the Animator component of the continue button
    public float wordSpeed;
    public bool playerIsClose;
    private Coroutine typingCoroutine;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && !isTyping)
        {
            thePlayer.CanMoveFalse();

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

                // Enable the animation object
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

        // Disable the animation object
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
        }
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        thePlayer.CanMoveTrue();
        isTyping = false;

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
        // Enable the animation object
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
}

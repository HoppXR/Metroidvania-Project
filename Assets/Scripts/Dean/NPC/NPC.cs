using System.Collections;
using System.Collections.Generic;
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

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose;

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
                zeroText();
                promptText.SetActive(true); 
            }
            else
            {
                dialoguePanel.SetActive(true);
                promptText.SetActive(false); 
                StartCoroutine(Typing());
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

        contButton.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
            promptText.SetActive(true); 
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);

        thePlayer.CanMoveTrue();
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
        contButton.SetActive(true);
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
        }
    }
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPCResponse2 : MonoBehaviour
{
    [SerializeField] private bool stopMovement;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;
    private bool isTyping;

    public Animator contButtonAnimator;
    public float wordSpeed;
    private Coroutine typingCoroutine;

    public AudioSource typingSound;
    public AudioClip typingClip;

    public BoxCollider2D boxCollider2D; // Reference to Box Collider 2D component

    private PlayerMovement _thePlayer;
    public PlayerResponse2 PlayerResponse2;

    private void Start()
    {
        dialoguePanel.SetActive(false);
        _thePlayer = FindFirstObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) && dialoguePanel.activeInHierarchy && !isTyping)
        {
            NextLine();
        }
    }

    public void StartResponse()
    {
        if (stopMovement)
        {
            _thePlayer.CanMoveFalse();
            _thePlayer.GrappleHook.CanGrappleFalse();
        }
        dialoguePanel.SetActive(true);
        index = 0;
        typingCoroutine = StartCoroutine(Typing());
        contButtonAnimator.gameObject.SetActive(true);
    }

    private void NextLine()
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
            if (PlayerResponse2 != null && PlayerResponse2.gameObject.activeInHierarchy && PlayerResponse2.enabled)
            {
                PlayerResponse2.StartResponse();
            }
        }
    }

    private void ZeroText()
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

        if (boxCollider2D != null) // Disable Box Collider 2D if it exists
        {
            boxCollider2D.enabled = false;
        }
    }

    IEnumerator Typing()
    {
        isTyping = true;
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            if (typingSound != null && typingClip != null)
            {
                typingSound.PlayOneShot(typingClip);
            }
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
        contButtonAnimator.gameObject.SetActive(true);
    }
}

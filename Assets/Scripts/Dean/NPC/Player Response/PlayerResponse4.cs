using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerResponse4 : MonoBehaviour
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

    private PlayerMovement _thePlayer;

    public NPCResponse4 NPCResponse4;
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
            if (NPCResponse4 != null && NPCResponse4.gameObject.activeInHierarchy && NPCResponse4.enabled)
            {
                NPCResponse4.StartResponse();
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

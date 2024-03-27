using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CollisionDialogue : MonoBehaviour
{
    [SerializeField] private bool stopMovement = true;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private string[] dialogue;
    [SerializeField] private float wordSpeed = 0.05f;
    [SerializeField] private bool shouldDestroyAfterDialogue = false;
    [SerializeField] private GameObject particleEffect;

    private int index;
    private bool isTyping;
    private Coroutine typingCoroutine;
    private PlayerMovement _thePlayer;

    public Animator contButtonAnimator;
    public AudioSource typingSound;
    public AudioClip typingClip;
    public PlayerDialogue playerDialogue;
    public Collider2D dialogueCollider;

    public Button skipButton;

    private void Start()
    {
        _thePlayer = FindFirstObjectByType<PlayerMovement>();

        skipButton.onClick.AddListener(SkipDialogue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (stopMovement)
            {
                _thePlayer.CanMoveFalse();
            }

            if (dialoguePanel.activeInHierarchy)
            {
                ZeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);

                index = 0;

                typingCoroutine = StartCoroutine(Typing());

                contButtonAnimator.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) && dialoguePanel.activeInHierarchy && !isTyping)
        {
            NextLine();
        }
    }

    private void SkipDialogue()
    {
        index = dialogue.Length - 1;
        NextLine();

        if (playerDialogue != null)
        {
            playerDialogue.enabled = false;
        }

        if (dialogueCollider != null)
        {
            dialogueCollider.enabled = false;
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

            if (playerDialogue != null && playerDialogue.gameObject.activeInHierarchy && playerDialogue.enabled)
            {
                playerDialogue.StartDialogue();
            }

            if (shouldDestroyAfterDialogue)
            {
                if (particleEffect != null)
                {
                    GameObject particleInstance = Instantiate(particleEffect, transform.position, Quaternion.identity);

                    Destroy(particleInstance, 2f);
                }

                Destroy(gameObject);
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

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = null;
    }

    private IEnumerator Typing()
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (dialoguePanel.activeInHierarchy)
            {
                ZeroText();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    [SerializeField] Transform posToGo;
    [SerializeField] GameObject keyTxt;
    [SerializeField] Image fadeImage;
    private float fadeDuration = 1.0f;
    private float teleportDelay = 1.5f;

    bool playerDetected;
    GameObject playerGO;

    // Start is called before the first frame update
    void Start()
    {
        playerDetected = false;
        fadeImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(Transition());
            }
        }
    }

    IEnumerator Transition()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0); // Start with transparent

        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(Color.clear, Color.black, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        fadeImage.color = new Color(0,0,0,255);

        Vector3 startPos = playerGO.transform.position;
        Vector3 targetPos = posToGo.position;

        playerGO.transform.position = targetPos;
        
        yield return new WaitForSeconds(teleportDelay);

        elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(Color.black, Color.clear, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = true;
            playerGO = collision.gameObject;
            keyTxt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = false;
            keyTxt.SetActive(false);
        }
    }
}
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip roomBackgroundMusic;

    private AudioSource audioSource; 

    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource.clip != roomBackgroundMusic)
            {
                audioSource.clip = roomBackgroundMusic;
                audioSource.Play(); 
            }
        }
    }
}

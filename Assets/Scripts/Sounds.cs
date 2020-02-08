using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip destroySound;
    public AudioClip gameOverSound;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDestroySound()
    {
        audioSource.PlayOneShot(destroySound, 0.1f);
    }

    public void PlayGameOverSound()
    {
        audioSource.PlayOneShot(gameOverSound, 0.5f);
    }
}

using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip music;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        if (music != null)
        {
            audioSource.clip = music;
            audioSource.Play();
        }
    }
}
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip music;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource.Play();
    }
}
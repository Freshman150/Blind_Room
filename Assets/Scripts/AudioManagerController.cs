using UnityEngine;

public enum Audio
{
    KEY,
    FOOTSTEP
}

public class AudioManagerController : MonoBehaviour
{
    
    private static AudioManagerController instance;

    [SerializeField] private AudioClip[] mAudioClip;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayAudio(Audio audio, float volume = 1f)
    {
        instance.audioSource.PlayOneShot(instance.mAudioClip[(int)audio], volume);
    }
}

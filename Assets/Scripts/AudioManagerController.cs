using UnityEngine;

public enum Audio
{
    KEY,
    FOOTSTEP,
    UNLOCKDOOR,
    DOORSQUEAK,
    METALSOUND,
    STONESCRATCH,
    WOODSCRATCH,
    SPARK
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

    public static void PlayAudioOnce(Audio audio, float volume = 1f)
    {
        if (instance.audioSource.loop)
        {
            instance.audioSource.loop = false;
        }
        instance.audioSource.PlayOneShot(instance.mAudioClip[(int)audio], volume);
    }

    public static void PlayAudioLoop(Audio audio, float volume = 1f)
    {
        if (!instance.audioSource.loop)
        {
            instance.audioSource.loop = true;
        }
        instance.audioSource.clip = instance.mAudioClip[(int)audio];
        instance.audioSource.volume = volume;
        instance.audioSource.Play();
    }

    public static void Stop()
    {
        instance.audioSource.Stop();
    }
}

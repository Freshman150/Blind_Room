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
    SPARK,
    STARTLEVEL,
    LABBEGIN,
    LABEND
}

public class AudioManagerController : MonoBehaviour
{
    
    private static AudioManagerController instance;

    [SerializeField] private AudioClip[] mAudioClip;

    private AudioSource[] audioSources;

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
        audioSources = GetComponents<AudioSource>();
    }

    public static void PlayAudioOnce(Audio audio, float volume = 1f)
    {
        if (instance.audioSources[0].loop)
        {
            instance.audioSources[0].loop = false;
        }
        instance.audioSources[0].pitch = Random.Range(0.2f, volume);
        instance.audioSources[0].PlayOneShot(instance.mAudioClip[(int)audio], volume);
    }

    public static void PlayAudioLoop(Audio audio, float volume = 1f)
    {
        if (!instance.audioSources[0].loop)
        {
            instance.audioSources[0].loop = true;
        }
        instance.audioSources[0].clip = instance.mAudioClip[(int)audio];
        instance.audioSources[0].volume = volume;
        instance.audioSources[0].Play();
    }

    public static void PlayFootsteps()
    {
        instance.audioSources[1].pitch = Random.Range(0.5f, 1f);
        instance.audioSources[1].PlayOneShot(instance.mAudioClip[(int)Audio.FOOTSTEP], 1f);
    }

    public static void PlayLabBegin()
    {
        instance.audioSources[2].PlayOneShot(instance.mAudioClip[(int)Audio.LABBEGIN], 1f);
    }

    public static void PlayLabEnd()
    {
        instance.audioSources[2].PlayOneShot(instance.mAudioClip[(int)Audio.LABEND], 1f);
    }

    public static void Stop()
    {
        instance.audioSources[0].Stop();
    }
}

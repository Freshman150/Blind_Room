using UnityEngine;

// Enum qui définit les types de sons disponibles.
// Cela permet d’appeler un son par nom lisible (ex: Audio.KEY) au lieu d’un index brut.
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
    // Instance unique (Singleton) de l'AudioManager
    private static AudioManagerController instance;

    // Tableau de clips audio à assigner dans l’inspector (un clip par type d’Audio)
    [SerializeField] private AudioClip[] mAudioClip;

    // Tableau d’AudioSources (plusieurs sources pour jouer plusieurs sons en parallèle)
    private AudioSource[] audioSources;

    // Singleton : garde une seule instance dans toute l’application
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre les scènes
        }
        else
        {
            Destroy(gameObject); // Supprime les doublons si une autre instance existe
        }
    }

    // Initialise les AudioSources attachés à ce GameObject
    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    // Joue un son une seule fois avec un pitch aléatoire
    public static void PlayAudioOnce(Audio audio, float volume = 1f)
    {
        if (instance.audioSources[0].loop)
        {
            instance.audioSources[0].loop = false; // On s'assure que l’audio ne boucle pas
        }
        instance.audioSources[0].pitch = Random.Range(0.2f, volume); // Pitch aléatoire
        instance.audioSources[0].PlayOneShot(instance.mAudioClip[(int)audio], volume);
    }

    // Joue un son en boucle jusqu’à ce qu’on l’arrête manuellement
    public static void PlayAudioLoop(Audio audio, float volume = 1f)
    {
        if (!instance.audioSources[0].loop)
        {
            instance.audioSources[0].loop = true;
        }
        instance.audioSources[0].clip = instance.mAudioClip[(int)audio]; // Affecte le clip à jouer
        instance.audioSources[0].volume = volume;
        instance.audioSources[0].Play(); // Démarre la lecture en boucle
    }

    // Joue un son de pas avec un pitch légèrement aléatoire
    public static void PlayFootsteps()
    {
        instance.audioSources[1].pitch = Random.Range(0.5f, 1f);
        instance.audioSources[1].PlayOneShot(instance.mAudioClip[(int)Audio.FOOTSTEP], 1f);
    }

    // Joue le son d’ambiance du début du labyrinthe
    public static void PlayLabBegin()
    {
        instance.audioSources[2].PlayOneShot(instance.mAudioClip[(int)Audio.LABBEGIN], 1f);
    }

    // Joue le son d’ambiance de fin du labyrinthe
    public static void PlayLabEnd()
    {
        instance.audioSources[2].PlayOneShot(instance.mAudioClip[(int)Audio.LABEND], 1f);
    }

    // Stoppe la lecture de l'audio en cours sur la source 0 (utile pour stopper un son en boucle)
    public static void Stop()
    {
        instance.audioSources[0].Stop();
    }
}

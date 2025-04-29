using System.Collections;
using UnityEngine;

//only used in the main menu
public class DummyNarrator : MonoBehaviour
{
    public static DummyNarrator Instance { get; private set; }

    //the 3 differents speech clips played (when the user do what the variable name says)
    public AudioClip _beginningSpeech;
    public AudioClip _hoverSpeech;
    public AudioClip _onClickSpeech;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Play speech for the main menu 
    public IEnumerator PlaySpeech(AudioClip audio)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource && !audioSource.isPlaying)
        {
            audioSource.clip = audio;
            audioSource.Play();
            yield return new WaitForSeconds(audio.length + 1f);
        }
    }
}
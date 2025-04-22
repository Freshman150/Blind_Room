using System.Collections;
using UnityEngine;

public class DummyNarrator : MonoBehaviour
{
    public static DummyNarrator Instance { get; private set; }

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
    private void Start()
    {
        StartCoroutine(PlaySpeech(_beginningSpeech));
    }

    public void ReadOption(string option)
    {
        Debug.Log($"Narrator: {option}");
    }

    public IEnumerator PlaySpeech(AudioClip audio)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.clip = audio;
            audioSource.Play();
            yield return new WaitForSeconds(audio.length + 1f);
        }
    }
}
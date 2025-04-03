using UnityEngine;
public class TongueClickDetector : MonoBehaviour
{
    public AudioSource audioSource;
    public int sampleSize = 1024;
    public float frequencyThreshold = 2000f; // 2 kHz minimum
    public float volumeThreshold = 0.02f; // Ajuste selon les tests

    private float[] spectrumData;

    void Start()
    {
        if (!AndroidRuntimePermissions.CheckPermission("android.permission.RECORD_AUDIO") )
        {
            AndroidRuntimePermissions.RequestPermission("android.permission.RECORD_AUDIO");
        }
        spectrumData = new float[sampleSize];
        audioSource.clip = Microphone.Start(null, true, 10, 44100);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { } // Attendre le début
        audioSource.Play();
    }

    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        DetectTongueClick();
    }

    void DetectTongueClick()
    {
        int minIndex = Mathf.FloorToInt(frequencyThreshold / (44100f / sampleSize));
        float maxVolume = 0f;

        for (int i = minIndex; i < spectrumData.Length; i++)
        {
            if (spectrumData[i] > maxVolume)
            {
                maxVolume = spectrumData[i];
            }
        }

        if (maxVolume > volumeThreshold)
        {
            //Debug.Log("Claquement détecté !");
            LidarSystem.TriggerClickDetected();
        }
    }
}
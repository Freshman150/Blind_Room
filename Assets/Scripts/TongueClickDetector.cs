using UnityEngine;

public class TongueClickDetector : MonoBehaviour
{
    public int sampleWindow = 1024;  
    public float frequencyThreshold = 2000f; 
    public float volumeThreshold = 0.02f; 
    public float cooldownTime = 0.5f; // ⏳ Time to wait before detecting a new click

    private AudioClip micClip;
    private string micName;
    private bool micInitialized = false;
    private float[] waveData;
    private float[] spectrumData; 

    private float lastClickTime = -Mathf.Infinity; // ⏳ Tracks the last click time

    void Start()
    {
        if (!AndroidRuntimePermissions.CheckPermission("android.permission.RECORD_AUDIO"))
        {
            Debug.Log("Permission non accordée, demande en cours...");
            AndroidRuntimePermissions.RequestPermission("android.permission.RECORD_AUDIO");
        }

        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            micClip = Microphone.Start(micName, true, 10, 44100);
            while (Microphone.GetPosition(micName) <= 0) { }
            micInitialized = true;
            Debug.Log($"Microphone '{micName}' initialisé avec succès.");
        }
        else
        {
            Debug.LogError("Aucun microphone détecté !");
        }

        waveData = new float[sampleWindow];
        spectrumData = new float[sampleWindow];
    }

    void Update()
    {
        if (!micInitialized || micClip == null) return;

        int clipPosition = Microphone.GetPosition(micName);
        DetectTongueClick(clipPosition);
    }

    void DetectTongueClick(int clipPosition)
    {
        if (Time.time - lastClickTime < cooldownTime) return; // 🚫 Skip if on cooldown

        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0) return;

        micClip.GetData(waveData, startPosition);
        FFT(waveData, spectrumData);

        float maxVolume = 0f;
        int minIndex = Mathf.FloorToInt(frequencyThreshold / (44100f / sampleWindow));

        for (int i = minIndex; i < spectrumData.Length; i++)
        {
            if (spectrumData[i] > maxVolume)
            {
                maxVolume = spectrumData[i];
            }
        }

        Debug.Log($"Max Volume: {maxVolume}");

        if (maxVolume > volumeThreshold)
        {
            Debug.Log("Claquement détecté !");
            lastClickTime = Time.time; // 🕒 Set cooldown start time
            LidarSystem.TriggerClickDetected();
        }
    }

    void FFT(float[] data, float[] spectrum)
    {
        int n = data.Length;
        for (int i = 0; i < n; i++)
        {
            spectrum[i] = Mathf.Abs(data[i]);
        }
    }
}

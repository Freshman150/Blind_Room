using UnityEngine;

//use lidar on detection of a tongue click
// not working properly (Can't work around the frequency of the click)
public class TongueClickDetector : MonoBehaviour
{
    public int sampleWindow = 1024;   // 🔊 Size of the audio sample window
    public float frequencyThreshold = 2000f;  // 🎵 Frequency threshold for detection
    public float volumeThreshold = 0.02f;  // 📊 Volume threshold for detection
    public float cooldownTime = 0.5f; // ⏳ Time to wait before detecting a new click

    //microphone variables
    private AudioClip micClip;
    private string micName;
    private bool micInitialized = false;
    private float[] waveData;
    private float[] spectrumData; 

    private float lastClickTime = -Mathf.Infinity; // ⏳ Tracks the last click time

    void Start()
    {
        // Check for microphone permissions
        if (!AndroidRuntimePermissions.CheckPermission("android.permission.RECORD_AUDIO"))
        {
            AndroidRuntimePermissions.RequestPermission("android.permission.RECORD_AUDIO");
        }
        // Initialize microphone
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            micClip = Microphone.Start(micName, true, 10, 44100);
            while (Microphone.GetPosition(micName) <= 0) { }
            micInitialized = true;
        }
        else
        {
            Debug.LogError("Aucun microphone détecté !");
        }
        
        // Initialize audio data arrays
        waveData = new float[sampleWindow];
        spectrumData = new float[sampleWindow];
    }

    void Update()
    {
        if (!micInitialized || !micClip ) return;

        
        int clipPosition = Microphone.GetPosition(micName);
        DetectTongueClick(clipPosition);
    }

    void DetectTongueClick(int clipPosition)
    {
        if (Time.time - lastClickTime < cooldownTime) return; // 🚫 Skip if on cooldown

        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0) return;

        
        micClip.GetData(waveData, startPosition); // 🎤 Get audio data from the microphone
        FFT(waveData, spectrumData); // 🔊 Perform FFT to get frequency spectrum

        float maxVolume = 0f;
        int minIndex = Mathf.FloorToInt(frequencyThreshold / (44100f / sampleWindow)); 

        // 📊 Find the maximum volume in the frequency range above the threshold
        for (int i = minIndex; i < spectrumData.Length; i++)
        {
            if (spectrumData[i] > maxVolume)
            {
                maxVolume = spectrumData[i];
            }
        }
        
        // 📊 Check if the maximum volume exceeds the threshold and trigger the click event
        if (maxVolume > volumeThreshold)
        {
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

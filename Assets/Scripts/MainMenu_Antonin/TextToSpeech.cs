using UnityEngine;

public class TextToSpeech : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject ttsObj;
    private AndroidJavaObject unityActivity;
#endif

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            ttsObj = new AndroidJavaObject("android.speech.tts.TextToSpeech", unityActivity, new TTSListener());
        }
#endif
    }

    public void Speak(string text)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (ttsObj != null)
        {
            ttsObj.Call<int>("speak", text, 0, null, null);
        }
#else
        Debug.Log("TTS: " + text);
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private class TTSListener : AndroidJavaProxy
    {
        public TTSListener() : base("android.speech.tts.TextToSpeech$OnInitListener") { }
        public void onInit(int status) { }
    }
#endif
}

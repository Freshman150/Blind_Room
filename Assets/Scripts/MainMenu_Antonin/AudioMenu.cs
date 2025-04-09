using UnityEngine;
using UnityEngine.Events;

public class AudioMenu : MonoBehaviour
{
    public AudioMenuItem[] menuItems;
    private int currentIndex = 0;
    private float inputCooldown = 0.3f;
    private float lastInputTime = -1f;

    public TextToSpeech tts;

    void Start()
    {
        SpeakCurrentItem();
    }

    void Update()
    {
        Vector2 joystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (Time.time - lastInputTime > inputCooldown)
        {
            if (joystick.y > 0.5f)
            {
                currentIndex = (currentIndex - 1 + menuItems.Length) % menuItems.Length;
                lastInputTime = Time.time;
                SpeakCurrentItem();
            }
            else if (joystick.y < -0.5f)
            {
                currentIndex = (currentIndex + 1) % menuItems.Length;
                lastInputTime = Time.time;
                SpeakCurrentItem();
            }
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            menuItems[currentIndex].onSelect.Invoke();
        }
    }

    void SpeakCurrentItem()
    {
        if (tts != null)
        {
            Debug.Log("TTS speaking: " + menuItems[currentIndex].label);
            tts.Speak(menuItems[currentIndex].label);
        }
    }
}

[System.Serializable]
public class AudioMenuItem
{
    public string label;
    public UnityEvent onSelect;
}

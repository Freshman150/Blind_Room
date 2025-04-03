using UnityEngine;

public class DummyNarrator : MonoBehaviour
{
    public static DummyNarrator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ReadOption(string option)
    {
        Debug.Log($"Narrator: {option}");
    }
}
using UnityEngine;

public class VRMenuButton : MonoBehaviour
{
    private string displayName;
    private string sceneName;
    private VRMenuSpawner menuSpawner;

    public void Initialize(string display, string scene, VRMenuSpawner spawner)
    {
        displayName = display;
        sceneName = scene;
        menuSpawner = spawner;
    }

    public void Select()
    {
        menuSpawner.SelectButton(gameObject);
    }

    public string GetDisplayName()
    {
        return displayName;
    }
}
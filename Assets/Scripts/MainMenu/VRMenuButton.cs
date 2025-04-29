using UnityEngine;

public class VRMenuButton : MonoBehaviour
{
    private string sceneName; // the scene to load when this button is selected
    private VRMenuSpawner menuSpawner; // reference to the menu spawner

    public void Initialize(string scene, VRMenuSpawner spawner)
    {
        sceneName = scene;
        menuSpawner = spawner;
    }

    public void Select()
    {
        menuSpawner.SelectButton(gameObject);
    }
}
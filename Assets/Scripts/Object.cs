using UnityEngine;

public enum Material
{
    METAL,
    STONE,
    WOOD
}

public class Object : MonoBehaviour
{
    [SerializeField] private Material material;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (material)
            {
                case Material.METAL:
                    AudioManagerController.PlayAudioOnce(Audio.METALSOUND);
                    break;
                case Material.STONE:
                    AudioManagerController.PlayAudioOnce(Audio.STONESCRATCH);
                    break;
                case Material.WOOD:
                    AudioManagerController.PlayAudioOnce(Audio.WOODSCRATCH);
                    break;
            }
        }
    }
}

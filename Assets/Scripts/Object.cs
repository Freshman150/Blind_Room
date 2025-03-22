using UnityEngine;

public enum Mat
{
    METAL,
    STONE,
    WOOD
}

public class Object : MonoBehaviour
{
    [SerializeField] private Mat material;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (material)
            {
                case Mat.METAL:
                    AudioManagerController.PlayAudioOnce(Audio.METALSOUND);
                    break;
                case Mat.STONE:
                    AudioManagerController.PlayAudioOnce(Audio.STONESCRATCH);
                    break;
                case Mat.WOOD:
                    AudioManagerController.PlayAudioOnce(Audio.WOODSCRATCH);
                    break;
            }
        }
    }
}

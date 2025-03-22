using UnityEngine;

public class KeyController : MonoBehaviour
{
    private bool inArea = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!inArea)
            {
                AudioManagerController.PlayAudioLoop(Audio.SPARK, 0.2f);
                inArea = true;
            }
            else
            {
                AudioManagerController.Stop();
                AudioManagerController.PlayAudioOnce(Audio.KEY);
                Destroy(gameObject);
                other.GetComponent<PlayerController>().hasKey = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop the audio loop
            AudioManagerController.Stop();
            inArea = false;
        }
    }
}

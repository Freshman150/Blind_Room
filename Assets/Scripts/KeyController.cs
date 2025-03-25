using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private GameObject Door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManagerController.PlayAudioOnce(Audio.KEY);
            Door.SetActive(true);
            Destroy(gameObject);
            other.GetComponent<PlayerController>().hasKey = true;
        }
    }

}

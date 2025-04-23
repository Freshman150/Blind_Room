using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeyController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject Door;
    [SerializeField] private XRGrabInteractable grabInteractable;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Update()
    {
        if (playerController != null)
        {
            if (grabInteractable.isSelected && !playerController.hasKey)
            {
                AudioManagerController.PlayAudioOnce(Audio.KEY);
                playerController.hasKey = true;
                Door.GetComponent<MeshCollider>().enabled = false;
                Door.GetComponentInChildren<AudioSource>().Play();
                Destroy(gameObject);
            }
            //if (!grabInteractable.isSelected && playerController.hasKey)
            //{
            //    audioSource.Play();
            //    playerController.hasKey = false;
            //    Door.GetComponent<MeshCollider>().isTrigger = false;
            //    Door.GetComponent<AudioSource>().Stop();
            //}
        }
    }

}

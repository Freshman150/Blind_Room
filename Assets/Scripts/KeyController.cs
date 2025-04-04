using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeyController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject Door;
    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (playerController != null)
        {
            if (grabInteractable.isSelected && !playerController.hasKey)
            {
                AudioManagerController.PlayAudioOnce(Audio.KEY);
                audioSource.Stop();
                playerController.hasKey = true;
                Door.GetComponent<MeshCollider>().isTrigger = true;
                Door.GetComponent<AudioSource>().Play();
            }
            if (!grabInteractable.isSelected && playerController.hasKey)
            {
                audioSource.Play();
                playerController.hasKey = false;
                Door.GetComponent<MeshCollider>().isTrigger = false;
                Door.GetComponent<AudioSource>().Stop();
            }
        }
    }

}

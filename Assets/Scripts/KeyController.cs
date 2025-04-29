using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeyController : MonoBehaviour
{
    // R�f�rence vers le script du joueur (pour modifier l'�tat "hasKey")
    [SerializeField] private PlayerController playerController;

    // R�f�rence vers la porte � ouvrir
    [SerializeField] private GameObject Door;

    // R�f�rence vers le composant XR qui permet de grab la cl�
    [SerializeField] private XRGrabInteractable grabInteractable;

    private void Start()
    {
        // S'assure qu'on a bien la r�f�rence au XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Update()
    {
        if (playerController != null)
        {
            // Si la cl� est s�lectionn�e (grab) et que le joueur ne l'a pas encore
            if (grabInteractable.isSelected && !playerController.hasKey)
            {
                // Joue le son de ramassage de cl�
                AudioManagerController.PlayAudioOnce(Audio.KEY);

                // Marque que le joueur poss�de maintenant la cl�
                playerController.hasKey = true;

                // D�sactive le collider de la porte pour la "d�verrouiller"
                Door.GetComponent<MeshCollider>().enabled = false;

                // Joue le son de la porte (ouverture, grincement�)
                Door.GetComponentInChildren<AudioSource>().Play();

                // D�truit l'objet cl� une fois qu'elle est ramass�e
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeyController : MonoBehaviour
{
    // Référence vers le script du joueur (pour modifier l'état "hasKey")
    [SerializeField] private PlayerController playerController;

    // Référence vers la porte à ouvrir
    [SerializeField] private GameObject Door;

    // Référence vers le composant XR qui permet de grab la clé
    [SerializeField] private XRGrabInteractable grabInteractable;

    private void Start()
    {
        // S'assure qu'on a bien la référence au XRGrabInteractable
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Update()
    {
        if (playerController != null)
        {
            // Si la clé est sélectionnée (grab) et que le joueur ne l'a pas encore
            if (grabInteractable.isSelected && !playerController.hasKey)
            {
                // Joue le son de ramassage de clé
                AudioManagerController.PlayAudioOnce(Audio.KEY);

                // Marque que le joueur possède maintenant la clé
                playerController.hasKey = true;

                // Désactive le collider de la porte pour la "déverrouiller"
                Door.GetComponent<MeshCollider>().enabled = false;

                // Joue le son de la porte (ouverture, grincement…)
                Door.GetComponentInChildren<AudioSource>().Play();

                // Détruit l'objet clé une fois qu'elle est ramassée
                Destroy(gameObject);
            }
        }
    }
}

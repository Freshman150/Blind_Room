using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Référence au CharacterController du joueur (pour lire sa vitesse)
    [SerializeField] private CharacterController characterController;

    // Deuxième clé à activer plus tard dans le niveau
    [SerializeField] private GameObject secondKey;

    // Mémorise la dernière vélocité pour éviter les valeurs figées
    private float lastVelocity = 0f;

    // Délai entre deux sons de pas
    private float stepInterval = 0.5f;
    private float nextStepTime = 0f;

    // Indique si le joueur possède une clé
    public bool hasKey = false;

    // Indique si le joueur est en train de marcher
    private bool isMoving = false;

    void Start()
    {
        // S'assure d'avoir la référence au CharacterController attaché au joueur
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        // Détection du mouvement du joueur
        // characterController.velocity.magnitude peut parfois rester figé à 2.5, donc on ajoute une condition sur le changement
        isMoving = characterController.velocity.magnitude > 2f && lastVelocity != characterController.velocity.magnitude;

        // Si le joueur bouge et qu'on attend pas un délai, on joue un son de pas
        if (isMoving && Time.time >= nextStepTime)
        {
            AudioManagerController.PlayFootsteps(); // Son de pas
            nextStepTime = Time.time + stepInterval;
            lastVelocity = characterController.velocity.magnitude;
        }
    }

    // Détection d’entrée dans des zones de trigger
    private void OnTriggerEnter(Collider other)
    {
        // Si le joueur a une clé et entre dans le trigger de la serrure principale
        if (hasKey && other.name == "SerrureSongTrigger")
        {
            StartCoroutine(UnlockDoorCoroutine()); // Joue les sons de serrure
            AudioManagerController.PlayLabBegin(); // Musique de début de labo
            other.transform.parent.gameObject.layer = LayerMask.NameToLayer("Default"); // Réinitialise le layer
            other.GetComponent<AudioSource>().Stop(); // Coupe l’audio de boucle de serrure
            other.GetComponent<Collider>().enabled = false; // Désactive le trigger
            secondKey.SetActive(true); // Active la deuxième clé
            hasKey = false; // Le joueur n’a plus la clé
        }

        // Deuxième serrure à débloquer
        if (hasKey && other.name == "SerrureSongTrigger2")
        {
            StartCoroutine(UnlockDoor2Coroutine());
        }

        // Fin du laboratoire
        if (other.name == "LabEndTrigger")
        {
            AudioManagerController.PlayLabEnd(); // Audio du narrateur a la sortie du labyrinthe
        }
    }

    // Coroutine pour jouer les sons de déverrouillage (clé + grincement) avec délai
    IEnumerator UnlockDoorCoroutine()
    {
        AudioManagerController.PlayAudioOnce(Audio.UNLOCKDOOR);
        yield return new WaitForSeconds(0.5f);
        AudioManagerController.PlayAudioOnce(Audio.DOORSQUEAK);
    }

    // Variante qui enchaîne déverrouillage, son de porte et chargement de la scène de fin
    IEnumerator UnlockDoor2Coroutine()
    {
        AudioManagerController.PlayAudioOnce(Audio.UNLOCKDOOR);
        yield return new WaitForSeconds(0.5f);
        AudioManagerController.PlayAudioOnce(Audio.DOORSQUEAK);
        yield return new WaitForSeconds(1.5f);
        Timer.End(); // Fin du chrono
        SceneManager.LoadScene("End"); // Change de scène
    }
}

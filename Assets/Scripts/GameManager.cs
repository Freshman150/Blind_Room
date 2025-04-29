using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class GameManager : MonoBehaviour
{
    // Canvas d'UI qui s'affiche à la fin du niveau 
    [SerializeField] private GameObject EndLevelCanvas;

    // Référence à la tête du joueur
    [SerializeField] private Transform head;

    // Distance à laquelle placer le canvas devant la tête
    private float distance = 1f;

    private void Start()
    {
        // S'assure que le temps est à vitesse normale (au cas où il aurait été mis en pause)
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        // Si l'écran de fin de niveau est actif...
        if (EndLevelCanvas.activeSelf)
        {
            // ...on place le canvas à une certaine distance devant la tête du joueur (horizontalement)
            EndLevelCanvas.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
        }

        // Le canvas regarde toujours le joueur, mais sans s'incliner verticalement (garde sa hauteur)
        EndLevelCanvas.transform.LookAt(new Vector3(head.position.x, EndLevelCanvas.transform.position.y, head.position.z));

        // On inverse la direction du forward, car LookAt retourne l'arrière du canvas vers le joueur
        EndLevelCanvas.transform.forward *= -1;
    }

    public void Quit()
    {
        // Ferme le jeu (build standalone)
        Application.Quit();

        // Arrête le mode Play dans l’éditeur Unity (utile pendant le développement)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Retry()
    {
        // Recharge la scène actuelle (ici nommée "BasicScene")
        SceneManager.LoadScene("BasicScene");
    }
}

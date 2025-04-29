using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class GameManager : MonoBehaviour
{
    // Canvas d'UI qui s'affiche � la fin du niveau 
    [SerializeField] private GameObject EndLevelCanvas;

    // R�f�rence � la t�te du joueur
    [SerializeField] private Transform head;

    // Distance � laquelle placer le canvas devant la t�te
    private float distance = 1f;

    private void Start()
    {
        // S'assure que le temps est � vitesse normale (au cas o� il aurait �t� mis en pause)
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        // Si l'�cran de fin de niveau est actif...
        if (EndLevelCanvas.activeSelf)
        {
            // ...on place le canvas � une certaine distance devant la t�te du joueur (horizontalement)
            EndLevelCanvas.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
        }

        // Le canvas regarde toujours le joueur, mais sans s'incliner verticalement (garde sa hauteur)
        EndLevelCanvas.transform.LookAt(new Vector3(head.position.x, EndLevelCanvas.transform.position.y, head.position.z));

        // On inverse la direction du forward, car LookAt retourne l'arri�re du canvas vers le joueur
        EndLevelCanvas.transform.forward *= -1;
    }

    public void Quit()
    {
        // Ferme le jeu (build standalone)
        Application.Quit();

        // Arr�te le mode Play dans l��diteur Unity (utile pendant le d�veloppement)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Retry()
    {
        // Recharge la sc�ne actuelle (ici nomm�e "BasicScene")
        SceneManager.LoadScene("BasicScene");
    }
}

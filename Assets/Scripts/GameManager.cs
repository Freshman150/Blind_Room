using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject EndLevelCanvas;
    [SerializeField] private Transform head;
    private float distance = 1f;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (EndLevelCanvas.activeSelf)
        {
            EndLevelCanvas.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
        }

        EndLevelCanvas.transform.LookAt(new Vector3(head.position.x, EndLevelCanvas.transform.position.y, head.position.z));
        EndLevelCanvas.transform.forward *= -1;
    }

    public void Quit()
    {
        // Quit game
        Application.Quit();
    }

    public void Retry()
    {
        // Reload current scene
        SceneManager.LoadScene("BasicScene");
    }
}

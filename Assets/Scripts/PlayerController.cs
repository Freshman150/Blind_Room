using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject EndCanvas;

    public bool hasKey = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasKey && other.name == "Door")
        {
            StartCoroutine(UnlockDoorCoroutine());
        }
    }

    IEnumerator UnlockDoorCoroutine()
    {
        AudioManagerController.PlayAudioOnce(Audio.UNLOCKDOOR);
        yield return new WaitForSeconds(2f);
        AudioManagerController.PlayAudioOnce(Audio.DOORSQUEAK);
        yield return new WaitForSeconds(2f);
        EndCanvas.SetActive(true);
        Time.timeScale = 0;
    }
}

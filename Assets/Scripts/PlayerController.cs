using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject EndCanvas;

    private float stepInterval = 0.5f;
    private float nextStepTime = 0f;

    public bool hasKey = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Play footsteps sound
        bool isMoving = characterController.velocity.magnitude > 0.1f;

        if (isMoving && Time.time >= nextStepTime)
        {
            AudioManagerController.PlayFootsteps();
            nextStepTime = Time.time + stepInterval;
        }
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
        yield return new WaitForSeconds(1.5f);
        EndCanvas.SetActive(true);
        Time.timeScale = 0;
    }
}

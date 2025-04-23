using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    //[SerializeField] private GameObject key;
    [SerializeField] private GameObject secondKey;

    private float lastVelocity = 0f;
    private float stepInterval = 0.5f;
    private float nextStepTime = 0f;

    public bool hasKey = false;
    private bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Play footsteps sound
        // pour des raisons obscures characterController.velocity.magnitude se bloque � 2.5 parfois
        isMoving = characterController.velocity.magnitude > 2f && lastVelocity != characterController.velocity.magnitude;
        if (isMoving && Time.time >= nextStepTime)
        {
            AudioManagerController.PlayFootsteps();
            nextStepTime = Time.time + stepInterval;
            lastVelocity = characterController.velocity.magnitude;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasKey && other.name == "SerrureSongTrigger")
        {
            StartCoroutine(UnlockDoorCoroutine());
            AudioManagerController.PlayLabBegin();
            other.transform.parent.gameObject.layer = LayerMask.NameToLayer("Default");
            other.GetComponent<AudioSource>().Stop();
            other.GetComponent<Collider>().enabled = false;
            //Destroy(key);
            secondKey.SetActive(true);
            hasKey = false;
        }
        if (hasKey && other.name == "SerrureSongTrigger2")
        {
            StartCoroutine(UnlockDoor2Coroutine());
        }
        if (other.name == "LabEndTrigger")
        {
            AudioManagerController.PlayLabEnd();
        }
    }

    IEnumerator UnlockDoorCoroutine()
    {
        AudioManagerController.PlayAudioOnce(Audio.UNLOCKDOOR);
        yield return new WaitForSeconds(0.5f);
        AudioManagerController.PlayAudioOnce(Audio.DOORSQUEAK);
    }

    IEnumerator UnlockDoor2Coroutine()
    {
        AudioManagerController.PlayAudioOnce(Audio.UNLOCKDOOR);
        yield return new WaitForSeconds(0.5f);
        AudioManagerController.PlayAudioOnce(Audio.DOORSQUEAK);
        yield return new WaitForSeconds(1.5f);
        Timer.End();
        SceneManager.LoadScene("End");
    }
}

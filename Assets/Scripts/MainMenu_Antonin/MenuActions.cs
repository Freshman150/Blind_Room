using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("BasicScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

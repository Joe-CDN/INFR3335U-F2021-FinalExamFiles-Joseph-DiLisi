using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Loading");
        //GetComponent<AudioSource> ().Stop();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

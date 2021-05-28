using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource aud;

    public void play()
    {
        SceneManager.LoadScene("Dungeon");
        if (aud.isPlaying)
        {
            aud.Stop();
        }
    }

    public void quit()
    {
        Application.Quit();
    }
}

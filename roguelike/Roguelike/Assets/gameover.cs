using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class gameover : MonoBehaviour
{
    public TextMeshProUGUI text;
    public AudioSource aud;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        text.text = "YOUR SCORE: " + score.ToString();
        if (aud.isPlaying)
        {
            aud.Stop();
        }

    }


    public void restart()
    {
        SceneManager.LoadScene("Dungeon");
        
    }
 

    public void mainmenu()
    {
        SceneManager.LoadScene("Tavern");
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

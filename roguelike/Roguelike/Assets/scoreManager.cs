using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scoreManager : MonoBehaviour
{

    public playerConroller myP;

    public static scoreManager sm;
    public TextMeshProUGUI text;
    int score;

    void Start()
    {
        if (sm == null)
        {
            sm = this;
        }

    }

    // Update is called once per frame
    void Update()
    {

        text.text = myP.point.ToString();

    }

    public void ChangeScore(int point)
    {
        score = point;
        text.text = score.ToString();
    }
}

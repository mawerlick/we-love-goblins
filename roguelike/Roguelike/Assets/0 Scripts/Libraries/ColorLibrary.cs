using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLibrary : MonoBehaviour
{
    public ColorRange[] metals0;
    public ColorRange[] metals1;
    public ColorRange[] skin;
    public ColorRange[] eye;
    public ColorRange[] cloth;
    public ColorRange[] leather;
    public ColorRange[] hair;
    public ColorRange[] gSkin;
    public ColorRange[] gEye;
}

[System.Serializable]
public struct ColorRange
{
    public Gradient grad0;
    public Gradient grad1;

    public Color RandomColor()
    {
        Color c0 = grad0.Evaluate(Random.Range(0f, 1f));
        Color c1 = grad0.Evaluate(Random.Range(0f, 1f));

        return Color.Lerp(c0,c1, Random.Range(0f, 1f));
    }
}

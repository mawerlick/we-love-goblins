using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorLibrary : MonoBehaviour
{
    public Helmet helmet;
    public TorsoArmor torsoArmor;
    public ArmArmor armArmor;
    public LegArmor legArmor;
    public Sprite[] weapon;
    public Sprite[] gWeapon;
}

[System.Serializable]
public struct Helmet
{
    public Sprite[] chain;
    public Sprite[] crown;
    public Sprite[] helm;
    public Sprite[] visor;
}

[System.Serializable]
public struct TorsoArmor
{
    public Sprite[] chain;
    public Sprite[] chest;
    public Sprite[] waist;
}

[System.Serializable]
public struct ArmArmor
{
    public Sprite[] chain;
    public Sprite[] spaulder;
    public Sprite[] wrist;
    public Sprite[] hand;
}


[System.Serializable]
public struct LegArmor
{
    public Sprite[] pants;
    public Sprite[] knee;
    public Sprite[] boots;
}



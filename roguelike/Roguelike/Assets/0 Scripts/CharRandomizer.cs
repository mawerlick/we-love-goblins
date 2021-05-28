using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharRandomizer : MonoBehaviour
{
    public bool goblin;
    public GameObject library;
    private ArmorLibrary libArmor;
    public ColorLibrary libColor;
    private BodyLibrary libBody;
    public Shader rgbcmy;
    public Shader grayscale;
    private GameObject torso;
    private GameObject head;
    private GameObject armL;
    private GameObject armR;
    private GameObject legL;
    private GameObject legR;

    public void Start()
    {
        RandomizeChar();
    }


    [ContextMenu("Randomize Char")]
    public void RandomizeChar()
    {
        ConfigureAssets();
        if (goblin) RandomizeGoblin();
        else
        {
            RemoveArmor();
            RandomizeArmor();
        }
    }

    public void RandomizeGoblin()
    {

        Color skinColor = libColor.gSkin[Random.Range(0, libColor.gSkin.Length)].RandomColor();
        float skinB = rgbValue(skinColor);
        Color eyeColor = libColor.gEye[Random.Range(0, libColor.gEye.Length)].RandomColor();
        Color clothColor = libColor.cloth[Random.Range(0, libColor.cloth.Length)].RandomColor();
        int clothSc = Random.Range(0, 2) * 2 - 1;

        SpriteRenderer clothSR = torso.transform.Find("cloth").GetComponent<SpriteRenderer>();
        clothSR.gameObject.transform.localScale = new Vector3(clothSc, 1, 1);
        SetGrayMat(clothColor, clothSR.gameObject, rgbValue(clothColor), 1);

        SpriteRenderer tsr = torso.GetComponent<SpriteRenderer>();
        tsr.sprite = libBody.torso[Random.Range(0, libBody.torso.Length)];
        SetGrayMat(skinColor, torso, skinB, 1f);

        SpriteRenderer hsr = head.GetComponent<SpriteRenderer>();
        hsr.sprite = libBody.head[Random.Range(0, libBody.head.Length)];
        SetGrayMat(skinColor, head, skinB, 1f);

        SetGrayMat(skinColor, armL, skinB, 1f);
        SetGrayMat(skinColor, armR, skinB, 1f);
        SetGrayMat(skinColor, legL, skinB, 1f);
        SetGrayMat(skinColor, legR, skinB, 1f);

        int eye = Random.Range(0, 3);
        int mouth = Random.Range(0, 4);
        SpriteRenderer eyeR = head.transform.Find("eye R").GetComponent<SpriteRenderer>();
        SpriteRenderer eyeL = head.transform.Find("eye L").GetComponent<SpriteRenderer>();
        SpriteRenderer mouthSR = head.transform.Find("mouth").GetComponent<SpriteRenderer>();
        SpriteRenderer earL = head.transform.Find("ear L").GetComponent<SpriteRenderer>();
        SpriteRenderer earR = head.transform.Find("ear R").GetComponent<SpriteRenderer>();
        //EYES
        eyeR.sprite = libBody.gEye[eye];
        Material tempMat = new Material(rgbcmy);
        tempMat.SetColor("tint_0", skinColor);
        tempMat.SetColor("tint_1", eyeColor);
        tempMat.SetVector("bc_red_mag", new Vector4(skinB, 1, 0.5f, 1));
        eyeR.sharedMaterial = tempMat;
        eyeL.sprite = libBody.gEye[eye];
        tempMat = new Material(rgbcmy);
        tempMat.SetColor("tint_0", skinColor);
        tempMat.SetColor("tint_1", eyeColor);
        tempMat.SetVector("bc_red_mag", new Vector4(skinB, 1, 0.5f, 1));
        eyeL.sharedMaterial = tempMat;
        //MOUTH
        mouthSR.sprite = libBody.gMouth[mouth];
        SetGrayMat(skinColor, mouthSR.gameObject, skinB, 1);
        //EARS
        earL.sprite = libBody.ear[Random.Range(0, 3)];
        SetGrayMat(skinColor, earL.gameObject, skinB, 1);
        earR.sprite = libBody.ear[Random.Range(0, 3)];
        SetGrayMat(skinColor, earR.gameObject, skinB, 1);

        armR.transform.Find("sword").GetComponent<SpriteRenderer>().sprite = libArmor.gWeapon[Random.Range(0, libArmor.gWeapon.Length)];
    }

    public void RandomizeArmor()
    {
        Color[] colors = GenerateColorScheme();
        RandomizeBody();

        bool chain = false;
        if (Random.Range(0, 2) != 1) { AddArmor(libArmor.torsoArmor.chain[0], torso, 1, colors); chain = true; }
        if (!chain) AddArmor(libArmor.torsoArmor.waist[0], torso, 3, colors);
        else
        {
            int waist = Random.Range(0, 2);
            AddArmor(libArmor.torsoArmor.waist[waist], torso, 3, colors);
        }

        int chest = Random.Range(0, 3);
        if (chest != 2) { AddArmor(libArmor.torsoArmor.chest[chest], torso, 2, colors); }

        if (chain) { int armchain = Random.Range(0, 2); AddArmor(libArmor.armArmor.chain[armchain], armR, 1, colors); AddArmor(libArmor.armArmor.chain[armchain], armL, 1, colors); }
        int spaulder = Random.Range(0, 2);
        if (spaulder != 1) { AddArmor(libArmor.armArmor.spaulder[0], armR, 2, colors); AddArmor(libArmor.armArmor.spaulder[0], armL, 2, colors); }

        if (chain && Random.Range(0, 2) == 0) { AddArmor(libArmor.helmet.chain[0], head, 1, colors); } else chain = false;
        int helm = Random.Range(0, 3);
        int crown = Random.Range(0, 3);
        int visor = Random.Range(0, 4);

        if (!(!chain && crown == 2 && visor == 3) && helm != 2) AddArmor(libArmor.helmet.helm[helm], head, 2, colors);
        if (crown != 2) AddArmor(libArmor.helmet.crown[crown], head, 3, colors);
        if (!(crown == 2 && helm == 2) && visor != 3) AddArmor(libArmor.helmet.visor[visor], head, 4, colors);

        int wrist = Random.Range(0, 3);
        int hand = Random.Range(0, 2);
        if (wrist != 2) { AddArmor(libArmor.armArmor.wrist[wrist], armL, 3, colors); AddArmor(libArmor.armArmor.wrist[wrist], armR, 3, colors); }
        if (hand != 1) { AddArmor(libArmor.armArmor.hand[0], armL, 2, colors); AddArmor(libArmor.armArmor.hand[0], armR, 2, colors); }

        int pants = Random.Range(0, 2);
        AddArmor(libArmor.legArmor.pants[pants], legL, 1, colors);
        AddArmor(libArmor.legArmor.pants[pants], legR, 1, colors);

        int boots = Random.Range(0, 2);
        AddArmor(libArmor.legArmor.boots[boots], legL, 1, colors);
        AddArmor(libArmor.legArmor.boots[boots], legR, 1, colors);

        int knee = Random.Range(0, 2);
        if (knee != 1) { AddArmor(libArmor.legArmor.knee[0], legL, 2, colors); AddArmor(libArmor.legArmor.knee[0], legR, 2, colors); }

        armR.transform.Find("sword").GetComponent<SpriteRenderer>().sprite = libArmor.weapon[Random.Range(0, libArmor.weapon.Length)];
    }

    public void AddArmor(Sprite sprite, GameObject limb, int order, Color[] colors)
    {
        GameObject armor = new GameObject();
        SpriteRenderer sR = armor.AddComponent<SpriteRenderer>();
        sR.sprite = sprite;

        Material material = new Material(rgbcmy);
        SetMaterialColors(colors, material);
        sR.material = material;

        armor.transform.parent = limb.transform;
        armor.transform.localPosition = Vector3.zero;
        armor.transform.localScale = Vector3.one;
        armor.transform.name = "armor";
        sR.sortingOrder = order + limb.GetComponent<SpriteRenderer>().sortingOrder;

    }

    public void RandomizeBody()
    {
        Color skinColor = libColor.skin[Random.Range(0, libColor.skin.Length)].RandomColor();
        Color hairColor = libColor.hair[Random.Range(0, libColor.hair.Length)].RandomColor();
        float skinB = rgbValue(skinColor);
        Color eyeColor = libColor.eye[Random.Range(0, libColor.eye.Length)].RandomColor();

        SpriteRenderer tsr = torso.GetComponent<SpriteRenderer>();
        tsr.sprite = libBody.torso[Random.Range(0, libBody.torso.Length)];
        SetGrayMat(skinColor, torso, skinB, 1f);

        SpriteRenderer hsr = head.GetComponent<SpriteRenderer>();
        hsr.sprite = libBody.head[Random.Range(0, libBody.head.Length)];
        SetGrayMat(skinColor, head, skinB, 1f);

        SetGrayMat(skinColor, armL, skinB, 1f);
        SetGrayMat(skinColor, armR, skinB, 1f);
        SetGrayMat(skinColor, legL, skinB, 1f);
        SetGrayMat(skinColor, legR, skinB, 1f);

        int eye = Random.Range(0, 3);
        int mouth = Random.Range(0, 4);
        int beard = Random.Range(0, 4);
        SpriteRenderer eyeR = head.transform.Find("eye R").GetComponent<SpriteRenderer>();
        SpriteRenderer eyeL = head.transform.Find("eye L").GetComponent<SpriteRenderer>();
        SpriteRenderer mouthSR = head.transform.Find("mouth").GetComponent<SpriteRenderer>();
        SpriteRenderer beardSR = head.transform.Find("beard").GetComponent<SpriteRenderer>();
        eyeR.sprite = libBody.eye[eye];
        Material tempMat = new Material(rgbcmy);
        tempMat.SetColor("tint_0", skinColor);
        tempMat.SetColor("tint_1", eyeColor);
        tempMat.SetVector("bc_red_mag", new Vector4(skinB, 1, 0.5f, 1));
        eyeR.sharedMaterial = tempMat;
        eyeL.sprite = libBody.eye[eye];
        tempMat = new Material(rgbcmy);
        tempMat.SetColor("tint_0", skinColor);
        tempMat.SetColor("tint_1", eyeColor);
        tempMat.SetVector("bc_red_mag", new Vector4(skinB, 1, 0.5f, 1));
        eyeL.sharedMaterial = tempMat;
        mouthSR.sprite = libBody.mouth[mouth];
        SetGrayMat(skinColor, mouthSR.gameObject, skinB, 1);

        //beardSR.material = new Material(grayscale);
        beardSR.sprite = beard != 3 ? libBody.beards[beard] : null;
        SetGrayMat(hairColor, beardSR.gameObject, rgbValue(hairColor), 1);
    }

    public void SetGrayMat(Color color, GameObject gobj, float b, float c)
    {
        Material tempMat = new Material(grayscale);
        tempMat.SetColor("tint", color);
        tempMat.SetVector("bc", new Vector2(b, c));
        gobj.GetComponent<SpriteRenderer>().sharedMaterial = tempMat;
    }

    public void SetRgbMat(Color color, GameObject gobj, float b, float c)
    {
        SpriteRenderer sR = gobj.GetComponent<SpriteRenderer>();
        //sR.material = new Material(grayscale);
        sR.material.SetColor("tint", color);
        sR.material.SetVector("bc", new Vector2(b, c));
    }

    public void SetMaterialColors(Color[] colors, Material material)
    {
        material.SetColor("tint_0", colors[0]);
        material.SetColor("tint_1", colors[1]);
        material.SetColor("tint_2", colors[2]);
        material.SetColor("tint_3", colors[3]);
        material.SetColor("tint_4", colors[4]);
        material.SetColor("tint_5", colors[5]);
    }

    public Color[] GenerateColorScheme()
    {
        Color metal0 = libColor.metals0[Random.Range(0, libColor.metals0.Length)].RandomColor();
        Color metal1 = libColor.metals0[Random.Range(0, libColor.metals0.Length)].RandomColor();
        Color metal2 = libColor.metals1[Random.Range(0, libColor.metals1.Length)].RandomColor();
        Color metal3 = libColor.metals1[Random.Range(0, libColor.metals1.Length)].RandomColor();
        Color leather0 = libColor.leather[Random.Range(0, libColor.leather.Length)].RandomColor();
        Color leather1 = libColor.leather[Random.Range(0, libColor.leather.Length)].RandomColor();
        Color cloth0 = libColor.cloth[Random.Range(0, libColor.cloth.Length)].RandomColor();
        Color cloth1 = libColor.cloth[Random.Range(0, libColor.cloth.Length)].RandomColor();

        return new Color[8] { metal0, metal1, metal2, metal3, leather0, leather1, cloth0, cloth1 };
    }

    [ContextMenu("Remove Armor")]
    public void RemoveArmor()
    {
        for (int i = torso.transform.childCount - 1; i >= 0; i--) if (torso.transform.GetChild(i).name == "armor") DestroyImmediate(torso.transform.GetChild(i).gameObject);
        for (int i = head.transform.childCount - 1; i >= 0; i--) if (head.transform.GetChild(i).name == "armor") DestroyImmediate(head.transform.GetChild(i).gameObject);
        for (int i = armL.transform.childCount - 1; i >= 0; i--) if (armL.transform.GetChild(i).name == "armor") DestroyImmediate(armL.transform.GetChild(i).gameObject);
        for (int i = armR.transform.childCount - 1; i >= 0; i--) if (armR.transform.GetChild(i).name == "armor") DestroyImmediate(armR.transform.GetChild(i).gameObject);
        for (int i = legL.transform.childCount - 1; i >= 0; i--) if (legL.transform.GetChild(i).name == "armor") DestroyImmediate(legL.transform.GetChild(i).gameObject);
        for (int i = legR.transform.childCount - 1; i >= 0; i--) if (legR.transform.GetChild(i).name == "armor") DestroyImmediate(legR.transform.GetChild(i).gameObject);
    }

    [ContextMenu("Configure Assets")]
    public void ConfigureAssets()
    {
        libArmor = library.GetComponent<ArmorLibrary>();
        libColor = library.GetComponent<ColorLibrary>();
        libBody = library.GetComponent<BodyLibrary>();

        torso = gameObject.transform.Find("torso").gameObject;
        head = torso.transform.Find("head").gameObject;
        armL = torso.transform.Find("arm L").gameObject;
        armR = torso.transform.Find("arm R").gameObject;
        legL = gameObject.transform.Find("leg L").gameObject;
        legR = gameObject.transform.Find("leg R").gameObject;
    }

    public float rgbValue(Color color)
    {
        Vector3 hsv = new Vector3();
        Color.RGBToHSV(color, out hsv.x, out hsv.y, out hsv.z);

        return hsv.z;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MaterialShare : MonoBehaviour
{
    public Material Material;
    public Shader shader;
    public TilemapRenderer tRenderer;
    public Color color;
    [Range(0f, 1f)]
    public float brightness = 0.5f;
    [Range(0f, 5f)]
    public float contrast = 1f;

    private void OnValidate()
    {
        Material mat = new Material(shader);
        mat.SetVector("bc", new Vector2(rgbValue(color), contrast));
        mat.SetColor("tint", color);
        tRenderer.material = mat;

     
    }

    public float rgbValue(Color color)
    {
        Vector3 hsv = new Vector3();
        Color.RGBToHSV(color, out hsv.x, out hsv.y, out hsv.z);

        return hsv.z;
    }
}

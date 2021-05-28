using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileLibrary : MonoBehaviour
{
    public Tileset dirt;
    public Tileset cobble;
    public Tileset brick;
    public Tileset grass;
    public Tileset wood;
    public Tileset shadow;
    public GameObject[] chest;

    public Environment[] envrs;

    public int envIdx = 0;

    public enum tileType { dirt, cobble, brick, grass, wood, shadow };


    public Tileset Tileset(tileType tileType)
    {
        Dictionary<tileType, Tileset> tilesets = new Dictionary<tileType, Tileset>()
        {
            { tileType.dirt,   dirt     },
            { tileType.cobble, cobble   },
            { tileType.brick,  brick    },
            { tileType.grass,  grass    },
            { tileType.wood,   wood     },
            { tileType.shadow, shadow   }
        };

        return tilesets[tileType];
    }
}

[System.Serializable]
public struct Environment
{
    public EnvLayer ground0;
    public EnvLayer ground1;
    public EnvLayer platform;
    public EnvLayer wall;
    public EnvLayer walltop;
    public EnvLayer shadow;
    [Space]
    public ColorRange portalColor;
    public ColorRange lightColor;

    public bool walls; 
}

[System.Serializable]
public struct EnvLayer
{
    public TileLibrary.tileType tile;
    public ColorRange colorRange;
    [Range(0f, 5f)]
    public float contrast;

    public float rgbValue(Color color)
    {
        Vector3 hsv = new Vector3();
        Color.RGBToHSV(color, out hsv.x, out hsv.y, out hsv.z);

        return hsv.z;
    }
}

[System.Serializable]
public struct Tileset
{
    public Tile[] t;
}


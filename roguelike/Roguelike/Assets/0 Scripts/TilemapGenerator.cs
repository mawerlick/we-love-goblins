using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.Universal;

public class TilemapGenerator : MonoBehaviour
{
    public TileLibrary tL;
    public DungeonGenerator dG;
    public Shader grayscale;

    public Tilemap ground0;
    public Tilemap ground1;
    public Tilemap platform;
    public Tilemap wall;
    public Tilemap walltop;
    public Tilemap shadow;

    public void GenerateTilemaps(Vector2Int mapdims)
    {
        Environment env = tL.envrs[Random.Range(0,tL.envrs.Length)];
        ground0.ClearAllTiles();
        ground1.ClearAllTiles();
        platform.ClearAllTiles();
        wall.ClearAllTiles();
        walltop.ClearAllTiles();
        shadow.ClearAllTiles();

        int hasWalls = env.walls ? 2 : 0;

        FillShadow(mapdims,hasWalls);

        Blobs(mapdims, ground1, tL.Tileset(env.ground1.tile));
        Blobs(mapdims, platform, tL.Tileset(env.platform.tile));
        FillGrid(new Vector2Int(mapdims.x + 4, mapdims.y + 4), new Vector2Int(-2, -2), ground0, tL.Tileset(env.ground0.tile).t[0]);
        if (hasWalls == 2)
        {
            FillGrid(new Vector2Int(mapdims.x, 1), new Vector2Int(0, mapdims.y), wall, tL.Tileset(env.wall.tile).t[7]);
            FillGrid(new Vector2Int(mapdims.x, 1), new Vector2Int(0, mapdims.y+1), wall, tL.Tileset(env.wall.tile).t[2]);
        }
        //
        //FillGrid(new Vector2Int(mapdims.x, 4), new Vector2Int(0, mapdims.y - 1), wall, tL.Tileset(env.wall.tile).t[0]);
        SetTilemapColors(env);
    }

    public void FillGrid(Vector2Int dims, Vector2Int offset, Tilemap tilemap, Tile tile)
    {
        for (int x = 0; x < dims.x; x++) for (int y = 0; y < dims.y; y++) tilemap.SetTile(new Vector3Int(x+offset.x, y+offset.y, 1), tile);
    }

    public void FillShadow(Vector2Int dims, int hW)
    {
        Tileset sdw = tL.shadow;
        Vector2Int fDims = new Vector2Int(dims.x + 22, dims.y + 12);
        Vector2Int eDims = new Vector2Int(dims.x + 2, dims.y + 2 + hW);
        int[,] gridVals = new int[fDims.x, fDims.y];

        for (int x = 0; x < fDims.x; x++) for (int y = 0; y < fDims.y; y++) gridVals[x, y] = 1;
        for (int x = 0; x < eDims.x; x++) for (int y = 0; y < eDims.y; y++) gridVals[x+10, y+5] = 0;

        for (int x = 0; x < fDims.x; x++) for (int y = 0; y < fDims.y; y++) if (gridVals[x, y] == 1) shadow.SetTile(new Vector3Int(x-11,y-6,1),sdw.t[0]); //pure shade

        for (int x = 0; x < dims.x; x++)
        {
            shadow.SetTile(new Vector3Int(x, -1, 1), sdw.t[1]);
            shadow.SetTile(new Vector3Int(x, dims.y+hW, 1), sdw.t[1]); shadow.SetTransformMatrix(new Vector3Int(x, dims.y + hW, 1), Matrix4x4.TRS(Vector2.zero, Quaternion.Euler(0, 0, 180f), Vector3.one));
        }
        for (int y = 0; y < dims.y+hW; y++)
        {
            shadow.SetTile(new Vector3Int(-1, y, 1), sdw.t[1]); shadow.SetTransformMatrix(new Vector3Int(-1, y, 1), Matrix4x4.TRS(Vector2.zero,Quaternion.Euler(0, 0, -90f), Vector3.one));
            shadow.SetTile(new Vector3Int(dims.x, y, 1), sdw.t[1]); shadow.SetTransformMatrix(new Vector3Int(dims.x, y, 1), Matrix4x4.TRS(Vector2.zero, Quaternion.Euler(0, 0, 90f), Vector3.one));
        }
        shadow.SetTile(new Vector3Int(dims.x, dims.y+hW, 1), sdw.t[2]);
        shadow.SetTile(new Vector3Int(-1, -1, 1), sdw.t[2]);        shadow.SetTransformMatrix(new Vector3Int(-1, -1, 1)      , Matrix4x4.TRS(Vector2.zero, Quaternion.Euler(0, 0, 180f), Vector3.one));
        shadow.SetTile(new Vector3Int(-1, dims.y+hW, 1), sdw.t[2]);  shadow.SetTransformMatrix(new Vector3Int(-1, dims.y+hW, 1), Matrix4x4.TRS(Vector2.zero, Quaternion.Euler(0, 0, 90f), Vector3.one));
        shadow.SetTile(new Vector3Int(dims.x, -1, 1), sdw.t[2]);    shadow.SetTransformMatrix(new Vector3Int(dims.x, -1, 1)  , Matrix4x4.TRS(Vector2.zero, Quaternion.Euler(0, 0, -90f), Vector3.one));

    }

    public void Blobs(Vector2Int dims, Tilemap tilemap, Tileset tileset)
    {
        int[,] gridVals = new int[dims.x + 6, dims.y + 6];
        int scr = dims.x*dims.y/5;
        for (int x = 0; x < dims.x+5; x++) for (int y = 0; y < dims.y+5; y++) if (Random.Range(0, scr + 1) == 0)
                { gridVals[x, y] = 1; gridVals[x + 1, y] = 1; gridVals[x, y + 1] = 1; gridVals[x + 1, y + 1] = 1; }

        scr /= 5;

        for (int x = 0; x < dims.x + 5; x++) for (int y = 0; y < dims.y + 5; y++)
                if ((gridVals[x, y] == 1 || gridVals[x + 1, y] == 1 || gridVals[x, y + 1] == 1 || gridVals[x + 1, y + 1] == 1) && Random.Range(0, scr + 1) == 0)
                { gridVals[x, y] = 1; gridVals[x + 1, y] = 1; gridVals[x, y + 1] = 1; gridVals[x + 1, y + 1] = 1; }
        for (int x = 0; x < dims.x + 5; x++) for (int y = 0; y < dims.y + 5; y++)
                if ((gridVals[x, y] == 1 || gridVals[x + 1, y] == 1 || gridVals[x, y + 1] == 1 || gridVals[x + 1, y + 1] == 1) && Random.Range(0, scr + 1) == 0)
                { gridVals[x, y] = 1; gridVals[x + 1, y] = 1; gridVals[x, y + 1] = 1; gridVals[x + 1, y + 1] = 1; }

        for (int x = 0; x < dims.x + 6; x++) for (int y = 0; y < dims.y + 6; y++) if (gridVals[x, y] == 1) tilemap.SetTile(new Vector3Int(x - 3, y - 3, 1), tileset.t[0]);
        for (int x = 1; x < dims.x + 5; x++) for (int y = 1; y < dims.y + 5; y++) if (gridVals[x, y] == 1) ConnectTextures(tilemap,tileset,new Vector2Int(x, y));
    }

    public void ConnectTextures(Tilemap tilemap, Tileset tileset, Vector2Int position)
    {
        Vector3Int pos = new Vector3Int(position.x-3, position.y-3, 1);
        Tile tile = tileset.t[0];
        int mask = tilemap.GetTile(pos + new Vector3Int(0, 1, 0))   != null ? 1 : 0;
            mask += tilemap.GetTile(pos + new Vector3Int(1, 0, 0))  != null ? 2 : 0;
            mask += tilemap.GetTile(pos + new Vector3Int(0, -1, 0)) != null ? 4 : 0;
            mask += tilemap.GetTile(pos + new Vector3Int(-1, 0, 0)) != null ? 8 : 0;
        int idx = TileIndex((byte)mask);
        if (idx > 0) tilemap.SetTile(pos,tileset.t[idx]);
    }

    private int TileIndex(byte mask)
    {
        switch (mask)
        {
            case 0 : return 0;
            case 6: return 1;
            case 14: return 2;
            case 12: return 3;
            case 7: return 4;
            case 13: return 5;
            case 3: return 6;
            case 11: return 7;
            case 9: return 8;
            case 15: return 0;
        }
        return -1;
    }

    public void SetTilemapColors(Environment env)
    {
        Color g0 = env.ground0.colorRange.RandomColor();   SetGrayMat(g0,ground0,rgbValue(g0), env.ground0.contrast);
        Color g1 = env.ground1.colorRange.RandomColor();   SetGrayMat(g1, ground1, rgbValue(g1), env.ground1.contrast);
        Color plt = env.platform.colorRange.RandomColor(); SetGrayMat(plt, platform, rgbValue(plt), env.platform.contrast);
        Color wl = env.wall.colorRange.RandomColor();      SetGrayMat(wl, wall, rgbValue(wl), env.wall.contrast);
        Color wltp = env.walltop.colorRange.RandomColor(); SetGrayMat(wltp, walltop, rgbValue(wltp), env.walltop.contrast);
        Color portalColor = env.portalColor.grad0.Evaluate(Random.Range(0f,1f));
        foreach (GameObject p in dG.portals)
        {
            Material tempmat = new Material(dG.portalShader);
            tempmat.SetColor("tint", portalColor);
            p.GetComponent<SpriteRenderer>().sharedMaterial = tempmat;
            p.transform.GetChild(0).GetComponent<Light2D>().color = Color.Lerp(portalColor, Color.white, 0.25f);
        }
    }

    public void SetGrayMat(Color color, Tilemap tilemap, float b, float c)
    {
        Material tempMat = new Material(grayscale);
        tempMat.SetColor("tint", color);
        tempMat.SetVector("bc", new Vector2(b, c));
        tilemap.GetComponent<TilemapRenderer>().sharedMaterial = tempMat;
    }

    public float rgbValue(Color color)
    {
        Vector3 hsv = new Vector3();
        Color.RGBToHSV(color, out hsv.x, out hsv.y, out hsv.z);

        return hsv.z;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject goblinPrf;
    public List<GameObject> goblins;
    public TilemapGenerator tG;
    public playerConroller player;
    public GameObject portal;
    public Shader portalShader;
    public List<GameObject> portals;
    [SerializeField]
    public int[,] grid;
    private List<Vector2Int> portalLocs = new List<Vector2Int>();
    public Vector2Int mapDims = Vector2Int.zero;
    public GameObject chest;

    void Start()
    {
        GenerateDungeon();
    }

    [ContextMenu("Generate Dungeon")]
    public void GenerateDungeon()
    {

        int dir = 1;
        if (player.gridLoc.x == 1) dir = -1;
        else if (player.gridLoc.y == 1) dir = -2;
        else if (player.gridLoc.y == mapDims.y-2) dir = 2;

        int i = Random.Range(5, 9);
        int j = Random.Range(5, 9);
        i *= 2; j *= 2;
        grid = new int[i, j];
        mapDims = new Vector2Int(i, j);


        if (goblins.Count != 0) for (int g = goblins.Count - 1; g >= 0; g--) { if (Application.isPlaying) Destroy(goblins[g].gameObject); else DestroyImmediate(goblins[g].gameObject); }
        goblins.Clear();

        if (portals.Count != 0) for (int p = portals.Count - 1; p >= 0; p--) { if (Application.isPlaying) Destroy(portals[p].gameObject); else DestroyImmediate(portals[p].gameObject); }
        portals.Clear();

        GeneratePortals(dir);
        for (int p = 0; p < portalLocs.Count; p++) grid[portalLocs[p].x, portalLocs[p].y] = 2;

        tG.GenerateTilemaps(mapDims);

        GenerateGoblins(mapDims);

        if (!Progressable()) GameOver();
    }


    private void GenerateGoblins(Vector2Int dims)
    {
        int scr = dims.x * dims.y / 5;
        int index = 0;

        for (int x = 1; x < dims.x - 1; x++) for (int y = 1; y < dims.y - 1; y++) if (Random.Range(0, scr + 1) == 0 && CheckAroundPortal(x,y) && grid[x,y] == 0)
                {
                    InstantiateGoblin(x, y, index); index++;
                }
    }

    private void InstantiateGoblin(int x, int y, int index)
    {
        GameObject goblin = Instantiate(goblinPrf);
        CharRandomizer randomizer = goblin.GetComponent<CharRandomizer>();
        randomizer.library = tG.tL.gameObject;
        randomizer.goblin = true;
        randomizer.RandomizeChar();
        goblin.transform.position = new Vector3(x * 64 + 32.5f, y * 64 + 12.5f, 1);
        goblins.Add(goblin);
        grid[x, y] = 4 + index;
        Debug.Log(grid[x, y]);
        GoblinPatrol gp = goblin.GetComponent<GoblinPatrol>();
        gp.dG = this;
        gp.index = index;
        gp.direction = gp.RandomDirection();
        gp.gridLoc = new Vector2Int(x, y);
    }

    private bool CheckAroundPortal(int x, int y)
    {
        if (grid[x + 1, y] == 2) return false;
        else if (grid[x, y + 1] == 2) return false;
        else if (grid[x - 1, y] == 2) return false;
        else if (grid[x, y - 1] == 2) return false;

        else if (grid[x + 1, y + 1] == 2) return false;
        else if (grid[x - 1, y + 1] == 2) return false;
        else if (grid[x - 1, y - 1] == 2) return false;
        else if (grid[x + 1, y - 1] == 2) return false;
        return true;
    }

    private void GeneratePortals(int entryDir)
    {
        int portalCount = Random.Range(0, 3);
        portalLocs.Clear();

        portalLocs.Add(InstantiatePortal(-entryDir));
        player.gameObject.transform.position = new Vector3(portalLocs[0].x * 64 + 32.5f, portalLocs[0].y * 64 + 12.5f, 1);
        player.direction = entryDir;
        player.gridLoc = portalLocs[0];
        if(Application.isPlaying)player.Move();

        List<int> dirs = new List<int> { 1, -1, 2, -2 };
        dirs.Remove(-entryDir);
        for (int i = 0; i < dirs.Count; i++)
        {
            int temp = dirs[i];
            int randomIndex = Random.Range(i, dirs.Count);
            dirs[i] = dirs[randomIndex];
            dirs[randomIndex] = temp;
        }
        for (int i = 0; i < portalCount + 1; i++)
        {
            portalLocs.Add(InstantiatePortal(dirs[i]));
        }

        int chs = Random.Range(0, 11);
        bool cst = chs == 0? true:false;
        if (cst)
        {
            while (cst)
            {
                int x = Random.Range(0, mapDims.x);
                int y = Random.Range(0, mapDims.y);
                if (grid[x,y] == 0)
                {
                    grid[x, y] = 3;
                    chest = Instantiate(tG.tL.chest[Random.Range(0, 2)]);
                    chest.transform.position = new Vector3(x * 64, y * 64, 1);
                    CharRandomizer cr = player.GetComponent<CharRandomizer>();
                    Material chestmat = new Material(cr.rgbcmy);
                    chestmat.SetColor("tint_0", cr.libColor.metals0[Random.Range(0, cr.libColor.metals0.Length)].RandomColor());
                    chestmat.SetColor("tint_4", cr.libColor.metals0[Random.Range(0, cr.libColor.metals0.Length)].RandomColor());
                    chest.GetComponent<SpriteRenderer>().sharedMaterial = chestmat;
                    cst = false;
                }
            }
        }
    }

    public void LootChest(Vector2Int loc)
    {
        player.health += (Random.Range(1, 5)*5);
        player.portalpoint += Random.Range(1, 5);
        grid[loc.x, loc.y] = 0;
        Destroy(chest);
        player.UpdateCurrency();
        if (!Progressable()) GameOver();
    }

    private Vector2Int InstantiatePortal(int portalDir)
    {
        GameObject portal0 = Instantiate(portal);
        int randX = Random.Range(2, mapDims.x - 2);
        int randY = Random.Range(2, mapDims.y - 2);

        Vector2Int portalLoc = Vector2Int.one;
        switch (portalDir)
        {
            case -1: portalLoc = new Vector2Int(0, randY); break;
            case 1:  portalLoc = new Vector2Int(mapDims.x-1, randY); break;
            case -2: portalLoc = new Vector2Int(randX, 0); break;
            case 2:  portalLoc = new Vector2Int(randX, mapDims.y - 1); portal0.GetComponent<SpriteRenderer>().sortingOrder = 19; break;
        }

        portal0.transform.position = new Vector3(portalLoc.x * 64 + 32, portalLoc.y * 64 + 64, 1);
        portals.Add(portal0);
        portal0.transform.name = "portal";
        return portalLoc;
    }

    public void MoveGoblins()
    {
        foreach (GameObject g in goblins) { g.GetComponent<GoblinPatrol>().Move(); }
        
    }

    public bool Progressable()
    {
        if (chest == null && goblins.Count == 0 && player.portalpoint == 0) return false;
        return true;
    }

    public void GameOver()
    {

    }
}

//  GRID
//
//  0 = walkable
//  1 = player
//  2 = portal
//  3 = chest
//  4+= goblins

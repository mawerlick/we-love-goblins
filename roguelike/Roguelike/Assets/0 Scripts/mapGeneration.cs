using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mapGeneration : MonoBehaviour
{

    public int width;
    public int height;


    public Tilemap tilemap;
    public TileBase[] tiles;
    int[,] map;

    // Start is called before the first frame update
    void Start()
    {
        int[,] map = new int[width, height];
        map= GenerateArray(width, height, false);

        RenderMap(map, tilemap, tiles);
    }
    /*
    private void CreateRandomPoints()
    {
        for (int i = 0; i < numberOfSeeds; i++)
        {

            Vector3 randomPosition = new Vector3(Random.Range(0, rows), 0, Random.Range(0, columns));
            seeds.Add(randomPosition);

            int randomTileNumber = Random.Range(0, tilePrefabs.Length);
            tilePrefabIndex.Add(randomTileNumber);

            GameObject tile = Instantiate(tilePrefabs[randomTileNumber], randomPosition, Quaternion.identity);
            tile.transform.parent = container.transform;

        }
    }

    */

    public static int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }

    public static void RenderMap(int[,] map, Tilemap tilemap, TileBase[] tiles)
    {
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    int aa = Random.Range(0, tiles.Length);

                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[aa]);
                }
            }
        }
    }
}

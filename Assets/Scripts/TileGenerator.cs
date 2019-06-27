using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Range(1,1000)]
    public int TileWidth = 10;

    [Range(1, 1000)]
    public int TileLength = 10;

    [Range(1, 10)]
    public int GenerationRadius = 1;

    public Material[] MaterialPool;

    private int TileFieldWidth;

    private int TileFieldLength;

    private (int X, int Z)[,] TileCoordinates;

    private Terrain[,] Tiles;

    // Start is called before the first frame update
    public void Start()
    {
        TileFieldWidth = TileFieldLength = (2 * GenerationRadius) + 1;
        TileCoordinates = new (int X, int Z)[TileFieldWidth, TileFieldLength];
        Tiles = new Terrain[GenerationRadius, GenerationRadius];

        SetTileCoordinates();
        for(int i = 0; i < TileFieldWidth; i++)
        {
            for(int j = 0; j < TileFieldLength; j++)
            {
                var terrainData = new TerrainData();
                terrainData.heightmapResolution = 129;
                terrainData.size = new Vector3(TileWidth, 600, TileLength);

                var heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
                for(int x = 0; x < terrainData.heightmapResolution; x++)
                {
                    for(int y = 0; y < terrainData.heightmapResolution; y++)
                    {
                        //heights[x, y] = Random.Range(0, 0.1F) * 0.05F;
                        heights[x, y] = Mathf.Sin(Mathf.Sqrt(Mathf.Pow(TileCoordinates[i, j].X + x, 2) + Mathf.Pow(TileCoordinates[i, j].Z + y, 2))) * 0.0001F;
                    }
                }
                terrainData.SetHeights(0, 0, heights);
                var terrain = Terrain.CreateTerrainGameObject(terrainData);
                terrain.transform.position = new Vector3(TileCoordinates[i, j].X, gameObject.transform.position.y, TileCoordinates[i, j].Z);
                terrain.transform.parent = gameObject.transform;

                if (MaterialPool.Any()) {
                    terrain.GetComponent<Terrain>().materialTemplate = MaterialPool[Random.Range(0, MaterialPool.Count())];
                }
                terrain.name = $"Tile[{i},{j}]";
                if(i == 0 && i == j)
                    terrain.AddComponent(typeof(WavyTerrain));
            }
        }

        //InvokeRepeating(nameof(ShuffleTileMaterials), 0F, 1F);

    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public void ShuffleTileMaterials()
    {
        foreach(var childTerrain in gameObject.GetComponentsInChildren<Terrain>())
        {
            childTerrain.materialTemplate = MaterialPool[Random.Range(0, MaterialPool.Count())];
        }
    }

    private void SetTileCoordinates()
    {
        (int X, int Z) middleTileCoordinates;
        var xIsNegative = transform.position.x < 0;
        var zIsNegative = transform.position.z < 0;
        var absX = Mathf.Abs(transform.position.x);
        var absZ = Mathf.Abs(transform.position.z);
        middleTileCoordinates.X = (int)(absX - (absX % TileWidth));
        middleTileCoordinates.Z = (int)(absZ - (absZ % TileWidth));

        var middleTileXIndex = TileFieldWidth / 2;
        var middleTileZIndex = TileFieldLength / 2;

        TileCoordinates[middleTileXIndex, middleTileZIndex] = middleTileCoordinates;

        for(int i = 0; i < TileFieldWidth; i++)
        {
            for(int j = 0; j < TileFieldLength; j++)
            {
                TileCoordinates[i, j].X = ((i - middleTileXIndex) * TileWidth) + middleTileCoordinates.X;
                TileCoordinates[i, j].Z = ((j - middleTileZIndex) * TileLength) + middleTileCoordinates.Z;
            }
        }
    }
}

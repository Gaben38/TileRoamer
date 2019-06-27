using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WavyTerrain : MonoBehaviour
{
    private Terrain Terrain;
    // Start is called before the first frame update
    public void Start()
    {
        Terrain = gameObject.GetComponent<Terrain>();
        InvokeRepeating(nameof(RemakeHeightMap), 0F, 0.01F);
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public void RemakeHeightMap()
    {

        var heights = Terrain.terrainData.GetHeights(0, 0, Terrain.terrainData.heightmapResolution, Terrain.terrainData.heightmapResolution);
        /*
        for (int x = 0; x < Terrain.terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < Terrain.terrainData.heightmapResolution; y++)
            {
                //heights[x, y] = Random.Range(0, 0.1F) * 0.05F;
                heights[x, y] = Mathf.Sin(
                    Mathf.Sqrt(
                        Mathf.Pow(
                            gameObject.transform.position.x + x, 2) 
                            + 
                            Mathf.Pow(gameObject.transform.position.z + y, 2))
                            -
                            Time.realtimeSinceStartup) 
                        * 
                        0.0001F;
            }
        }*/

        var res = Terrain.terrainData.heightmapResolution;
        var xPos = gameObject.transform.position.x;
        var zPos = gameObject.transform.position.z;
        var time = Time.realtimeSinceStartup;
        Parallel.For(0, res,
            x =>
            {
                Parallel.For(0, res,
                y =>
                {
                    heights[x, y] = Mathf.Sin(Mathf.Sqrt(Mathf.Pow(xPos + x, 2) + Mathf.Pow(zPos + y, 2)) - 2 * time) * 0.0001F;
                    y++;
                });
                x++;
            }
            );
        Terrain.terrainData.SetHeights(0, 0, heights);
    }
}

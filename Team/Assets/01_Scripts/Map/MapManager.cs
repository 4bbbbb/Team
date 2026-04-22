using UnityEngine;

/// <summary>
/// 1. 甘 单捞磐 积己
/// 2. 没农 积己
/// 3. 阿 没农啊 磊扁 备开 鸥老 积己
/// </summary>

[ExecuteAlways]
public class MapManager : MonoBehaviour
{
    [Header("<< 甘 荤捞令 >>")]
    [SerializeField] private int mapWidth = 128;
    [SerializeField] private int mapHeight = 128;
    [SerializeField] private int chunkSize = 16;

    [Header("<< 甘 橇府普 >>")]
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private GameObject waterPrefab;

    private TileData[,] mapData;

    private void Start()
    {
        if (Application.isPlaying)
        {
            GenerateMap();
        }
    }

    [ContextMenu("Generate Map")]
    private void GenerateMap()
    {
        ClearMap();
        GenerateMapData();
        GenerateChunks();
    }

    [ContextMenu("Clear Map")]
    private void ClearMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (Application.isPlaying)
            {
                Destroy(child);
            }
            else
            {
                DestroyImmediate(child);
            }
        }
    }


    // 1. 甘 单捞磐 积己
    private void GenerateMapData()
    {
        mapData = new TileData[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                mapData[x, z] = new TileData
                {
                   x = x,
                   z = z,
                   height = 1,
                   tileType = TileType.Grass,
                   buildable = true,
                   occupied = false

                };
            }
        }
    }

    // 2. 鸥老 橇府普 积己
    private void GenerateChunks()
    {
       int chunkCountX = mapWidth / chunkSize;
       int chunkCountZ = mapHeight / chunkSize;

        for (int ccx  = 0; ccx < chunkCountX; ccx++)
        {
            for (int ccz = 0;  ccz < chunkCountZ; ccz++)
            {
                GameObject chunkObject = new GameObject($"Chunk_{ccx}_{ccz}");                
                chunkObject.transform.parent = transform;

                Chunk chunk = chunkObject.AddComponent<Chunk>();
                chunk.Init(ccx, ccz, chunkSize, mapData, grassPrefab, waterPrefab);
                chunk.GenerateChunk();
            }
        }           
    }
}

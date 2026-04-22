using UnityEngine;

public class Chunk : MonoBehaviour
{
    private int chunkX;
    private int chunkZ;
    private int chunkSize;

    private TileData[,] mapData;

    private GameObject grassPrefab;
    private GameObject waterPrefab;

    public void Init(
        int chunkX, 
        int chunkZ, 
        int chunkSize, 
        TileData[,] mapData, 
        GameObject grassPrefab, 
        GameObject waterPrefab)
    {
        this.chunkX = chunkX;
        this.chunkZ = chunkZ;
        this.chunkSize = chunkSize;
        this.mapData = mapData;
        this.grassPrefab = grassPrefab;
        this.waterPrefab = waterPrefab;
    }

    public void GenerateChunk()
    {
        if (mapData == null)
            return;

        int startX = chunkX * chunkSize;
        int startZ = chunkZ * chunkSize;

        float offsetX = mapData.GetLength(0) / 2f;
        float offsetZ = mapData.GetLength(1) / 2f;

        for (int x = startX; x < startX + chunkSize; x++)
        {
            for (int z = startZ; z < startZ + chunkSize; z++)
            {
                TileData tileData = mapData[x, z];

                GameObject tilePrefabToSpawn = null;

                switch (tileData.tileType)
                {
                    case TileType.Grass:
                        tilePrefabToSpawn = grassPrefab;
                        break;

                    case TileType.Water:
                        tilePrefabToSpawn = waterPrefab;
                        break;
                }

                if (tilePrefabToSpawn != null)
                {
                    Vector3 spawnPos = new Vector3(x - offsetX, 0, z - offsetZ);
                    Instantiate(tilePrefabToSpawn, spawnPos, Quaternion.identity, transform);
                }
            }
        }
    }
}

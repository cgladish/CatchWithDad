using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDetails : MonoBehaviour
{
    private Terrain m_Terrain;
    private float terrainWidthToDetailsWidthRatio;
    private float terrainHeightToDetailsHeightRatio;
    private Vector2 centerOfTerrain;

    // Start is called before the first frame update
    void Start()
    {
        m_Terrain = GetComponent<Terrain>();
        Vector3 terrainSize = m_Terrain.terrainData.size;
        terrainWidthToDetailsWidthRatio = terrainSize.x / m_Terrain.terrainData.detailWidth;
        terrainHeightToDetailsHeightRatio = terrainSize.z / m_Terrain.terrainData.detailHeight;
        centerOfTerrain = new Vector2(terrainSize.x / 2, terrainSize.z / 2);
        SetDensity(0, 50, 0);
        SetDensity(1, 50, 0);
        SetDensity(2, 0, 50);
        SetDensity(3, 0, 50);
        SetDensity(4, 0, 50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit() {
        for (int layerIndex = 0; layerIndex < m_Terrain.terrainData.detailPrototypes.Length; layerIndex++) {
            SetDensity(layerIndex, 0, 0);
        }
    }

    private void SetDensity(int layerIndex, int densityWithinBoundaries, int densityOutsideBoundaries) {
        int[,] detailLayer = m_Terrain.terrainData.GetDetailLayer(
            0,
            0,
            m_Terrain.terrainData.detailWidth,
            m_Terrain.terrainData.detailHeight,
            layerIndex
        );
        for (int x = 0; x < m_Terrain.terrainData.detailWidth; x++) {
            for (int y = 0; y < m_Terrain.terrainData.detailHeight; y++) {
                Vector2 location = new Vector2(x * terrainWidthToDetailsWidthRatio, y * terrainHeightToDetailsHeightRatio);
                if (Vector2.Distance(location, centerOfTerrain) > TerrainWall.WALL_RADIUS) {
                    detailLayer[x, y] = densityOutsideBoundaries;
                } else {
                    detailLayer[x, y] = densityWithinBoundaries;
                }
            }
        }
        m_Terrain.terrainData.SetDetailLayer(0, 0, layerIndex, detailLayer);
    }
}

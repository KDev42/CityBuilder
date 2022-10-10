using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMaterial : MonoBehaviour
{
    [SerializeField] Material grass;
    [SerializeField] Material sand;
    [SerializeField] Material water;
    [SerializeField] Material swamp;

    public static TerrainMaterial Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(GetComponent<TerrainMaterial>());
    }

    public Material GetMaterial(Types.Terrain typeTerrain)
    {
        switch (typeTerrain)
        {
            case Types.Terrain.grass:
                return grass;
            case Types.Terrain.sand:
                return sand;
            case Types.Terrain.water:
                return water;
            case Types.Terrain.swamp:
                return swamp;
        }
        return null;
    }
}

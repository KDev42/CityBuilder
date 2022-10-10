using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBuilding : MonoBehaviour
{
    public List<Building> buildings;
    private int maxCount;
    private int buildCount;

    private byte[] permutationTable;

    private int GetPseudoRandomGradient(int x, int y, int maxRange)
    {
        int v = (int)(((x * 1836311903) ^ (y * 2971215073) + 4807526976) & 1023);
        v = permutationTable[v] & maxRange;

        Debug.Log(v);

        return v;
    }

    public void GetBuildingGrid(Cell[,] gridTerrain, int seed = 0)
    {
        var rand = new System.Random(seed);
        permutationTable = new byte[1024];
        rand.NextBytes(permutationTable);

        buildCount = 0;

        maxCount = (int)(gridTerrain.Length * 0.1f);

        GenerateCity(gridTerrain);
    }

    private void GenerateCity(Cell[,] gridTerrain)
    {
        Vector2Int sizeBuilding;
        int randomBuildingID;

        for (int i = 0; i < gridTerrain.GetLength(0); i+=3)
            for (int j = 0; j < gridTerrain.GetLength(1); j+=3)
            {
                randomBuildingID = GetPseudoRandomGradient(i,j,buildings.Count-1);
                sizeBuilding = buildings[randomBuildingID].size;

                if (Grid.Instance.CanBuild(new Vector2Int(i, j), sizeBuilding, buildings[randomBuildingID].buildingSite))
                {
                    Grid.Instance.SetBuild(new Vector2Int(i, j), sizeBuilding, randomBuildingID);
                    buildCount+= sizeBuilding.x* sizeBuilding.y;

                    if(buildCount >= maxCount)
                    {
                        return;
                    }
                }
            }
    }
}

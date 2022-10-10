using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class SaveLoadData
{
    private static StoredData storedData = new StoredData();
    private static bool storedDataIsLoad;

    public static bool HasStoredData()
    {
        return PlayerPrefs.HasKey("StoredData");
    }

    public static StoredData GetStoredData()
    {
        if (!storedDataIsLoad)
            LoadData(() => { });

        return storedData;
    }

    public static void LoadData(Action callBack)
    {
        storedDataIsLoad = true;
        storedData = JsonUtility.FromJson<StoredData>(PlayerPrefs.GetString("StoredData"));
        Debug.Log( PlayerPrefs.GetString("StoredData"));
        callBack();
    }

    public static void SaveSeed(int newSeed)
    {
        storedData.seed = newSeed;
    }

    public static void AddChangeCell(Cell changedCell)
    {
        if (storedData.changedCells == null)
            storedData.changedCells = new List<StoredCell>();

        StoredCell storedCell = SaveCell(changedCell);

        if(HasCell(changedCell.Coordinates, out int index))
        {
            storedData.changedCells[index] = storedCell;
        }
        else
        {
            storedData.changedCells.Add(storedCell);
        }
    }

    private static StoredCell SaveCell(Cell cell)
    {
        return new StoredCell
        {
            terrainType = cell.TerrainType,
            hasBuild = cell.HasBuild,
            coordinates = cell.Coordinates,
            buildId = cell.BuildId
        };
    }

    private static bool HasCell(Vector2Int coordinates, out int index)
    {
        for(int i=0;i<storedData.changedCells.Count;i++)
        {
            if(storedData.changedCells[i].coordinates == coordinates)
            {
                index = i;
                return true;
            }
        }

        index = 0;
        return false;
    }

    public static void SaveData()
    {
        string js = JsonUtility.ToJson(storedData);
        PlayerPrefs.SetString("StoredData", js);
    }
}

[Serializable] 
public class StoredData
{
    public int seed;
    public List<StoredCell> changedCells;
}

[Serializable] 
public class StoredCell
{
    public Types.Terrain terrainType;
    public bool hasBuild;
    public Vector2Int coordinates;
    public int buildId;
}

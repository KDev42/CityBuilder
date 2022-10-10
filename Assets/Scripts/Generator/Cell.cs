using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Cell
{
    private Types.Terrain terrainType = Types.Terrain.nothing;
    private bool hasBuild;
    private bool notFirst;

    public Types.Terrain TerrainType
    {
        get { return terrainType; }
        set
        {
            if (value != terrainType && terrainType != Types.Terrain.nothing)
            {
                CellWasChange();
            }
            terrainType = value;
        }
    }
    public bool HasBuild
    {
        get { return hasBuild; }
        set
        {
            if (value != hasBuild && notFirst)
            {
                notFirst = true;
                CellWasChange();
            }
            hasBuild = value;
        }
    }

    public Tile tile { get; set; }
    public float HeightValue { get; set; }
    public Vector2Int Coordinates { get; set; }
    //public bool CellChanged { get;private set; }
    public int BuildId { get; set; } = -1;

    private void CellWasChange()
    {
        //CellChanged = true;
        SaveLoadData.AddChangeCell(this);
    }
}

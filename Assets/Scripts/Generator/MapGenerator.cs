using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
	[SerializeField] List<MapFilling> mapContent;
	[SerializeField] int width = 100;
	[SerializeField] int height = 100;
	[SerializeField] int terrainOctaves =3;
	[SerializeField] float terrainFrequency = 1.25f;
	[SerializeField] GenerateBuilding generateBuilding;

	private PerlinNoise noisePerlin;

	private float[,] heightMap;
	private MapData heightData;
	private Cell[,] grid;

	public List<MapFilling> MapContent
    {
        get
		{
			for (int i=0;i<mapContent.Count;i++)
			{
				if(i>0)
					mapContent[i].maxNumberTiles = (int)(width * height * (mapContent[i].percent - mapContent[i-1].percent));
				else
					mapContent[i].maxNumberTiles = (int)(width * height * mapContent[i].percent);
			}
			return mapContent;
        }
        //private set { mapContent = value; }
	}

	public Cell[,] GetGrid(int seed)
	{
		foreach (MapFilling m in MapContent)
		{
			m.currentNumberTiles = 0;
		}
		noisePerlin = new PerlinNoise(seed);
		SaveLoadData.SaveSeed(seed);

		Initialize();
        GetData(ref heightData);
        LoadTiles();
		Grid.Instance.grid = grid; 
		generateBuilding.GetBuildingGrid(grid, seed);

		return grid;
    }

	private void Initialize()
	{
		heightMap = noisePerlin.Generate(width, height, terrainOctaves, terrainFrequency);
	}

	private void GetData(ref MapData mapData)
	{
		mapData = new MapData(width, height);

		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				float value = heightMap[x, y];

                if (value > mapData.Max) mapData.Max = value;
                if (value < mapData.Min) mapData.Min = value;

                mapData.Data[x, y] = value;
			}
		}
	}

	private void LoadTiles()
	{
		grid = new Cell[width, height];

		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				Cell t = new Cell();
				t.Coordinates = new Vector2Int(x, y);

				float value = heightData.Data[x, y];
				value = (value - heightData.Min) / (heightData.Max - heightData.Min);

				CounterHeinght(value);

				t.HeightValue = value;

				grid[x, y] = t;
			}
		}

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
				HeightIncrease(grid[x, y].HeightValue, out float value);
				grid[x, y].HeightValue = value;
				grid[x, y].TerrainType = GetType(grid[x, y].HeightValue);
            }
        }
    }

	private void HeightIncrease(float height, out float newHeight)
    {
		newHeight = height;

		for (int i = 0; i < MapContent.Count; i++)
		{
			if ( MapContent[i].fixNumber && i > 0 && height> MapContent[i-1].percent* MapContent[i-1].gain && height < MapContent[i].percent* MapContent[i].gain)
			{
				newHeight = MapContent[i].percent;
				MapFilling currentFilling = GetMapFilling(newHeight);
				if (currentFilling.currentNumberTiles < currentFilling.maxNumberTiles)
				{
					currentFilling.currentNumberTiles++;
					return;
				}
				else
					newHeight = height;
			}
		}
	}

	private void CounterHeinght(float value)
	{
		foreach (MapFilling m in MapContent)
		{
			if (value <= m.percent)
			{
				m.currentNumberTiles++;
				break;
			}
		}
	}

	private MapFilling GetMapFilling(float height)
    {
		foreach (MapFilling m in MapContent)
		{
			if (height <= m.percent)
				return m;
		}

		return null;
	}

	private Types.Terrain GetType(float value)
    {
		Types.Terrain currentType = Types.Terrain.grass;

		foreach (MapFilling m in MapContent)
        {
			if(value <= m.percent)
            {
				currentType = m.terrainType;
				break;
			}
        }

		Debug.Log(currentType);

		return currentType;
	}
}

[Serializable]
public class MapFilling
{
    public Types.Terrain terrainType;
    [Range(0,1)] public float percent;
	public int maxNumberTiles { get; set; }
	public int currentNumberTiles { get; set; }
	public bool fixNumber;
	public float gain;
}

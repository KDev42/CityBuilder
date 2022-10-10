using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnMap : MonoBehaviour
{
    [SerializeField] GameObject grassTile;
    [SerializeField] GameObject sandTile;
    [SerializeField] GameObject swampTile;
    [SerializeField] GameObject waterTile;
    [SerializeField] MapGenerator mapGenerator;
	[SerializeField] float sizeTile;
    [SerializeField] List<Building> buildings;
    [SerializeField] Transform buildingsParent;

    private Cell[,] grid;
    private Tile[,] tilesGrid;

    private void Start()
    {
        Spawn();
        Events.Load += UpdateGrid;
        Events.AddBuilding += AddBuilding;
    }

    public void Spawn(Action action = null)
    {
		grid = mapGenerator.GetGrid(UnityEngine.Random.Range(0, int.MaxValue));

		Vector3 spawnPosition;
        Tile tile; tilesGrid = new Tile[grid.GetLength(0), grid.GetLength(1)];

        for (var x = 0; x < grid.GetLength(0); x++)
		{
			for (var y = 0; y < grid.GetLength(1); y++)
			{
				spawnPosition = transform.position + new Vector3(x * sizeTile, 0, y * sizeTile);
                tile = Instantiate(GetTilePrefab(grid[x, y].TerrainType), spawnPosition, new Quaternion(0, 0, 0, 0), transform).GetComponent<Tile>();
                tile.Coordinate = new Vector2Int(x, y);
                tilesGrid[x, y] = tile;
                grid[x, y].tile = tile;

                if(grid[x, y].BuildId >= 0)
                {
                    SpawnBuilding(spawnPosition, grid[x, y].BuildId);
                }
			}
		}

        if(action!=null)
            action();
	}

    public void UpdateGrid()
    {
        Vector3 spawnPosition;

        for (int i = 0; i < buildingsParent.childCount; i++)
            Destroy(buildingsParent.GetChild(i).gameObject);

        grid = Grid.Instance.grid;
        List<Cell> storedCell = new List<Cell>();

        foreach(StoredCell s in SaveLoadData.GetStoredData().changedCells)
        {
            storedCell.Add(DeserealizeCell(s));
        }

        foreach(Cell c in storedCell)
        {
            grid[c.Coordinates.x, c.Coordinates.y] = c;
        }

        for (int x = 0; x < grid.GetLength(0); x++)
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                tilesGrid[x, y].meshRenderer.material = TerrainMaterial.Instance.GetMaterial(grid[x, y].TerrainType);

                grid[x, y].tile = tilesGrid[x, y];
                if (grid[x, y].BuildId >= 0)
                {
                    spawnPosition = transform.position + new Vector3(x * sizeTile, 0, y * sizeTile);
                    SpawnBuilding(spawnPosition, grid[x, y].BuildId);
                }
            }
    }

    private Cell DeserealizeCell(StoredCell storedCell)
    {
        return new Cell
        {
            TerrainType = storedCell.terrainType,
            HasBuild = storedCell.hasBuild,
            Coordinates = storedCell.coordinates,
            BuildId = storedCell.buildId
        };
    }

    private void SpawnBuilding(Vector3 spawnPosition, int buildingId)
    {
        spawnPosition += new Vector3((buildings[buildingId].size.x - 1) / 2f, 0, (buildings[buildingId].size.y - 1) / 2f);
        spawnPosition.y += 1f;
        Instantiate(buildings[buildingId], spawnPosition, new Quaternion(0, 0, 0, 0), buildingsParent);
    }

    private void AddBuilding(Transform buildingObj)
    {
        buildingObj.parent = buildingsParent;
    }

	private GameObject GetTilePrefab(Types.Terrain terrainType)
    {
        switch (terrainType)
        {
            case Types.Terrain.grass:
                return grassTile;
            case Types.Terrain.sand:
                return sandTile;
            case Types.Terrain.swamp:
                return swampTile;
            case Types.Terrain.water:
                return waterTile;
        }

        return null;
    }
}

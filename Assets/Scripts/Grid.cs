using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] TextureGenerator textGenerator;
    [SerializeField] GameObject gridTexture;

    public static Grid Instance { get; private set; }

    public Cell[,] grid { get; set; }
    //public Tile[,] gridTiles { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(GetComponent<Grid>());
    }

    public bool CanBuild(Vector2Int coordinate, Vector2Int size, Types.Terrain buildingSite)
    {
        for(int i =0;i< size.x; i++)
            for(int j =0; j < size.y; j++)
            {
                try
                {
                    if (grid[coordinate.x + i, coordinate.y + j].HasBuild || (buildingSite & grid[coordinate.x + i, coordinate.y + j].TerrainType) == 0)
                        return false;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }

        return true;
    }

    public void SetBuild(Vector2Int coordinate, Vector2Int size, int buildId=0)
    {
        grid[coordinate.x, coordinate.y].BuildId = buildId;

        for (int i = 0; i < size.x; i++)
            for (int j = 0; j < size.y; j++)
            {
                grid[coordinate.x + i, coordinate.y + j].HasBuild = true;
            }
    }

    public void TransformationTile(Vector2Int coordinates)
    {
        Debug.Log(grid[coordinates.x, coordinates.y].tile);
        if (Transformation.CanTurnInto(grid[coordinates.x,coordinates.y].TerrainType, out Types.Terrain newTerrain))
        {
            grid[coordinates.x, coordinates.y].TerrainType = newTerrain;
            grid[coordinates.x, coordinates.y].tile.meshRenderer.material = TerrainMaterial.Instance.GetMaterial(newTerrain);
        }
    }

    public void ActivationGridTexture(Types.Terrain buildingSite)
    {
        gridTexture.GetComponent<MeshRenderer>().materials[0].mainTexture = textGenerator.GetTexture(buildingSite);
        gridTexture.SetActive(true);
    }

    public void DeactivationGridTexture()
    {
        gridTexture.SetActive(false);
    }
}

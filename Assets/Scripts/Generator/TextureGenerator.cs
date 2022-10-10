using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
	private static Color red = new Color(255, 0, 0, 0.3f);
	private static Color green = new Color(0, 255, 0, 0.3f);
	private Dictionary<Types.Terrain, Texture2D> textures;

	public Texture2D GetTexture(Types.Terrain buildingSite)
	{
        try
        {
			return textures[buildingSite];
		}
        catch (System.Exception)
		{
			CreateTecture(buildingSite, out Texture2D texture);
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Point;
			return texture;
		}
    }

	private void CreateTecture(Types.Terrain buildingSite, out Texture2D texture)
	{
		Cell[,] grid = Grid.Instance.grid;
		int width = grid.GetLength(0);
		int height = grid.GetLength(1);
		texture = new Texture2D(width, height);
		var pixels = new Color[width * height];

		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
                if ((grid[x, y].TerrainType&buildingSite)==0 && !grid[x, y].HasBuild)
                {
					pixels[x + y * width] = red;
				}
                else
				{
					pixels[x + y * width] = green;
				}
			}
		}

		texture.SetPixels(pixels);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
	}

	public void UpdateTecture(Vector2Int coordinate, Vector2Int size, Types.Terrain buildingSite)
    {
		Texture2D texture = textures[buildingSite];

	}
}

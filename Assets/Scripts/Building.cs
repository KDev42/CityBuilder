using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Types.Terrain buildingSite;
    public Renderer MainRenderer { get; set; }
    public Vector2Int size = Vector2Int.one;

    private void Awake()
    {
        MainRenderer = GetComponent<Renderer>();
    }

    public void SetTransparent(bool available)
    {
        if (available)
        {
            MainRenderer.material.color = Color.green;
        }
        else
        {
            MainRenderer.material.color = Color.red;
        }
    }

    public void SetNormal()
    {
        MainRenderer.material.color = Color.white;
    }
}

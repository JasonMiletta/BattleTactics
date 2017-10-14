using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileMap : MonoBehaviour {

    public GameObject[,] grid;
    public float width = 10;
    public float height = 10;

    public GameObject gridTile;

    // Use this for initialization
    void Start () {
        createGrid();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            if(width > 0 && height > 0 && grid[0,0] != null)
            {
                foreach(GameObject obj in grid)
                {
                    Destroy(obj);
                }
            } else
            {
                createGrid();
            }
        }
	}

    public void setHeight(UnityEngine.UI.Slider slider)
    {
        height = slider.value;
    }

    public void setWidth(UnityEngine.UI.Slider slider)
    {
        width = slider.value;
    }

    private void createGrid()
    {
        grid = new GameObject[(int)width, (int)height];
        for (var i = 0; i < width; ++i)
        {
            for (var j = 0; j < height; ++j)
            {
                Vector3 spawnPosition = new Vector3(i, 0, j);
                grid[i, j] = Instantiate(gridTile, spawnPosition, this.transform.rotation, this.transform);
                grid[i, j].name = "GridTile " + i + " " + j;

                //Create a border of blank tiles to mess with
                if (i != 0 && i != width - 1 && j != 0 && j != height - 1)
                {
                   grid[i, j].GetComponent<GridTile>().enableTile();
                }
            }
        }
    }

    public static WorldTileMap createGridFromJSONData(WorldTileEditor.WorldJSONWrapper jsonWrapper)
    {
        WorldTileMap map = new WorldTileMap();
        //TODO Manually walk through json, reating the grid and each tile
        map.height = jsonWrapper.height;
        map.width = jsonWrapper.width;
        
        return map;
    }
}

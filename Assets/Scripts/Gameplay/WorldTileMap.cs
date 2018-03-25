﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileMap : MonoBehaviour {

    [SerializeField]
    public GameObject[,] grid;
    public float width = 10;
    public float height = 10;

    public GameObject gridTile;
    public string levelName;

    // Use this for initialization
    void Start () {
        if (levelName != null)
        {
            //Create the new one from WorldJsonUtility
            WorldJsonUtility.WorldJSONWrapper newMapWrapper = WorldJsonUtility.loadLevelData(this, levelName);

            //Properly initialize it into the game
            updateMapFromJsonWrapper(newMapWrapper);
        }
        else
        {
            createGrid();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (width > 0 && height > 0 && grid[0, 0] != null)
            {
                foreach (GameObject obj in grid)
                {
                    Destroy(obj);
                }
            }
            else
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
            }
        }
    }

    public void updateMapFromJsonWrapper(WorldJsonUtility.WorldJSONWrapper jsonWrapper)
    {
        //Manually walk through json, creating the grid and each tile
        this.height = jsonWrapper.height;
        this.width = jsonWrapper.width;

        this.grid = new GameObject[(int)this.width, (int)this.height];
        List<WorldJsonUtility.TileJSONWrapper> tileWrapperList = jsonWrapper.tileList;

        foreach(WorldJsonUtility.TileJSONWrapper tileWrapper in tileWrapperList)
        {
            var x = tileWrapper.xCoor;
            var y = tileWrapper.yCoor;

            GameObject newGridTile = this.createNewGridTile(tileWrapper.tileType, x, y);
            this.grid[x, y] = newGridTile;
        }
    }

    private GameObject createNewGridTile(GridTile.tileType tileType, int xCoor, int yCoor)
    {
        Vector3 spawnPosition = new Vector3(xCoor, 0, yCoor);
        GameObject newGridTile = Instantiate(this.gridTile, spawnPosition, this.transform.rotation, this.transform);
        newGridTile.name = "GridTile " + xCoor + " " + yCoor;
        
        GridTile gridTile = newGridTile.GetComponent<GridTile>();
        gridTile.xCoor = xCoor;
        gridTile.yCoor = yCoor;
        gridTile.currentTileType = tileType;
        gridTile.enableTile();

        return newGridTile;
    }

}
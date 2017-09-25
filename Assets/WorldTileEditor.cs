﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldTileEditor : MonoBehaviour {
    public Cursor cursor;
    public WorldTileMap worldMap;

    private string gameDataProjectFilePath = "/levelData/levelData.json";

    // Use this for initialization
    void Start () {
        cursor = FindObjectOfType<Cursor>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setSelectedTile(string tileType)
    {
    
        StartCoroutine(cursor.selectedGridTile.setTilePrefab(tileType));
        cursor.cursorDeselect();
    }

    public void saveCurrentGridToJSON()
    {
        if (worldMap != null)
        {
            WorldTileMap map = worldMap;

            string jsonData = "";

            jsonData = convertWorldTileGridArrayToJSON(map);
            Debug.Log(jsonData);
            string filePath = Application.dataPath + gameDataProjectFilePath;
            Debug.Log(filePath);
            File.WriteAllText(filePath, jsonData);
        }
    }

    public void loadGridFromJSON(string json)
    {
        WorldTileMap newMap = (WorldTileMap)JsonUtility.FromJson(json, System.Type.GetType("WorldTileMap"));
    }

    private string convertWorldTileGridArrayToJSON(WorldTileMap world)
    {
        WorldJSONWrapper jsonWrapper = new WorldJSONWrapper();
        
        GameObject[,] gridArray = world.grid;

        int width = (int)world.width;
        int height = (int)world.height;

        string tileJsonWrapperList = "[";
        for (var i = 0; i < width; ++i)
        {
            for (var j = 0; j < height; ++j)
            {
                GridTile tileComponent = gridArray[i, j].GetComponent<GridTile>();
                TileJSONWrapper tileWrapper = new TileJSONWrapper(tileComponent.currentTileType, i, j);
                tileJsonWrapperList += JsonUtility.ToJson(tileWrapper) + ", ";
            }
        }
        tileJsonWrapperList = tileJsonWrapperList.TrimEnd(',' , ' ');
        tileJsonWrapperList += "]";
        jsonWrapper.width = width;
        jsonWrapper.height = height;
        jsonWrapper.tileList = tileJsonWrapperList;
        
        return JsonUtility.ToJson(jsonWrapper);
    }

    private class WorldJSONWrapper
    {
        public int width;
        public int height;
        
        public string tileList;
    }

    private class TileJSONWrapper
    {
        public GridTile.tileType tileType;
        public int xCoor;
        public int yCoor;

        public TileJSONWrapper(GridTile.tileType type, int x, int y)
        {
            tileType = type;
            xCoor = x;
            yCoor = y;
        }
        
    }
}

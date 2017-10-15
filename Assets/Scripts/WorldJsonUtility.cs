﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class WorldJsonUtility : MonoBehaviour {
    
    private static string gameDataProjectFilePath = "levelData/levelData";

    #region SAVE
    //TODO: Flesh out saved file structure -> Currently we're simply overwriting levelData
    //TODO: Ideally we'll have a complete map directory 
    //TODO: Properly handle overwriting levels?
    public static void saveCurrentGridToJSON(WorldTileMap worldMap)
    {
        if (worldMap != null)
        {
            WorldTileMap map = worldMap;

            string jsonData = "";

            jsonData = convertWorldTileGridArrayToJSON(map);
            string filePath = Application.dataPath + "/Resources/" + gameDataProjectFilePath;

            if (File.Exists(filePath + ".json"))
            {
                File.WriteAllText(filePath + "1.json", jsonData);
            }
            else
            {
                File.WriteAllText(filePath + ".json", jsonData);
            }
            
        }
    }


    private static string convertWorldTileGridArrayToJSON(WorldTileMap world)
    {
        WorldJSONWrapper jsonWrapper = new WorldJSONWrapper();

        GameObject[,] gridArray = world.grid;

        int width = (int)world.width;
        int height = (int)world.height;

        TileListJSONWrapper<TileJSONWrapper> tileList = new TileListJSONWrapper<TileJSONWrapper>();
        tileList.list = new List<TileJSONWrapper>();

        for (var i = 0; i < width; ++i)
        {
            for (var j = 0; j < height; ++j)
            {
                GridTile tileComponent = gridArray[i, j].GetComponent<GridTile>();
                string debugTileComponent = "[" + i + ", " + j + "]" + " " + tileComponent.currentTileType.ToString();
                Debug.Log(debugTileComponent);
                TileJSONWrapper tileWrapper = new TileJSONWrapper(tileComponent.currentTileType, i, j);
                tileList.list.Add(tileWrapper);
            }
        }

        jsonWrapper.width = width;
        jsonWrapper.height = height;
        jsonWrapper.tileList = tileList.list;
        return JsonUtility.ToJson(jsonWrapper);
    }
    #endregion

    #region LOAD
    //TODO: Flesh out file structure -> Ideally we'll be able to load maps by name
    public static WorldJSONWrapper loadCurrentLevelJSONData(WorldTileMap worldMap)
    {
        string levelFilePath = gameDataProjectFilePath.Replace(".json", "");
        TextAsset targetFile = Resources.Load<TextAsset>(levelFilePath);
        
        return loadGridFromJSON(targetFile.text, worldMap);
    }

    private static WorldJSONWrapper loadGridFromJSON(string json, WorldTileMap worldMap)
    {
        WorldJSONWrapper newMapWrapper = (WorldJSONWrapper)JsonUtility.FromJson(json, System.Type.GetType("WorldJsonUtility").GetNestedType("WorldJSONWrapper"));

        return newMapWrapper;
    }
    #endregion

    #region JSONWrappers
    [Serializable]
    public class WorldJSONWrapper
    {
        public int width;
        public int height;

        public List<TileJSONWrapper> tileList;
    }

    [Serializable]
    public class TileListJSONWrapper<TileJSONWrapper>
    {
        public List<TileJSONWrapper> list;
    }

    [Serializable]
    public class TileJSONWrapper
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
    #endregion
}

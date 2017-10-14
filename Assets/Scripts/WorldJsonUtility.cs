using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class WorldJsonUtility : MonoBehaviour {
    
    private static string gameDataProjectFilePath = "levelData/levelData.json";

    public static void saveCurrentGridToJSON(WorldTileMap worldMap)
    {
        if (worldMap != null)
        {
            WorldTileMap map = worldMap;

            string jsonData = "";

            jsonData = convertWorldTileGridArrayToJSON(map);
            Debug.Log(jsonData);
            string filePath = Application.dataPath + "/Resources/" + gameDataProjectFilePath;
            Debug.Log(filePath);
            File.WriteAllText(filePath, jsonData);
        }
    }

    public static void loadCurrentLevelJSONData(WorldTileMap worldMap)
    {
        string levelFilePath = gameDataProjectFilePath.Replace(".json", "");
        Debug.Log(levelFilePath);
        TextAsset targetFile = Resources.Load<TextAsset>(levelFilePath);

        Debug.Log(targetFile.text);
        loadGridFromJSON(targetFile.text, worldMap);
    }

    public static void loadGridFromJSON(string json, WorldTileMap worldMap)
    {
        WorldJSONWrapper newMapWrapper = (WorldJSONWrapper)JsonUtility.FromJson(json, System.Type.GetType("WorldTileEditor").GetNestedType("WorldJSONWrapper"));

        destroyCurrentWorld();

        WorldTileMap newMap = worldMap.createGridFromJSONData(newMapWrapper);
        worldMap.grid = newMap.grid;
        worldMap.width = newMap.width;
        worldMap.height = newMap.height;
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
                TileJSONWrapper tileWrapper = new TileJSONWrapper(tileComponent.currentTileType, i, j);
                tileList.list.Add(tileWrapper);
            }
        }

        jsonWrapper.width = width;
        jsonWrapper.height = height;
        jsonWrapper.tileList = tileList.list;
        return JsonUtility.ToJson(jsonWrapper);
    }

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
}

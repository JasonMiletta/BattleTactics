using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class WorldJsonUtility : MonoBehaviour {
    
    private static string gameDataProjectFilePath = "levelData/levelData";
    private static string levelDataResourceFolder = "levelData/";
    public static string completeFilePath = Application.dataPath + "/Resources/" + levelDataResourceFolder;

    #region SAVE
    //TODO: Properly handle overwriting levels?
    public static void saveMapAsJSON(WorldTileMap worldMap)
    {
        saveMapAsJSON(worldMap, null);
    }

    public static void saveMapAsJSON(WorldTileMap worldMap, string mapName)
    {
        if (worldMap != null)
        {
            WorldTileMap map = worldMap;

            string jsonData = "";

            jsonData = convertWorldTileGridArrayToJSON(map);
            mapName = mapName == null || mapName == "" ? "levelData" : mapName;  
            string filePath = completeFilePath + mapName;

            File.Create(filePath + ".json").Close();
            File.WriteAllText(filePath + ".json", jsonData);
            
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
                GameObject gridGameObject = gridArray[i, j];

                //Get Grid data
                GridTile tileComponent = gridGameObject.GetComponent<GridTile>();
                Debug.Log(tileComponent.currentTileType);
                TileJSONWrapper tileWrapper = new TileJSONWrapper(tileComponent.currentTileType, i, j);

                //Get Unit data
                Unit unit = gridGameObject.GetComponentInChildren<Unit>();
                if(unit != null){
                    Debug.Log(unit);
                    tileWrapper.tileUnitName = unit.unitName;
                }

                //Get Structure data
                Structure structure = gridGameObject.GetComponentInChildren<Structure>();
                if(structure != null){
                    Debug.Log(structure);
                    tileWrapper.tileStructureName = structure.structureName;
                }
                
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
    public static WorldJSONWrapper loadLevelData(WorldTileMap worldMap, string levelName)
    {
        return loadLevelFromJSONData(worldMap, levelName);
    }
    public static WorldJSONWrapper loadLevelData(WorldTileMap worldMap)
    {
        return loadLevelFromJSONData(worldMap, null);
    }

    private static WorldJSONWrapper loadLevelFromJSONData(WorldTileMap worldMap, string levelName)
    {

        string levelFilePath = levelDataResourceFolder;
        TextAsset targetFile;
        if (levelName == null || levelName == "")
        {
            levelFilePath = gameDataProjectFilePath.Replace(".json", "");
        } else
        {
            levelFilePath += levelName.Replace(".json", "");
        }
        Debug.Log(levelFilePath);
        targetFile = Resources.Load<TextAsset>(levelFilePath);

        return wrapMapFromJSON(targetFile.text, worldMap);
    }

    private static WorldJSONWrapper wrapMapFromJSON(string json, WorldTileMap worldMap)
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
        public string tileUnitName;
        public string tileStructureName;

        public TileJSONWrapper(GridTile.tileType type, int x, int y)
        {
            tileType = type;
            xCoor = x;
            yCoor = y;
        }
    }

    [Serializable]
    public class UnitJSONWrapper
    {
        public int xCoor;
        public int yCoor;

        public UnitJSONWrapper(int x, int y){
            xCoor = x;
            yCoor = y;
        }
    }

    [Serializable]
    public class StructureJSONWrapper
    {
        public int xCoor;
        public int yCoor;

        public StructureJSONWrapper(int x, int y){
            xCoor = x;
            yCoor = y;
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class WorldTileEditor : MonoBehaviour {
    public Cursor cursor;
    public WorldTileMap worldMap;

    private string gameDataProjectFilePath = "levelData/levelData.json";

    // Use this for initialization
    void Start() {
        cursor = FindObjectOfType<Cursor>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void setSelectedTile(string tileType)
    {

        StartCoroutine(cursor.selectedGridTile.setTilePrefab(tileType));
        cursor.cursorDeselect();
    }

    public void spawnUnitOnTile()
    {
        cursor.selectedGridTile.createTestUnitOnTile();
        cursor.cursorDeselect();
    }

    public void saveCurrentGridToJSON()
    {
        //Display text prompt for filename
        string fileName = null;
        WorldJsonUtility.saveMapAsJSON(worldMap, null);
    }

    //TODO: pull from existing levels in levelData resources and display them in the UI
    public void displayLevelSelection()
    {
        string selectedLevelName = null;
        loadLevel(selectedLevelName);
    }

    public void loadLevel()
    {
        loadLevel(null);
    }

    public void loadLevel(string levelName)
    {
        //Destroy the world!!
        destroyCurrentWorld();

        //Create the new one from WorldJsonUtility
        WorldJsonUtility.WorldJSONWrapper newMapWrapper = WorldJsonUtility.loadLevelData(worldMap);

        //Properly initialize it into the game
        worldMap.updateMapFromJsonWrapper(newMapWrapper);
    }
    
    private void destroyCurrentWorld()
    {
        foreach (GameObject obj in worldMap.grid)
        {
            Destroy(obj);
        }
    }
}

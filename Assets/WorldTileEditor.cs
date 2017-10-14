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
        WorldJsonUtility.saveCurrentGridToJSON(worldMap);
    }

    public void loadCurrentLevelJSONData()
    {
        //Destroy the world!!
        destroyCurrentWorld();

        //Create the new one from WorldJsonUtility
        WorldJsonUtility.WorldJSONWrapper newMapWrapper = WorldJsonUtility.loadCurrentLevelJSONData(worldMap);

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

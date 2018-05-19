using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class WorldTileEditor : MonoBehaviour {

    public enum action { None, Saving, Loading };

    public action currentAction = action.None;
    public Cursor cursor;
    public WorldTileMap worldMap;

    public GameObject savePrompt;
    public GameObject loadSelect;

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
        cursor.createTestUnitOnTile();
        cursor.cursorDeselect();
    }

    public void spawnBuildingOnTile(){
        cursor.createTestStructureOnTile();
        cursor.cursorDeselect();
    }

    public void promptForSaving()
    {
        currentAction = action.Saving;
        toggleSavePrompt();
    }

    public void promptForLoading()
    {
        currentAction = action.Loading;
        toggleLevelSelection();
    }

    public void saveCurrentGridToJSON(string filename)
    {
        WorldJsonUtility.saveMapAsJSON(worldMap, filename);
        currentAction = action.None;
        savePrompt.SetActive(false);
    }

    private void toggleSavePrompt()
    {
        savePrompt.SetActive(!savePrompt.activeSelf);
    }
    
    private void toggleLevelSelection()
    {
        loadSelect.SetActive(!loadSelect.activeSelf);
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
        WorldJsonUtility.WorldJSONWrapper newMapWrapper = WorldJsonUtility.loadLevelData(worldMap, levelName);

        //Properly initialize it into the game
        worldMap.updateMapFromJsonWrapper(newMapWrapper);

        loadSelect.SetActive(false);
        currentAction = action.None;
    }
    
    private void destroyCurrentWorld()
    {
        foreach (GameObject obj in worldMap.grid)
        {
            Destroy(obj);
        }
    }
}

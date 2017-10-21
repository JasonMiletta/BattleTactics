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

    public GameObject saveLoadPrompt;

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

    public void promptForSaving()
    {
        currentAction = action.Saving;
        toggleSaveLoadPrompt();
    }

    public void promptForLoading()
    {
        currentAction = action.Loading;
        toggleSaveLoadPrompt();
    }

    public void saveCurrentGridToJSON()
    {
        //Display text prompt for filename
        string fileName = null;
        WorldJsonUtility.saveMapAsJSON(worldMap, null);

        currentAction = action.None;
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

        currentAction = action.None;
    }
    
    private void destroyCurrentWorld()
    {
        foreach (GameObject obj in worldMap.grid)
        {
            Destroy(obj);
        }
    }

    private void toggleSaveLoadPrompt()
    {
        if(saveLoadPrompt != null)
        {
            bool isCurrentlyActive = saveLoadPrompt.activeSelf;
            saveLoadPrompt.SetActive(!isCurrentlyActive);

            //! DO we want to inversely toggle the rest of the UI?
            //Weird stuff will probably happen if we dont just pause the whole game or handle it
        } else
        {
            Debug.Log("WorldTileEditor - toggleSaveLoadPrompt: No saveLoadPrompt Object was found!");
        }
    }
}

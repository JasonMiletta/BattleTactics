using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class WorldTileEditor : MonoBehaviour {

    public enum action { None, Saving, Loading };

    #region COMPONENTS
    public RTS_Cam.RTS_Camera mainCamera;
    [SerializeField]
    private PrefabDataTable UTIL;
    public Cursor cursor;
    public WorldTileMap worldMap;

    public GameObject savePrompt;
    public GameObject loadSelect;
    #endregion

    #region STATE
    public action currentAction = action.None;
    public int currentTeamEditing = 1;
    #endregion
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

    public void updateSelectedTeamEditing(UnityEngine.UI.Slider slider)
    {
        currentTeamEditing = (int)slider.value;
        Text TeamSelectorText = slider.GetComponentInChildren<Text>();
        if(TeamSelectorText != null){
            TeamSelectorText.text = currentTeamEditing.ToString();
        }
    }

    public void spawnObjectOnTile(string objectName){
        GameObject unitStructurePrefab = UTIL.getPrefabByName(objectName);
        if(unitStructurePrefab != null){
            GridTile selectedTile = cursor.selectedGridTile;
            if(selectedTile != null){
                placeObjectOntile(selectedTile, unitStructurePrefab, currentTeamEditing);
            }
        }
        cursor.cursorDeselect();
    }

    public void removeObjectFromTile(){
        GridTile selectedTile = cursor.selectedGridTile;
        Unit tileUnit = selectedTile.getChildUnit();
        if(tileUnit != null){
            Destroy(tileUnit.gameObject);
        }
        Structure tileStructure = selectedTile.getChildStructure();
        if(tileStructure != null){
            Destroy(tileStructure.gameObject);
        }

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
        if(savePrompt.activeSelf){
            savePrompt.SetActive(false);
            mainCamera.enabled = true;
        } else {
            savePrompt.SetActive(true);
            mainCamera.enabled = false;
        }
    }
    
    private void toggleLevelSelection()
    {
        if(loadSelect.activeSelf){
            loadSelect.SetActive(false);
            mainCamera.enabled = true;
        } else {
            loadSelect.SetActive(true);
            mainCamera.enabled = false;
        }
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

        toggleLevelSelection();
        currentAction = action.None;
    }
    
    private void destroyCurrentWorld()
    {
        foreach (GameObject obj in worldMap.grid)
        {
            Destroy(obj);
        }
    }

    private GameObject placeObjectOntile(GridTile tile, GameObject objectPrefab, int teamNumber){
        if(objectPrefab != null){
            GameObject newUnitStructure = Instantiate(objectPrefab, tile.transform.position, tile.transform.rotation, tile.transform);
            Unit unit = newUnitStructure.GetComponent<Unit>();
            if(unit != null){
                unit.teamNumber = teamNumber;
            } else {
                Structure structure = newUnitStructure.GetComponent<Structure>();
                if(structure != null){
                    structure.teamNumber = teamNumber;
                }
            }
            return newUnitStructure;
        }
        return null;
    }
}

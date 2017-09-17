using System.Collections;
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

            jsonData = JsonUtility.ToJson(map);
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
}

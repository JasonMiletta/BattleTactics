using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileEditor : MonoBehaviour {
    public Cursor cursor;

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
}

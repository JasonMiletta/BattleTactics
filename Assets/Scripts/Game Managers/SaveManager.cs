using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour {

    public WorldTileEditor worldTileEditor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void saveLevel()
    {
        var fileName = GetComponentInChildren<InputField>().text;
        worldTileEditor.saveCurrentGridToJSON(fileName);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public GameObject levelSelectButtonPrefab;
    public WorldTileEditor worldTileEditor;

	// Use this for initialization
	void Start () {
        Transform childGrid = transform.GetChild(0);
        string[] files = Directory.GetFiles(WorldJsonUtility.completeFilePath, "*.json");
        foreach(string file in files)
        {
            if(levelSelectButtonPrefab != null)
            {
                GameObject NewButton = Instantiate(levelSelectButtonPrefab, childGrid);

                NewButton.GetComponent<Button>().onClick.AddListener(loadLevel);

                Text text = NewButton.GetComponentInChildren<Text>();
                if (text != null)
                {
                    text.text = file.Substring(file.LastIndexOf("/") + 1);
                }
            }
        }
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadLevel()
    {
        Text text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
        string buttonText = text.text;
        worldTileEditor.loadLevel(buttonText);
    }
}

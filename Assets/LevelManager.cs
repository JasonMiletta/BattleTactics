using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public GameObject levelSelectButtonPrefab;
    public WorldTileEditor worldTileEditor;
    
    private string[] files;
    private List<GameObject> levelSelectOptions = new List<GameObject>();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        Transform grid = transform.GetChild(0);
        files = Directory.GetFiles(WorldJsonUtility.completeFilePath, "*.json");
        foreach (string file in files)
        {
            if (levelSelectButtonPrefab != null)
            {
                GameObject NewButton = Instantiate(levelSelectButtonPrefab, grid);

                NewButton.GetComponent<Button>().onClick.AddListener(loadLevel);

                Text text = NewButton.GetComponentInChildren<Text>();
                if (text != null)
                {
                    text.text = file.Substring(file.LastIndexOf("/") + 1);
                }
                levelSelectOptions.Add(NewButton);
            }
        }
    }

    private void OnDisable()
    {
        foreach(GameObject selectOption in levelSelectOptions)
        {
            Destroy(selectOption);
        }
    }

    public void loadLevel()
    {
        Text text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>();
        string buttonText = text.text;
        worldTileEditor.loadLevel(buttonText);
    }
}

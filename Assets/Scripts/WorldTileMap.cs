using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileMap : MonoBehaviour {

    public GameObject[,] grid;
    public float width = 1;
    public float height = 1;

    public GameObject gridTile;
    public GameObject ground;

	// Use this for initialization
	void Start () {
        createGrid();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1))
        {
            if(width > 0 && height > 0 && grid[0,0] != null)
            {
                foreach(GameObject obj in grid)
                {
                    Destroy(obj);
                }
            } else
            {
                createGrid();
            }
        }
	}

    public void setHeight(UnityEngine.UI.Slider slider)
    {
        height = slider.value;
    }

    public void setWidth(UnityEngine.UI.Slider slider)
    {
        width = slider.value;
    }

    private void createGrid()
    {
        ground.transform.position = new Vector3(((width/2 - 0.5f)), ground.transform.position.y, (height/2 - 0.5f));
        ground.transform.localScale = new Vector3(width, 50, height);
        grid = new GameObject[(int)width, (int)height];
        for (var i = 0; i < width; ++i)
        {
            for (var j = 0; j < height; ++j)
            {
                Vector3 spawnPosition = new Vector3(i, 0, j);
                grid[i, j] = Instantiate(gridTile, spawnPosition, this.transform.rotation, this.transform);
                grid[i, j].name = "GridTile " + i + " " + j;
            }
        }
    }
}

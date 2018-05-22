using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTileMap : MonoBehaviour {

    [SerializeField]
    private PrefabDataTable UTIL;

    [SerializeField]
    public GameObject[,] grid;
    public float width = 10;
    public float height = 10;

    public GameObject gridTile;
    public string levelName;

    // Use this for initialization
    void Start () {
        if(UTIL == null){
            Debug.LogError("ERROR: Reference to UTIL PrefabDataTable has not been set!");
        }
        if (levelName != null)
        {
            //Create the new one from WorldJsonUtility
            WorldJsonUtility.WorldJSONWrapper newMapWrapper = WorldJsonUtility.loadLevelData(this, levelName);

            //Properly initialize it into the game
            updateMapFromJsonWrapper(newMapWrapper);
        }
        else
        {
            createGrid();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

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

    public void updateMapFromJsonWrapper(WorldJsonUtility.WorldJSONWrapper jsonWrapper)
    {
        //Manually walk through json, creating the grid and each tile
        this.height = jsonWrapper.height;
        this.width = jsonWrapper.width;

        this.grid = new GameObject[(int)this.width, (int)this.height];
        List<WorldJsonUtility.TileJSONWrapper> tileWrapperList = jsonWrapper.tileList;

        foreach(WorldJsonUtility.TileJSONWrapper tileWrapper in tileWrapperList)
        {
            var x = tileWrapper.xCoor;
            var y = tileWrapper.yCoor;

            GameObject newGridTile = this.createNewGridTile(tileWrapper.tileType, x, y);
            this.grid[x, y] = newGridTile;

            if(tileWrapper.tileUnitName != null && tileWrapper.tileUnitName != ""){
                placeUnitOrStructureOnTile(newGridTile, x, y, tileWrapper.tileUnitName);
            } else if(tileWrapper.tileStructureName != null && tileWrapper.tileStructureName != ""){
                placeUnitOrStructureOnTile(newGridTile, x, y, tileWrapper.tileStructureName);
            }
        }
    }

    private GameObject createNewGridTile(GridTile.tileType tileType, int xCoor, int yCoor)
    {
        Vector3 spawnPosition = new Vector3(xCoor, 0, yCoor);
        GameObject newGridTile = Instantiate(this.gridTile, spawnPosition, this.transform.rotation, this.transform);
        newGridTile.name = "GridTile " + xCoor + " " + yCoor;
        
        GridTile gridTile = newGridTile.GetComponent<GridTile>();
        gridTile.xCoor = xCoor;
        gridTile.yCoor = yCoor;
        gridTile.currentTileType = tileType;
        gridTile.enableTile();

        return newGridTile;
    }

    private GameObject placeUnitOrStructureOnTile(GameObject tile, int xCoor, int yCoor, string unitStructureName){
        Vector3 spawnPosition = new Vector3(xCoor, 0, yCoor);
        GameObject unitStructurePrefab = UTIL.getPrefabByName(unitStructureName);
        if(unitStructurePrefab != null){
            GameObject newUnitStructure = Instantiate(unitStructurePrefab, spawnPosition, tile.transform.rotation, tile.transform);
            return newUnitStructure;
        }
        return null;
    }

}

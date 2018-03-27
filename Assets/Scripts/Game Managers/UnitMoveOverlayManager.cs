using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveOverlayManager : MonoBehaviour {
    public enum Action {Move, Attack};

    public WorldTileMap tileMap;
    public GameObject[,] grid;

    private List<GridTile> enabledGridList;

    void OnEnable()
    {
        //Unit.OnUnitSelect += displayAttackMoveOverlays;
        Unit.OnUnitDeselect += disableAttackMoveOverlays;
        Unit.OnUnitMove += displayMoveOverlays;
        Unit.OnUnitAttack += displayAttackOverlays;
    }

    void OnDisable()
    {
        //Unit.OnUnitSelect -= displayAttackMoveOverlays;
        Unit.OnUnitDeselect -= disableAttackMoveOverlays;
        Unit.OnUnitMove -= displayMoveOverlays;
        Unit.OnUnitAttack -= displayAttackOverlays;
    }

    // Use this for initialization
    void Start ()
    {
        enabledGridList = new List<GridTile>();
        grid = tileMap.grid;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void displayMoveOverlays(int xCoor, int yCoor, int moveDistance){
        Debug.Log("Display Move Overlays!");
        displayOverlays(xCoor, yCoor, moveDistance, Action.Move);
    }

    public void displayAttackOverlays(int xCoor, int yCoor, int attackDistance){
        Debug.Log("Display Attack Overlays!");
        displayOverlays(xCoor, yCoor, attackDistance, Action.Attack);
    }

    
    public void displayOverlays(int xCoor, int yCoor, int distance, Action action)
    {
        GameObject tile = tileMap.grid[xCoor, yCoor];
        if (tile != null) {
            GridTile originTile = tile.GetComponent<GridTile>();

            //right
            displayAdjacentTileOverlays(xCoor + 1, yCoor, distance, action);
            //left
            displayAdjacentTileOverlays(xCoor - 1, yCoor, distance, action);
            //up
            displayAdjacentTileOverlays(xCoor, yCoor + 1, distance, action);
            //down
            displayAdjacentTileOverlays(xCoor, yCoor - 1, distance, action);
        }
    
    }

    private void displayAdjacentTileOverlays(int xCoor, int yCoor, int distance, Action action)
    {
        if (xCoor >= 0 && xCoor < tileMap.grid.GetLength(0) && yCoor >= 0 && yCoor < tileMap.grid.GetLength(1))
        {
            GameObject currentTile = tileMap.grid[xCoor, yCoor];
            if (currentTile != null)
            {
                GridTile tile = currentTile.GetComponent<GridTile>();
                if (tile.isOverlayActive())
                {
                    return;
                }
                //Display moveOverlay and then display attack overlays on adjacent tiles
                if (distance > 0)
                {
                    int remainingDistance = distance - 1 ;
                    if(action == Action.Move){
                        tile.enableMoveOverlay();
                    } else if(action == Action.Attack){
                        tile.enableAttackOverlay();
                    }
                    enabledGridList.Add(tile);
                    //right
                    displayAdjacentTileOverlays(xCoor + 1, yCoor, remainingDistance, action);
                    //left
                    displayAdjacentTileOverlays(xCoor - 1, yCoor, remainingDistance, action);
                    //up
                    displayAdjacentTileOverlays(xCoor, yCoor + 1, remainingDistance, action);
                    //down
                    displayAdjacentTileOverlays(xCoor, yCoor - 1, remainingDistance, action);
                }
            }
        }
    }

    public void disableAttackMoveOverlays(int xCoor, int yCoor, int moveDistance)
    {
        foreach(GridTile tile in enabledGridList)
        {
            tile.disableMoveOverlay();
            tile.disableAttackOverlay();
        }
        enabledGridList = new List<GridTile>();
    }
}

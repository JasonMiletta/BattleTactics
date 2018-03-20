using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveOverlayManager : MonoBehaviour {

    public WorldTileMap tileMap;
    public GameObject[,] grid;

    private List<GridTile> enabledGridList;

    void OnEnable()
    {
        Unit.OnUnitSelect += displayAttackMoveOverlays;
        Unit.OnUnitDeselect += disableAttackMoveOverlays;
    }

    void OnDisable()
    {
        Unit.OnUnitSelect -= displayAttackMoveOverlays;
        Unit.OnUnitDeselect -= disableAttackMoveOverlays;
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

    public void displayAttackMoveOverlays(int xCoor, int yCoor, int moveDistance)
    {
        GameObject tile = tileMap.grid[xCoor, yCoor];
        if (tile != null) {
            GridTile originTile = tile.GetComponent<GridTile>();

            //right
            displayAdjacentTileOverlays(xCoor + 1, yCoor, moveDistance);
            //left
            displayAdjacentTileOverlays(xCoor - 1, yCoor, moveDistance);
            //up
            displayAdjacentTileOverlays(xCoor, yCoor + 1, moveDistance);
            //down
            displayAdjacentTileOverlays(xCoor, yCoor - 1, moveDistance);
        }
    
    }

    private void displayAdjacentTileOverlays(int xCoor, int yCoor, int moveDistance)
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
                if (moveDistance > 0)
                {
                    int remainingMoveDistance = moveDistance - 1 ;
                    tile.enableMoveOverlay();
                    enabledGridList.Add(tile);
                    //right
                    displayAdjacentTileOverlays(xCoor + 1, yCoor, remainingMoveDistance);
                    //left
                    displayAdjacentTileOverlays(xCoor - 1, yCoor, remainingMoveDistance);
                    //up
                    displayAdjacentTileOverlays(xCoor, yCoor + 1, remainingMoveDistance);
                    //down
                    displayAdjacentTileOverlays(xCoor, yCoor - 1, remainingMoveDistance);
                }
                //Display attack overlays
                else
                {
                    tile.enableAttackOverlay();
                    enabledGridList.Add(tile);
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

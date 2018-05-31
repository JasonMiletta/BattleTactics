using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveOverlayManager : MonoBehaviour {
    public enum Action {Move, Attack};
    public enum PreviousActionDirection {Up, Down, Left, Right};

    #region COMPONENTS
    public WorldTileMap tileMap;
    public GameObject[,] grid;
    #endregion

    #region STATE
    private List<GridTile> enabledGridList;
    private GridTile currentOriginTile;
    #endregion

    void OnEnable()
    {
        Unit.OnUnitDeselect += disableAttackMoveOverlays;
        Unit.OnUnitMoving += displayMoveOverlays;
        Unit.OnUnitAttacking += displayAttackOverlays;
    }

    void OnDisable()
    {
        Unit.OnUnitDeselect -= disableAttackMoveOverlays;
        Unit.OnUnitMoving -= displayMoveOverlays;
        Unit.OnUnitAttacking -= displayAttackOverlays;
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

    public void displayMoveOverlays(int xCoor, int yCoor, int moveDistance, Unit.UnitActionLayoutType movementLayoutType){
        Debug.Log("Display Move Overlays!");
        displayOverlays(xCoor, yCoor, moveDistance, Action.Move, movementLayoutType);
    }

    public void displayAttackOverlays(int xCoor, int yCoor, int attackDistance, Unit.UnitActionLayoutType attackLayoutType){
        Debug.Log("Display Attack Overlays!");
        displayOverlays(xCoor, yCoor, attackDistance, Action.Attack, attackLayoutType);
    }

    
    public void displayOverlays(int xCoor, int yCoor, int distance, Action action, Unit.UnitActionLayoutType actionLayoutType)
    {
        GameObject tile = tileMap.grid[xCoor, yCoor];
        if (tile != null) {
            currentOriginTile = tile.GetComponent<GridTile>();

            //right
            displayAdjacentTileOverlays(xCoor + 1, yCoor, distance, action, actionLayoutType, PreviousActionDirection.Right);
            //left
            displayAdjacentTileOverlays(xCoor - 1, yCoor, distance, action, actionLayoutType, PreviousActionDirection.Left);
            //up
            displayAdjacentTileOverlays(xCoor, yCoor + 1, distance, action, actionLayoutType, PreviousActionDirection.Up);
            //down
            displayAdjacentTileOverlays(xCoor, yCoor - 1, distance, action, actionLayoutType, PreviousActionDirection.Down);
        }
    
    }

    private void displayAdjacentTileOverlays(int xCoor, int yCoor, int distance, Action action, Unit.UnitActionLayoutType actionLayoutType, PreviousActionDirection prevActionDirection)
    {
        if (xCoor >= 0 && xCoor < tileMap.grid.GetLength(0) && yCoor >= 0 && yCoor < tileMap.grid.GetLength(1))
        {
            GameObject currentTile = tileMap.grid[xCoor, yCoor];
            if (currentTile != null)
            {
                GridTile tile = currentTile.GetComponent<GridTile>();
                if(currentOriginTile != tile){
                    //Display moveOverlay and then display attack overlays on adjacent tiles
                    if (distance > 0)
                    {
                        int remainingDistance = distance - 1 ;
                        if(action == Action.Move){
                            Debug.Log("x,y : " + xCoor + ", " + yCoor);
                            tile.enableMoveOverlay();
                        } else if(action == Action.Attack){
                            Debug.Log("x,y : " + xCoor + ", " + yCoor);
                            tile.enableAttackOverlay();
                        }
                        enabledGridList.Add(tile);
                        if(actionLayoutType == Unit.UnitActionLayoutType.Any){
                            //right
                            displayAdjacentTileOverlays(xCoor + 1, yCoor, remainingDistance, action, actionLayoutType, PreviousActionDirection.Right);
                            //left
                            displayAdjacentTileOverlays(xCoor - 1, yCoor, remainingDistance, action, actionLayoutType, PreviousActionDirection.Left);
                            //up
                            displayAdjacentTileOverlays(xCoor, yCoor + 1, remainingDistance, action, actionLayoutType, PreviousActionDirection.Up);
                            //down
                            displayAdjacentTileOverlays(xCoor, yCoor - 1, remainingDistance, action, actionLayoutType, PreviousActionDirection.Down);
                        } else if(actionLayoutType == Unit.UnitActionLayoutType.Line){
                            //right
                            if(prevActionDirection == PreviousActionDirection.Right){
                                displayAdjacentTileOverlays(xCoor + 1, yCoor, remainingDistance, action, actionLayoutType, PreviousActionDirection.Right);
                            }
                            //left
                            if(prevActionDirection == PreviousActionDirection.Left){
                                displayAdjacentTileOverlays(xCoor - 1, yCoor, remainingDistance, action, actionLayoutType, PreviousActionDirection.Left);
                            }
                            //up
                            if(prevActionDirection == PreviousActionDirection.Up){
                                displayAdjacentTileOverlays(xCoor, yCoor + 1, remainingDistance, action, actionLayoutType, PreviousActionDirection.Up);
                            }
                            //down
                            if(prevActionDirection == PreviousActionDirection.Down){
                                displayAdjacentTileOverlays(xCoor, yCoor - 1, remainingDistance, action, actionLayoutType, PreviousActionDirection.Down);
                            }
                        }
                    }
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

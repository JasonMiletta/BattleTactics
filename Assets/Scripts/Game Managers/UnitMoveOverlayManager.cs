using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveOverlayManager : MonoBehaviour {
    public enum Action {Move, Attack, AttackRange};
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
        Unit.OnUnitRangeInfo += displayAttackRangeOverlays;
    }

    void OnDisable()
    {
        Unit.OnUnitDeselect -= disableAttackMoveOverlays;
        Unit.OnUnitMoving -= displayMoveOverlays;
        Unit.OnUnitAttacking -= displayAttackOverlays;
        Unit.OnUnitRangeInfo -= displayAttackRangeOverlays;
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

    public void displayMoveOverlays(int xCoor, int yCoor, Unit unit){
        Debug.Log("Display Move Overlays!");
        displayOverlays(xCoor, yCoor, 0, unit.moveDistance, Action.Move, unit.movementLayoutType);
    }

    public void displayAttackOverlays(int xCoor, int yCoor, Unit unit){
        Debug.Log("Display Attack Overlays!");
        displayOverlays(xCoor, yCoor, unit.minAttackRange, unit.maxAttackRange, Action.Attack, unit.attackLayoutType);
    }

    public void displayAttackRangeOverlays(int xCoor, int yCoor, Unit unit){
        Debug.Log("Display Attack Range Overlays!");
        displayOverlays(xCoor, yCoor, unit.minAttackRange, unit.maxAttackRange + unit.moveDistance, Action.AttackRange, unit.attackLayoutType);
    }

    //Recursive Base case 
    private void displayOverlays(int xCoor, int yCoor, int minDistance, int maxDistance, Action action, Unit.UnitActionLayoutType actionLayoutType)
    {
        //right
        displayAdjacentTileOverlays(xCoor + 1, yCoor, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Right);
        //left
        displayAdjacentTileOverlays(xCoor - 1, yCoor, minDistance,  maxDistance, action, actionLayoutType, PreviousActionDirection.Left);
        //up
        displayAdjacentTileOverlays(xCoor, yCoor + 1, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Up);
        //down
        displayAdjacentTileOverlays(xCoor, yCoor - 1, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Down);
    }

    //Recursive Loop
    private void displayAdjacentTileOverlays(int xCoor, int yCoor, int minDistance, int maxDistance, Action action, Unit.UnitActionLayoutType actionLayoutType, PreviousActionDirection prevActionDirection)
    {
        //Check that we havent reached the edge of the tilegrid and that there's distance remaining
        if (xCoor >= 0 && xCoor < tileMap.grid.GetLength(0) && yCoor >= 0 && yCoor < tileMap.grid.GetLength(1) && maxDistance > 0)
        {
            //Check that we somehow dont have a missing tile in the tileMap
            GameObject currentTile = tileMap.grid[xCoor, yCoor];
            if (currentTile != null)
            {
                GridTile tile = currentTile.GetComponent<GridTile>();
                if(currentOriginTile != tile){

                    //If we havent reached the minimum distance, decrement and continue on. Else, enable tiles overlay based on action
                    if(minDistance > 0){
                        --minDistance;
                        --maxDistance;
                    } else {
                        --maxDistance;
                        if(action == Action.Move){
                            tile.enableMoveOverlay();
                        } else if(action == Action.Attack){
                            tile.enableAttackOverlay();
                        } else if(action == Action.AttackRange){
                            tile.enableAttackRangeOverlay();
                        }
                        enabledGridList.Add(tile);
                    }

                    //Recurse based unitAction layout type (Currently Any or Line)
                    if(actionLayoutType == Unit.UnitActionLayoutType.Any){
                        //right
                        if(prevActionDirection != PreviousActionDirection.Left){
                            displayAdjacentTileOverlays(xCoor + 1, yCoor, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Right);
                        }
                        //left
                        if(prevActionDirection != PreviousActionDirection.Right){
                            displayAdjacentTileOverlays(xCoor - 1, yCoor, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Left);
                        }
                        //up
                        if(prevActionDirection != PreviousActionDirection.Down){
                            displayAdjacentTileOverlays(xCoor, yCoor + 1, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Up);
                        }
                        //down
                        if(prevActionDirection != PreviousActionDirection.Up){
                            displayAdjacentTileOverlays(xCoor, yCoor - 1, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Down);
                        }
                    } else if(actionLayoutType == Unit.UnitActionLayoutType.Line){
                        //right
                        if(prevActionDirection == PreviousActionDirection.Right){
                            displayAdjacentTileOverlays(xCoor + 1, yCoor, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Right);
                        }
                        //left
                        if(prevActionDirection == PreviousActionDirection.Left){
                            displayAdjacentTileOverlays(xCoor - 1, yCoor, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Left);
                        }
                        //up
                        if(prevActionDirection == PreviousActionDirection.Up){
                            displayAdjacentTileOverlays(xCoor, yCoor + 1, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Up);
                        }
                        //down
                        if(prevActionDirection == PreviousActionDirection.Down){
                            displayAdjacentTileOverlays(xCoor, yCoor - 1, minDistance, maxDistance, action, actionLayoutType, PreviousActionDirection.Down);
                        }
                    }
                }
            }
        }
    }

    public void disableAttackMoveOverlays(int xCoor, int yCoor, Unit unit)
    {
        foreach(GridTile tile in enabledGridList)
        {
            tile.disableMoveOverlay();
            tile.disableAttackOverlay();
        }
        enabledGridList = new List<GridTile>();
    }
}

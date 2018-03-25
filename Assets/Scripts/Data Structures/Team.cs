using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team {
    public int teamNumber = 1;

    public List<Unit> teamUnitList = new List<Unit>();

    #region EVENTS
    public delegate void UnitAddedToList(Unit unit);
    public static event UnitAddedToList OnUnitAdded;

    public delegate void UnitRemovedFromList(Unit unit);
    public static event UnitRemovedFromList OnUnitRemoved;
    #endregion

    public Team(int teamNumber){
        InitializeTeam(teamNumber, new List<Unit>());
    }
    public Team(int teamNumber, List<Unit> unitList){
        InitializeTeam(teamNumber, unitList);
    }

    private void InitializeTeam(int teamNumber, List<Unit> unitList){
        this.teamNumber = teamNumber;
        this.teamUnitList = unitList;
    }

    public void addUnit(Unit u){
        teamUnitList.Add(u);
        OnUnitAdded(u);
    }

    public void removeUnit(Unit u){
        teamUnitList.Remove(u);
        OnUnitRemoved(u);
    }
}
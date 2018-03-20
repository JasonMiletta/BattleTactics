using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team {
    public int teamNumber = 0;

    public List<Unit> teamUnitList = new List<Unit>();

    public Team(int teamNumber, List<Unit> unitList){
        this.teamNumber = teamNumber;
        this.teamUnitList = unitList;
    }

    public void addUnit(Unit u){
        teamUnitList.Add(u);
    }

    public void removeUnit(Unit u){
        teamUnitList.Remove(u);
    }
}
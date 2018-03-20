using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour {

    #region GAME_STATE
    private int turnNumber = 1;
    private int teamCount = 1;
    private int currentTurnTeamNumber = 0;
    Dictionary<int, Team> teamDictionary = new Dictionary<int, Team>();
    #endregion
    void Start(){
        //TODO: Initialize mainmenu
        startLevel();
    }

    void Update(){
    }

    public void loadEditor(){
        //TODO load editor scene
    }

    public void startLevel(){
        initializeTeams();
    }

    public int getCurrentTurnNumber(){
        return turnNumber;
    }
    public int getCurrentTeamNumberTurn(){
        return currentTurnTeamNumber;
    }
    public void nextTeamsTurn(){
        ++currentTurnTeamNumber;
        if(currentTurnTeamNumber >= teamCount){
            currentTurnTeamNumber = 0;
            ++turnNumber;
        }
    }
    public void initializeTeams(){
        foreach(Unit unit in GameObject.FindObjectsOfType<Unit>()){
            int teamNumber = unit.teamNumber;
            Team team;
            if(teamDictionary.ContainsKey(teamNumber)){
                team = teamDictionary[unit.teamNumber];
                team.addUnit(unit);
            } else {
                team = new Team(teamNumber, new List<Unit> {unit});
                teamDictionary.Add(teamNumber, team);
            }
        }
    }
}
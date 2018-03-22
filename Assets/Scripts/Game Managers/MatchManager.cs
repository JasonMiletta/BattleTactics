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

    void OnEnable()
    {
        Unit.OnUnitCreate += handleUnitCreated;
        Unit.OnUnitDestroy += handleUnitDeleted;
    }

    void OnDisable()
    {
        Unit.OnUnitCreate -= handleUnitCreated;
        Unit.OnUnitDestroy -= handleUnitDeleted;
    }

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

    public Team getCurrentTeam(int teamNumber){
        Team currentTeamList;
        teamDictionary.TryGetValue(teamNumber, out currentTeamList);

        return currentTeamList;
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

    public void addUnitToTeam(Unit unit){
        int teamNumber = unit.teamNumber;
        if(teamNumber != null){
            Team team;
            teamDictionary.TryGetValue(teamNumber, out team);
            if(team != null){
                team.addUnit(unit);
            } else {
                team = addNewTeam(teamNumber);
                team.addUnit(unit);
            }
        }
    }

    public void removeUnitFromTeam(Unit unit){
        int teamNumber = unit.teamNumber;
        if(teamNumber != null){
            Team team;
            teamDictionary.TryGetValue(teamNumber, out team);
            if(team != null){
                team.removeUnit(unit);
            } else {
                Debug.LogWarning("No Team was found for the destroyed unit!" + teamNumber);
            }
        }
    }

    private void handleUnitCreated(Unit unit){
        Debug.Log("A new unit was created!");
        Debug.Log(unit.name);
        addUnitToTeam(unit);
    }

    private void handleUnitDeleted(Unit unit){
        Debug.Log("A unit was destroyed!");
        Debug.Log(unit.name);
        removeUnitFromTeam(unit);
    }

    //NOTE: Should we be creating a new team on the fly or should this be properly be created at the start of a match? Could the amount of teams change during a match?
    private Team addNewTeam(int teamNumber){
        Team team = new Team(teamNumber);
        teamDictionary.Add(teamNumber, team);
        return team;
    }
}
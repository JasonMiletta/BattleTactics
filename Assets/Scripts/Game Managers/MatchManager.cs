using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour {

    public GameObject HUD;

    #region GAME_STATE
    public int turnNumber = 1;
    public int teamCount = 2;
    private int teamNumberCurrentlyPlaying = 1;
    Dictionary<int, Team> teamDictionary = new Dictionary<int, Team>();
    #endregion

    #region COMPONENTS
    private UI_HUDManager HUDManager;
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
        if(HUD == null){
            HUD = GameObject.FindGameObjectWithTag("HUD");
        }
        if(HUD != null){
            HUDManager = HUD.GetComponent<UI_HUDManager>();
        }
        startLevel();
    }

    void Update(){
    }

    public void loadEditor(){
        //TODO load editor scene
    }

    public void startLevel(){
        initializeTeams();
        prepareUnitsByTeam(1);
    }

    public int getCurrentTurnNumber(){
        return turnNumber;
    }
    public int getTeamNumberCurrentlyPlaying(){
        return teamNumberCurrentlyPlaying;
    }

    public Team getTeamByNumber(int teamNumber){
        Team currentTeamList;
        teamDictionary.TryGetValue(teamNumber, out currentTeamList);

        return currentTeamList;
    }
    public void proceedToNextTeamsTurn(){
        cleanupUnitsByTeam(teamNumberCurrentlyPlaying);
        ++teamNumberCurrentlyPlaying;
        if(teamNumberCurrentlyPlaying > teamCount){
            teamNumberCurrentlyPlaying = 1;
            ++turnNumber;
        }
        prepareUnitsByTeam(teamNumberCurrentlyPlaying);
        Team team = getTeamByNumber(teamNumberCurrentlyPlaying);
        HUDManager.updateHud(turnNumber, team);

    }

    public void initializeTeams(){
        teamDictionary = new Dictionary<int, Team>();
        for(int i = 0; i <= teamCount; ++i){
            teamDictionary.Add(i, new Team(i));
        }
        foreach(Unit unit in GameObject.FindObjectsOfType<Unit>()){
            int teamNumber = unit.teamNumber;
            Team team;
            if(teamDictionary.ContainsKey(teamNumber)){
                team = teamDictionary[teamNumber];
                team.addUnit(unit);
            } else {
                team = new Team(teamNumber, new List<Unit> {unit});
                teamDictionary.Add(teamNumber, team);
            }
        }
    }

    public void addUnitToTeam(Unit unit){
        int teamNumber = unit.teamNumber;
        Team team;
        teamDictionary.TryGetValue(teamNumber, out team);
        if(team != null){
            team.addUnit(unit);
        } else {
            team = addNewTeam(teamNumber);
            team.addUnit(unit);
        }
    }

    public void removeUnitFromTeam(Unit unit){
        int teamNumber = unit.teamNumber;
        Team team;
        teamDictionary.TryGetValue(teamNumber, out team);
        if(team != null){
            team.removeUnit(unit);
        } else {
            Debug.LogWarning("No Team was found for the destroyed unit!" + teamNumber);
        }
    }

    private void handleUnitCreated(Unit unit){
        Debug.Log(unit.name + " was created!");
        addUnitToTeam(unit);
    }

    private void handleUnitDeleted(Unit unit){
        Debug.Log(unit.name + " was destroyed!");
        removeUnitFromTeam(unit);
    }

    //NOTE: Should we be creating a new team on the fly or should this be properly be created at the start of a match? Could the amount of teams change during a match?
    private Team addNewTeam(int teamNumber){
        Team team = new Team(teamNumber);
        teamDictionary.Add(teamNumber, team);
        return team;
    }

    private void prepareUnitsByTeam(int teamNumber){
        Team team = getTeamByNumber(teamNumber);
        if(team != null){
            foreach(Unit u in team.teamUnitList){
                u.prepareUnitForTurn();
            }
        } else {
            Debug.LogError("We couldn't find the team for the indicated number: " + teamNumber);
        }
    }

    private void cleanupUnitsByTeam(int teamNumber){
        Team team = getTeamByNumber(teamNumber);
        if(team != null){
            foreach(Unit u in team.teamUnitList){
                u.cleanpUnitForTurn();
            }
        } else {
            Debug.LogError("We couldn't find the team for the indicated number: " + teamNumber);
        }
    }
}
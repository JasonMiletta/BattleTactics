using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUDManager : MonoBehaviour {

	#region HUD_COMPONENTS
	public GameObject Text_TurnCounter;
	public GameObject Text_CurrentTeamPlaying;
	public UI_TeamList TeamList;
	public UI_UnitActionPanel UnitActionPanel;
	#endregion

	void OnEnable(){
		Unit.OnUnitSelect += displayUnitActionPanel;
		Unit.OnUnitDeselect += hideUnitActionPanel;
	}

	void OnDisable(){
		Unit.OnUnitSelect -= displayUnitActionPanel;
		Unit.OnUnitDeselect -= hideUnitActionPanel;
	}

	// Use this for initialization
	void Start () {
		if(Text_TurnCounter == null){
			Debug.LogWarning("Turn Counter text object is missing!");
		}
		if(Text_CurrentTeamPlaying == null){
			Debug.LogWarning("CurrentTeamPlaying text object is missing!");
		}
		if(TeamList == null){
			Debug.LogWarning("TeamList object is missing!");
		}
		if(UnitActionPanel == null){
			Debug.LogWarning("Unit Action panel object is missing!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateHud(int turnCount, Team team){
		int currentTeamPlaying = team.teamNumber;

		Text turnCounterText = Text_TurnCounter.GetComponent<Text>();
		turnCounterText.text = "Turn: " + turnCount;

		Text currentTeamPlayingText = Text_CurrentTeamPlaying.GetComponent<Text>();
		currentTeamPlayingText.text = "Player " + currentTeamPlaying + "'s Turn";

		TeamList.resetTeamListForTeam(team);
	}

	private void displayUnitActionPanel(int xCoor, int yCoor, Unit unit){
		if(UnitActionPanel != null){
			UnitActionPanel.gameObject.SetActive(true);
		}
	}

	private void hideUnitActionPanel(int xCoor, int yCoor, Unit unit){
		if(UnitActionPanel != null){
			UnitActionPanel.gameObject.SetActive(false);
		}
	}
}

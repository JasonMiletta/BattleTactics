using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUDManager : MonoBehaviour {

	#region HUD_COMPONENTS
	public GameObject Text_TurnCounter;
	public GameObject Text_CurrentTeamPlaying;
	public UI_TeamList TeamList;
	#endregion
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
}

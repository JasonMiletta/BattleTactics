using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	#region COMPONENTS
	public GameObject Menu;
	#endregion

	#region EVENTS
	public delegate void PauseMenuEvent();
    public static event PauseMenuEvent OnPauseMenuDisplay;
    public static event PauseMenuEvent OnPauseMenuHide;
	#endregion
	// Use this for initialization
	void Start () {
		if(Menu == null){
			Debug.LogError("No Error is attached to the Menu Manager!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			toggleMenu();
		}
	}

	
	public void continueGame(){
		hideMenu();
	}
	public void settings(){

	}
	public void quitToMenu(){

	}
	public void quitToDesktop(){
		Application.Quit();
	}

	private void toggleMenu(){
		if(!Menu.activeSelf){
			showMenu();
		} else {
			hideMenu();
		}
	}
	private void showMenu(){
		Menu.SetActive(true);
		if(OnPauseMenuDisplay != null){
			OnPauseMenuDisplay();
		}
	}

	private void hideMenu(){
		Menu.SetActive(false);
		if(OnPauseMenuHide != null){
			OnPauseMenuHide();
		}
	}
}

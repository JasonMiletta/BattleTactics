using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	#region COMPONENTS
	public UI_Form Menu;
	#endregion

	#region EVENTS
	public delegate void PauseMenuEvent();
    public static event PauseMenuEvent OnPauseMenuDisplay;
    public static event PauseMenuEvent OnPauseMenuHide;
	#endregion

	#region STATE
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
		SceneManager.LoadScene("MainMenu");
	}
	public void quitToDesktop(){
		Application.Quit();
	}

	private void toggleMenu(){
		if(!Menu.gameObject.activeSelf){
			showMenu();
		} else {
			hideMenu();
		}
	}
	private void showMenu(){
		Menu.activateForm();
		if(OnPauseMenuDisplay != null){
			OnPauseMenuDisplay();
		}
	}

	private void hideMenu(){
		Menu.deactivateForm();
		if(OnPauseMenuHide != null){
			OnPauseMenuHide();
		}
	}
}

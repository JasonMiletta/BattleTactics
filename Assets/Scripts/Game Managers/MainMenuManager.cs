using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
#region COMPONENTS
	public GameObject Menu;
	#endregion

	#region EVENTS
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
	}

	public void startGame(){
		SceneManager.LoadScene("TestLevel1");
	}

	public void startEditor(){
		SceneManager.LoadScene("TestEditorScene");

	}

	public void openSettings(){

	}

	public void quitToDesktop(){
		Application.Quit();
	}
}

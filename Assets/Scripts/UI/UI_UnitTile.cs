using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTile : MonoBehaviour {

    public Unit unit;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void setUnit(Unit unit){
		this.unit = unit;
		Text[] textComponents = GetComponentsInChildren<Text>();
		foreach(Text textObject in textComponents){
			if(textObject.name == "Name"){
				textObject.text = "Name: " + unit.unitName;
			} else if(textObject.name == "Health"){
				textObject.text = "Health: " + unit.currentHealth;
			}
		}

	}
}

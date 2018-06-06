using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitActionPanel : MonoBehaviour {

	#region COMPONENTS
	public Cursor cursor;
	#endregion

	// Use this for initialization
	void Start () {
		if(cursor == null){
			Debug.LogError("UI_UnitActionPanel: cursor component is not set!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void unitMove(){
		Unit currentlySelectedUnit = cursor.getCurrentlySelectedUnit();
		if(currentlySelectedUnit != null){
			cursor.beginMovingUnit();
			EntityActionController.beginMovingAction(currentlySelectedUnit);
		}
	}

	public void unitAttack(){
		Unit currentlySelectedUnit = cursor.getCurrentlySelectedUnit();
		if(currentlySelectedUnit != null){
			cursor.beginAttackingUnit();
			EntityActionController.beginAttackAction(currentlySelectedUnit);
		}
	}

	public void unitAttackRange(){
		Unit currentlySelectedUnit = cursor.getCurrentlySelectedUnit();
		if(currentlySelectedUnit != null){
			EntityActionController.beginAttackRangeInfoAction(currentlySelectedUnit);
		}
	}

	public void cancel(){
		cursor.cancel();
	}
}

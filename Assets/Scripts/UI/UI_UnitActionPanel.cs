using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnitActionPanel : MonoBehaviour {

	#region COMPONENTS
	public Cursor cursor;
	private Anim_OpenCloseForm anim_OpenCloseForm;
	#endregion

	// Use this for initialization
	void Start () {
		if(cursor == null){
			Debug.LogError("UI_UnitActionPanel: cursor component is not set!");
		}
		
		anim_OpenCloseForm = GetComponent<Anim_OpenCloseForm>();
		if(anim_OpenCloseForm == null){
			Debug.LogError("UI_UnitActionPanel: Animator is missing from this object!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void openPanel(){
		if(anim_OpenCloseForm == null){
			anim_OpenCloseForm = GetComponent<Anim_OpenCloseForm>();
		}
		if(anim_OpenCloseForm != null){
			anim_OpenCloseForm.OpenPanel();
		}
	}

	public void closePanel(){
		if(anim_OpenCloseForm != null){
			anim_OpenCloseForm.ClosePanel();
		}
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

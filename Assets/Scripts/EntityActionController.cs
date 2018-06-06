using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityActionController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void beginMovingAction(Unit unit){
		unit.startMoving();
	}

	public static void beginAttackAction(Unit unit){
		unit.startAttacking();
	}

	public static void beginAttackRangeInfoAction(Unit unit){
		//unit.
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {

	public enum StructureType {City, Capitol, Barracks, Airstrip, Dock};
	
	#region COMPONENTS
	public GameObject structureModel;
	#endregion

	#region STATS
	public string structureName;
	public StructureType type = StructureType.City;
	public int totalHealth = 5;
	#endregion

	#region STATE
	public int currentHealth = 5;
	public int teamNumber = 1;
	public bool isDestroyed = false;
	public bool isUnowned = true;
	#endregion

	#region EVENTS
	public delegate void StructureEvent();
	public static event StructureEvent OnStructureCreated;
	public static event StructureEvent OnStructureCaptured;
	public static event StructureEvent OnStructureDestroyed;
	#endregion
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

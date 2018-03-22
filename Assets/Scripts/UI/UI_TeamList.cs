using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TeamList : MonoBehaviour {

	public GameObject UnitTilePrefab;
	public Dictionary<Unit, GameObject> unitTileDictionary = new Dictionary<Unit, GameObject>();
	private List<UI_UnitTile> unitTileList = new List<UI_UnitTile>();
	
	 void OnEnable()
    {
        Team.OnUnitAdded += handleUnitAdded;
        Team.OnUnitRemoved += handleUnitRemoved;
    }

	void OnDisable(){
        Team.OnUnitAdded -= handleUnitAdded;
        Team.OnUnitRemoved -= handleUnitRemoved;
	}

	// Use this for initialization
	void Start () {
		if(UnitTilePrefab == null){
			Debug.LogError("UI_TeamList does not have an assigned UnitTilePrefab!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void addNewUnitTile(Unit unit){
		GameObject newTile = Instantiate(UnitTilePrefab, this.transform);
		UI_UnitTile newUnitTile = newTile.GetComponent<UI_UnitTile>();
		newUnitTile.setUnit(unit);

		unitTileDictionary.Add(unit, newTile);
	}

	public void removeUnitTile(Unit unit){
		GameObject unitTileToRemove;
		unitTileDictionary.TryGetValue(unit, out unitTileToRemove);
		Destroy(unitTileToRemove);
	}

	public void handleUnitAdded(Unit unit){
		Debug.Log("A unit has been added to the team!");
		addNewUnitTile(unit);
	}

	public void handleUnitRemoved(Unit unit){
		Debug.Log("A unit has been removed from the team");
		removeUnitTile(unit);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour {

    public GameObject[] tilePrefabs;
    public Unit testUnitCreate;

    private GameObject tile;

	// Use this for initialization
	void Start () {
        var randomIndex = Random.Range(0, tilePrefabs.Length);
        tile = Instantiate(tilePrefabs[randomIndex], this.transform.position, this.transform.rotation, this.transform);
        tile.transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        tile.transform.localScale = Vector3.Lerp(tile.transform.localScale, new Vector3(0.1f, 1, 0.1f), 0.25f);
    }

    public void selectTile()
    {
        Unit unit = GetComponentInChildren<Unit>();
        if(unit != null)
        {
            unit.selectUnit();
        } else
        {
            //DEBUG
            Unit newUnit = Instantiate<Unit>(testUnitCreate, transform.position, transform.rotation, transform);
        }
    }
}

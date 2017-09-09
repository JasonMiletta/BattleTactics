using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public GameObject unitModel;
    public Material selectedMaterial;

    private Material originalMaterial;

	// Use this for initialization
	void Start () {
        originalMaterial = unitModel.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void moveUnitToGridTile(GridTile tile)
    {
        transform.parent = tile.transform;
        transform.position = tile.transform.position;
        deselectUnit();
    }

    public void selectUnit()
    {
        unitModel.GetComponent<Renderer>().material = selectedMaterial;
    }

    public void deselectUnit()
    {
        unitModel.GetComponent<Renderer>().material = originalMaterial;
    }
}

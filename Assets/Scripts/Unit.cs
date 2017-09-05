using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public GameObject unitModel;
    public Material selectedMaterial;

    private Material originalMaterial;

	// Use this for initialization
	void Start () {
        originalMaterial = unitModel.GetComponent<Material>();
	}
	
	// Update is called once per frame
	void Update () {
		
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

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
        StartCoroutine(smoothMovementCoRoutine(transform.position, tile.transform.position, 0.05f));
        deselectUnit();
    }

    private IEnumerator smoothMovementCoRoutine(Vector3 source, Vector3 destination, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            transform.position = Vector3.Lerp(source, destination, (Time.time - startTime) / duration);
            yield return null;
        }
        transform.position = destination;
        yield return null;
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

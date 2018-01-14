using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public GameObject unitModel;
    public int moveDistance = 1;
    public int minAttackRange = 1;
    public int maxAttackRange = 1;

    public Material selectedMaterial;

    private Material originalMaterial;


    public delegate void UnitEvent(int xCoor, int yCoor, int moveDistance);
    public static event UnitEvent OnUnitSelect;
    public static event UnitEvent OnUnitDeselect;

    // Use this for initialization
    void Start () {
        //originalMaterial = unitModel.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void moveUnitToGridTile(GridTile tile)
    {
        float angle = Vector3.Angle(transform.forward, tile.transform.position - transform.position);
        Vector3 cross = Vector3.Cross(transform.forward, tile.transform.position - transform.position);
        if(cross.y < 0)
        {
            angle = -angle;
        }
        transform.Rotate(Vector3.up, angle);
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
        //unitModel.GetComponent<Renderer>().material = Resources.Load("Selected") as Material;
        GridTile tile = GetComponentInParent<GridTile>();
        if(tile != null && OnUnitSelect != null)
        {
            OnUnitSelect(tile.xCoor, tile.yCoor, moveDistance);
        }
    }

    public void deselectUnit()
    {
        //unitModel.GetComponent<Renderer>().material = originalMaterial;
        GridTile tile = GetComponentInParent<GridTile>();
        if (tile != null && OnUnitDeselect != null)
        {
            OnUnitDeselect(tile.xCoor, tile.yCoor, moveDistance);
        }
    }
}

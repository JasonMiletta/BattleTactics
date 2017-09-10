using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour {

    public GameObject[] tilePrefabs;
    public Unit testUnitCreate;
    public GameObject ground;

    private bool isTileBlank = true;
    [SerializeField]
    private GameObject tile;
    private Material originalTileMaterial;

	// Use this for initialization
	void Start ()
    {
        originalTileMaterial = tile.GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
    }

    public GridTile selectTile()
    {
        //tile.GetComponent<Renderer>().material = Resources.Load("Selected") as Material;
        Unit unit = GetComponentInChildren<Unit>();
        if(unit != null)
        {
            unit.selectUnit();
        }
        return this;
    }

    public void deSelectTile()
    {
        tile.GetComponent<Renderer>().material = originalTileMaterial;
        Unit unit = GetComponentInChildren<Unit>();
        if (unit != null)
        {
            unit.deselectUnit();
        }
    }

    public void enableTile()
    {
        var randomIndex = Random.Range(0, tilePrefabs.Length);
        Destroy(tile);
        tile = Instantiate(tilePrefabs[randomIndex], this.transform.position, this.transform.rotation, this.transform);
        //originalTileMaterial = tile.GetComponent<Renderer>().material;
        isTileBlank = false;
        tile.transform.localScale = Vector3.zero;
        StartCoroutine(expandObjToScale(tile, new Vector3(0.1f, 1, 0.1f), 0.25f));

        ground.transform.localScale = Vector3.zero;
        ground.SetActive(true);
        StartCoroutine(expandObjToScale(ground, new Vector3(1.0f, 50.0f, 1.0f), 0.25f));
    }

    public void clearTile()
    {
        if (ground != null)
        {
            ground.SetActive(false);
        }
        if (tile != null)
        {
            tile.SetActive(false);
        }
    }

    //DEBUG
    public void createTestUnitOnTile()
    {
        //DEBUG
        Unit newUnit = Instantiate<Unit>(testUnitCreate, transform.position, transform.rotation, transform);
    }

    public Unit getChildUnit()
    {
        return GetComponentInChildren<Unit>();
    }

    private IEnumerator expandObjToScale(GameObject obj, Vector3 targetScale, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, targetScale, duration);
            yield return null;
        }
        obj.transform.localScale = targetScale;
        yield return null;
    }

}

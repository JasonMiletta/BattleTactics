using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour {

    public GameObject[] tilePrefabs;
    public Unit testUnitCreate;
    public GameObject ground;

    private bool isTileBlank = true;
    private GameObject tile;

	// Use this for initialization
	void Start () {
        if (!isTileBlank)
        {
            enableTile();
        }
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void selectTile()
    {
        Unit unit = GetComponentInChildren<Unit>();
        if(unit != null)
        {
            unit.selectUnit();
        }
    }

    public void enableTile()
    {
        var randomIndex = Random.Range(0, tilePrefabs.Length);
        tile = Instantiate(tilePrefabs[randomIndex], this.transform.position, this.transform.rotation, this.transform);
        isTileBlank = false;
        tile.transform.localScale = Vector3.zero;
        StartCoroutine(expandInTile(0.25f));
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

    private IEnumerator expandInTile(float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            tile.transform.localScale = Vector3.Lerp(tile.transform.localScale, new Vector3(0.1f, 1, 0.1f), duration);
            yield return null;
        }
        transform.localScale = new Vector3(0.1f, 1, 0.1f);
        yield return null;
    }

}

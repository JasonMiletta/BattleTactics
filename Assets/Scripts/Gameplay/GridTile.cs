using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour {

    public enum tileType {Blank, Tree, Grass, Boulder, Plain};

    public tileType currentTileType;
    public GameObject[] tilePrefabs;
    public GameObject ground;
    public int xCoor;
    public int yCoor;

    private bool isTileBlank = true;
    [SerializeField]
    private GameObject tile;
    [SerializeField]
    private GameObject moveOverlay;
    [SerializeField]
    private GameObject attackOverlay;
    private Material originalTileMaterial;

    private bool tileIsChanging = false;

    #region EVENTS
    public delegate void TileSelectEvent(GridTile tile);
    public static event TileSelectEvent OnTileSelect;
    public static event TileSelectEvent OnTileDeselect;
    #endregion

	// Use this for initialization
	void Start ()
    {
        originalTileMaterial = tile.GetComponent<Renderer>().material;
        if(originalTileMaterial == null)
        {
        }
        setTilePrefab(currentTileType.ToString());
    }
	
	// Update is called once per frame
	void Update () {
    }

    public GridTile selectTile()
    {
        if(OnTileSelect != null){
            OnTileSelect(this);
        }
        Unit unit = GetComponentInChildren<Unit>();
        if(unit != null)
        {
            unit.selectUnit();
        }
        return this;
    }

    public void deSelectTile()
    {
        if(OnTileDeselect != null){
            OnTileDeselect(this);
        }
        Unit unit = GetComponentInChildren<Unit>();
        if (unit != null)
        {
            unit.deselectUnit();
        }
    }

    public void enableTile()
    {
        Destroy(tile);
        GameObject tilePrefab = getTilePrefab(currentTileType.ToString());
        tile = Instantiate(tilePrefab, transform.position, transform.rotation, transform);
        if (!tile.name.Contains("Blank"))
        {
            isTileBlank = false;
            tile.transform.localScale = Vector3.zero;
            StartCoroutine(lerpObjToScale(tile, new Vector3(0.1f, 1, 0.1f), 0.25f));

            ground.transform.localScale = Vector3.zero;
            ground.SetActive(true);
            StartCoroutine(lerpObjToScale(ground, new Vector3(1.0f, 50.0f, 1.0f), 0.25f));
        }
    }

    public IEnumerator setTilePrefab(string tileTypeName)
    {
        //Shrink existing tiles
        if (ground != null)
        {
            StartCoroutine(lerpObjToScale(ground, new Vector3(0f, 0, 0f), 0.25f));
        }
        if (tile != null)
        {
            StartCoroutine(lerpObjToScale(tile, new Vector3(0f, 0, 0f), 0.25f));
        }

        //Find new tilePrefab
        GameObject tilePrefab = getTilePrefab(tileTypeName);

        //Wait till existing tiles finish shrinking and delete
        while (tileIsChanging)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(tile);
        tile = Instantiate(tilePrefab, transform.position, transform.rotation, transform);
        currentTileType = getTileTypeByName(tileTypeName);

        isTileBlank = false;
        tile.transform.localScale = Vector3.zero;
        StartCoroutine(lerpObjToScale(tile, new Vector3(0.1f, 1, 0.1f), 0.25f));
        if (!tileTypeName.Contains("Blank"))
        {
            ground.transform.localScale = Vector3.zero;
            ground.SetActive(true);
            StartCoroutine(lerpObjToScale(ground, new Vector3(1.0f, 50.0f, 1.0f), 0.25f));
        }
        yield return null;
    }

    public Unit getChildUnit()
    {
        return GetComponentInChildren<Unit>();
    }   

    public Structure getChildStructure(){
        return GetComponentInChildren<Structure>();
    }

    private IEnumerator lerpObjToScale(GameObject obj, Vector3 targetScale, float duration)
    {
        tileIsChanging = true;
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, targetScale, duration);
            yield return null;
        }
        obj.transform.localScale = targetScale;
        if(targetScale == Vector3.zero)
        {
            obj.SetActive(false);
        }
        tileIsChanging = false;
        yield return null;
    }

    private GameObject getTilePrefab(string tileTypeName)
    {
        ArrayList prefabsOfMatchingTiles = new ArrayList();
        foreach (GameObject prefab in tilePrefabs)
        {
            if (prefab.name.Contains(tileTypeName))
            {
                prefabsOfMatchingTiles.Add(prefab);
            }
        }
        if(prefabsOfMatchingTiles.Count == 0){
            Debug.LogWarning("Ensure that the prefab for tile type: " + tileTypeName + " has been added to the tilePrefabs list");
        } else {
            var randomIndex = Random.Range(0, prefabsOfMatchingTiles.Count);
            return (GameObject)prefabsOfMatchingTiles[randomIndex];
        }
        return null;
    }

    private tileType getTileTypeByName(string tileTypeName)
    {
        return (tileType)System.Enum.Parse(typeof(tileType), tileTypeName);
    }

    public void enableMoveOverlay()
    {
        if(moveOverlay != null && currentTileType != tileType.Blank && getChildUnit() == null)
        {
            moveOverlay.SetActive(true);
        }
    }

    public void disableMoveOverlay()
    {
        if (moveOverlay != null)
        {
            moveOverlay.SetActive(false);
        }
    }

    public bool isMoveable()
    {
        return moveOverlay.activeSelf;
    }

    public bool isAttackable()
    {
        return attackOverlay.activeSelf;
    }

    public void enableAttackOverlay()
    {
        Unit childUnit = getChildUnit();
        if(attackOverlay != null && !moveOverlay.activeSelf && childUnit != null)
        {
            attackOverlay.SetActive(true);
        }
    }

    public void disableAttackOverlay()
    {
        if(attackOverlay != null)
        {
            attackOverlay.SetActive(false);
        }
    }

    public bool isOverlayActive()
    {
        return attackOverlay.activeSelf || moveOverlay.activeSelf;
    }
}

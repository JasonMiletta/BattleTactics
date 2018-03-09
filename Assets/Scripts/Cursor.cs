using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cursor : MonoBehaviour {
    public GridTile selectedGridTile;
    public GridTile currentHighlightedTile;
    public GameObject selectedCursorPrefab;

    private bool currentlyHasSelectedTile = false;
    private Unit currentlySelectedUnit;

    public Unit testUnit;

    // Use this for initialization
    void Start () {
        selectedCursorPrefab = Instantiate(selectedCursorPrefab);
        selectedCursorPrefab.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
        updateCursorPosition();
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        if (!eventSystem.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentlySelectedUnit)
                {
                    cursorSelect();
                }
                else if (currentlyHasSelectedTile)
                {
                    cursorDeselect();
                }
                else
                {
                    cursorSelect();
                }
            }
        }

        //DEBUG
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedGridTile != null)
            {
                createTestUnitOnTile();
            } else
            {
                cursorDeselect();
            }
        }
    }

    //DEBUG
    public void createTestUnitOnTile()
    {
        Transform gridTransform = selectedGridTile.transform;
        Unit newUnit = Instantiate<Unit>(testUnit, gridTransform.position, gridTransform.rotation, gridTransform);
        cursorDeselect();
    }

    private void updateCursorPosition()
    {
        Ray mouseRayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRayCast, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GridLayer")))
        {
            Debug.DrawLine(mouseRayCast.origin, hit.point);
            GameObject gridObject = hit.collider.gameObject;
            currentHighlightedTile = gridObject.GetComponent<GridTile>();
        }

        if (currentHighlightedTile != null)
        {
            transform.position = Vector3.Lerp(transform.position, currentHighlightedTile.transform.position, 0.25f);
        }
    }

    public void cursorSelect()
    {
        //If we're currently moving a unit
        if (currentlySelectedUnit != null)
        {
            if (currentHighlightedTile.isMoveable())
            {
                currentlySelectedUnit.moveUnitToGridTile(currentHighlightedTile);
                cursorDeselect();
            } else if (currentHighlightedTile.isAttackable()){
                currentlySelectedUnit.attackTile(currentHighlightedTile);
                cursorDeselect();
            }
        }
        else
        {
            currentlyHasSelectedTile = selectCurrentTile();
        }
        selectedCursorPrefab.transform.position = selectedGridTile.transform.position;
        selectedCursorPrefab.SetActive(true);

        Vector3 destination = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
        StartCoroutine(smoothMovement(transform.position, destination, 0.1f));
    }

    public void cursorDeselect()
    {
        //Set highlighter to inactive
        selectedCursorPrefab.SetActive(false);
        selectedGridTile.deSelectTile();
        selectedGridTile = null;
        currentlyHasSelectedTile = false;
        currentlySelectedUnit = null;
        Vector3 destination = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
        StartCoroutine(smoothMovement(transform.position, destination, 0.1f));
    }

    private bool selectCurrentTile()
    {
        if (selectedGridTile == null)
        {
            selectedGridTile = currentHighlightedTile.selectTile();
            currentlySelectedUnit = currentHighlightedTile.getChildUnit();
            return true;
        }
        return false;
    }

    private IEnumerator smoothMovement(Vector3 source, Vector3 destination, float duration)
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
}

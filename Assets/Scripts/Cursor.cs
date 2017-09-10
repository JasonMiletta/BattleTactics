using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
    public GridTile selectedGridTile;

    private bool currentlyHasSelectedTile = false;
    private Unit currentlySelectedUnit;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        updateCursorPosition();
        if (Input.GetMouseButtonDown(0))
        {
            if (currentlyHasSelectedTile)
            {
                if (currentlySelectedUnit)
                {
                    currentlySelectedUnit.moveUnitToGridTile(selectedGridTile);
                }
                cursorDeselect();
            }
            else
            {
                cursorSelect();
            }
        } else if (Input.GetMouseButtonDown(2))
        {
            selectedGridTile.createTestUnitOnTile();
        }
    }

    private bool selectCurrentTile()
    {
        if (selectedGridTile.getChildUnit())
        {
            selectedGridTile.selectTile();
            currentlySelectedUnit = selectedGridTile.getChildUnit();
            return true;
        }

        return false;
    }

    private void updateCursorPosition()
    {
        Ray mouseRayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRayCast, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GridLayer")))
        {
            Debug.DrawLine(mouseRayCast.origin, hit.point);
            GameObject gridObject = hit.collider.gameObject;
            selectedGridTile = gridObject.GetComponent<GridTile>();
            transform.position = Vector3.Lerp(transform.position, gridObject.transform.position, 0.25f);
        }
    }

    private void cursorSelect()
    {
        currentlyHasSelectedTile = selectCurrentTile();
        Vector3 destination = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
        StartCoroutine(smoothMovement(transform.position, destination, 0.1f));
    }

    private void cursorDeselect()
    {
        currentlyHasSelectedTile = false;
        currentlySelectedUnit = null;
        Vector3 destination = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
        StartCoroutine(smoothMovement(transform.position, destination, 0.1f));
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

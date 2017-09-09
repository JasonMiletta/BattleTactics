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
        if (Input.GetMouseButtonDown(0))
        {
            if (currentlyHasSelectedTile)
            {
                currentlyHasSelectedTile = false;
                if (currentlySelectedUnit)
                {
                    currentlySelectedUnit.moveUnitToGridTile(selectedGridTile);
                }
            }
            else
            {
                selectCurrentTile();
            }
        } else if (Input.GetMouseButtonDown(2))
        {
            selectedGridTile.createTestUnitOnTile();
        }
        updateCursorPosition();
    }

    private void selectCurrentTile()
    {
        if (selectedGridTile.getChildUnit())
        {
            currentlyHasSelectedTile = true;
            selectedGridTile.selectTile();
            currentlySelectedUnit = selectedGridTile.getChildUnit();
        }
    }

    private void updateCursorPosition()
    {
        Ray mouseRayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRayCast, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GridLayer")))
        {
            Debug.DrawLine(mouseRayCast.origin, hit.point);
            GameObject gridObject = hit.collider.gameObject;
            selectedGridTile = gridObject.GetComponentInParent<GridTile>();

            transform.position = Vector3.Lerp(this.transform.position, gridObject.transform.parent.position, 0.25f);
        }
    }

    private void expandCursor()
    {
        transform.localScale.Scale(new Vector3(1.5f, 1.5f, 1.5f));
    }

    private void shrinkCursor()
    {
        transform.localScale.Scale(new Vector3(1f, 1f, 1f));
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
    public GridTile selectedGridTile;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectCurrentTile();
        }
        updateCursorPosition();
    }

    private void selectCurrentTile()
    {

        selectedGridTile.selectTile();
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

}

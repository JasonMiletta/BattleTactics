using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TilePreviewController : MonoBehaviour {

	#region COMPONENTS
	public Camera previewCamera;
	private Anim_OpenCloseForm anim_OpenCloseForm;
	#endregion

	#region PARAMETER
	public Vector3 positionOffset = new Vector3(0.0f, 2.0f, -1.0f);
	#endregion
	void OnEnable(){
		GridTile.OnTileSelect += handleTileSelectedEvent;
		GridTile.OnTileDeselect += handleTileDeselectedEvent;
	}

	void OnDisable(){
		GridTile.OnTileSelect -= handleTileSelectedEvent;
		GridTile.OnTileDeselect -= handleTileDeselectedEvent;
	}

	// Use this for initialization
	void Start () {
		if(previewCamera == null){
			previewCamera = GetComponentInChildren<Camera>();
		}
		if(previewCamera == null){
			Debug.LogError("The UnitPreviewController can't find a camera! Make sure its in the child object or manually set in the editor");
		} else {
			previewCamera.gameObject.SetActive(false);
		}
		
		anim_OpenCloseForm = GetComponent<Anim_OpenCloseForm>();
		if(anim_OpenCloseForm == null){
			Debug.LogError("UI_UnitActionPanel: Animator is missing from this object!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void handleTileSelectedEvent(GridTile tile){
		previewCamera.gameObject.transform.position = tile.transform.position + positionOffset;
		previewCamera.gameObject.transform.LookAt(tile.transform);
		previewCamera.gameObject.SetActive(true);
		if(anim_OpenCloseForm == null){
			anim_OpenCloseForm = GetComponent<Anim_OpenCloseForm>();
		}
		if(anim_OpenCloseForm != null){
			anim_OpenCloseForm.OpenPanel();
		}
		Debug.Log("Camera Active!");
	}
	private void handleTileDeselectedEvent(GridTile tile){
		if(anim_OpenCloseForm != null){
			anim_OpenCloseForm.ClosePanel();
		}
	}	
}

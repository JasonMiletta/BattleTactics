using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Form : MonoBehaviour {

	#region PARAMETERS
	public Vector3 MaxScale = new Vector3(1.0f, 1.0f, 1.0f);
	public Vector3 MinScale = new Vector3(0.0f, 0.0f, 0.0f);
	#endregion

	#region COMPONENTS
	List<GameObject> children = new List<GameObject>();
	#endregion

	#region STATE
	private bool isFormChanging = false;
	private int awaitingFormCount = 0;
	#endregion

	void OnEnable(){
		Util_TransformManipulation.OnLerpComplete += handleOnLerpComplete;
	}

	void OnDisable(){
		Util_TransformManipulation.OnLerpComplete -= handleOnLerpComplete;
	}

	// Use this for initialization
	void Start () {
		for(var i = 0; i < transform.childCount; ++i){
			GameObject child = transform.GetChild(i).gameObject;
			children.Add(child);
		}	
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}

	public void activateForm(){
		gameObject.SetActive(true);
		foreach(GameObject child in children){
			child.SetActive(true);
			StartCoroutine(Util_TransformManipulation.lerpObjToScale(child, MaxScale, 0.5f));
		}
		
	}

	public void deactivateForm(){
		isFormChanging = true;
		foreach(GameObject child in children){
			StartCoroutine(Util_TransformManipulation.lerpObjToScale(child, Vector3.zero, 0.5f));
			++awaitingFormCount;
		}
		//TODO: We need to capture when all of the children are inactive and set this to inactive.
		// Alternatively handle this menu UIForm staying active for enabling/disabling
	}

	private void handleOnLerpComplete(){
		if(isFormChanging){
			--awaitingFormCount;
			if(awaitingFormCount <= 0){
				awaitingFormCount = 0;
				isFormChanging = false;
				gameObject.SetActive(false);
			}
		}
	}
}

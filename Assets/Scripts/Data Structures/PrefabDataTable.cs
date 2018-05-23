using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDataTable : MonoBehaviour {

	public List<GameObject> prefabList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject getPrefabByName(string prefabName){
		if(prefabList != null){
			foreach(GameObject obj in prefabList){
				if(obj.name.Equals(prefabName)){
					return obj;
				}
			}
		}
		return null;
	}
}

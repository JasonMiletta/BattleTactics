using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Floating : MonoBehaviour {
    
    public float amplitude = 0.01f;
    public float frequency = 1.0f;
    public bool isActive = false;

    // Position Storage Variables
    float scale = new float();

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive){
            // Float up/down with a Sin()
            scale = Mathf.Sin(Time.fixedTime* Mathf.PI * frequency) * amplitude;
            transform.localPosition += new Vector3(0, scale, 0);
        }
    }

    public void toggleAnimation(){
        isActive = !isActive;
    }
}

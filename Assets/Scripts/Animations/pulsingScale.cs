using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulsingScale : MonoBehaviour {
    
    public float amplitude = 0.001f;
    public float frequency = 0.05f;

    // Position Storage Variables
    float scale = new float();

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Float up/down with a Sin()
        scale = Mathf.Cos(Time.fixedTime* Mathf.PI * frequency) * amplitude;
        transform.localScale += new Vector3(scale, scale, scale);
    }
}

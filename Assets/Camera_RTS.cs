using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_RTS : MonoBehaviour {

    public float scrollSpeedMultiplier = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        cameraMovementLoop();
        if (Input.GetButtonDown("RotateLeft"))
        {
            transform.Rotate(new Vector3(0,1,0), -45.0f, Space.World);
        }
        if (Input.GetButtonDown("RotateRight"))
        {

            transform.Rotate(new Vector3(0, 1, 0), 45.0f, Space.World);
        }
    }

    void cameraMovementLoop()
    {
        float horizontalVector = Input.GetAxis("Horizontal") * Time.deltaTime * scrollSpeedMultiplier;
        float verticalVector = Input.GetAxis("Vertical") * Time.deltaTime * scrollSpeedMultiplier;

        transform.Translate(Vector3.right * horizontalVector, Space.Self);
        transform.Translate(Vector3.forward * verticalVector, Space.Self);
    }
}

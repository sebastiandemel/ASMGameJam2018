using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraMovement : MonoBehaviour {

    public float speed;
    public float maxPositionX,minPositionX;
    public float maxPosY, minPosY;

    private Rigidbody cameraRig;

    private void Awake()
    {
        cameraRig = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
  
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ);
        cameraRig.velocity = movement * speed;

      
    }

    private void LateUpdate()
    {
        cameraRig.position = new Vector3(
            Mathf.Clamp(cameraRig.position.x, minPositionX, maxPositionX),
            transform.position.y,
            Mathf.Clamp(cameraRig.position.z, minPosY, maxPosY)
            );
    }
}

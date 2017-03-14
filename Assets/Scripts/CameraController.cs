using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float cameraSpeed = 0.5f;
	public float movementBuffer = 0.5f;
	private Vector3 cameraTarget;

	// Use this for initialization
	void Start () {
		target 	= GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		cameraTarget = new Vector3 (target.position.x, transform.position.y, transform.position.z);
		//if (Mathf.Abs(transform.position.x - target.position.x) > movementBuffer) {
			transform.position = Vector3.Lerp (transform.position, cameraTarget, Time.deltaTime * cameraSpeed);
		//}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraFollow : MonoBehaviour
{
	private Camera _camera;
	private float _cameraZ;

	private void Awake()
	{
		_camera = Camera.main;
		_cameraZ = _camera.transform.position.z;
	}

	private void Update()
	{
		_camera.transform.position = new Vector3(transform.position.x, transform.position.y, _cameraZ);
	}
}

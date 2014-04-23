﻿using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

	[SerializeField]
	private Rigidbody throwObj;
	[SerializeField]
	private Vector3 offSet = Vector3.zero;

	//reference to the camera so we can get camState!
	private ThirdPersonCamera camera;

	Vector3 force;

	public LineRenderer arcLine;
	//player transform
	private Transform PlayerXForm;
	// Use this for initialization
	void Start () {
		PlayerXForm = GameObject.FindWithTag ("Player").transform;
		if (PlayerXForm == null)
						Debug.Log ("Could not find player transform");

		//arcLine = new LineRenderer ();
		arcLine.SetVertexCount (180);
		arcLine.SetWidth (0.2f, 0.2f);

		camera = Camera.main.GetComponent<ThirdPersonCamera> ();

		if (arcLine == null)
						Debug.Log ("arcLine");
	}
	
	// Update is called once per frame
	void Update () {
		float rightY = Input.GetAxis("RightStickVertical");

		force = ((PlayerXForm.forward + PlayerXForm.up) * 5);
		force = force + ((PlayerXForm.forward + PlayerXForm.up) * ((-rightY * 0.5f) * 10));
		if (camera.camState == ThirdPersonCamera.CamStates.FirstPerston) {
						//if (Input.GetKeyDown (KeyCode.H))
						UpdatePredictionLine ();
						if (Input.GetButtonDown("Fire1"))
								ThrowObject ();
				} else
						arcLine.SetVertexCount (0);

	}

	void ThrowObject() {
		Rigidbody clone;

		clone = Instantiate(throwObj,PlayerXForm.position + (PlayerXForm.forward * 1) + new Vector3(0f,1f,0f),Quaternion.identity) as Rigidbody;

		clone.AddForce(force, ForceMode.Impulse);
	}
	void UpdatePredictionLine() {
		arcLine.SetVertexCount(180);
		for(int i = 0; i < 180; i++)
		{
			Vector3 posN = GetTrajectoryPoint(PlayerXForm.position + (PlayerXForm.forward * 1) + new Vector3(0f,1f,0f),force, i, Physics.gravity);
			arcLine.SetPosition(i,posN);
		}
	}
	
	Vector3 GetTrajectoryPoint(Vector3 startingPosition, Vector3 initialVelocity, float timestep, Vector3 gravity)
	{
		float physicsTimestep = Time.fixedDeltaTime;
		Vector3 stepVelocity = physicsTimestep * initialVelocity;
		
		//Gravity is already in meters per second, so we need meters per second per second
		Vector3 stepGravity = physicsTimestep * physicsTimestep * gravity;
		
		return startingPosition + (timestep * stepVelocity) + ((( timestep * timestep + timestep) * stepGravity ) / 2.0f);
	}
}

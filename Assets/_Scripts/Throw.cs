using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {

	[SerializeField]
	private Rigidbody throwObj;
	[SerializeField]
	private Vector3 offSet = Vector3.zero;

	//reference to the camera so we can get camState!
	private ThirdPersonCamera camera;

	private Animator _anim;
	Vector3 force;
	float forceStick = 0;

	public Vector3 highestPos;
	public Vector3 lastPos;


	GameObject target;

	Rigidbody throbject;

	private float clock;

	[Range (1f,25f)]
	public float maxForce = 10.0f;
	

	private bool throwing = false;
	private float throwClock;

	public float throwOffset = 0.6f;
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

		_anim = GameObject.FindWithTag ("Player").GetComponent<Animator>();
		target = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		target.transform.localScale.Set (10f, 10f, 10f);
		target.renderer.enabled = false;
		target.layer = 2;
		target.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		Color c = target.renderer.material.color;
		c.a = 0.25f;
		target.renderer.material.color = c;
		target.collider.enabled = false;
	 	//Shader testSphere = target.renderer.material.shader;
		//testSphere.
		clock = Time.time;
	}

	// Update is called once per frame
	void Update () {
		float rightY = -Input.GetAxis("Vertical");



		if (rightY > 0.0f || rightY < 0.0f)
			forceStick += -rightY * 0.5f;
		

		if (forceStick > maxForce)
			forceStick = maxForce;
		else if (forceStick < -1)
			forceStick = -1.0f;

		force = ((PlayerXForm.forward + PlayerXForm.up) * 5);
		force = force + ((PlayerXForm.forward + PlayerXForm.up) * forceStick);
		//if (camera.camState == ThirdPersonCamera.CamStates.FirstPerston) {
		if (_anim.GetBool ("ThrowMode")) {
						if(throbject == null && _anim.GetCurrentAnimatorStateInfo(0).IsName("Throw Prepare") && withinTimeLimit()){
							throbject = Instantiate(throwObj, GameObject.Find("L_wrist_ctrl").transform.position,Quaternion.identity) as Rigidbody;
							throbject.transform.parent = GameObject.Find("L_wrist_ctrl").transform;
							throbject.rigidbody.useGravity = false;
						}	
						//if (Input.GetKeyDown (KeyCode.H))
						target.renderer.enabled = true;
						UpdatePredictionLine ();
						if (Input.GetButtonDown ("Fire1") && !throwing && _anim.GetCurrentAnimatorStateInfo (0).IsName ("Throw Idle")) {
								throwClock = Time.time + throwOffset;
								changeThrowStatus();
						}
						if (Time.time > throwClock && throwing) {
								ThrowObject ();
								changeThrowStatus();
						}
		} else {
						arcLine.SetVertexCount (0); 
						GameObject.Find("L_wrist_ctrl").transform.DetachChildren();
			if(throbject != null)			
				throbject.rigidbody.useGravity = true;
						target.renderer.enabled = false;

			if(_anim.GetBool("Throw"))
						changeThrowStatus();						
		}

	}

	void ThrowObject() {
		//Rigidbody clone;

		//clone = Instantiate(throwObj, GameObject.Find("L_wrist_ctrl").transform.position/*PlayerXForm.position + (PlayerXForm.forward * 1) + new Vector3(0f,1f,0f)*/,Quaternion.identity) as Rigidbody;


		throbject.rigidbody.useGravity = true;
		GameObject.Find("L_wrist_ctrl").transform.DetachChildren();

		throbject.AddForce(force, ForceMode.Impulse);
		throbject = null;
	}
	/*
	void UpdatePredictionLine() {
		arcLine.SetVertexCount(180);
		for(int i = 0; i < 180; i++)
		{
			Vector3 posN = GetTrajectoryPoint(PlayerXForm.position + (PlayerXForm.forward * 1) + new Vector3(0f,1f,0f),force, i, Physics.gravity);
			arcLine.SetPosition(i,posN);
		}
	}
	*/

	void UpdatePredictionLine()
	{
		arcLine.SetVertexCount(180);
		Vector3 previousPosition = PlayerXForm.position + (PlayerXForm.forward * 1) + new Vector3(0f,1f,0f);

		highestPos = previousPosition;
		
		for(int i = 0; i < 180; i++)
		{
			Vector3 posN = GetTrajectoryPoint(PlayerXForm.position + (PlayerXForm.forward * 1) + new Vector3(0f,1f,0f), force, i, Physics.gravity);
			Vector3 direction = posN - previousPosition;
			direction.Normalize();
			
			float distance = Vector3.Distance(posN, previousPosition);
			
			RaycastHit hitInfo = new RaycastHit();
			if(Physics.Raycast(previousPosition, direction, out hitInfo, distance))
			{
				if(highestPos.y < hitInfo.point.y)
					highestPos = hitInfo.point;
				if(hitInfo.transform.tag != "Throw") {

				arcLine.SetPosition(i,hitInfo.point);
				arcLine.SetVertexCount(i);
					lastPos = hitInfo.point;
				break;
				}
			}

			if(highestPos.y < posN.y)
				highestPos = posN;

			previousPosition = posN;
			arcLine.SetPosition(i,posN);
		}
		target.transform.position = lastPos;
	}

	Vector3 GetTrajectoryPoint(Vector3 startingPosition, Vector3 initialVelocity, float timestep, Vector3 gravity)
	{
		float physicsTimestep = Time.fixedDeltaTime;
		Vector3 stepVelocity = physicsTimestep * initialVelocity;
		
		//Gravity is already in meters per second, so we need meters per second per second
		Vector3 stepGravity = physicsTimestep * physicsTimestep * gravity;
		
		return startingPosition + (timestep * stepVelocity) + ((( timestep * timestep + timestep) * stepGravity ) / 2.0f);
	}

	void changeThrowStatus(){
		throwing = !throwing;
		_anim.SetBool ("Throw", !_anim.GetBool ("Throw"));
	}

	bool withinTimeLimit(){
		return _anim.GetCurrentAnimatorStateInfo (0).normalizedTime > 0.5 && _anim.GetCurrentAnimatorStateInfo (0).normalizedTime < 0.75;
	}
}

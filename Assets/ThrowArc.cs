using UnityEngine;
using System.Collections;

public class ThrowArc : MonoBehaviour {
	private LineRenderer lineRenderer;
	public int segmentCount = 20;
	public float segmentScale = 1.0f;

	public PlayerFire playerFire;
	private Collider _hitObject;
	public Collider hitObject{get {return _hitObject; } }

	bool test;
	void FixedUpdate() {
		if (!test) {
			simulatePath ();
			test = true;
		}
	}

	void simulatePath()
	{
		Vector3[] segments = new Vector3[segmentCount];
		
		// The first line point is wherever the player's cannon, etc is
		segments[0] = playerFire.transform.position;


		// The initial velocity
		Vector3 segVelocity = transform.forward * playerFire.fireStrength * Time.deltaTime;

		//segVelocity *= -1.0f;

		// reset our hit object
		_hitObject = null;
		
		for (int i = 1; i < segmentCount; i++)
		{
			// Time it takes to traverse one segment of length segScale (careful if velocity is zero)
			float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0;
			Debug.Log(segVelocity);
			// Add velocity from gravity for this segment's timestep
			segVelocity = segVelocity + Physics.gravity * segTime;
			
			// Check to see if we're going to hit a physics object
			RaycastHit hit;
			if (Physics.Raycast(segments[i - 1], segVelocity, out hit, segmentScale))
			{
				// remember who we hit
				_hitObject = hit.collider;
				
				// set next position to the position where we hit the physics object
				//segments[i] = segments[i - 1] + segVelocity.normalized * hit.distance;
				// correct ending velocity, since we didn't actually travel an entire segment
				//segVelocity = segVelocity - (Physics.gravity * 0.5f) * (segmentScale - hit.distance) / segVelocity.magnitude;
				// flip the velocity to simulate a bounce
				//segVelocity = Vector3.Reflect(segVelocity, hit.normal);


				segmentCount = i;
				break;
				//if(hit.normal)
				//	Debug.Log(hit.normal);

				/*
				 * Here you could check if the object hit by the Raycast had some property - was 
				 * sticky, would cause the ball to explode, or was another ball in the air for 
				 * instance. You could then end the simulation by setting all further points to 
				 * this last point and then breaking this for loop.
				 */
			}
			// If our raycast hit no objects, then set the next position to the last one plus v*t
			else
			{
				segments[i] = segments[i - 1] + segVelocity * segTime;
			}
		}
		
		// At the end, apply our simulations to the LineRenderer
		
		// Set the colour of our path to the colour of the next ball
		Color startColor = playerFire.nextColor;
		Color endColor = startColor;
		startColor.a = 1;
		endColor.a = 0;
		//lineRenderer.SetWidth (0.1f, 0.1f);
		lineRenderer.SetColors(startColor, endColor);
		
		lineRenderer.SetVertexCount(segmentCount);
		for (int i = 0; i < segmentCount; i++)
			lineRenderer.SetPosition(i, segments[i]);
	}

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
		playerFire = GameObject.FindWithTag("CameraFollow").GetComponent<PlayerFire> ();
		if (lineRenderer == null)
						Debug.Log ("No lineRender!");

		lineRenderer.SetWidth (0.2f, 0.2f);

		test = false;

	}

	// Update is called once per frame
	void Update () {

	}
}

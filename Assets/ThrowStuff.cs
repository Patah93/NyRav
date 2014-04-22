using UnityEngine;
using System.Collections;

public class ThrowStuff : MonoBehaviour {

	public Rigidbody obj;
	public Transform throwpos;
	public Transform throwtar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetButton ("Fire1")) {
			throwstuff();
		}

	}

	private void throwstuff() {
		
		
		Rigidbody clone;
		
		//Debug.Log (throwpos.position.x + " " + throwpos.position.y + " " + throwpos.position.z);
		
		
		//Quaternion rot = Quaternion.LookRotation(throwtar.position - throwpos.position);
		//Debug.Log ("X:" + Mathf.Acos(rot.x) + " " + "Y:" + Mathf.Acos(rot.y) + " " + "Z:" + rot.z + "" + "W:" + rot.w);
		//Debug.Log ("Start: " + throwpos.position + "To: " + throwtar.position);
		
		
		
		Vector3 relRot = throwtar.position - throwpos.position;
		//relRot += throwpos.position;
		
		//Quaternion rot = Quaternion.FromToRotation(transform.forward,relRot);
		
		Vector3 rots = throwtar.position - throwpos.position;
		
		clone = Instantiate(obj,throwpos.position,transform.rotation) as Rigidbody;
		//clone.transform.Rotate (Vector3.up,Vector3.Angle(clone.transform.forward,relRot),Space.Self);
		float rotation = Vector3.Angle (clone.transform.forward,relRot);
		
		clone.transform.Rotate (Vector3.right, -rotation);

		//clone.rotation = rot;
		clone.useGravity = true;
		
		//clone.transform.rotation = Quaternion.Lerp(
		
		
		
		clone.AddForce(relRot.normalized * 10f, ForceMode.Impulse);
		//clone.AddRelativeForce(transform.InverseTransformPoint(relRot.normalized) * 10f, ForceMode.Impulse);
		
	}

}

using UnityEngine;
using System.Collections;

public class CubeCollision : MonoBehaviour {

	public string _playername = "Betafab";
	bool _collided = false;
	bool _firstcol= false;
	int _collidedItems = 0;

	public Vector3 _lastPos;
	// Use this for initialization
	void Start () {
		_lastPos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(!_collided){
			_lastPos = gameObject.transform.position;
		}
	}

	void OnCollisionEnter(Collision collision) {
		if((collision.transform.name != _playername)){
			foreach (ContactPoint contact in collision.contacts) {
				//Debug.DrawRay(contact.point, contact.normal*5, Color.blue);
				if(contact.normal != Vector3.up){
					_collided = true;
					Debug.Log ("Collided with wall");
					return;
				}
			}
		}
		//_collidedItems +=1;
	}

	/*void OnCollisionStay(Collision other){
		Vector3 dir = (transform.position - other.transform.position).normalized;
		float dist = 0.3f;
		rigidbody.MovePosition(transform.position + dir*dist);
	}*/


	void OnCollisionExit(Collision collision) {
		if((collision.transform.name != _playername)){
			foreach (ContactPoint contact in collision.contacts) {
				//Debug.DrawRay(contact.point, contact.normal*5, Color.blue);
				if(contact.normal != Vector3.up){
					//_collided = false;
					Debug.Log ("Exit collision with wall");
					return;
				}
			}
		}
	}

	public void deactivateCollision(){
		_collided = false;
	}

	public bool getCollision(){
		return _collided;
	}
}

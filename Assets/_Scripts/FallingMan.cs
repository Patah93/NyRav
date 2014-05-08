using UnityEngine;
using System.Collections;

public class FallingMan : MonoBehaviour {

	[Range (0.1f, 2.0f)]
	public float _length = 1.0f;

	Animator _ani;
	private Vector3 _fallVelocity;
	CharacterController _controller;
	bool _grounded;

	// Use this for initialization
	void Start () {
		_ani = gameObject.GetComponent<Animator> ();
		_controller = gameObject.GetComponent<CharacterController> ();
		_grounded = true;
	}
	
	// Update is called once per frame
	void Update () {

		if(_controller.isGrounded){
			_grounded = true;
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Run") && _grounded){
			_fallVelocity.x = _controller.velocity.x;
			//_fallVelocity.z = _controller.velocity.y;
			_fallVelocity.z = _controller.velocity.z;
		
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Jump")){
			_grounded = false;
			_controller.Move(new Vector3(_fallVelocity.x*_length, _fallVelocity.y, _fallVelocity.z*_length));	
			Debug.Log("Vel in Jump: " + _controller.velocity);
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Fallin'")){
			//_fallVelocity.y -= 9.82f*Time.deltaTime*0.0001f;

			_controller.Move(new Vector3(_fallVelocity.x*_length, _fallVelocity.y, _fallVelocity.z*_length));
			Debug.Log("Vel in fall: " + _controller.velocity);
			_grounded = false;


		}

		/*if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle Jump") && _ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.25){
			_fallVelocity.x = _controller.velocity.x;
			_fallVelocity.z = _controller.velocity.z;	
		}*/


	}
}

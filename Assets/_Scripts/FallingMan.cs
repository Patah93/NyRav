using UnityEngine;
using System.Collections;

public class FallingMan : MonoBehaviour {

	[Range (0.01f, 0.02f)]
	public float _length = 0.15f;

	Animator _ani;
	private Vector3 _fallVelocity;
	CharacterController _controller;

	// Use this for initialization
	void Start () {
		_ani = gameObject.GetComponent<Animator> ();
		_controller = gameObject.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Run")){
			_fallVelocity.x = _controller.velocity.x;
			_fallVelocity.z = _controller.velocity.z;
		
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Jump")){
			_controller.Move(_fallVelocity*_length);	
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Fallin'")){
		//	Debug.Log ("FallVelocity: " + _fallVelocity);
			_controller.Move(_fallVelocity*_length);
			 
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle Jump") && _ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.25){
			_fallVelocity.x = _controller.velocity.x;
			_fallVelocity.z = _controller.velocity.z;	
		}


	}
}

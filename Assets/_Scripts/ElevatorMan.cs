using UnityEngine;
using System.Collections;

public class ElevatorMan : TriggerAction {
	
	public GameObject _pos1;
	public GameObject _pos2;
	public float _moveSpeed = 0.2f;

	bool _triggered;
	bool _goingUp;
	float _offSet;

	// Use this for initialization
	void Start () {
		_triggered = true;
		_goingUp = true;
		_offSet = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {

		if(_triggered){
			if(_goingUp){

				Vector3 targetMove = Vector3.Lerp(transform.position, _pos2.transform.position, _moveSpeed);
				transform.position = targetMove;

				if((transform.position - _pos2.transform.position).magnitude <= _offSet){
					transform.position = _pos2.transform.position;
					_goingUp = !_goingUp;
				}
			}


			else{

				Vector3 targetMove = Vector3.Lerp(transform.position, _pos1.transform.position, _moveSpeed);
				transform.position = targetMove;

				if((transform.position - _pos1.transform.position).magnitude <= _offSet){
					transform.position =_pos1.transform.position;
					_goingUp = !_goingUp;
				}
			}

		}
	}
	
	public override void onActive(){

		Debug.Log("ACTIVE?!");

		_triggered = true;
		if(gameObject.CompareTag("Player")){
			gameObject.transform.parent = transform;
		}
	}
	
	public override void onInactive(){
		_triggered = false;
	}


}

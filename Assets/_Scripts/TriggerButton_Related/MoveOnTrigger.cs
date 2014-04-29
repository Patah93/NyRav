using UnityEngine;
using System.Collections;

public class MoveOnTrigger : TriggerAction {

	Vector3 _originalPos;

	public Vector3 _offset = new Vector3(0, 5, 0);

	public float _moveTime = 0.02f;

	bool _isMoving = false;

	// Use this for initialization
	void Start () {
		_originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(_isMoving){
			transform.position = Vector3.Lerp(gameObject.transform.position, _offset + _originalPos, _moveTime);
		}
		else{
			transform.position = Vector3.Lerp(gameObject.transform.position, _originalPos, _moveTime);
		}
	
	}

	public override void onActive(){
		_isMoving = true;

	}

	public override void onInactive(){
		_isMoving = false;
	}
}

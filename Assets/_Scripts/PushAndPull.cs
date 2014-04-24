using UnityEngine;
using System.Collections;

public class TestKnuff : MonoBehaviour {

	Vector3 _position;
	Vector3 _deltapos;
	Vector3 _objpos;
	Transform _obj;
	bool _pushing;
	public float _deadZone = 0.2f;
	public float _maxSpeed = 0.1f;
	public float _lerpTime = 0.06f;
	public float _offset = 1;
	float _speed;
	Animator _ani;
	// Use this for initialization

	void Start () {
		_ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if(_pushing){
			if(Input.GetAxis("Vertical") > _deadZone || Input.GetAxis("Vertical") < -_deadZone){
				_speed = Mathf.Sign(Input.GetAxis("Vertical")); 
			} 
			else{
				_speed = 0;
			}
			_ani.SetFloat("Speed", _speed);
			_deltapos = transform.position - _position;

			if(Mathf.Abs(_deltapos.x) > 0.01 || Mathf.Abs(_deltapos.z) >0.01){
				_obj.rigidbody.MovePosition(_obj.position + new Vector3(_deltapos.x,0,_deltapos.z));
			}
			_position = new Vector3(transform.position.x, 0, transform.position.z);
		}
	}

	public void Activate(bool isActivated, Transform _object, Vector3 direction){
		if(isActivated){

			_obj = _object;
			_objpos = _obj.position;
			Vector3 temp = direction*-1;
			float angle = Vector3.Angle(temp, transform.forward);
			transform.forward = temp;
			Vector3 temppos = _obj.position;
			transform.position = new Vector3(temppos.x,transform.position.y,temppos.z) + ((_obj.localScale.x/2) + _offset)*direction;
			_obj.rigidbody.constraints = RigidbodyConstraints.None;
			_obj.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			_position = new Vector3(transform.position.x, 0, transform.position.z);

			_ani.SetBool("Pushing",true);
		}
		else{
			_ani.SetBool("Pushing",false);
			_ani.SetFloat("Speed",0);
			_obj.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
		_pushing = isActivated;
	}

	public bool getActivate(){
		return _pushing;
	}
}

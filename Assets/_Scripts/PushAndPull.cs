using UnityEngine;
using System.Collections;

public class PushAndPull : MonoBehaviour {

	Vector3 _position;
	Vector3 _deltapos;
	Vector3 _deltaobjpos;
	Vector3 _objpos;
	Transform _obj;
	float _objposy;
	float _boyposx;
	bool _pushing;
	public float _deadZone = 0.2f;
	public float _maxSpeed = 0.1f;
	public float _lerpTime = 0.06f;
	public float _offset = 1;
	float _speed;
	Animator _ani;
	BoyStateManager _boystate; 
	bool _blockedBackwards = false;
	bool _blockedForward = false;
	float _distance;
	Vector3 _direction;
	RaycastHit _derp;
	bool _collidedf = false;
	bool _collidedb = false;

	//Vector3 _herpaderp;
	
	void Start () {
		_ani = GetComponent<Animator>();
		_boystate = GetComponent<BoyStateManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(_pushing){
			if(Input.GetAxis("Vertical") > _deadZone || Input.GetAxis("Vertical") < -_deadZone){
				_speed = Mathf.Sign(Input.GetAxis("Vertical")); 
			} 
			else{
				_speed = 0;
			}

			//if(Mathf.Abs(_obj.position.y - _objpos.y)>0.05){

				//_boystate.ActivateWalk();
			//}

			if(_obj.rigidbody.SweepTest(_direction* -1, out _derp, 0.1f)){
				if(!_collidedf){
					if(_speed > 0){
						_blockedForward = true;
						_speed = 0;
						_collidedf = true;
					}
					else if(_speed < 0){
						_blockedBackwards = true;
					}
					_speed = 0;
				}
				else if(_collidedf){
					if(_blockedForward){
						if(_speed>0){
							_speed = 0;
						}
						else if(_speed<0){
							_blockedForward = false;
						}
					}
				}
			}
			else if(_obj.rigidbody.SweepTest(_direction, out _derp, 0.1f)){
				if(!_collidedb){
					if(_speed < 0){
						_blockedBackwards = true;
						_speed = 0;
						_collidedb = true;
					}
				}
				else if(_collidedb){
					if(_blockedBackwards){
						if(_speed<0){
							_speed = 0;
						}
						else if(_speed>0){
							_blockedBackwards = false;
						}
					}
				}
				_collidedb = true;
				Debug.Log("THIS MOTHAFUCKA COLLIDED");
			}
			else{
				_collidedb = false;
				_collidedf = false;
			}

			if(_speed == 0){
				transform.position = _position;
			}

			_ani.SetFloat("Speed", _speed);
			_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy,transform.position.z) + _distance*_direction*-1);
			//Debug.Log("Collided is "+_collided);
	
			_ani.SetFloat("Speed", _speed);
			if(_speed == 0){
				transform.position = _position;
			}
			transform.position = new Vector3(_position.x,transform.position.y,transform.position.z);
			transform.forward = -_direction;
			_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy,transform.position.z) + _distance*_direction*-1);
			_position = transform.position;
		}
	}

	public void Activate(bool isActivated, Transform _object, Vector3 direction){
		if(isActivated){
			_obj = _object;
			_objpos = _obj.position;
			_objposy = _obj.position.y;

			Vector3 temp = direction*-1;
			float angle = Vector3.Angle(temp, transform.forward);
			transform.forward = temp;
			Vector3 _objdir = _obj.TransformDirection(temp);
			float _objside;
			if(Mathf.Abs(_objdir.x) > Mathf.Abs(_objdir.z)){
				_objside = (_obj.collider as BoxCollider).size.x;
			}
			else{
				_objside = (_obj.collider as BoxCollider).size.z;
			}
			Vector3 temppos = _obj.position;
			_distance = ((_objside/2) + _offset);
			Vector3 tempDir = _direction;
			_direction = direction;
			if(tempDir != _direction){
				_blockedForward = false;
				_blockedBackwards = false;
			}
			transform.position = new Vector3(temppos.x,transform.position.y,temppos.z) + _distance*_direction;
			_ani.SetBool("Pushing",true);
			_position = transform.position;
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

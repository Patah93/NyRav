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
//	CubeCollision _cubecol;
	BoyStateManager _boystate; 
	bool _blockedBackwards = false;
	bool _blockedForward = false;
	float _distance;
	Vector3 _direction;
	RaycastHit _derp;
	bool _collided = false;

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
			
		/*	_deltapos = transform.position - _position;
			_deltaobjpos = _obj.position - _objpos;

			/*
			if(_hasMoved && Mathf.Abs(_deltaobjpos.x)< float.Epsilon && Mathf.Abs(_deltapos.z)< float.Epsilon){
				_boystate.ActivateWalk();
			}


			_hasMoved = false;
			*/
		/*	if(Mathf.Abs(_deltapos.x) > 0 || Mathf.Abs(_deltapos.z) >0){
				//_obj.rigidbody.MovePosition(_obj.position + new Vector3(_deltapos.x,0,_deltapos.z));
				//_hasMoved = true;
			}
			
			if(_cubecol.getCollision()){
				if(_speed>0){
					_blockedForward = true;
					//_cubecol.deactivateCollision();
				}
				else if(_speed<0){
					_blockedBackwards = true;
					//_cubecol.deactivateCollision();
				}
				_speed = 0;
				//Vector3 temp = _obj.position;
				_obj.position = _cubecol._lastPos;
				_cubecol.deactivateCollision();
				//transform.position += _obj.position - temp;
				Vector3 _objdir = _obj.TransformDirection(transform.forward);
				float _objside;
				if(Mathf.Abs(_objdir.x) > Mathf.Abs(_objdir.z)){
					_objside = (_obj.collider as BoxCollider).size.x;
				}
				else{
					_objside = (_obj.collider as BoxCollider).size.z;
				}
				_distance = ((_objside/2) + _offset);
				transform.position = new Vector3(_obj.position.x,transform.position.y,_obj.position.z) + _distance*_direction;

			}
			if(_blockedForward){
				if( _speed < 0){
					_blockedForward = false;
				}
				else if(_speed > 0){
					_speed = 0;
					_obj.position = _cubecol._lastPos;
					Vector3 _objdir = _obj.TransformDirection(transform.forward);
					float _objside;
					if(Mathf.Abs(_objdir.x) > Mathf.Abs(_objdir.z)){
						_objside = (_obj.collider as BoxCollider).size.x;
					}
					else{
						_objside = (_obj.collider as BoxCollider).size.z;
					}
					_distance = ((_objside/2) + _offset);
					transform.position = new Vector3(_obj.position.x,transform.position.y,_obj.position.z) + _distance*_direction;
				}
				else{
					_obj.position = _cubecol._lastPos;
					Vector3 _objdir = _obj.TransformDirection(transform.forward);
					float _objside;
					if(Mathf.Abs(_objdir.x) > Mathf.Abs(_objdir.z)){
						_objside = (_obj.collider as BoxCollider).size.x;
					}
					else{
						_objside = (_obj.collider as BoxCollider).size.z;
					}
					_distance = ((_objside/2) + _offset);
					transform.position = new Vector3(_obj.position.x,transform.position.y,_obj.position.z) + _distance*_direction;
				}
			}
			*/
			//if((_blockedForward && _speed>0) || (_blockedBackwards && _speed<0)){
				//_speed=0;
			//}

			//if(Mathf.Abs(_obj.position.y - _objpos.y)>0.05){

				//_boystate.ActivateWalk();
			//}
			/*
			if(_blockedBackwards){
				if( _speed > 0){
					_blockedBackwards = false;
				}
				else if(_speed < 0){
					_obj.position = _cubecol._lastPos;
					_boystate.ActivateWalk();
					_blockedForward = false;
					_blockedBackwards = false;
					Vector3 _objdir = _obj.TransformDirection(transform.forward);
					float _objside;
					if(Mathf.Abs(_objdir.x) > Mathf.Abs(_objdir.z)){
						_objside = (_obj.collider as BoxCollider).size.x;
					}
					else{
						_objside = (_obj.collider as BoxCollider).size.z;
					}
					_distance = ((_objside/2) + _offset);
					transform.position = new Vector3(_obj.position.x,transform.position.y,_obj.position.z) + _distance*_direction;
				}
				else{
					_obj.position = _cubecol._lastPos;
					Vector3 _objdir = _obj.TransformDirection(transform.forward);
					float _objside;
					if(Mathf.Abs(_objdir.x) > Mathf.Abs(_objdir.z)){
						_objside = (_obj.collider as BoxCollider).size.x;
					}
					else{
						_objside = (_obj.collider as BoxCollider).size.z;
					}
					_distance = ((_objside/2) + _offset);
					transform.position = new Vector3(_obj.position.x,transform.position.y,_obj.position.z) + _distance*_direction;
				}
			}*/
			//if(_speed != 0){
			if(!_obj.rigidbody.SweepTest(-_direction*Mathf.Sign(_speed), out _derp, 0.05f)){
				_collided = false;
				Debug.Log("Skriv inte ut detta plz");
				_position = transform.position;
			}
			else{
			//	_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy,transform.position.z) + _distance*_direction*-1);
				if(!_collided){
					if(_speed > 0){
						_blockedForward = true;
					}
					else if(_speed < 0){
						_blockedBackwards = true;
					}
					_speed = 0;
				}
				if(_collided){
					if(_blockedForward){
						if(_speed>0){
							_speed = 0;
						}
						else if(_speed<0){
							_blockedForward = false;
							//_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy,transform.position.z) + _distance*_direction*-1);
						}
					}
					else if(_blockedBackwards){
						if(_speed<0){
							_speed = 0;
						}
						else if(_speed>0){
							_blockedBackwards = false;
							//_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy,transform.position.z) + _distance*_direction*-1);
						}
					}
				}
				_collided = true;
				Debug.Log("THIS MOTHAFUCKA COLLIDED");
			}

				//_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy,transform.position.z) + _distance*_direction*-1);
				//_obj.rigidbody.MovePosition(new Vector3(_ani.bodyPosition.x,_objposy,_ani.bodyPosition.z) + _distance*_direction*-1);
				//_herpaderp = _ani.bodyPosition;
			//}else{
				//_ani.bodyPosition = _herpaderp;
			//}

			if(_speed == 0){
				transform.position = _position;
			}

			_ani.SetFloat("Speed", _speed);
			_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy,transform.position.z) + _distance*_direction*-1);
			Debug.Log("Collided is "+_collided);
			//_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy,transform.position.z) + _distance*_direction*-1);

			transform.forward = _direction * -1;
			transform.localPosition = new Vector3(_boyposx, transform.localPosition.y, transform.localPosition.z);
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
			//_cubecol = _obj.GetComponent<CubeCollision>();

			_boyposx = transform.localPosition.x;
		}
		else{
			_ani.SetBool("Pushing",false);
			_ani.SetFloat("Speed",0);
			_obj.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			//_obj.position = _cubecol._lastPos;
			//_blockedForward = false;
			//_blockedBackwards = false;
//			_cubecol = null;
		}
		_pushing = isActivated;
	}

	public bool getActivate(){
		return _pushing;
	}
}

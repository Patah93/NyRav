using UnityEngine;
using System.Collections;

public class PushAndPull : MonoBehaviour {
	
	Vector3 _position;
	//Vector3 _deltapos;
	//Vector3 _deltaobjpos;
	//Vector3 _objpos;
	Transform _obj;
	float _objposy;
	bool _pushing;
	public float _deadZone = 0.2f;
	public float _maxSpeed = 0.1f;
	public float _lerpTime = 0.06f;
	public float _offset = 1;
	float _speed;
	Animator _ani;
	CubeCollision _cubecol;
	BoyStateManager _boystate; 
	bool _blockedBackwards = false;
	bool _blockedForward = false;
	float _distance;
	Vector3 _direction;
	// Use this for initialization
	
	bool _hasMoved = false;
	
	void Start () {

		_ani = GetComponent<Animator>();
		_boystate = GetComponent<BoyStateManager>();
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		if(_pushing){
			if(Input.GetAxis("Vertical") > _deadZone || Input.GetAxis("Vertical") < -_deadZone){
				_speed = Mathf.Sign(Input.GetAxis("Vertical")); 
			} 
			else{
				_speed = 0;
			}

			if(_cubecol.getCollision()){
				if(_speed>0){
					_blockedForward = true;
					_cubecol.deactivateCollision();
				}
				else if(_speed<0){
					_blockedBackwards = true;
					_cubecol.deactivateCollision();
				}
			}

			if(_blockedForward){
				if( _speed < 0){
					_blockedForward = false;
				}
				else if(_speed > 0){
					_speed = 0;
				}
			}
			if(_blockedBackwards){
				if( _speed > 0){
					_blockedBackwards = false;
				}
				else if(_speed < 0){
					//_speed = 0;
					_boystate.ActivateWalk();
					_blockedForward = false;
					_blockedBackwards = false;
				}
			}
			/*
			if(_hasMoved && Mathf.Abs(_deltaobjpos.x)< float.Epsilon && Mathf.Abs(_deltapos.z)< float.Epsilon){
				_boystate.ActivateWalk();
			}


			_hasMoved = false;
			*/
			/*if(_cubecol.getCollision()){
				if(_speed>0){
					_blockedForward = true;
				}
				else if(_speed<0){
					_blockedBackwards = true;
				}
			}

			if(_blockedForward && _speed<0){
				_blockedForward = false;
			}

			if(_blockedBackwards && _speed>0){
				_blockedBackwards=false;
			}

			//if((_blockedForward && _speed>0) || (_blockedBackwards && _speed<0)){
				//_speed=0;
			//}
			*/

			_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy,transform.position.z) + _distance*_direction*-1);
			_ani.SetFloat("Speed", _speed);
			//_objpos = _obj.position;
			_position = new Vector3(transform.position.x, 0, transform.position.z);
		}
	}
	
	public void Activate(bool isActivated, Transform _object, Vector3 direction){
		if(isActivated){
			
			_obj = _object;
			_objposy = _obj.position.y;
			//_objpos = _obj.position;
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
			_direction = direction;
			transform.position = new Vector3(temppos.x,transform.position.y,temppos.z) + _distance*_direction;
		//	_obj.rigidbody.constraints = RigidbodyConstraints.None;
		//	_obj.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			_position = new Vector3(transform.position.x, 0, transform.position.z);
			//	Physics.IgnoreCollision(transform.collider,_obj.collider,true);
			//
			//Physics.IgnoreCollision(GameObject.Find("GroundplaneLower").collider, _obj.collider,true);
			_ani.SetBool("Pushing",true);

			_cubecol = _obj.GetComponent<CubeCollision>();
		}
		else{
			_ani.SetBool("Pushing",false);
			_ani.SetFloat("Speed",0);
			_obj.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			_blockedForward = false;
			_blockedBackwards = false;
			//Physics.IgnoreCollision(transform.collider,_obj.collider,false);

			_cubecol = null;
		}
		_pushing = isActivated;
	}
	
	public bool getActivate(){
		return _pushing;
	}
}

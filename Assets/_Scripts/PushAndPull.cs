using UnityEngine;
using System.Collections;

public class PushAndPull : MonoBehaviour {
	
	public float _deadZone = 0.2f;

	Vector3 _position;
	Transform _obj;
	float _objposy;
	bool _pushing;
	float _speed;
	Animator _ani;
	BoyStateManager _boystate; 
	CharacterController _charContr;
	bool _blockedBackwards = false;
	bool _blockedForward = false;
	float _distance;
	Vector3 _direction;
	RaycastHit _derp;
	bool _collidedf = false;
	bool _collidedb = false;
	bool _sideZ;

	
	void Start () {
		_ani = GetComponent<Animator>();
		_boystate = GetComponent<BoyStateManager>();
		_charContr = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(_pushing){
			if(Input.GetAxis("Vertical") > _deadZone || Input.GetAxis("Vertical") < -_deadZone){ //Joystick or WASD input
				_speed = Mathf.Sign(Input.GetAxis("Vertical")); 
			} 
			else{
				_speed = 0;
			}

			if(_obj.rigidbody.SweepTest(-_direction, out _derp, 0.1f)){ //Box collided forward
				if(!_collidedf){
					if(_speed > 0){
						_blockedForward = true;
						_speed = 0;
					}
				}
				else if(_collidedf){
					if(_speed>0){
						_speed = 0;
					}
				}
				_collidedf = true;
			}
			else if(_obj.rigidbody.SweepTest(_direction, out _derp, 0.1f)){ //Box collided backwards
				if(!_derp.collider.CompareTag("Player")){
					if(!_collidedb){
						if(_speed < 0){
							_blockedBackwards = true;
							_speed = 0;
						}
					}
					else if(_collidedb){
						if(_speed<0){
							_speed = 0;
						}
					}
					_collidedb = true;
				}
			}
			else{	//Box didn't collide with anything
				_collidedf = false;
				_collidedb = false;
			}

			_ani.SetFloat("Speed", _speed);		
			if(_speed == 0){					//Prevents box from gliding through walls because of animations
				transform.position = _position;
			}
			
			if(_sideZ){ 
				transform.position = new Vector3(_position.x,transform.position.y,transform.position.z);
			}
			else if(!_sideZ){
				transform.position = new Vector3(transform.position.x,transform.position.y,_position.z);
			}

			transform.forward = -_direction;	//Prevents character from rotating or moving sideways
			_position = transform.position;		//
			_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy + 0.5f,transform.position.z) + _distance*_direction*-1); //Moves box twice to make gravity work
			if(!_obj.rigidbody.SweepTest(Vector3.down,out _derp,0.55f)){																	//
				_boystate.enterWalkMode();																									//		
			}																																//
			_obj.rigidbody.MovePosition(new Vector3(transform.position.x,_objposy,transform.position.z) + _distance*_direction*-1);			//
			
			if(!_charContr.isGrounded){			//Gravity on player
				_boystate.enterWalkMode();
			}
		}
	}
	
	public void Activate(bool isActivated, Transform _object, Vector3 direction, bool sideZ, float distance){	
		if(isActivated){	//Activates push
			_obj = _object;
			_objposy = _obj.position.y;
			_distance = distance;
			Vector3 tempDir = _direction;
			_direction = direction;
			if(tempDir != _direction){
				_blockedForward = false;
				_blockedBackwards = false;
			}
			_position = transform.position;
			_sideZ = sideZ;
		}
		else{		//Deactivates push
			_ani.SetFloat("Speed",0);
			_collidedf = false;
			_collidedb = false;
		}
		_pushing = isActivated;
	}
}

using UnityEngine;
using System.Collections;

public class BoyStateManager : MonoBehaviour {
	
	public float _rayXOffset = 0.25f;
	public float _rayYOffset = 0.6f;
	public float _raylength = 3;
	public float _pushOffset = 1;
	public Rect _pos;

	RaycastHit _rayHit;
	Vector3 ray1;
	Vector3 ray2;
	PushAndPull _push;
	AnimationMan _walk;
	JumpingMan _jump;
	WalkToPushPos _wtpp;
	bool _drawInteract = false;
	string _text = "Press E to push";
	Animator _ani;
	bool _leavePush = false;
	bool _enterPush = false;
	Transform _obj;
	bool _sideZ;
	float _distance;
	Vector3 _direction;
	Vector3 _dudepos;
	bool _cooldown = false;
	bool _pathfinding = false;
	
	
	// Use this for initialization
	void Start () {
		
		_ani = gameObject.GetComponent<Animator> ();
		_push = gameObject.GetComponent<PushAndPull>();
		_walk = gameObject.GetComponent<AnimationMan>();
		_jump = gameObject.GetComponent<JumpingMan> ();
		_wtpp = gameObject.GetComponent<WalkToPushPos>();
	}
	
	// Update is called once per frame
	void Update () {

		if(_pathfinding){
			if(_wtpp.hasFinished()){
				_ani.SetFloat("Speed",0);
				_ani.SetBool("Pushing",true);																																		
				_cooldown = true;	
				_wtpp.enabled = false;
				_pathfinding = false;
				_ani.applyRootMotion = true;
				_enterPush = true;
			}
		}
		else{
			ray1 = transform.position + transform.right * _rayXOffset;
			ray1 = new Vector3(ray1.x,transform.position.y + _rayYOffset,ray1.z);
			
			ray2 = transform.position - transform.right * _rayXOffset;
			ray2 = new Vector3(ray2.x,transform.position.y + _rayYOffset,ray2.z);
			
			if(_walk.enabled && _leavePush == false){ // If we currently are in walkmode
				if(Physics.Raycast(ray1, transform.forward,out _rayHit,_raylength) || Physics.Raycast(ray2, transform.forward,out _rayHit, _raylength)){ //If we collided with something
					//Debug.DrawRay(ray1,transform.forward,Color.red,_raylength,true);
					//Debug.DrawRay(ray2,transform.forward,Color.red,_raylength,true);
					if(_rayHit.collider.transform.tag == "Interactive"&& !_jump.isJumping()){ //If that object is Interactive
						_drawInteract = true;
						if(Input.GetButtonDown("Interact")){	//Enter pushmode	
							enterPushMode();
							_pathfinding = true;
						}
					}
				}
				else{
					_drawInteract = false;
				}
			}
			else if(_push.enabled && _enterPush == false){ 	//If we currently are in push mode
				if(Input.GetButtonDown("Interact")){		//Enter walk mode	
					enterWalkMode();
				}
			}
			
			if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Push/Pull Idle") && _enterPush){	//Makes sure enterpush animation finishes before activating push
				transform.position = _dudepos;
				_cooldown = false;
				_enterPush = false;
				float temp = Mathf.Abs((transform.position - _dudepos).magnitude); 
				_distance = _distance - temp;
				if(_sideZ){
					transform.position = new Vector3(_dudepos.x,transform.position.y,transform.position.z);
				}
				if(!_sideZ){
					transform.position = new Vector3(transform.position.x,transform.position.y,_dudepos.z);
				}
				_push.enabled = true;
				_push.Activate(true, _obj,_direction*-1,_sideZ,_distance);
			}

			else if(!_ani.GetCurrentAnimatorStateInfo(0).IsName("Push/Pull Prepare") && _cooldown){ //Prevents player from gliding through box with animation
				transform.position = _dudepos;
			}
			
			if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _leavePush){	//Makes sure leavepush animation finishes before activating walk
				Physics.IgnoreCollision(transform.collider,_obj.collider,false);
				_walk.enabled = true;
				_leavePush = false;
				_obj = null;
			}
		}
	}
	
	void OnGUI(){
		if(_drawInteract){
			GUI.Label(_pos,_text,GUIStyle.none);  //Types out interact message on screen
		}
	}

	void enterPushMode(){
		_drawInteract = false;		//Disable all other modes
		_walk.enabled = false;		//
		_ani.SetFloat("Speed",0);	//
		_jump.enabled = false;		//
		
		_obj = _rayHit.collider.transform;				
		Physics.IgnoreCollision(transform.collider,_obj.collider,true);
		_direction = _rayHit.normal*-1;										//Calculates rotation for snap
		//transform.forward = _direction;									  //
		Vector3 _objdir = _obj.TransformDirection(_direction);				//
		float _objside;														//
		Vector3 temppos = _obj.position;									//

		if(_obj.eulerAngles.y < 45 || _obj.eulerAngles.y > 315 || _obj.eulerAngles.y > 135 && _obj.eulerAngles.y < 225){ 
			if(Mathf.Abs(_objdir.x) > Mathf.Abs(_objdir.z)){
					_objside = (_obj.collider as BoxCollider).bounds.size.x;
					_sideZ = false;
			}
			else{
				_objside = (_obj.collider as BoxCollider).bounds.size.z;
				_sideZ = true;
			}
		}
		else{
			if(Mathf.Abs(_objdir.x) > Mathf.Abs(_objdir.z)){
				_objside = (_obj.collider as BoxCollider).bounds.size.z;
				_sideZ = true;
			}
			else{
				_objside = (_obj.collider as BoxCollider).bounds.size.x;
				_sideZ = false;
			}
		}
			
		_distance = ((_objside/2) + _pushOffset);													//Calculates position for snap
		_dudepos = new Vector3(temppos.x,transform.position.y,temppos.z) + _distance*_direction*-1;	//
		//transform.position = _dudepos;	
		_ani.SetFloat("Speed",0.26f);
		_ani.applyRootMotion = false;

		_wtpp.enabled = true;
		Debug.Log("Objside is "+_objside);
		_wtpp.setDestination(transform.position,_dudepos,_direction);
	}
	
	public void enterWalkMode(){
		_ani.SetBool ("Pushing", false);
		_push.Activate(false, null,Vector3.zero,false,_distance);
		_push.enabled = false;
		_jump.enabled = true;
		_leavePush = true;
	}
}

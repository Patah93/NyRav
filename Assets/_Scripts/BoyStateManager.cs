using UnityEngine;
using System.Collections;

public class BoyStateManager : MonoBehaviour {

	public float _rayXOffset = 1;
	public float _rayYOffset = 4;
	public float _raylength = 2;
	public Rect _pos;
	RaycastHit _rayHit;
	Vector3 ray1;
	Vector3 ray2;
	PushAndPull _push;
	AnimationMan _walk;
	JumpingMan _jump;
	bool _drawInteract = false;
	string _text = "Press E to push";
	Animator _ani;
	bool _leavePush = false;
	bool _enterPush = false;
	Transform _obj;

	// Use this for initialization
	void Start () {

		_ani = gameObject.GetComponent<Animator> ();
		_push = gameObject.GetComponent<PushAndPull>();
		_walk = gameObject.GetComponent<AnimationMan>();
		_jump = gameObject.GetComponent<JumpingMan> ();
	}
	
	// Update is called once per frame
	void Update () {

		ray1 = transform.position + transform.right * _rayXOffset;
		ray1 = new Vector3(ray1.x,transform.position.y + _rayYOffset,ray1.z);
		
		ray2 = transform.position - transform.right * _rayXOffset;
		ray2 = new Vector3(ray2.x,transform.position.y + _rayYOffset,ray2.z);

		if(_walk.getActivate() && _leavePush == false){
			if(Physics.Raycast(ray1, transform.forward,out _rayHit,_raylength) || Physics.Raycast(ray2, transform.forward,out _rayHit, _raylength)){
			//	print ( "YOU COLLIDED WITH SOMETHING");
				Debug.DrawRay(ray1,transform.forward,Color.red,_raylength,true);
				Debug.DrawRay(ray2,transform.forward,Color.red,_raylength,true);
				if(_rayHit.collider.transform.tag == "Interactive"&& !_jump.isJumping()){
					//print ("YOU CAN INTERRACT WITH THIS");
					_drawInteract = true;
					if(Input.GetButtonDown("Interact")){		//INTERACT-KNAPPEN HÄR
						_obj = _rayHit.collider.transform;
						Physics.IgnoreCollision(transform.collider,_obj.collider,true);
						_enterPush = true;
						_ani.SetBool("Pushing",true);
						_push.enabled = true;
						_push.Activate(true, _rayHit.collider.transform,_rayHit.normal);
						_walk.Activate(false);
						_walk.enabled = false;
						//_walk.Activate = false;
						//print ("YOU ARE IN PUSH MODE! :D");
						_drawInteract = false;
						_jump.disableJump(true);
					}
				}
			}
			else{
				_drawInteract = false;
			}
			Debug.Log (_leavePush); 
		}

		else if(_push.getActivate() && _enterPush == false){
			if(Input.GetButtonDown("Interact")){		
				_ani.SetBool("Pushing",false);
				_push.Activate(false, null,Vector3.zero);
				_walk.enabled = true;
				_walk.Activate(true);
				print ("YOU ARE IN WALK MODE! :D");
				_jump.disableJump(false);
				//transform.collider.enabled = true;
				_leavePush = true;
			}
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Push/Pull Idle") && _enterPush){
			_enterPush = false;
		}

		if(_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _leavePush){
			Physics.IgnoreCollision(transform.collider,_obj.collider,false);
			_leavePush = false;
			_obj = null;
			Debug.Log ("EY DET FUNKAR");
		}
	}

	void OnGUI(){
		if(_drawInteract){
			GUI.Label(_pos,_text,GUIStyle.none);
			//Debug.Log("wrote stuff");
		}
	}

	public void ActivateWalk(){
		_ani.SetBool ("Pushing", false);
		_push.Activate(false, null,Vector3.zero);
		_push.enabled = false;
		_walk.enabled = true;
		_walk.Activate(true);
		//print ("YOU ARE IN WALK MODE! :D");
		_jump.disableJump(false);
		//transform.collider.enabled = true;
	//	Physics.IgnoreCollision(transform.collider,_rayHit.collider,false);
		_leavePush = true;
		//Physics.IgnoreCollision(transform.collider,_rayHit.collider,false);
	}
}

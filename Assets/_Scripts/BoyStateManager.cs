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

	// Use this for initialization
	void Start () {
	
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

		if(_walk.getActivate()){
			if(Physics.Raycast(ray1, transform.forward,out _rayHit,_raylength) || Physics.Raycast(ray2, transform.forward,out _rayHit, _raylength)){
			//	print ( "YOU COLLIDED WITH SOMETHING");
				Debug.DrawRay(ray1,transform.forward,Color.red,_raylength,true);
				Debug.DrawRay(ray2,transform.forward,Color.red,_raylength,true);
				if(_rayHit.collider.transform.tag == "Interactive"&& !_jump.isJumping()){
					//print ("YOU CAN INTERRACT WITH THIS");
					_drawInteract = true;
					if(Input.GetButtonDown("Interact")){		//INTERACT-KNAPPEN HÄR
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
		}

		else if(_push.getActivate()){
			if(Input.GetButtonDown("Interact")){		//TILLFÄLLIG KNAPP, SKA ÄNDRAS SEN
				_push.Activate(false, null,Vector3.zero);
				_walk.enabled = true;
				_walk.Activate(true);
				print ("YOU ARE IN WALK MODE! :D");
				_jump.disableJump(false);
				transform.collider.enabled = true;
			}
		}
	}

	void OnGUI(){
		if(_drawInteract){
			GUI.Label(_pos,_text,GUIStyle.none);
			//Debug.Log("wrote stuff");
		}
	}

	public void ActivateWalk(){
		_push.Activate(false, null,Vector3.zero);
		_push.enabled = false;
		_walk.enabled = true;
		_walk.Activate(true);
		//print ("YOU ARE IN WALK MODE! :D");
		_jump.disableJump(false);
		transform.collider.enabled = true;
	}
}

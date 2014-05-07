using UnityEngine;
using System.Collections;

public class FoxAI : MonoBehaviour {

	public FoxNode _targetNode;
	FoxNode _currentNode;

	ShadowDetection _shadowDetect;

	GameObject _derp;

	bool _pathSafe;

	bool _testing = false;

	bool _fleeing = false;

	public bool _refuseMoveIfLight = false;

	public float CHECK_LIGHT_INTERVAL = 0.625f;

	int _updateTick = 0;

	public int _sleep_updates_shadowDet = 8;

	Vector3 _direction;

	Quaternion _desiredRotation;

	BoxCollider _box;

	public float _fallAcc = 0.01f;

	private Vector3 _fallVec;

	// Use this for initialization
	void Start () {

		_derp = new GameObject ();
		_derp.AddComponent<FoxNode> ();

		_currentNode = _derp.GetComponent<FoxNode> ();
		_currentNode._nextNode = _targetNode;

		_currentNode.transform.position = gameObject.transform.position;
		_targetNode = null;

		_pathSafe = false;

		_shadowDetect = GetComponent<ShadowDetection>();

		_desiredRotation = transform.rotation;

		_box = GetComponent<BoxCollider>();

		_fallVec = Vector3.down * _fallAcc;
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.DrawLine(transform.position + transform.forward * 0.28f + Vector3.up * 0.5f, transform.position + transform.forward * 0.28f + Vector3.up * 0.5f + Vector3.down * 1, Color.red);
		//Debug.DrawLine(transform.position - transform.forward * 0.28f + Vector3.up * 0.5f, transform.position - transform.forward * 0.28f + Vector3.up * 0.5f + Vector3.down * 1, Color.red);
		if (_targetNode != null) {
			if (reachedTarget()) {
				transform.position = new Vector3(_targetNode.transform.position.x, transform.position.y, _targetNode.transform.position.z);
				_fleeing = false;
				_currentNode = _targetNode;
				_targetNode = null;
				_pathSafe = false;
			} else {
				if(!_pathSafe){
					checkPathForShadows();
				}
				if(_pathSafe || !_refuseMoveIfLight){
					move ();
				}
				else{
					/* TODO Hantera "Oh no mr Boy, me no can walk!" */
					_targetNode = null;
				}
			}
		} else {
			if (Input.GetButtonDown ("FoxForward") || Input.GetAxis("FoxCall") > 0){
				if (_currentNode._nextNode != null) {
					_targetNode = _currentNode._nextNode;
				}
			}
			if (Input.GetButtonDown ("FoxBackward") || Input.GetAxis("FoxCall") < 0) {
				if (_currentNode != null) {
					_targetNode = _currentNode._prevNode;
				}
			}


			/* Fall-kod */
			/*
			RaycastHit rayInfoFront, rayInfoBack;

			Vector3 frontFeetRayPoint = transform.position + transform.forward * 0.28f + Vector3.up * 0.5f;
			Vector3 backFeetRayPoint = transform.position - transform.forward * 0.28f + Vector3.up * 0.5f;
			
			bool frontCast = Physics.Raycast(frontFeetRayPoint, Vector3.down, out rayInfoFront, 1.0f);
			bool backCast = Physics.Raycast(backFeetRayPoint, Vector3.down, out rayInfoBack, 1.0f);
			
			if(!(frontCast || backCast)){
				/* TODO HANDLE BOTH FEET IN DAT AIR! *
				//Debug.Log("I'M FLYYING :D");
				_fallVec += Vector3.down * _fallAcc;
				transform.position += _fallVec * Time.deltaTime;
			}else{
				_fallVec = Vector3.down * _fallAcc;
			}

			Physics.Raycast(transform.position + transform.forward * 0.28f + Vector3.up * 0.5f, Vector3.down, out rayInfoFront, 1.0f);
			Vector3 yOffset = new Vector3(0, -(rayInfoFront.distance - 0.5f), 0);
			transform.position += yOffset;
			*/
		}
	}

	void FixedUpdate(){

		/* TODO other points of interest in ShadowDet-Script
			 * 	kolla olika punkter varje updateTick, basera på %
			 *	istället för _updateTick == 0 bajset... 
			 */
		if(_updateTick == 0 && !_testing){

			if(_shadowDetect.isObjectInLight()){
				Debug.Log("DIEDIEDIEDIE POOR FOXIE! >='[");

				if(!_fleeing){
					if(_targetNode == null){
						_targetNode = _currentNode._prevNode;
						_pathSafe = true;
					}else{
						_targetNode = _currentNode;
					}
					_fleeing = true;
				}
			}
		}

		_updateTick++;
		_updateTick %= _sleep_updates_shadowDet;
	}

	bool reachedTarget(){
		/* Magi */
		return (new Vector2(transform.position.x, transform.position.z) - new Vector2(_targetNode.transform.position.x, _targetNode.transform.position.z)).sqrMagnitude < 0.01f;
	}

	void checkPathForShadows(){
		_testing = true;

		Vector3 originalPos = transform.position;
		Quaternion originalRotation = transform.rotation;
		Vector3 lastCheckPos = transform.position - new Vector3(100, 100, 100);
		Vector3 prevPos = originalPos;

		//int count = 0;
		while(!reachedTarget()){
			move ();
			if((transform.position - lastCheckPos).sqrMagnitude > CHECK_LIGHT_INTERVAL){
				if(_shadowDetect.isObjectInLight()){
					_pathSafe = false;
					transform.position = originalPos;
					transform.rotation = originalRotation;
					_desiredRotation = originalRotation;
					_testing = false;
					return;
				}
				lastCheckPos = transform.position;
				//count++;
			}
			else if((transform.position - prevPos).sqrMagnitude < float.Epsilon){ /* BILLIGARE ÄN == 0 ?? */
				_pathSafe = false;
				transform.position = originalPos;
				transform.rotation = originalRotation;
				_desiredRotation = originalRotation;
				_testing = false;
				return;
			}
			prevPos = transform.position;
		}
		
		transform.position = originalPos;
		transform.rotation = originalRotation;
		_desiredRotation = originalRotation;
		//Debug.Log(count + ": lightchecks!"); 

		_pathSafe = true;
		_testing = false;
	}

	void move(){

		//Debug.Log("HERP");

		RaycastHit rayInfoFront, rayInfoBack;

		_direction = new Vector3((_targetNode.transform.position - transform.position).x, 0, (_targetNode.transform.position - transform.position).z);
		_direction.Normalize();

		transform.rotation = Quaternion.LookRotation(_direction);

		Vector3 frontFeetRayPoint = transform.position + transform.forward * 0.28f + Vector3.up * 0.5f;
		Vector3 backFeetRayPoint = transform.position - transform.forward * 0.28f + Vector3.up * 0.5f;

		bool frontCast = Physics.Raycast(frontFeetRayPoint, Vector3.down, out rayInfoFront, 1.0f);
		bool backCast = Physics.Raycast(backFeetRayPoint, Vector3.down, out rayInfoBack, 1.0f);

		if(!(frontCast || backCast)){
				/* TODO HANDLE BOTH FEET IN DAT AIR! */
				//Debug.Log("I'M FLYYING :D");
			/*
			_fallVec += Vector3.down * _fallAcc;
			transform.position += _fallVec * Time.deltaTime;
			*/
		}
		else{

			_fallVec = Vector3.down * _fallAcc;

			//Debug.Log("I'M GROUNDED D:");

			float angle = Vector3.Angle(_direction, (rayInfoFront.point - rayInfoBack.point).normalized);

			if(rayInfoFront.distance > rayInfoBack.distance){
				transform.rotation = Quaternion.Euler(transform.localEulerAngles.x + angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
				//Debug.Log("LUTNING! =D");
			}
			else if(rayInfoFront.distance < rayInfoBack.distance){
				transform.rotation = Quaternion.Euler(transform.localEulerAngles.x - angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
				//Debug.Log("LUTNING! =D");
			}else{
				transform.rotation = Quaternion.Euler(transform.localEulerAngles.x + angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
			}
			/*
			Physics.Raycast(frontFeetRayPoint, Vector3.down, out rayInfoFront, 1.0f);

			Vector3 yOffset = new Vector3(0, -(rayInfoFront.distance - 0.5f), 0);

			transform.position += yOffset + transform.forward * 4 * Time.deltaTime;
			*/

			transform.position += transform.forward * 4 * Time.deltaTime;
		}	

		Physics.Raycast(transform.position + transform.forward * 0.28f + Vector3.up * 0.5f, Vector3.down, out rayInfoFront, 1.0f);
		
		Vector3 yOffset = new Vector3(0, -(rayInfoFront.distance - 0.5f), 0);
		
		transform.position += yOffset;
	}
}

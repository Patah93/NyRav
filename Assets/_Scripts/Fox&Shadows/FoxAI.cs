using UnityEngine;
using System.Collections;

public class FoxAI : MonoBehaviour {

	public FoxNode _targetNode;
	FoxNode _currentNode;

	ShadowDetection _shadowDetect;

	GameObject _derp;

	bool _pathSafe;

	public float CHECK_LIGHT_INTERVAL = 0.625f;

	int _updateTick = 0;

	public int _sleep_updates_shadowDet = 8;

	Vector3 _direction;

	Quaternion _desiredRotation;

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
	}
	
	// Update is called once per frame
	void Update () {

		Debug.DrawLine(transform.position - transform.up * -1 * GetComponent<BoxCollider>().bounds.min.y + transform.forward * 0.8f, transform.position - transform.up * -1 * GetComponent<BoxCollider>().bounds.min.y + transform.forward * 0.8f + Vector3.down * 5.0f, Color.red);

		if (_targetNode != null) {
			if (reachedTarget()) {
				//transform.position = new Vector3(_targetNode.transform.position.x, transform.position.y, _targetNode.transform.position.z);
				_currentNode = _targetNode;
				_targetNode = null;
				_pathSafe = false;
			} else {
				if(!_pathSafe){
					checkPathForShadows();
				}
				if(_pathSafe){
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
		}
	}

	void FixedUpdate(){

		/* TODO STÄMMER DETTA? inte döda räven under testet, mn annars avlid */
		if(_updateTick == 0 && (_pathSafe || _targetNode == null)){

			/* TODO other points of interest in ShadowDet-Script
			 * 	kolla olika punkter varje updateTick, basera på %
			 *	istället för _updateTick == 0 bajset... 
			 */
			if(_shadowDetect.isObjectInLight()){
				Debug.Log("DIEDIEDIEDIE POOR FOXIE! >='[");
			}
		}

		_updateTick++;
		_updateTick %= _sleep_updates_shadowDet;
	}

	bool reachedTarget(){
		/* Magi */
		return (new Vector2(transform.position.x, transform.position.z) - new Vector2(_targetNode.transform.position.x, _targetNode.transform.position.z)).sqrMagnitude < 0.1f;
	}

	void checkPathForShadows(){
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
				return;
			}
			prevPos = transform.position;
		}

		_pathSafe = true;
		transform.position = originalPos;
		transform.rotation = originalRotation;
		_desiredRotation = originalRotation;
		//Debug.Log(count + ": lightchecks!"); 
	}

	void move(){

		RaycastHit rayInfoFront, rayInfoBack;

		_direction = new Vector3((_targetNode.transform.position - transform.position).x, 0, (_targetNode.transform.position - transform.position).z);
		_direction.Normalize();

		if(!Physics.Raycast(transform.position + transform.up + transform.forward*0.8f, Vector3.down, out rayInfoFront, 5.0f)
		   && !Physics.Raycast(transform.position + transform.up + transform.forward*-0.8f, Vector3.down, out rayInfoBack, 5.0f)){
				/* TODO HANDLE BOTH FEET IN DAT AIR! */
		}
		else{

			float angle = Vector3.Angle(_direction, (rayInfoFront.point - rayInfoBack.point).normalized);

			transform.rotation = Quaternion.LookRotation(_direction);

			if(rayInfoFront.point.y > rayInfoBack.point.y){
				transform.rotation = Quaternion.Euler(transform.localEulerAngles.x + angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
			}
			else if(rayInfoFront.point.y < rayInfoBack.point.y){
				transform.rotation = Quaternion.Euler(transform.localEulerAngles.x - angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
			}

			transform.position += transform.forward * 4 * Time.deltaTime;
		}	
	}
}

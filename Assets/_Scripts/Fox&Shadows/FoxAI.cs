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

		Debug.DrawLine(transform.position - new Vector3(0, GetComponent<BoxCollider>().size.y/4.0f * 0.12f, 0) + transform.forward*0.8f, transform.position - new Vector3(0, GetComponent<BoxCollider>().size.y/2.0f, 0) + transform.forward*0.8f + Vector3.down * 5.0f, Color.red);

		if (_targetNode != null) {
			if (reachedTarget()) {
				//transform.position = new Vector3(_targetNode.transform.position.x, transform.position.y, _targetNode.transform.position.z);
				_currentNode = _targetNode;
				_targetNode = null;
				_pathSafe = false;
			} else {
				if(!_pathSafe){
					/* TODO imitate move when calc path */
					checkPathForShadows();
				}
				if(_pathSafe){
					/* TODO MOVE */
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

		RaycastHit rayInfo;

		_direction = new Vector3((_targetNode.transform.position - transform.position).x, 0, (_targetNode.transform.position - transform.position).z);
		_direction.Normalize();

		if(Physics.Raycast(transform.position + transform.up + transform.forward*0.8f, Vector3.down, out rayInfo, 5.0f)){

			//Debug.DrawLine(transform.position + transform.up + transform.forward*0.8f, transform.position + transform.up + transform.forward*0.8f + Vector3.down * 5.0f, Color.red);
			//Debug.DrawRay(rayInfo.point, rayInfo.normal, Color.blue);
			//Debug.DrawLine(transform.position, transform.position + _direction * 10, Color.green);

			float angle = Vector3.Angle(Vector3.up, rayInfo.normal);

			transform.rotation = Quaternion.LookRotation(_direction);

			if(Vector3.Angle(_direction, rayInfo.normal) < 89){

				//transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y-angle, transform.rotation.z);
				_desiredRotation = Quaternion.Euler(transform.localEulerAngles.x+angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
				//Debug.Log("Downhill");
			}
			else if(Vector3.Angle(_direction, rayInfo.normal) > 91){

				//transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y+angle, transform.rotation.z);
				_desiredRotation = Quaternion.Euler(transform.localEulerAngles.x-angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
				//Debug.Log("Uphill");
			}
			else{
				//transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
				//transform.rotation = Quaternion.LookRotation(_direction);
				_desiredRotation = transform.rotation;
				//Debug.Log("Straight ground");
			}

			if(_desiredRotation != transform.rotation){
			//	transform.rotation = _desiredRotation;
				transform.rotation = Quaternion.Lerp(transform.rotation, _desiredRotation, 1.0f);
			}

			//Vector3 tempPos = transform.position;

			//Debug.DrawRay(transform.position, transform.forward, Color.yellow);
			transform.position += transform.forward * 4 * Time.deltaTime;
		}
		else{
			/* TODO FALL DOWN FFS */
			//transform.position += Vector3.down * 10 * Time.deltaTime;
		}
		
	}
}

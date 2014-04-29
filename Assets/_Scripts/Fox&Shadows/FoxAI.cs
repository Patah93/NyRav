using UnityEngine;
using System.Collections;

public class FoxAI : MonoBehaviour {

	public FoxNode _targetNode;
	FoxNode _currentNode;

	ShadowDetection _shadowDetect;

	GameObject _derp;

	bool _pathSafe;

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
	}
	
	// Update is called once per frame
	void Update () {
		
		if (_targetNode != null) {
			if (reachedTarget()) {
				transform.position = _targetNode.transform.position;
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
					transform.position += (_targetNode.transform.position - transform.position).normalized * 10 * Time.deltaTime;
				}
				else{
					_targetNode = null;
				}
			}
		} else {
			if (Input.GetButtonDown ("FoxForward")) {
				if (_currentNode._nextNode != null) {
					_targetNode = _currentNode._nextNode;
				}
			}
			if (Input.GetButtonDown ("FoxBackward")) {
				if (_currentNode != null) {
					_targetNode = _currentNode._prevNode;
				}
			}
		}
	}

	bool reachedTarget(){
		return (transform.position - _targetNode.transform.position).sqrMagnitude < 0.1f;
	}

	void checkPathForShadows(){
		Vector3 originalPos = transform.position;
		while(!reachedTarget()){
			transform.position += (_targetNode.transform.position - transform.position).normalized * 10 * Time.deltaTime;
			if(_shadowDetect.isObjectInLight()){
				_pathSafe = false;
				transform.position = originalPos;
				return;
			}
		}
		_pathSafe = true;
		transform.position = originalPos;
	}
}

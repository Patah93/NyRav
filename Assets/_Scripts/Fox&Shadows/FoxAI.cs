using UnityEngine;
using System.Collections;

public class FoxAI : MonoBehaviour {

	public FoxNode _targetNode;
	FoxNode _currentNode;

	GameObject _derp;

	// Use this for initialization
	void Start () {

		_derp = new GameObject ();
		_derp.AddComponent<FoxNode> ();

		_currentNode = _derp.GetComponent<FoxNode> ();
		_currentNode._nextNode = _targetNode;

		_currentNode.transform.position = gameObject.transform.position;
		_targetNode = null;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (_targetNode != null) {
			if ((transform.position - _targetNode.transform.position).sqrMagnitude < 0.1f) {
				transform.position = _targetNode.transform.position;
				_currentNode = _targetNode;
				_targetNode = null;
			} else {
				/* TODO Move dat foxie */
				transform.position += (_targetNode.transform.position - transform.position).normalized * 10 * Time.deltaTime;
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
}

using UnityEngine;
using System.Collections;

public class FoxNode : MonoBehaviour {

	public FoxNode _nextNode;
	public FoxNode _prevNode;

	// Use this for initialization
	void Start () {
		if (_nextNode != null) {
			_nextNode.setPrevNode (this);
		}

		GetComponent<Renderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void setPrevNode (FoxNode prevNode){
		_prevNode = prevNode;
	}
}

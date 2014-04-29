using UnityEngine;
using System.Collections;

public class FoxAI : MonoBehaviour {

	public FoxNode _startNode;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if((transform.position - _startNode.transform.position).sqrMagnitude < 0.1f){
			_startNode = _startNode._nextNode;
		}else{
			transform.position += (_startNode.transform.position - transform.position).normalized * 0.5f;
		}
	}
}

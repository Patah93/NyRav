using UnityEngine;
using System.Collections;

public class StickyMan : MonoBehaviour {
		
	bool _OnPlatform;
	Collider _Thing;
	private Vector3 tempPlat = Vector3.zero;

	// Use this for initialization
	void Start () {
		_OnPlatform = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(_OnPlatform){
			_Thing.transform.position += (transform.position - tempPlat);


		}

		tempPlat = transform.position;

	}
	
	void OnTriggerEnter(Collider other) {
		_OnPlatform = true;
		_Thing = other;
		//Vector3 tempScale = other.transform.localScale;
		//other.transform.parent = transform;
	}

	void OnTriggerExit(Collider other) {
		_OnPlatform = false;
		//other.transform.parent = null;
	}

}

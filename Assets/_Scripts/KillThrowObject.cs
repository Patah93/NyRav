using UnityEngine;
using System.Collections;

public class KillThrowObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision deadthing){

		if(deadthing.gameObject.tag == "Destructable"){
			Destroy(deadthing.gameObject);
		}
		Destroy (gameObject);
	}
}

using UnityEngine;
using System.Collections;

public class KillThrowObject : MonoBehaviour {

	string _playername = "HajPojken";

	// Use this for initialization
	void Start () {
		Physics.IgnoreCollision (transform.collider, GameObject.FindWithTag("Player").collider, true); 
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

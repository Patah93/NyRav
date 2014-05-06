using UnityEngine;
using System.Collections;

public class ParticleSwitch : TriggerAction {
	
	public float _countDown;

	// Use this for initialization
	void Start () {
		//_countDown = 20;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(particleSystem.loop == false){
			_countDown -= Time.deltaTime;
			if(_countDown <= 0){
				Destroy(gameObject);
			}
		}
	}

	public override void onActive(){
<<<<<<< HEAD
		if (gameObject.name.Equals ("ParticleTo")) { 

=======
		if (gameObject.name == "ParticleTo") { 
>>>>>>> d3cfab251ecc2834a2c8b6366d0b777c059708a4
			gameObject.particleSystem.Play ();

		}

		if(gameObject.name.Equals ("ParticleFrom")) {

			gameObject.particleSystem.loop = false;
<<<<<<< HEAD
=======
		} else if(gameObject.name == "ParticleFrom") {
			gameObject.particleSystem.loop = false;
			GetComponent<Trigger>().enabled = false;
>>>>>>> d3cfab251ecc2834a2c8b6366d0b777c059708a4
			GetComponent<BoxCollider>().enabled = false;
			GetComponent<Trigger>().enabled = false;

		}
	}

	public override void onInactive(){

	}

}

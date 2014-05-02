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
		if (gameObject.name == "ParticleTo") { 
			gameObject.particleSystem.Play ();
			gameObject.particleSystem.loop = false;
		} else if(gameObject.name == "ParticleFrom") {
			gameObject.particleSystem.loop = false;
			GetComponent<Trigger>().enabled = false;
			GetComponent<BoxCollider>().enabled = false;
		}
	}

	public override void onInactive(){

	}

}

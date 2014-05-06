using UnityEngine;
using System.Collections;

public class ParticleSwitch : TriggerAction {
	
	public float _countDown;
	private bool _counterStart;
	private bool _triggered;

	// Use this for initialization
	void Start () {
		//_countDown = 20;
		_counterStart = false;
		_triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(_counterStart == true){
			_countDown -= Time.deltaTime;
			if(_countDown <= 0){
				Destroy(gameObject);
			}
		}
	}

	public override void onActive(){
<<<<<<< HEAD
		if (gameObject.name.Equals ("ParticleTo") && _triggered == false) { 
			_triggered = true;
=======

		if (gameObject.name.Equals ("ParticleTo")) { 
>>>>>>> 0404969fcd98d48da18bf7647dd5e00ccff86ed6
			gameObject.particleSystem.Play ();
			_counterStart = true;

		}

<<<<<<< HEAD
		if(gameObject.name.Equals ("ParticleFrom") && _triggered == false) {
			_triggered = true;
			gameObject.particleSystem.loop = false;
			_counterStart = true;
=======
		if(gameObject.name.Equals ("ParticleFrom")) {

			gameObject.particleSystem.loop = false;
			GetComponent<BoxCollider>().enabled = false;
			GetComponent<Trigger>().enabled = false;
>>>>>>> 0404969fcd98d48da18bf7647dd5e00ccff86ed6

		}
	}

	public override void onInactive(){

	}

}

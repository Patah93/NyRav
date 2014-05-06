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
		if (gameObject.name.Equals ("ParticleTo") && _triggered == false) { 
			_triggered = true;
			gameObject.particleSystem.Play ();
			_counterStart = true;

		}

		if(gameObject.name.Equals ("ParticleFrom") && _triggered == false) {
			_triggered = true;
			gameObject.particleSystem.loop = false;
			_counterStart = true;

		}
	}

	public override void onInactive(){

	}

}

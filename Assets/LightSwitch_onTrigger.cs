using UnityEngine;
using System.Collections;

public class LightSwitch_onTrigger : TriggerAction {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void onActive(){
		gameObject.light.enabled = !gameObject.light.enabled;
		Debug.Log("HERP");
	}
	
	public override void onInactive(){
		gameObject.light.enabled = !gameObject.light.enabled;
		Debug.Log("HERP");
	}
}

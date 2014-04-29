using UnityEngine;
using System.Collections;

public class triggerGroup : TriggerAction {
	
	public int _numberOfTriggers = 5;
	
	private int _triggered = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	public override void onActive(){
		_triggered++;
		if(_triggered == _numberOfTriggers){
			gameObject.GetComponent<MoveOnTrigger>().onActive();
		}	
	}
	
	public override void onInactive(){
		_triggered--;
		if(_triggered == _numberOfTriggers - 1){
			gameObject.GetComponent<MoveOnTrigger>().onInactive();
		}
	}
}
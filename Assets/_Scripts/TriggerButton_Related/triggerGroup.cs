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
			TriggerAction[] triggerActions = gameObject.GetComponents<TriggerAction>();
			if(triggerActions[0] != this){
				triggerActions[0].onActive();
			}
			else{
				triggerActions[1].onActive();
			}
		}	
	}
	
	public override void onInactive(){
		_triggered--;
		if(_triggered == _numberOfTriggers - 1){
			TriggerAction[] triggerActions = gameObject.GetComponents<TriggerAction>();
			for(int i = 0; i < triggerActions.Length; i++){
				if(triggerActions[i] != this){
					if(triggerActions[0] != this){
						triggerActions[0].onInactive();
					}
					else{
						triggerActions[1].onInactive();
					}
				}
			}
		}
	}
}
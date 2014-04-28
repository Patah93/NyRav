using UnityEngine;
using System.Collections;

public abstract class TriggerAction : MonoBehaviour{
	public abstract void onActive();
	public abstract void onInactive();
}

public class Trigger : MonoBehaviour {

	public GameObject[] _actionObj;

	public GameObject[] _triggerableObjects;

	TriggerAction[] _action;

	public bool _activatedByInteractables = true;

	int _numberOfThings = 0;

	// Use this for initialization
	void Start () {
		_action = new TriggerAction[_actionObj.Length];
		for(int i = 0; i < _action.Length; i++){
			TriggerAction[] _tAction = _actionObj[i].GetComponents<TriggerAction>();
			if(_tAction.Length > 1){
				if(_tAction[0].GetType() != typeof(triggerGroup)){
					_action[i] = _tAction[0];
				}
				else{
					_action[i] = _tAction[1];
				}
			}else{
				_action[i] = _tAction[0];
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(_activatedByInteractables && other.tag == "Interactive"){
			if(_numberOfThings == 0){
				for(int i = 0; i < _action.Length; i++){
					_action[i].onActive();
				}
				gameObject.renderer.material.color = Color.red;
			}
			_numberOfThings++;
			return;
		}

		for(int i = 0 ; i < _triggerableObjects.Length; i++){
			if(_triggerableObjects[i] == other.gameObject){
				if(_numberOfThings == 0){
					for(int j = 0; j < _action.Length; j++){
						_action[j].onActive();
					}
					gameObject.renderer.material.color = Color.red;
				}
				_numberOfThings++;
				return;
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(_activatedByInteractables && other.tag == "Interactive"){
			_numberOfThings--;
			if(_numberOfThings <= 0){
				for(int i = 0; i < _action.Length; i++){
					_action[i].onInactive();
				}
				gameObject.renderer.material.color = Color.green;
				return;
			}
		}

		for(int i = 0 ; i < _triggerableObjects.Length; i++){
			if(_triggerableObjects[i] == other.gameObject){
				_numberOfThings--;
				if(_numberOfThings <= 0){
					for(int j = 0; j < _action.Length; j++){
						_action[j].onInactive();
					}
					gameObject.renderer.material.color = Color.green;
					return;
				}
			}
		}
	}
}

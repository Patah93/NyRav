using UnityEngine;
using System.Collections;

public abstract class TriggerAction : MonoBehaviour{
	public abstract void onActive();
	public abstract void onInactive();
}

public class Trigger : MonoBehaviour {

	public GameObject _actionObj;

	public GameObject[] _triggerableObjects;

	TriggerAction _action;

	public bool _activatedByInteractables = true;

	int _numberOfThings = 0;

	// Use this for initialization
	void Start () {
		_action = _actionObj.GetComponent<TriggerAction>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(_activatedByInteractables && other.tag == "Interactive"){
			_action.onActive();
			_numberOfThings++;
			return;
		}

		for(int i = 0 ; i < _triggerableObjects.Length; i++){
			if(_triggerableObjects[i] == other.gameObject){
				_action.onActive();
				_numberOfThings++;
				return;
			}
		}
	}

	void OnTriggerExit(Collider other) {

		if(_activatedByInteractables && other.tag == "Interactive"){
			_numberOfThings--;
			if(_numberOfThings <= 0){
				_action.onInactive();
				return;
			}
		}

		for(int i = 0 ; i < _triggerableObjects.Length; i++){
			if(_triggerableObjects[i] == other.gameObject){
				_numberOfThings--;
				if(_numberOfThings <= 0){
					_action.onInactive();
					return;
				}
			}
		}
	}
}

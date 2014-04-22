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

	// Use this for initialization
	void Start () {
		_action = _actionObj.GetComponent<TriggerAction>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Interactive"){
			_action.onActive();
			return;
		}

		for(int i = 0 ; i < _triggerableObjects.Length; i++){
			if(_triggerableObjects[i] == other.gameObject){
				_action.onActive();
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Interactive"){
			_action.onInactive();
			return;
		}

		for(int i = 0 ; i < _triggerableObjects.Length; i++){
			if(_triggerableObjects[i] == other.gameObject){
				_action.onInactive();
			}
		}
	}
}

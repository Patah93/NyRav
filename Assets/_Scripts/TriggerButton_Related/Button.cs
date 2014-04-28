using UnityEngine;
using System.Collections;


public class Button : MonoBehaviour {
	
	public GameObject[] _actionObj;

	GameObject _boy;
	
	TriggerAction[] _action;

	const float _PRESS_DISTANCE = 4;

	const float _PRESS_ANGLE = 45;

	bool _pressed = false;
	
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

		_boy = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Interact")){
			if((_boy.transform.position - gameObject.transform.position).sqrMagnitude <= _PRESS_DISTANCE){
				if(Vector2.Angle((new Vector2(gameObject.transform.position.x, gameObject.transform.position.z) - new Vector2(_boy.transform.position.x, _boy.transform.position.z)).normalized, new Vector2(_boy.transform.forward.x, _boy.transform.forward.z))  <= _PRESS_ANGLE){
					if(_pressed){
						for(int i = 0; i < _action.Length; i++){
							_action[i].onInactive();
						}
						_pressed = false;
					}else{
						for(int i = 0; i < _action.Length; i++){
							_action[i].onActive();
						}
						_pressed = true;
					}
					Color temp = gameObject.renderer.materials[1].color;
					gameObject.renderer.materials[1].color = gameObject.renderer.materials[0].color;
					gameObject.renderer.materials[0].color = temp;
				}
			}
		}
	}
	/*
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
	*/
}

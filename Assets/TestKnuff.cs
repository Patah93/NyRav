using UnityEngine;
using System.Collections;

public class TestKnuff : MonoBehaviour {

	Vector3 _position;
	Vector3 _deltapos;
	Vector3 _objpos;
	Transform _obj;
	bool _pushing;
	public float _deadZone = 0.2f;
	public float _maxSpeed = 0.1f;
	public float _lerpTime = 0.06f;
	float _speed;
	Animator _ani;
	// Use this for initialization
	void Start () {
		_ani = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame
	void Update () {

		if(_pushing){
			if(Input.GetAxis("Vertical") > _deadZone || Input.GetAxis("Vertical") < -_deadZone){
				//_speed = _maxSpeed * Mathf.Sign(Input.GetAxis("Vertical")); 
				_speed = Mathf.Sign(Input.GetAxis("Vertical")); 
				//Mathf.Lerp(_speed, _maxSpeed * Mathf.Sign(Input.GetAxis("Vertical")), _lerpTime);
			} 
			else{
				//_speed = Mathf.Lerp(_speed, 0f, _lerpTime);
				/*_speed *= 0.8f;
					if(_speed<0.025f && _speed >= 0){
						_speed = 0;
					}
					else if(_speed > -0.025f && _speed <= 0){
						_speed = 0;
					}*/
				_speed = 0;
			}
			_ani.SetFloat("Speed", _speed);

			_position = new Vector3(transform.position.x, 0, transform.position.z);
			_objpos = _obj.position;

			if(Mathf.Abs(_deltapos.x) > 0 || Mathf.Abs(_deltapos.z) >0){
				_obj.position = _objpos + _deltapos;
				Debug.Log (_deltapos);
				//_obj.gameObject.name
			}
			_deltapos = transform.position - _position;
		}
	}

	public void Activate(bool isActivated, Transform _object, float direction){
		if(isActivated){
			_ani.SetBool("Pushing",true);
			_obj = _object;
			_objpos = _obj.position;
		}
		else{
			_ani.SetBool("Pushing",false);
			_ani.SetFloat("Speed",0);
		}
		_pushing = isActivated;
	}

	public bool getActivate(){
		return _pushing;
	}
}

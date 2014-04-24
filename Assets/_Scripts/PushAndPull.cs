using UnityEngine;
using System.Collections;

public class PushAndPull : MonoBehaviour {

	public float _deadZone = 0.2f;
	public float _maxSpeed = 0.1f;
	public float _lerpTime = 0.06f;
	public float _offset = 1;
	float _speed;
	bool _pushMode = false;
	Transform _obj;
	GameObject _collider;
	bool _blockedForward, _blockedBackward;
	Animator _ani;
	float _pos;


	// Use this for initialization
	void Start () {
		_ani = GetComponent<Animator>();
		_blockedForward = false; 
		_blockedBackward = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(_pushMode){
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
		/*	if(_boxcol.isCollided() && _speed > 0){
				Debug.Log("Ey kan du reagera nån gång plz");
				_blockedForward = true;
			}
*/
			if((_speed > 0 && _blockedForward) || (_speed < 0 && _blockedBackward)){
				_speed = 0;
			}

			if(_blockedForward && _speed < 0){
				_blockedForward = false;
			}
			else if(_blockedBackward && _speed > 0){
				_blockedBackward = false;
			}
			//transform.position += transform.forward * _speed * (gameObject.rigidbody.mass/_obj.rigidbody.mass);
			_ani.SetFloat("Speed", _speed);
		}
	}


	public void Activate(bool isActivated, Transform _object, Vector3 direction){
		if(!isActivated){
			_ani.SetBool("Pushing",false);
			_ani.SetFloat("Speed",0);
			_obj.parent = null;
		//	_obj.gameObject.AddComponent<BoxCollider>();
	//		_obj.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		//	DestroyObject(_collider);

	//		transform.rigidbody.constraints = RigidbodyConstraints.None;
	//		transform.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			transform.collider.enabled = false;

			_blockedForward = false; 
			_blockedBackward = false;
			transform.position = new Vector3(transform.position.x,_pos,transform.position.z);
			transform.rigidbody.velocity = Vector3.zero;
			//transform.rigidbody.useGravity = false;

		}
		else{
			_pos = transform.position.y;
			_obj = _object;
			Debug.Log(direction);
			Vector3 temp = direction*-1;
			float angle = Vector3.Angle(temp, transform.forward);
			transform.forward = temp;
			Vector3 temppos = _obj.position;
			transform.position = new Vector3(temppos.x,transform.position.y,temppos.z) + ((_obj.localScale.x/2) + _offset)*direction;
			_ani.SetBool("Pushing",true);
			_obj.parent = transform;
			//_boxcol = _obj.GetComponent<BoxCollision>();

			/* Create new object, set box collider from _obj 
   			 * & remove box collider from _obj
			 */
			/*_collider = new GameObject();
			_collider.transform.position = _obj.transform.position;
			_collider.transform.rotation = _obj.transform.rotation;
			_collider.transform.localScale = _obj.transform.lossyScale;

			_collider.transform.parent = transform;
			
			Destroy(_obj.GetComponent<BoxCollider>());
			_collider.AddComponent<BoxCollider>();
*/
		/*	Vector3 _tempDir = transform.TransformDirection(transform.right);
			//_tempMass = transform.rigidbody.mass;
			//transform.rigidbody.mass = _object.rigidbody.mass;

			if(_tempDir == Vector3.right | _tempDir == Vector3.left){
				//Fryser karaktären v 
				transform.rigidbody.constraints = RigidbodyConstraints.FreezePositionX| RigidbodyConstraints.FreezePositionY 
					|RigidbodyConstraints.FreezeRotationZ|RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationY;
			}
			else{
				Debug.Log ("1");
				transform.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY 
				|RigidbodyConstraints.FreezeRotationZ|RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationY;
			}
			if(_obj != null){
				_dir = Mathf.Abs(_obj.eulerAngles.y - direction);
				Debug.Log(_dir);
				if((_dir > 45 && _dir < 135) ||( _dir > 225 && _dir < 315)){
					_obj.rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
				}
				else{
					_obj.rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX; 
				}
			}*/
		}
		_pushMode = isActivated;
	}
	public bool getActivate(){
		return _pushMode;
	}


	void OnCollisionEnter(Collision collision) {
		if(_pushMode){
			if(_speed > 0){
				_blockedForward = true;
			}
			else if(_speed < 0){
				_blockedBackward = true;
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class AnimationMan : MonoBehaviour {

	private Animator _animator;
	Vector2 _cameraRotationForward;
	Vector2 _cameraRotationRight;
	Vector2 _targetRotation;
	Vector2 _characterRotation;
	public float _length;
	float _angle;
	float _clock;
	bool _active = true;

	public Animator Animator {get{return this._animator;} }
	private AnimatorStateInfo stateInfo;
	private ThirdPersonCamera camera;

	//Animation hashes
	int m_LocomotionId = 0;

	public float _lerpTime = 5f;
	public float _lerpThrowTime = 2f;


	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		m_LocomotionId = Animator.StringToHash ("Base Layer.Run");
		camera = Camera.main.GetComponent<ThirdPersonCamera> ();

		if (camera == null)
						Debug.Log ("no camera detected");

		if (_animator == null)
			Debug.LogError ("No animator!");
	}
	
	// Update is called once per frame
	void Update () {

		if(_active){
			stateInfo = _animator.GetCurrentAnimatorStateInfo (0);
			updateCameraRotation();
			joystickConvert ();
			updateCharacterRotation();
			float lerpit = _lerpTime;
			if(Input.GetButtonDown("Fire3"))
				_animator.SetBool("ThrowMode", !_animator.GetBool("ThrowMode"));

			if(!_animator.GetBool("ThrowMode")){
				_length = Mathf.Sqrt(Mathf.Pow (Mathf.Abs(Input.GetAxis("Horizontal")),2) + Mathf.Pow (Mathf.Abs(Input.GetAxis("Vertical")),2));	
			} else
				lerpit = _lerpThrowTime;

			if (camera.camState != ThirdPersonCamera.CamStates.FirstPerston) {

				if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0 && !_animator.GetBool("ThrowMode")){

					_angle = Vector2.Angle (_cameraRotationForward, _targetRotation) * Mathf.Sign(Input.GetAxis ("Horizontal"));

					Quaternion targetRotation = Quaternion.Slerp (transform.rotation, Camera.main.transform.rotation * Quaternion.Euler(0, _angle, 0), Time.deltaTime * lerpit);
					transform.rotation = new Quaternion(transform.rotation.x, targetRotation.y, transform.rotation.z, targetRotation.w);

				}
				else
				{
					_length = Mathf.Lerp(_length, 0, _lerpTime);
				}

				_animator.SetFloat("Speed", _length);
			}
		}

		//if (Input.GetButtonDown ("Jump") && !_jump && !_animator.GetBool("Jump")){
		//
		//	_animator.SetBool ("Jump", true);
		//	_jump = true;
		//	Debug.Log("JUMPING!!");
		//
		//}

	}

	void OnCollisionEnter(){

	} 

	private void updateCameraRotation(){
		_cameraRotationForward = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z).normalized;
		_cameraRotationRight = new Vector2(Camera.main.transform.right.x, Camera.main.transform.right.z).normalized;
	}

	private void joystickConvert(){
		if(_animator.GetBool("ThrowMode"))
			_targetRotation = (Mathf.Clamp(Input.GetAxis("Vertical"),0,1) * _cameraRotationForward) + (Input.GetAxis("Horizontal") * _cameraRotationRight);
		else
			_targetRotation = (Input.GetAxis("Vertical") * _cameraRotationForward) + (Input.GetAxis("Horizontal") * _cameraRotationRight);
	}

	private void updateCharacterRotation(){
		_characterRotation = new Vector2(transform.forward.x, transform.forward.z);
	}

	private void jumpUpdate(){

	}

	public void Activate(bool isActive){
		_active = isActive;
		if(!_active){
			_animator.SetFloat("Speed", 0);
		}
	}
	
	public bool getActivate(){
		return _active;	
	}
	
	public bool IsInLocomotion() {
		return stateInfo.GetHashCode() == m_LocomotionId;
	}
}

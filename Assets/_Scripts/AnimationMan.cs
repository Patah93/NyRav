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
	[Range (1f,5f)]
	public float _lerpTime = 5f;
	[Range (1f,2f)]
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
			stateInfo = _animator.GetCurrentAnimatorStateInfo (0);
			updateCameraRotation();
			joystickConvert ();
			updateCharacterRotation();
			
		//Debug.Log (gameObject.GetComponent<CharacterController>().velocity);
			
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

					Quaternion targetRot = Quaternion.Slerp (transform.rotation, Camera.main.transform.rotation * Quaternion.Euler(0, _angle, 0), Time.deltaTime * lerpit);
					transform.rotation = new Quaternion(transform.rotation.x, targetRot.y, transform.rotation.z, targetRot.w);

				}
				else
				{
					_length = Mathf.Lerp(_length, 0, _lerpTime);

				}
				_angle = Vector2.Angle (_cameraRotationForward, _targetRotation) * Mathf.Sign(Input.GetAxis ("Horizontal"));

			}
			else
			{
				_length = Mathf.Lerp(_length, 0, _lerpTime);
			}

			
			_animator.SetFloat("Speed", _length);

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

	private bool leftStickMoved(){
		return Mathf.Abs (Input.GetAxis ("Horizontal")) > 0 || Mathf.Abs (Input.GetAxis ("Vertical")) > 0;
	}

	public bool IsInLocomotion() {
		return stateInfo.GetHashCode() == m_LocomotionId;
	}
}

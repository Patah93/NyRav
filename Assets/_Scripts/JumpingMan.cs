using UnityEngine;
using System.Collections;

public class JumpingMan : MonoBehaviour {

	private Animator _animator;
	public float _jumpForce = 300;
	public float _offsetX = 0;
	public float _offsetY = 0;
	public float _offsetZ = 0;
	float _clock;
	float _maxTime = 0.5f;
	//float _startPosition;
	bool _jump = false;
	bool _disJump;
	RaycastHit _rayHit;
	

	//FUNGERAR HELT OKEJ MEN BEHÖVER KASTA RAYS FRÅN KANTERNA PÅ GUBBEN

	// Use this for initialization
	void Start () {
		//_animator = GetComponent<Animator>();
		//_startPosition = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Jump")){	//Aktiverar hoppet
			Debug.Log("you pressed jump)");
			if(!_jump && !_disJump){
				//transform.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationZ; //hindrar den från att snörra cp

				/* TODO Plz ta bort desa två superdåliga rader kod, my bad */
				//gameObject.GetComponent<CharacterController>().enabled = false;
				//gameObject.GetComponent<CapsuleCollider>().enabled = true;


				Vector3 temp = rigidbody.velocity;
				transform.rigidbody.velocity = new Vector3(temp.x,0,temp.z);
				transform.rigidbody.AddForce(Vector3.up*_jumpForce);
				Debug.Log("jumping");
				_jump = true;

				//gameObject.GetComponent<CapsuleCollider>().enabled = true;


				//_animator.SetBool("Jump", true);
				_clock = Time.time;

			}
		}


	/*	if(_jump && transform.position.y - _startPosition > _jumpHeight){	//Hoppat så högt den klarar, kör en cooldown
			//Debug.Log("cooldown");
			_cooldown = true;
		}
*/
		if(_jump){
			Vector3 temp = new Vector3(_offsetX,_offsetY,_offsetZ);
			if(Time.time - _clock > _maxTime){

				if(Physics.SphereCast(transform.position, 0.3f + temp.y ,Vector3.down,out _rayHit,0.2f)){	//Nuddat marken och kan hoppa igen
					Debug.DrawRay(transform.position + temp,Vector3.down,Color.blue,1 + temp.y,true);
					Debug.Log("Collided with "+ _rayHit.collider.name);
					if(!_rayHit.collider.name.Equals(this.name)){
					//transform.rigidbody.constraints &= ~ RigidbodyConstraints.FreezeRotationX|~RigidbodyConstraints.FreezeRotationZ;
					Debug.Log ("hit something"); 
					//_startPosition = transform.position.y;
					_jump = false;
					//_animator.SetBool("Jump", false);
					//rigidbody.constraints = ; 



					/* TODO Plz ta bort desa två superdåliga rader kod, my bad */
					//gameObject.GetComponent<CharacterController>().enabled = true;
					//gameObject.GetComponent<CapsuleCollider>().enabled = false;

					}
				}
			}
		}
		//Debug.Log("cooldown is" + _cooldown);
	}

	public bool isJumping(){
		return _jump;
	}

	public void disableJump(bool setJump){
		_disJump = setJump;
	}
}

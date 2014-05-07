using UnityEngine;
using System.Collections;

public class WalkToPushPos : MonoBehaviour {

	Vector3 _destination;
	Quaternion _startRot;
	Vector3 _finishRotV;
	Quaternion _finishRotQ;
	public float _runtospeed = 0.1f;
	public float _rotatetospeed = 0.1f;
	bool _finished = false;
	Vector3 _startPos;
	float _step = 0;
	bool _go = false;
	bool _doneRunning = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_go){

		//	Vector3.Slerp(transform.forward,_startRot,_speed);
			if(_doneRunning){
				transform.rotation = Quaternion.Slerp (transform.rotation,_finishRotQ,_rotatetospeed);
				Debug.Log("Rotating");
				if(Mathf.Abs(Quaternion.Angle(transform.rotation,_finishRotQ))<3){
					_go = false;
					_finished = true;
					_doneRunning = false;
					Debug.Log("And done!");
				}
			}
			else{
				if(Mathf.Abs((transform.position - _destination).magnitude)<0.1f){
					_doneRunning = true;
					//Debug.Log("Ska börja rotera nu");
					float temp = Vector3.Angle(transform.forward,_finishRotV.normalized);
					float rightVecAngle = Vector3.Angle(transform.right,_finishRotV.normalized);
					if(rightVecAngle > 90){
						_finishRotQ = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y - temp,transform.eulerAngles.z);
					}
					else{
						_finishRotQ = Quaternion.Euler(transform.eulerAngles.x,transform.eulerAngles.y + temp,transform.eulerAngles.z);
					}
					Debug.Log("Reached destination");
				}
				else{
					Debug.Log("Running");
					transform.rotation = Quaternion.Slerp(transform.rotation,_startRot,0.1f);
					transform.position = Vector3.Lerp(transform.position,_destination,_runtospeed);

				}
			}
		}

		Debug.Log (Vector3.Distance(_startPos,_destination));
	}

	public void setDestination(Vector3 currentpos,Vector3 destinationpos, Vector3 rotation){
		_destination = destinationpos; 
		_finishRotV = rotation;
		_go = true;
		_finished = false;
		float temp = Vector3.Angle(transform.forward,(_destination - transform.position).normalized); 	//DEN SKA ROTERA HIT SEN INNAN DEN SPRINGER

		float rightVecAngle = Vector3.Angle(transform.right,(_destination - transform.position).normalized); 	
		if(rightVecAngle > 90){
			_startRot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - temp, transform.eulerAngles.z);
		}else{
			_startRot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + temp, transform.eulerAngles.z);
		}
		Debug.Log("Ready");
	}

	public bool hasFinished(){
		return _finished;
		if(_finished){
			_finished = false;
			_go = false;
		}
	}
}

using UnityEngine;
using System.Collections;

public class CubeCollision : MonoBehaviour {

	public string _playername = "Betafab";
	bool _collided = false;
	bool _firstcol= false;
	int _collidedItems = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		if((collision.transform.name != _playername)){
			_collided = true;
		}
		//_collidedItems +=1;
	}
	void OnCollisionExit(Collision collisionInfo) {
		//_collidedItems -=1;
		//if(_collided && _collidedItems <=0){
	//		_collided = false;
		//}
	}

	public bool getCollision(){
		return _collided;
	}
}

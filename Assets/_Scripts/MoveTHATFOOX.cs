using UnityEngine;
using System.Collections;

public class MoveTHATFOOX : MonoBehaviour {

	public GameObject _dest;

	Vector3 _lastDest;

	ShadowDetection _shadowDet;

	NavMeshAgent _theAgentOfDoom;

	Vector3 last_pos;

	// Use this for initialization
	void Start () {
		_shadowDet = gameObject.GetComponent<ShadowDetection>();
		_theAgentOfDoom = gameObject.GetComponent<NavMeshAgent>();
		_theAgentOfDoom.Stop();
		last_pos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(!_dest.transform.position.Equals(_lastDest)){
			_theAgentOfDoom.SetDestination(_dest.transform.position);
			_lastDest.Set (_dest.transform.position.x, _dest.transform.position.y, _dest.transform.position.z);
		}

		gameObject.transform.position = _theAgentOfDoom.nextPosition;

		if(_shadowDet.isObjectInLight()){
			gameObject.transform.position = last_pos;
			_theAgentOfDoom.Stop();
		}
		else{
			last_pos = gameObject.transform.position;
		}

		if(Input.GetKeyDown(KeyCode.C)){
			_theAgentOfDoom.Resume();
		}
	}
}

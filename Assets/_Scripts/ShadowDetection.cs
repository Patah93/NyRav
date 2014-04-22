using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowDetection : MonoBehaviour {

	private Vector3 _sunDirection; //Direction TOWARDS the sun

	private Vector3[] _pointsOfInterest;

	private bool temp_isLighted;

	private int _numberLightedVertices;

	GameObject[] lamps;

	GameObject[] spotLights;

	GameObject[] _shadowCasters;

	// Use this for initialization
	void Start () {
		_sunDirection = GameObject.Find ("Sun").transform.forward * -1;
		updatePointsOfInterest ();

		temp_isLighted = false;

		_numberLightedVertices = 0;

		lamps = GameObject.FindGameObjectsWithTag("Lamp");

		spotLights = GameObject.FindGameObjectsWithTag("SpotLight");

		GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		
		initiateShadowCasters(ref allObjects);

	}
	
	// Update is called once per frame
	void Update () {

		/* TODO If fox (fox-ghost) moved */
			updatePointsOfInterest ();
		/* END */

		/* TODO If fox (fox-ghost) moved OR shadows have changed */
			if (isObjectInLight ()) {
				/* Handle ... */
					// - Stop the actual fox
					// - Kill the fox
					// - etc... (Whatever we decide to do...)

				//if(!temp_isLighted){
					Debug.Log("OH MY GOD THE LIGHT! IT IS SO BRIGHT! D:\n" + _numberLightedVertices + " vertices in the sun! D=");
					temp_isLighted = true;
				//}
			}

		else{
		/* END */

		if(temp_isLighted){
			Debug.Log("Mmm vad SKÖÖN SKUGGA MUMS");
			temp_isLighted = false;
		}
		}

		//gameObject.collider.bounds.IntersectRay(new Ray(gameObject.collider.bounds., _sunDirection
	}

	void updatePointsOfInterest(){

		/* These should be the corners of the kollisionsbox så att sägaah */

		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

		Vector3[] temp  = meshFilter.sharedMesh.vertices;
		//Mesh mesh = meshCollider.GetComponent<MeshFilter>().mesh;

		//_pointsOfInterest = mesh.vertices;

		_pointsOfInterest = new Vector3[Mathf.FloorToInt(((float)temp.Length) / 20f)+1]; 
		int j = 0;
		for(int i = 0; i < temp.Length; i++){

			if(i%20 == 0){
				_pointsOfInterest[j] = gameObject.transform.TransformPoint(temp[i]);
				j++;
			}
		}




		for(int i = 0; i < _pointsOfInterest.Length-1; i++){

			Debug.DrawLine(_pointsOfInterest[i], _pointsOfInterest[i+1]);
		}
		/*
		_pointsOfInterest [0] = new Vector3 (gameObject.collider.bounds.max.x, gameObject.collider.bounds.max.y, gameObject.collider.bounds.max.z);
		_pointsOfInterest [1] = new Vector3 (gameObject.collider.bounds.max.x, gameObject.collider.bounds.max.y, gameObject.collider.bounds.min.z);
		_pointsOfInterest [2] = new Vector3 (gameObject.collider.bounds.max.x, gameObject.collider.bounds.min.y, gameObject.collider.bounds.max.z);
		_pointsOfInterest [3] = new Vector3 (gameObject.collider.bounds.max.x, gameObject.collider.bounds.min.y, gameObject.collider.bounds.min.z);
		_pointsOfInterest [4] = new Vector3 (gameObject.collider.bounds.min.x, gameObject.collider.bounds.max.y, gameObject.collider.bounds.max.z);
		_pointsOfInterest [5] = new Vector3 (gameObject.collider.bounds.min.x, gameObject.collider.bounds.max.y, gameObject.collider.bounds.min.z);
		_pointsOfInterest [6] = new Vector3 (gameObject.collider.bounds.min.x, gameObject.collider.bounds.min.y, gameObject.collider.bounds.max.z);
		_pointsOfInterest [7] = new Vector3 (gameObject.collider.bounds.min.x, gameObject.collider.bounds.min.y, gameObject.collider.bounds.min.z);

		Debug.DrawLine(_pointsOfInterest[0], _pointsOfInterest[1]);
		Debug.DrawLine(_pointsOfInterest[1], _pointsOfInterest[2]);
		Debug.DrawLine(_pointsOfInterest[2], _pointsOfInterest[3]);
		Debug.DrawLine(_pointsOfInterest[3], _pointsOfInterest[4]);
		Debug.DrawLine(_pointsOfInterest[4], _pointsOfInterest[5]);
		Debug.DrawLine(_pointsOfInterest[5], _pointsOfInterest[6]);
		Debug.DrawLine(_pointsOfInterest[6], _pointsOfInterest[7]);
		Debug.DrawLine(_pointsOfInterest[7], _pointsOfInterest[0]);
		*/
	}

	public bool isObjectInLight(){
		/* TODO Maybe no keepe this here */
		updatePointsOfInterest ();

		GameObject[] shadowCasters = getPotentialShadowCasters ();

		bool return_value = false;
		_numberLightedVertices = 0;

		for (int i = 0; i < _pointsOfInterest.Length; i++) {

			/* Solen */
			if(isPointInLight(_pointsOfInterest[i], ref shadowCasters)){
				//return true;
				return_value = true;
				_numberLightedVertices++;
			}

			/* Lampor */
			else if(isPointInLampLight(/*getLamps()*/ lamps, _pointsOfInterest[i], ref shadowCasters)){
				//return true;
				return_value = true;
				_numberLightedVertices++;
			}

			/* SpotLights */
			else if(isPointInSpotLight(/*getLamps()*/ spotLights, _pointsOfInterest[i], ref shadowCasters)){
				//return true;
				return_value = true;
				_numberLightedVertices++;
			}
		}

		//return false;
		return return_value;
	}

	bool isPointInLight(Vector3 point, ref GameObject[] shadowCasters){

		Ray theRay = new Ray(point, _sunDirection);

		for(int i = 0; i < shadowCasters.Length; i++){
			if(shadowCasters[i].collider.bounds.IntersectRay(theRay)){
				return false;
			}
		}

		return true;
	}

	bool isPointInLampLight(GameObject[] lamps, Vector3 point, ref GameObject[] shadowCasters){
		Ray theRay = new Ray();

		if(lamps.Length <= 0){
			return false;
		}

		bool return_value;

		for(int i = 0; i < lamps.Length; i++){

			if((point - lamps[i].transform.position).sqrMagnitude <= lamps[i].light.range *lamps[i].light.range){
				theRay.origin = point;
				theRay.direction = (lamps[i].transform.position - point).normalized; 

				return_value = true;

				for(int j = 0; j < shadowCasters.Length; j++){
					if(shadowCasters[j].collider.bounds.IntersectRay(theRay)){
						return_value = false;
					}
				}

				if(return_value){
					return true;
				}
			}
		}

		return false;
	}

	bool isPointInSpotLight(GameObject[] spotLights, Vector3 point, ref GameObject[] shadowCasters){
		Ray theRay = new Ray();
		
		if(spotLights.Length <= 0){
			return false;
		}
		
		bool return_value;
		
		for(int i = 0; i < spotLights.Length; i++){
			
			if((point - spotLights[i].transform.position).sqrMagnitude <= spotLights[i].light.range * spotLights[i].light.range){
				theRay.origin = point;
				theRay.direction = (spotLights[i].transform.position - point).normalized; 

				if(Vector3.Angle(spotLights[i].transform.forward, (theRay.direction*-1)) <= spotLights[i].light.spotAngle/2.0f){
				
					return_value = true;
					
					for(int j = 0; j < shadowCasters.Length; j++){
						if(shadowCasters[j].collider.bounds.IntersectRay(theRay)){
							return_value = false;
						}
					}
					
					if(return_value){
						return true;
					}

				}
			}
		}
		
		return false;
	}

	GameObject[] getLamps(){
		/* TODO find relevant lamps and yao 
		 * Maybe find relevant lamps using logical volumes in the map,
		 * could be the same as the music-areas
		 */ 

		ArrayList nearbyLamps = new ArrayList();

		for(int i = 0; i < lamps.Length; i++){
			if((lamps[i].transform.position - gameObject.transform.position).sqrMagnitude < lamps[i].light.range * lamps[i].light.range){
				nearbyLamps.Add(lamps[i]);
				Debug.Log("Distance to Lamp: " + (lamps[i].transform.position - gameObject.transform.position).sqrMagnitude
				          + "\nMax Distance: " + lamps[i].light.range * lamps[i].light.range);
			}
		}

		GameObject[] array = new GameObject[nearbyLamps.Count];
		nearbyLamps.CopyTo ( array );

		return array;

		//return nearbyLamps.ToArray() as GameObject[];

	}

	GameObject[] getPotentialShadowCasters(){
		/* TODO Use position difference
		 * and compare with direction of sun
		 * using dot product ?? BARA EN IDE LOL
		 * 
		 * Will minska antalet object att testa 
		 * hemsk dyr raycasting mot
		 */
		
		return _shadowCasters;
	}

	void initiateShadowCasters(ref GameObject[] allObjects){
		
		ArrayList shadowCasters = new ArrayList();

		for(int i = 0; i < allObjects.Length; i++){
			if(allObjects[i].GetComponent(typeof(Renderer)) != null && allObjects[i].renderer.castShadows){
				shadowCasters.Add (allObjects[i]);
			}
		}
		
		_shadowCasters = new GameObject[shadowCasters.Count];

		shadowCasters.CopyTo ( _shadowCasters );
	}

	/*
	GameObject[] FindGameObjectsWithLayer (int layer, GameObject[] objects) {
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < objects.Length; i++) {
			if (objects[i].layer == layer) {
				list.Add(objects[i]);
			}
		}
		if (list.Count == 0) {
			return null;
		}
		return list.ToArray();
	}
	*/
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowDetection : MonoBehaviour {

	private Vector3 _sunDirection; //Direction TOWARDS the sun

	private Vector3 _lastPosition;

	private Vector3[] _pointsOfInterest;

	private Vector3[] _localPointsOfInterest;

	private bool temp_isLighted;

	private int _numberLightedVertices;

	GameObject[] lamps;

	GameObject[] spotLights;

	GameObject[] _shadowCasters;

	// Use this for initialization
	void Start () {
		_sunDirection = GameObject.Find ("Sun").transform.forward * -1;

		temp_isLighted = false;

		_numberLightedVertices = 0;

		lamps = GameObject.FindGameObjectsWithTag("Lamp");

		spotLights = GameObject.FindGameObjectsWithTag("SpotLight");

		GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		
		initiateShadowCasters(ref allObjects);

		initiateLocalPointsOfInterest();
		updatePointsOfInterest ();
	}
	
	// Update is called once per frame
	void Update () {

		updatePointsOfInterest ();

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

	void initiateLocalPointsOfInterest(){
		BoxCollider boxColl = GetComponent<BoxCollider>();
		
		float minX = boxColl.center.x - boxColl.size.x/2.0f;
		float maxX = boxColl.center.x + boxColl.size.x/2.0f;
		float minY = boxColl.center.y - boxColl.size.y/2.0f;
		float maxY = boxColl.center.y + boxColl.size.y/2.0f;
		
		/* These should be the corners of the kollisionsbox så att sägaah */
		
		//MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
		
		//Vector3[] temp  = meshFilter.sharedMesh.vertices;
		
		//_pointsOfInterest = new Vector3[1];
		//_pointsOfInterest[0] = gameObject.transform.TransformPoint(temp[0]);
		//Mesh mesh = meshCollider.GetComponent<MeshFilter>().mesh;
		
		//_pointsOfInterest = mesh.vertices;
		
		//_pointsOfInterest = new Vector3[Mathf.FloorToInt(((float)temp.Length) / 20f)+1]; 
		//int j = 0;
		_localPointsOfInterest = new Vector3[21];
		_pointsOfInterest = new Vector3[21];
		//for(int i = 0; i < 4; i++){
		
		/*if(i%20 == 0){
				_pointsOfInterest[j] = gameObject.transform.TransformPoint(temp[i]);
				j++;
			}*/
		//}
		
		_localPointsOfInterest[0] = new Vector3(minX, minY, boxColl.center.z + boxColl.size.z/2.0f);
		_localPointsOfInterest[1] = new Vector3(minX, maxY, boxColl.center.z + boxColl.size.z/2.0f);
		_localPointsOfInterest[2] = new Vector3(maxX, minY, boxColl.center.z + boxColl.size.z/2.0f);
		_localPointsOfInterest[3] = new Vector3(maxX, maxY, boxColl.center.z + boxColl.size.z/2.0f);
		
		_localPointsOfInterest[4] = new Vector3(minX/2.0f, maxY, boxColl.center.z + boxColl.size.z/2.0f);
		_localPointsOfInterest[5] = new Vector3(maxX/2.0f, maxY, boxColl.center.z + boxColl.size.z/2.0f);
		
		//_localPointsOfInterest[6] = new Vector3(boxColl.center.x, maxY, boxColl.center.z + boxColl.size.z/2.0f);
		
		_localPointsOfInterest[6] = new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z + boxColl.size.z/2.0f);
		//_localPointsOfInterest[8] = new Vector3(minX, maxY/3.0f, boxColl.center.z + boxColl.size.z/2.0f);
		//_localPointsOfInterest[9] = new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z + boxColl.size.z/2.0f);
		_localPointsOfInterest[7] = new Vector3(maxX, maxY/3.0f, boxColl.center.z + boxColl.size.z/2.0f);
		
		_localPointsOfInterest[8] = new Vector3(minX, maxY/3.0f/2.0f, boxColl.center.z + boxColl.size.z/2.0f);
		//_localPointsOfInterest[12] = new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z + boxColl.size.z/2.0f);
		//_localPointsOfInterest[13] = new Vector3(maxX, maxY/3.0f/2.0f, boxColl.center.z + boxColl.size.z/2.0f);
		_localPointsOfInterest[9] = new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z + boxColl.size.z/2.0f);
		
		_localPointsOfInterest[10] = new Vector3(minX, boxColl.center.y, boxColl.center.z + boxColl.size.z/2.0f);
		//_localPointsOfInterest[16] = new Vector3(maxX, boxColl.center.y, boxColl.center.z + boxColl.size.z/2.0f);
		
		_localPointsOfInterest[11] = new Vector3(minX, minY, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[12] = new Vector3(minX, maxY, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[13] = new Vector3(maxX, minY, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[14] = new Vector3(maxX, maxY, boxColl.center.z - boxColl.size.z/2.0f);
		
		//_localPointsOfInterest[21] = new Vector3(minX/2.0f, maxY, boxColl.center.z - boxColl.size.z/2.0f);
		//_localPointsOfInterest[22] = new Vector3(maxX/2.0f, maxY, boxColl.center.z - boxColl.size.z/2.0f);
		
		_localPointsOfInterest[15] = new Vector3(boxColl.center.x, maxY, boxColl.center.z - boxColl.size.z/2.0f);
		
		//_localPointsOfInterest[24] = new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[16] = new Vector3(minX, maxY/3.0f, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[17] = new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z - boxColl.size.z/2.0f);
		//_localPointsOfInterest[27] = new Vector3(maxX, maxY/3.0f, boxColl.center.z - boxColl.size.z/2.0f);
		
		//_localPointsOfInterest[28] = new Vector3(minX, maxY/3.0f/2.0f, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[18] = new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[19] = new Vector3(maxX, maxY/3.0f/2.0f, boxColl.center.z - boxColl.size.z/2.0f);
		//_localPointsOfInterest[31] = new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/2.0f);
		
		//_localPointsOfInterest[32] = new Vector3(minX, boxColl.center.y, boxColl.center.z - boxColl.size.z/2.0f);
		_localPointsOfInterest[20] = new Vector3(maxX, boxColl.center.y, boxColl.center.z - boxColl.size.z/2.0f);
	}

	void updatePointsOfInterest(){
		if(_lastPosition == null || (_lastPosition-transform.position).sqrMagnitude > 0.1f){
			for(int i = 0; i < _pointsOfInterest.Length; i++){
				_pointsOfInterest[i] = gameObject.transform.TransformPoint(_localPointsOfInterest[i]);
			}
			_lastPosition = transform.position;
		}
		/*
		BoxCollider boxColl = GetComponent<BoxCollider>();

		float minX = boxColl.center.x - boxColl.size.x/2.0f;
		float maxX = boxColl.center.x + boxColl.size.x/2.0f;
		float minY = boxColl.center.y - boxColl.size.y/2.0f;
		float maxY = boxColl.center.y + boxColl.size.y/2.0f;
		*/
		/* These should be the corners of the kollisionsbox så att sägaah */

		//MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

		//Vector3[] temp  = meshFilter.sharedMesh.vertices;

		//_pointsOfInterest = new Vector3[1];
		//_pointsOfInterest[0] = gameObject.transform.TransformPoint(temp[0]);
		//Mesh mesh = meshCollider.GetComponent<MeshFilter>().mesh;

		//_pointsOfInterest = mesh.vertices;

		//_pointsOfInterest = new Vector3[Mathf.FloorToInt(((float)temp.Length) / 20f)+1]; 
		//int j = 0;
		//_pointsOfInterest = new Vector3[34];
		//for(int i = 0; i < 4; i++){

			/*if(i%20 == 0){
				_pointsOfInterest[j] = gameObject.transform.TransformPoint(temp[i]);
				j++;
			}*/
		//}
		/*
		_pointsOfInterest[0] = gameObject.transform.TransformPoint(new Vector3(minX, minY, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[1] = gameObject.transform.TransformPoint(new Vector3(minX, maxY, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[2] = gameObject.transform.TransformPoint(new Vector3(maxX, minY, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[3] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY, boxColl.center.z + boxColl.size.z/2.0f));
		
		_pointsOfInterest[4] = gameObject.transform.TransformPoint(new Vector3(minX/2.0f, maxY, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[5] = gameObject.transform.TransformPoint(new Vector3(maxX/2.0f, maxY, boxColl.center.z + boxColl.size.z/2.0f));

		_pointsOfInterest[6] = gameObject.transform.TransformPoint(new Vector3(boxColl.center.x, maxY, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[0] = gameObject.transform.TransformPoint(new Vector3(minX, minY, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[1] = gameObject.transform.TransformPoint(new Vector3(minX, maxY, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[2] = gameObject.transform.TransformPoint(new Vector3(maxX, minY, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[3] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY, boxColl.center.z - boxColl.size.z/2.0f));

		_pointsOfInterest[7] = gameObject.transform.TransformPoint(new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[8] = gameObject.transform.TransformPoint(new Vector3(minX, maxY/3.0f, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[9] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[10] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY/3.0f, boxColl.center.z + boxColl.size.z/2.0f));

		_pointsOfInterest[11] = gameObject.transform.TransformPoint(new Vector3(minX, maxY/3.0f/2.0f, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[12] = gameObject.transform.TransformPoint(new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[13] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY/3.0f/2.0f, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[14] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z + boxColl.size.z/2.0f));

		_pointsOfInterest[15] = gameObject.transform.TransformPoint(new Vector3(minX, boxColl.center.y, boxColl.center.z + boxColl.size.z/2.0f));
		_pointsOfInterest[16] = gameObject.transform.TransformPoint(new Vector3(maxX, boxColl.center.y, boxColl.center.z + boxColl.size.z/2.0f));

		_pointsOfInterest[17] = gameObject.transform.TransformPoint(new Vector3(minX, minY, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[18] = gameObject.transform.TransformPoint(new Vector3(minX, maxY, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[19] = gameObject.transform.TransformPoint(new Vector3(maxX, minY, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[20] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY, boxColl.center.z - boxColl.size.z/2.0f));

		_pointsOfInterest[21] = gameObject.transform.TransformPoint(new Vector3(minX/2.0f, maxY, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[22] = gameObject.transform.TransformPoint(new Vector3(maxX/2.0f, maxY, boxColl.center.z - boxColl.size.z/2.0f));
		
		_pointsOfInterest[23] = gameObject.transform.TransformPoint(new Vector3(boxColl.center.x, maxY, boxColl.center.z - boxColl.size.z/2.0f));
		
		_pointsOfInterest[24] = gameObject.transform.TransformPoint(new Vector3(minX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[25] = gameObject.transform.TransformPoint(new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[26] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY/3.0f*2.0f+maxY/6.0f, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[27] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/2.0f));
		
		_pointsOfInterest[28] = gameObject.transform.TransformPoint(new Vector3(minX, maxY/3.0f/2.0f, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[29] = gameObject.transform.TransformPoint(new Vector3(minX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[30] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY/3.0f/2.0f, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[31] = gameObject.transform.TransformPoint(new Vector3(maxX, maxY/3.0f*2.0f, boxColl.center.z - boxColl.size.z/2.0f));
		
		_pointsOfInterest[32] = gameObject.transform.TransformPoint(new Vector3(minX, boxColl.center.y, boxColl.center.z - boxColl.size.z/2.0f));
		_pointsOfInterest[33] = gameObject.transform.TransformPoint(new Vector3(maxX, boxColl.center.y, boxColl.center.z - boxColl.size.z/2.0f));
*/
		/*
		for(int i = 0; i < _pointsOfInterest.Length-1; i++){
			for(int j = i+1; j < _pointsOfInterest.Length; j++){ 
				Debug.DrawLine(_pointsOfInterest[i], _pointsOfInterest[j]);
			}
		}
		*/
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

		//ArrayList nearbyLamps = new ArrayList();
		List<GameObject> nearbyLamps = new List<GameObject>();


		for(int i = 0; i < lamps.Length; i++){
			if((lamps[i].transform.position - gameObject.transform.position).sqrMagnitude < lamps[i].light.range * lamps[i].light.range){
				nearbyLamps.Add(lamps[i]);
				//Debug.Log("Distance to Lamp: " + (lamps[i].transform.position - gameObject.transform.position).sqrMagnitude
				         // + "\nMax Distance: " + lamps[i].light.range * lamps[i].light.range);
			}
		}

		//GameObject[] array = new GameObject[nearbyLamps.Count];
		//nearbyLamps.CopyTo ( array );

		//return array;

		return nearbyLamps.ToArray();

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
			if(allObjects[i].GetComponent(typeof(Renderer)) != null && allObjects[i].renderer.castShadows && allObjects[i].GetComponent<Collider>() != null){
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

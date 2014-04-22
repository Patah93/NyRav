using UnityEngine;
using System.Collections;

struct CameraPosistion {
	
	private Vector3 posistion;
	private Transform xForm;
	
	public Vector3 Posistion { get { return posistion; } set { posistion = value; } }
	public Transform XForm {get { return xForm; } set { xForm = value;} }
	
	public void Init(string camName, Vector3 pos, Transform transform, Transform parent) {
		posistion = pos;
		xForm = transform;
		xForm.name = camName;
		xForm.parent = parent;
		xForm.localPosition = Vector3.zero;
		xForm.localPosition = posistion;
	}
}

public class ThirdPersonCamera : MonoBehaviour {
	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float maxDistanceUp = 5;
	[SerializeField]
	private float minDistanceUp = 1;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform follow;
	[SerializeField]
	private Vector3 offset = new Vector3(0.0f,1.5f,0.0f);
	[SerializeField]
	private float widescreen = 0.2f;
	[SerializeField]
	private float targetingTime = 0.5f;
	[SerializeField]
	private float firstPersonCamPosThreshold = 1.5f;
	//this is used to check for locomotion ie is the character moving.
	[SerializeField] 
	private AnimationMan referenceToController;
	[SerializeField]
	private float firstPersonLookSpeed = 3.0f;
	[SerializeField]
	private float fpsRotationDegreePerSecond = 120f;
	[SerializeField]
	private Vector2 firstPersonXAxisClamp = new Vector2 (-70.0f, 90.0f);
    [SerializeField]
    private float RightStickRotationSpeed = 100.0f;
	
	
	[SerializeField]
	private float controllerRotationSpeed = 20.0f;
	
	private CameraPosistion firstPersonCamPos;
	
	private Vector3 lookDir;
	private Vector3 targetPosistion;
	private Vector3 curLookDir;
	
	private float xAxisRot;
	private float lookWeight;
	private const float TARGETING_THRESHOLD = 2.01f;
	
	private Transform PlayerXform;
	
	
	public enum CamStates{
		Behind,
		FirstPerston,
		Target,
		Free
	}
	
	public CamStates camState = CamStates.Behind;
	
	
	
	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime = 0.1f;
	[SerializeField]
	private float LookDirDampTime = 0.1f;
	private Vector3 velocityLookDir = Vector3.zero;
	
	
	// Use this for initialization
	void Start () {
		//follow = GameObject.FindWithTag ("CameraFollow").transform;
		PlayerXform = GameObject.FindWithTag ("Player").transform;
		lookDir = PlayerXform.forward;
		curLookDir = PlayerXform.forward;
		referenceToController = GameObject.FindWithTag ("Player").GetComponent<AnimationMan> ();




		firstPersonCamPos = new CameraPosistion ();
		firstPersonCamPos.Init ("First Person Camera",
		                        new Vector3 (0.0f, 1.6f, 0.2f),
		                        new GameObject ().transform,
		                        PlayerXform);
	}
	
	// Update is called once per frame
	void Update () {

		Transform test = GameObject.FindWithTag ("Player").transform;
		PlayerXform = GameObject.FindWithTag ("Player").transform;

		//if (firstPersonCamPos == null)
			//Debug.Log ("error");

		//Debug.Log (rightStickVertical);	
	}
	
	void LateUpdate() {
		Vector3 characterOffset = follow.position + offset;
		Vector3 lookAt = characterOffset;
		
		
		float rightX = Input.GetAxis ("RightStickHorizontal");
		float rightY = Input.GetAxis ("RightStickVertical");
		float leftX = Input.GetAxis ("Horizontal");
		float leftY = Input.GetAxis ("Vertical");
		//Debug.Log (Input.GetButton("enterFPV"));
		//Debug.Log ("X: " + rightX);
		//Debug.Log ("Y: " + rightY);
		if (Input.GetAxis ("Target") > 0.01f) {
			camState = CamStates.Target;		
		} else {
			//First person camstate
			if(Input.GetButton("enterFPV") && camState != CamStates.FirstPerston && camState != CamStates.Free && !referenceToController.IsInLocomotion()) {
				xAxisRot = 0;
				lookWeight = 0f;
				camState = CamStates.FirstPerston;
			}
			if((camState == CamStates.FirstPerston && Input.GetButton("ExitFPV")) ||
			   (camState == CamStates.Target && (Input.GetAxis("Target") <= TARGETING_THRESHOLD))) {
				camState = CamStates.Behind;
				rightX = 0;
			}
		}
		
		//referenceToController.Animator.SetLookAtWeight (lookWeight);
		
		//set camstate
		switch(camState) {
		case CamStates.Behind:
			resetCamera();
              

			//only update if we moved the camers
			//if(referenceToController.Speed > referenceToController.LocomotionThreshold && referenceToController.IsInLocomotion()) {
			//calculate where we want to look based on the posistion on the controll stick. if stick value is negative we want to look left/down otherwise right/up.
			lookDir = Vector3.Lerp(PlayerXform.right * (leftX < 0 ? -1.0f : 1.0f),PlayerXform.forward * (leftY < 0 ? -1.0f : 1.0f),Mathf.Abs(Vector3.Dot (this.transform.forward, PlayerXform.forward)));
			
			//direction vector from camers to player
			curLookDir = Vector3.Normalize(characterOffset - this.transform.position);
			//kill y sicne we are not intrested in y.
			curLookDir.y = 0;
			
			//smoothing it out
			curLookDir = Vector3.SmoothDamp(curLookDir,lookDir, ref velocityLookDir,LookDirDampTime);
			//}


			targetPosistion = characterOffset + PlayerXform.up * distanceUp - Vector3.Normalize(curLookDir) * distanceAway;
			break;
		case CamStates.Target:
			//restting lookdir to players forward vector
			curLookDir = PlayerXform.forward;
			lookDir = PlayerXform.forward;
			
			targetPosistion = characterOffset + follow.up * distanceUp - follow.forward * distanceAway;
			
			
			break;
		case CamStates.FirstPerston:
			//Debug.Log("In person");
			//calc rotation on head
			xAxisRot += (leftY * 0.5f * firstPersonLookSpeed);
			xAxisRot = Mathf.Clamp(xAxisRot,firstPersonXAxisClamp.x, firstPersonXAxisClamp.y);
			firstPersonCamPos.XForm.localRotation = Quaternion.Euler(xAxisRot,0,0);
			
			//impose that rotation to the camera
			Quaternion rotationShift = Quaternion.FromToRotation(this.transform.forward,firstPersonCamPos.XForm.forward);
			this.transform.rotation = rotationShift * this.transform.rotation;
			
			//rotate head up and down
			referenceToController.Animator.SetLookAtPosition(firstPersonCamPos.XForm.position + firstPersonCamPos.XForm.forward);
			lookWeight = Mathf.Lerp(lookWeight,1.0f,Time.deltaTime * firstPersonLookSpeed);
			
			//rotate head l and r
			Vector3 rotationAmount = Vector3.Lerp(Vector3.zero,new Vector3(0f,fpsRotationDegreePerSecond * (leftX < 0f ? -1f : 1f),0f),Mathf.Abs(leftX));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
			referenceToController.transform.rotation = (referenceToController.transform.rotation * deltaRotation);
			
			//set t
			targetPosistion = firstPersonCamPos.XForm.position;
			
			//chose look at target based on distance.
			lookAt = (Vector3.Lerp(this.transform.position + this.transform.forward, lookAt, Vector3.Distance(this.transform.position,firstPersonCamPos.XForm.position)));
			break;
		}

        Transform test = this.transform;



		
		CompenstaForWalls (characterOffset, ref targetPosistion);
		smoothPosistion(this.transform.position, targetPosistion);
       
        //this.transform.RotateAround(follow.transform.position, Vector3.up, rightX * 100 * Time.deltaTime);
		
		transform.LookAt (lookAt);
	}
	
	private void smoothPosistion(Vector3 fromPos,Vector3 toPos) {
        float rightX = Input.GetAxis("RightStickHorizontal");
        float rightY = Input.GetAxis("RightStickVertical");
        float leftX = Input.GetAxis("Horizontal");
        float leftY = Input.GetAxis("Vertical");
        toPos.y += (rightY * RightStickRotationSpeed * Time.deltaTime);

        Transform test = this.transform;

        test.RotateAround(follow.transform.position, Vector3.up, rightX * RightStickRotationSpeed * Time.deltaTime);

        if(rightX >= 0.1 && leftX >= 0.1 && camState == CamStates.Behind || rightX >= 0.1 && leftX == 0 && camState == CamStates.Behind)
            fromPos = Vector3.MoveTowards(fromPos, test.position, 0.1f);
        else if (rightX <= -0.1 && leftX <= -0.1 && camState == CamStates.Behind || rightX <= 0.1 && leftX == 0 && camState == CamStates.Behind)
            fromPos = Vector3.MoveTowards(fromPos, test.position, 0.1f);

        this.transform.position = Vector3.SmoothDamp (fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
        //this.transform.position = new Vector3(this.transform.position.x ,this.transform.position.y,this.transform.position.z);
	}
	
	private void CompenstaForWalls(Vector3 fromObject,ref Vector3 toTarget) {
		RaycastHit wallHit = new RaycastHit ();
		if (Physics.Linecast (fromObject, toTarget, out wallHit)) {
			toTarget = new Vector3(wallHit.point.x,toTarget.y,wallHit.point.z);
		}
	}
	private void resetCamera() {
		lookWeight = Mathf.Lerp(lookWeight,0.0f,Time.deltaTime * firstPersonLookSpeed);
		transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.identity, Time.deltaTime);
	}
}

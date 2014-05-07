using UnityEngine;
using System.Collections;

/*
 * Made by Tobias Tevemark
 * 2014-04-29
 * 
 * There’s a certain amount of craftsmanship involved in making things move around algorithmically.  
 * Getting a camera to slide smoothly into position, gliding on the gossamer wings of math, can be a perilous undertaking.
 * One wrong wobble, one tiny pop, and every pixel on the screen becomes wrong, the needle slides off the record, and the magic spell is broken.
 * - Jeff Farris - Epic Games
 */

public class ThirdPersonCamera : MonoBehaviour {
	
	#region Public variables
	
	//Camera posistion and look at variables
	public Vector3 Offset = Vector3.zero;
	public Transform LookAt;

	[Range (0.0f,5.0f)]
	public float CameraUp = 1.0f;
	[Range (0.0f,5.0f)]
	public float CameraAway = 3.0f;
	[Range (0.0f,5.0f)]
	public float ThorowCameraUp = 1.0f;
	[Range (0.0f,5.0f)]
	public float ThrowCameraAway = 1.0f;
	[Range (0.0f,5.0f)]
	public float ThrowCameraShoulderOffset = 1.0f;

	//Camera max movement delta (Low value to create a moothing effect)
	[Range (0.0f,0.5f)]
	public float camSmoothDampTme = 0.1f;
	
	//Controller Deadzone for rotating the camera. We only want to rotate camera when we move the stick far enough.
	//values from 0 to 1
	[Range (0.0f,1.0f)]
	public float deadZoneX = 0.3f;
	[Range (0.0f,1.0f)]
	public float deadZoneY = 0.3f;
	
	
	//Controller variables for rotation speed etc

	[Range (1.0f,5.0f)]
	public float RotationSpeedX = 2.0f;
	[Range (1.0f,5.0f)]
	public float RotationSpeedY = 2.0f;
	public bool InvertedX = true;
	public bool InvertedY = false;

	
	//Clamping values for Y axis so we can't go to far up or down.
	public Vector2 cameraClampingY = new Vector2(-40.0f, 60.0f);
	
	//Camera State
	public CamStates camState = CamStates.Behind;
	
	//Starting moving behind after set time.
	[Range (0.0f,2.0f)]
	public float moveBehind = 0.5f;
	//What smoothing time do you want for that movement.
	[Range (0.0f,2.0f)]

	public float autoMoveSmooth = 1.0f;
	
	//Reference to the throw scripts to get the higest point in the aiming arc
	private Throw referenceToThrow;
	
	#endregion
	
	#region Private variables

	Vector3 deltaPos;
	Vector3 previousPos;

	//corners of the camera.
	ArrayList cameraCornersInWorldpsace;
	
	//what is our current look direction
	Vector3 currentLookDirection;
	
	//where we want to be looking in the end
	Vector3 lookDirection;
	
	//Target posistion
	private Vector3 targetPosistion;
	
	//TODO : Change to corresponds to correct throw posistion later
	//The first Person Camera Posistion
	private CameraPosistion FirstPersonCameraPosistion;
	
	//The behind Person Camera Posistion
	private CameraPosistion BehindPersonCameraPosistion;
	
	//Reference to the characters transform
	private Transform PlayerXform;
	
	//Reference to out controller scripts on the character.
	private AnimationMan referenceToController;
	
	//TODO : add other controller input values
	//The controller input values
	private float rightX = 0.0f;
	private float rightY = 0.0f;
	private float leftX  = 0.0f;
	private float leftY  = 0.0f;	

	
	//private containers holding the amount of rotation around the character that have been applied from the controller input
	private float rotationAmountY = 0.0f;
	
	//time since last input
	private float deltaLastInput = 0.0f;
	//do we need to start moving?
	private bool startMoving = false;

	public Vector3 followerVelocity;
	Vector3 pastFollowerPosition, pastTargetPosition;

	#endregion
	
	#region Structs
	struct CameraPosistion {
		//Posistion
		private Vector3 posistion;
		//Transform of object
		private Transform xForm;
		
		//getters and setters
		public Vector3 Posistion { get { return posistion; } set { posistion = value; } }
		public Transform XForm { get { return xForm; } set { xForm = value; } }
		
		//Init
		public void Init(string camName, Vector3 pos, Transform transform, Transform parent) {
			posistion = pos;
			xForm = transform;
			xForm.name = camName;
			xForm.parent = parent;
			xForm.localPosition = Vector3.zero;
			xForm.localPosition = posistion;
		}

	}
	#endregion
	
	#region Enums
	public enum CamStates
	{
		Behind,
		FirstPerston,
		Target,
		Free,
		Throw
	}
	#endregion
	
	#region Inits
	//Called even if script component is not enabled
	//best used for references between scripts and Inits
	void Awake() {
		//grabbing the transform from the character.
		PlayerXform = GameObject.FindWithTag("Player").transform;
		
		//Init out look direction to correspond where the character is looking at i.e it's forward vector.
		currentLookDirection = PlayerXform.forward;
		lookDirection = PlayerXform.forward;
		
		//Grabbing the reference to our character controller.
		referenceToController = GameObject.FindWithTag("Player").GetComponent<AnimationMan>();
		
		//grabbing the reference to the throw component.
		referenceToThrow = GameObject.FindWithTag("Player").GetComponent<Throw>();

	}
	
	//Called if script component is enabled
	void Start () {
		
	}
	#endregion
	
	#region Update funtions
	void Update () {

		if (Input.GetButtonDown("Fire3"))
			camState = (camState != CamStates.Throw) ? CamStates.Throw : CamStates.Behind;
		
		//We need to update the players transform so we always have the correct values.
		PlayerXform = GameObject.FindWithTag("Player").transform;
		
		//TODO : add more controller input grabs
		//We need to grab the controller input values
		rightX = Input.GetAxis ("RightStickHorizontal");
		rightY = Input.GetAxis ("RightStickVertical");
		leftX  = Input.GetAxis ("Horizontal");
		leftY  = Input.GetAxis ("Vertical");
		
		//check for inputs so the camera does not auto move if we've not used the right stick
		//TODO Needs to check for all inputs!
		if  (Mathf.Abs(referenceToController._length) >= 0.1 && Mathf.Abs(rightX) == 0.0 && Mathf.Abs(rightY) == 0.0){
			deltaLastInput += Time.deltaTime;
		}
		else
			deltaLastInput = 0;
		
		if (deltaLastInput >= moveBehind)
			startMoving = true;
		else
			startMoving = false;

}
void LateUpdate () {
		//Here we check what camera  state we are actually in.
		switch (camState) {
			//If we are in the default camera state
		case CamStates.Behind:
			
			//adding rotation
			if(Mathf.Abs(rightX) > deadZoneX)
				currentLookDirection = Vector3.RotateTowards(currentLookDirection, this.transform.right, rightX * Time.deltaTime * ((InvertedX == false) ? RotationSpeedX : RotationSpeedX*-1.0f), 0.0f);
			
			//now if we are not directly in front of the character we want to slowly move behind the character.
			//if the conditon for start moving is met.
			if (!(Vector3.Dot(PlayerXform.forward, this.transform.forward) <= -0.8f))
			{
				//We are not 
				if (Vector3.Dot(PlayerXform.forward, this.transform.forward) >= 0.90f )
					startMoving = false;
				
				if(startMoving) {
					currentLookDirection = Vector3.RotateTowards(
						currentLookDirection,
						//we need to check where the camera is posistoned in relation to the character so we know what way the rotation need to be.
						Vector3.Angle(-PlayerXform.right, this.transform.forward) > 90 ? -this.transform.right : this.transform.right,
						Time.deltaTime * autoMoveSmooth, 0.0f
						);
					Debug.Log("Moving!");
				}
			}       
					
			//we need to clamp this value so we don't go over the character.
			if (Mathf.Abs(rightY) >= deadZoneY){
				rotationAmountY += Mathf.Rad2Deg * rightY * Time.deltaTime * RotationSpeedY;
				if (rotationAmountY < cameraClampingY.x){
					rotationAmountY = cameraClampingY.x;
				}else if (rotationAmountY > cameraClampingY.y) {
					rotationAmountY = cameraClampingY.y;
				}
				
				if (rotationAmountY > cameraClampingY.x && rotationAmountY < cameraClampingY.y) 
					currentLookDirection = Vector3.RotateTowards(currentLookDirection, this.transform.up, rightY * Time.deltaTime *((!InvertedY) ? RotationSpeedY : RotationSpeedY*-1.0f), 0.0f);
			}
			
			targetPosistion =
				//moving target pos up according to CameraUp variable 
				(LookAt.position + (Vector3.Normalize(PlayerXform.up) * CameraUp)) -
					//move the target a bit back according to the CameraAway variable
					(Vector3.Normalize(currentLookDirection) * CameraAway);                
			
			//Debug.Log("RightX: " + rightX + " " + "RightY: " + rightY + " " + "Distance: " + Vector3.Distance(this.transform.position, LookAt.position));
			
			
			CompenstaForWalls(LookAt.position, ref targetPosistion);
			//this.transform.position = SuperSmoothLerp(pastFollowerPosition,pastTargetPosition,this.transform.position,Time.deltaTime,10.5f);
			pastFollowerPosition = this.transform.position;
			pastTargetPosition = targetPosistion;
			//smoothPosistion(this.transform.position, targetPosistion);
			this.transform.position = targetPosistion;
			transform.LookAt(LookAt);
			break;
		case CamStates.Throw:
			//find the lookAt posistion
			float pitch = 0.0f;
			//find the top point in throw arc and save location
			
			Vector3 higestpos = referenceToThrow.highestPos;
			//Vector3 higestpos = Vector3.zero;
			//LookAt.position + PlayerXform.forward * 10.0f + PlayerXform.up * 100.0f;
			Debug.Log(higestpos);
			
			//angle between the camera to the target
			pitch = Vector3.Dot(Vector3.Normalize(higestpos - this.transform.position), PlayerXform.transform.forward);
			pitch = Mathf.Acos(pitch) * Mathf.Rad2Deg;
			Debug.Log(pitch);
			
			//set camerea to right pos
			targetPosistion =
				//set it to be at the appropiate pos for over the shoulder.
				LookAt.position + PlayerXform.up * ThorowCameraUp -
					PlayerXform.forward * ThrowCameraAway - PlayerXform.right * ThrowCameraShoulderOffset;
			
			//compensate for ze walls
			CompenstaForWalls(LookAt.position, ref targetPosistion);
			//set the target to be smoothed
			//smoothPosistion(this.transform.position, targetPosistion);
			this.transform.position = targetPosistion;
			//set the new lookAt
			transform.LookAt(higestpos);
			break;
		} 
		
		
	}

	#endregion
	
	#region Private functions
	private void smoothPosistion(Vector3 fromPos, Vector3 toPos) {

		this.transform.position = Vector3.SmoothDamp (fromPos, toPos, ref followerVelocity, camSmoothDampTme);
		deltaPos = fromPos - previousPos;
		previousPos = fromPos;
		Debug.Log (deltaPos);
	}
	
	Vector3 SuperSmoothLerp( Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float time, float speed ) {
		Vector3 f = pastPosition - pastTargetPosition + (targetPosition - pastTargetPosition) / (speed * time);
		return targetPosition - (targetPosition - pastTargetPosition) / (speed*time) + f * Mathf.Exp(-speed*time);
	}
	
	private void CompenstaForWalls(Vector3 fromObject, ref Vector3 toTarget) {
		RaycastHit wallHit = new RaycastHit();
		
		if (Physics.Linecast(fromObject, toTarget, out wallHit)) {
			
			if(Vector3.Magnitude(fromObject - wallHit.point) > 0.5f) {
				if(Physics.Linecast(fromObject + Vector3.up * 1.0f,toTarget,out wallHit)) {
					toTarget = new Vector3(wallHit.point.x, wallHit.point.y, wallHit.point.z) + Vector3.Normalize(fromObject - wallHit.point) *
						(Vector3.Magnitude (Camera.main.ViewportToWorldPoint (
							new Vector3 (0.5f, 0.0f, Camera.main.nearClipPlane)
							) - Camera.main.ViewportToWorldPoint (
							new Vector3 (0.5f, 1.0f, Camera.main.nearClipPlane))) * 1.0f);;
				}
			}else
				toTarget = new Vector3(wallHit.point.x, wallHit.point.y, wallHit.point.z) + Vector3.Normalize(fromObject - wallHit.point) *
					(Vector3.Magnitude (Camera.main.ViewportToWorldPoint (
						new Vector3 (0.5f, 0.0f, Camera.main.nearClipPlane)
						) - Camera.main.ViewportToWorldPoint (
						new Vector3 (0.5f, 1.0f, Camera.main.nearClipPlane))) * 1.5f);
		}
		
		
		
	}
	#endregion
	
	#region Public functions
	#endregion
}


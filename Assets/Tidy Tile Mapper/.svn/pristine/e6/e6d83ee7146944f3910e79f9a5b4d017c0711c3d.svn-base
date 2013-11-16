using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlatformerPlayer : Entity {

	//The speed at which the character walks
	public float walkSpeed;
	//The height to which you will jump... if you so choose to jump
	public float jumpHeight;
	//The speed at which the character falls
	public float gravity;
	//The CharacterController that the character will utilise for movement
	CharacterController cc;
	
	//Our input keys - you'll need to assure you set these up in your project input
	
	//I am officially changing this to use axes and the inbuilt "Jump" button (I was using lowercase)
	/*public string input_Left;
	public string input_Right;
	public string input_Down;
	public string input_Up;*/
	public string input_Jump = "Jump";
	
	//Our input states
	bool isJumping = false;
	bool left = false;
	bool right = false;
	bool down = false;
	bool up = false;
	bool isFalling = false;
	
	//This helps us return jump intervals instead of absolute values
	float lastRawJump = 0.0f;
	//The current time for which we're been jumping
	float currentJumpTime = 0.0f;
	
	//We'll go ahead and memorise these direction vectors
	//we can test against them later to check what direction we are facing
	static Vector3 leftDirection = new Vector3(1.0f,0.0f,0.0f);
	static Vector3 rightDirection = new Vector3(-1.0f,0.0f,0.0f);
	static Vector3 frontDirection = new Vector3(0.0f,0.0f,1.0f);
	static Vector3 backDirection = new Vector3(0.0f,0.0f,-1.0f);
	
	#region Camera
	
	//See the InitializeCamera() function to discover the function of this
	public bool bindCameraToPlayer = true;
	Camera playerCamera;
	GameObject cameraRoot;
	public float cameraDistance = 5.0f;
	public float cameraHeightOffset = 1.0f;
	
	#endregion
		
	public override void OnInitializeEntity ()
	{	
		//Retrieve our character controller
		this.cc = gameObject.GetComponent<CharacterController>();
		
		controller.RegisterPlayer(this);
		
		//Place the camera so we can see everything nicely
		InitializeCamera();
		
	}
	
	void InitializeCamera(){
		
		//What we're going to do with our camera is:
		//Create a new gameobject that will follow the character
		//Bind the camera to this object, at an offset
		//This way, the character can rotate and such
		//But the camera will behave as if it's parented to it
				
		playerCamera = Camera.main;
		
		if(playerCamera == null){
			playerCamera = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
		}
		
		if(playerCamera == null){
			GameObject o = new GameObject("Player_Camera");
			playerCamera = o.AddComponent<Camera>();
		}
		
		cameraRoot = new GameObject("Camera Root");
		
		cameraRoot.transform.parent = transform;
		cameraRoot.transform.localPosition = Vector3.zero;
		cameraRoot.transform.forward = transform.forward;
		cameraRoot.transform.parent = null;
		
		
		playerCamera.transform.parent = cameraRoot.transform;
		playerCamera.transform.forward = cameraRoot.transform.forward * -1;
		playerCamera.transform.localPosition = new Vector3(0.0f,cameraHeightOffset,cameraDistance);
	}
	
	void LateUpdate(){
		UpdateCamera();
	}
	
	void UpdateCamera(){
		
		if(playerCamera == null){
			return;
		}
		
		cameraRoot.transform.position = transform.position;
	}
	
	public override void OnUpdateEntity (float deltaTime)
	{
		UpdateGravity();
		
		UpdateInput(Time.deltaTime);
		
		UpdateMovement(Time.deltaTime);
		
		//We'll just assure our states are all nice
		if(!left && !right && !isJumping){
			SetIsIdle(true);
			SetIsMoving(false);
		}
		else{
			SetIsIdle(false);
			SetIsMoving(true);
		}
	}
	
	//Update the falling flags for the character
	void UpdateGravity(){
		
		if(arbitraryGroundPointEnabled){
			
			if((eTransform.position.y - yGroundOffset) <= arbitraryGroundPoint){
				
				isFalling = false;
				isJumping = false;
			}
			
		}
		
		// || cc.isGrounded
		if((cc.collisionFlags & CollisionFlags.Below) != 0){
						
			isFalling = false;
			isJumping = false;
		}
		else if(!isJumping){
			isFalling = true;
		}
		
		if((cc.collisionFlags  & CollisionFlags.Above) != 0){
			isJumping = false;
		}
		
		
	}
	
	//We can use this if we are taking input from some other source (e.g mobile)
	public void SetCharacterState(bool left, bool right, bool up, bool down){
		this.left = left;
		this.right = right;
		this.up = up;
		this.down = down;
	}
	
	void UpdateInput(float deltaTime){
		
		//Everything is false, until we prove otherwise
		//It's almost philosophical
		left = false;
		right = false;
		down = false;
		up = false;
		
		//Word on the street is that I should use GetAxis() for movement
		//But I'll level with you: I'm a renegade
		
		/*if(Input.GetButton(input_Left)){
			left = true;
		}
		
		if(Input.GetButton(input_Right)){
			right = true;
		}
		
		if(Input.GetButton(input_Down)){
			down = true;
		}
		
		if(Input.GetButton(input_Up)){
			up = true;
		}*/
		
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis ("Vertical");
		
		if(h > 0.0f){
			right = true;
		}
		
		if(h < 0.0f){
			left = true;
		}
		
		if(v > 0.0f){
			up = true;
		}
		
		if(v < 0.0f){
			down = true;
		}
				
		//We'll use OnButtonDown() here too
		//It feels a lot nicer
		if(Input.GetButtonDown(input_Jump)){
			Jump();
		}		
	}
	
	void UpdateMovement(float deltaTime){
		
		Vector3 movement = Vector3.zero;
					
		if(isFalling && !isJumping){
			movement.y -= gravity;
		}
		
		//Now let us bear in mind that left is positive x, and right is negative x
		if(left && !right){
			movement.x += walkSpeed;
			//And we'll also face in this direction
			cc.transform.forward = leftDirection;
		}
		
		if(right && !left){
			movement.x -= walkSpeed;
			//And we'll also face in this direction
			cc.transform.forward = rightDirection;
		}
		
		if(down && !left && !right && !up){
			cc.transform.forward = frontDirection;
		}
		
		if(!down && !left && !right && up){
			cc.transform.forward = backDirection;
		}
		
		movement *= deltaTime;
						
		if(isJumping){
			movement.y += GetJumpHeight(deltaTime);
		}
		
		cc.Move(movement);
		
		if(arbitraryGroundPointEnabled){
			
			if((eTransform.position.y - yGroundOffset) <= arbitraryGroundPoint){
				
				Vector3 pos = eTransform.position;
				
				pos.y = arbitraryGroundPoint + yGroundOffset;
				
				eTransform.position = pos;
				
			}
			
		}
				
	}
	
	public void Jump(){
		
		if(isJumping){
			return;
		}
		
		currentJumpTime = 0.0f;
		
		isJumping = true;
		
		lastRawJump = 0.0f;
		
	}
	
	float GetJumpHeight(float deltaTime){
		
		currentJumpTime += deltaTime;
		
		float y = -Mathf.Pow(
		                     (currentJumpTime*(0.5f*gravity)
		                      -
		                      (Mathf.Sqrt(jumpHeight)))
		                      ,2.0f
		                      ) + jumpHeight;
		   
		float retValue = (y - lastRawJump);
		
		lastRawJump = y;
		
		return retValue;
		
	}
}

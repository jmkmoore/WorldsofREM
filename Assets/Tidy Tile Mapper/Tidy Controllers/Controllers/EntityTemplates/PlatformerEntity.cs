using UnityEngine;
using System.Collections;
using System;
using DopplerInteractive.TidyTileMapper.Utilities;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public abstract class PlatformerEntity : Entity {
	
	//The Side Scrolling Entity will walk backward and forward
	//It will jump if it is able to make the distance / height
	//It will about-face if it meets an obstacle
	//It will about-face if it meets another entity
	
	protected CharacterController cc = null;
	
	//The speed at which the character walks
	public float walkSpeed;
	//The height to which you will jump... if you so choose to jump
	public float jumpHeight;
	//The speed at which the character falls
	public float gravity;
	//Our character will never move in such a way so as to fall further than this
	public int maximumFallAllowance;
	//We'll calculate this mathematically upon initialize
	protected int jumpDistance;
	//Our jump height, measured in blocks
	protected int jumpHeight_Blocks;
	
	protected bool isJumping = false;
	protected bool isFalling = false;
	//This helps us return jump intervals instead of absolute values
	protected float lastRawJump = 0.0f;
	//The current time for which we're been jumping
	protected float currentJumpTime = 0.0f;
	
	//Our 'forward' vectors
	public static Vector3 leftDirection = new Vector3(-1.0f,0.0f,0.0f);
	public static Vector3 rightDirection = new Vector3(1.0f,0.0f,0.0f);
	public static Vector3 frontDirection = new Vector3(0.0f,0.0f,1.0f);
	public static Vector3 backDirection = new Vector3(0.0f,0.0f,-1.0f);
	
	public bool randomiseStartingDirection = false;
	public int startingDirection = 1;
	public float actMargin = 0.0f;
	
	//Our intentions - it's good to know these things
	//-1 is left, 1 is right
	protected int currentDirection = -1;
	protected Vector3 currentTilePosition;
	protected Vector3 targetTilePosition;
	//Our current jump height may vary from jumpHeight -
	//as it fluctuates pending on the target
	protected float currentJumpHeight;
	protected bool hasActedForBlock = false;
	
	public float chanceToIdle = 0.0f;
	public float idleTime = 0.0f;
	public float minimumIdleInterval = 0.0f;
	protected float currentIdleTime = 0.0f;
	protected float lastIdle = 0.0f;
		
	public virtual void OnHandleAnimation(float deltaTime){}
	
	//We need to bind to the z position
	//This is to avoid unforseen physics occurences with character controllers
	//(Say, one character controller stands atop another and pushes it clear off the level)
	protected float bound_z_position;
	
	//We'll just declare this once. It will be useful in our evaluation function
	protected List<Vector3> reservedBlocks = new List<Vector3>();
	
	#region Initialization logic
	
	public override void OnInitializeEntity ()
	{				
		cc = GetComponent<CharacterController>();
		
		transform.parent = this.parentMap.transform;
		
		bound_z_position = eTransform.localPosition.z;
		
		RecalculateJumpDistance();
							
		OnBlockEntry(x,y,z);
		
		jumpHeight_Blocks = (int)((float)jumpHeight / parentMap.tileScale.y);
		
		//We'll randomise our starting direction if we have chosen to
		if(randomiseStartingDirection){
			currentDirection = UnityEngine.Random.Range(0,2);
			if(currentDirection == 0){
				currentDirection = -1;
			}
		}
		else{
			currentDirection = startingDirection;
			
			//If something odd has been put in, I will just disregard and randomise
			if(currentDirection == 0 || currentDirection > 1 || currentDirection < -1){
				currentDirection = UnityEngine.Random.Range(0,2);
				if(currentDirection == 0){
					currentDirection = -1;
				}
			}
		}
	}
	#endregion
	
	public override void OnBlockEntry (int x, int y, int z)
	{
		hasActedForBlock = false;
		currentTilePosition = BlockUtilities.GetMathematicalPosition(parentMap,x,y,z);
	}
		
	public override void OnBlockExit (int x, int y, int z)
	{
		if(!CanMoveToBlock(x+currentDirection,y,z)){
			hasActedForBlock = false;
			EvaluateAction();
		}
	}
	
	public override void OnUpdateEntity(float deltaTime){
		
		if(!isIdle){
			UpdateBlockAction();
		}
		
		UpdateMovement(deltaTime);
		UpdateIdle(deltaTime);
		UpdateGravity();
	}
	
	protected void UpdateBlockAction(){
		
		if(hasActedForBlock){
			return;
		}
		
		if(isFalling || isJumping){
			return;
		}
		
		bool act = false;
				
		if(currentDirection == -1){
			
			if(eTransform.localPosition.x >= currentTilePosition.x + actMargin){
				act = true;
			}
		}
		else{
			
			if(eTransform.localPosition.x <= currentTilePosition.x - actMargin){
				act = true;
			}
		}
		
		if(act){
			
			hasActedForBlock = EvaluateAction();
			
		}
		
	}
		
	public abstract bool EvaluateAction();
		
	protected bool BlockIsEmpty(int x, int y, int z){
		
		if(!BlockUtilities.IsWithinMapBounds(parentMap,x,y,z)){
			return false;
		}
		
		Block b = BlockUtilities.GetBlockAt(parentMap,x,y,z);
		
		if(b == null || (b.isNullBlock || b.actAsEmptyBlock)){
		
			return true;
			
		}
		
		return false;
	}
	
	protected bool CanMoveToBlock(int x, int y, int z){
		
		if(!BlockUtilities.IsWithinMapBounds(parentMap,x,y,z)){
			return false;
		}
		
		Block b = BlockUtilities.GetBlockAt(parentMap,x,y,z);
		
		if(b == null || (b.isNullBlock || b.actAsEmptyBlock)){
		
			if(controller.CanMoveTo(x,y,z)){
			
				return true;
				
			}
			
		}
		
		return false;
		
	}
	
	#region Behaviour updates

	protected void Idle(){
		
		SetIsIdle(true);
		SetIsMoving(false);
		currentIdleTime = 0.0f;
	}
	
	protected void UpdateIdle(float deltaTime){
		
		if(!isIdle){
			return;
		}
		
		currentIdleTime += deltaTime;
		
		if(currentIdleTime >= idleTime){
			currentIdleTime = 0.0f;
			SetIsIdle(false);
			SetIsMoving(true);
			hasActedForBlock = false;
			
			lastIdle = 0.0f;
			
		}
	}
		
	protected bool CanIdle(){
		if(lastIdle >= minimumIdleInterval){
			return true;
		}
		
		return false;
	}
			
	protected void UpdateMovement(float deltaTime){
		
		if(eTransform.localPosition.z != bound_z_position){
			Vector3 p = eTransform.localPosition;
			p.z = bound_z_position;
			eTransform.localPosition = p;
		}
		
		Vector3 movement = Vector3.zero;
		
		if(!isIdle){
			
			lastIdle += deltaTime;
			
			//Now let us bear in mind that left is positive x, and right is negative x
			if(currentDirection == 1){
				//And we'll also face in this direction
				eTransform.forward = leftDirection;
			}
			else{
				//And we'll also face in this direction
				eTransform.forward = rightDirection;
			}
		
			movement = eTransform.forward * walkSpeed * deltaTime;
					
		}	
		
		if(isFalling && !isJumping){
			movement.y -= gravity * Time.deltaTime;
		}
		
		if(!isIdle){
			if(isJumping){
				movement.y += GetJumpHeight(deltaTime);
			}
		}
				
		cc.Move(movement);
				
	}
	
	//Update the falling flags for the character
	protected void UpdateGravity(){
				
		if((cc.collisionFlags & CollisionFlags.Below) != 0 || cc.isGrounded){
			isFalling = false;
			if(isJumping){
				isJumping = false;
			}
		}
		else if(!isJumping){
			isFalling = true;
		}
		
		if((cc.collisionFlags  & CollisionFlags.Above) != 0){
			if(isJumping){
				isJumping = false;
			}
		}
		
		
	}
	
	public bool Jump(){
		
		if(isJumping){
			return false;
		}
				
		currentJumpTime = 0.0f;
		
		isJumping = true;
		
		lastRawJump = 0.0f;
		
		return true;
	}
	
	public void RecalculateJumpDistance(){
		
		float jumpDuration = GetJumpSpan();
		
		float jumpSpan = jumpDuration * walkSpeed;
		
		jumpDistance = (int)(jumpSpan / parentMap.tileScale.x);
		
	}
	
	float GetJumpSpan(){
				
		/*float y = -Mathf.Pow(
			(0.0f*(0.5f*gravity))
		                      ,2.0f) + jumpHeight;*/
		
		double x1,x2;
		
		//-(9.8*2)x^2 + x + 2
		
		SolveQuadratic(-gravity * 2.0d,1.0d,(double)jumpHeight, out x1, out x2);
		
		return (float)(x2 - x1);
		
	}
	
	//This is used for finding the x-intercepts of the parabola we use for jumping
	//Note: I didn't write this, I found it on a website during my quadratic travels
	public void SolveQuadratic(double a, double b, double c, out double x1, out double x2)
	{
	    //Calculate the inside of the square root
	    double insideSquareRoot = (b * b) - 4 * a * c;
	
	    if (insideSquareRoot < 0)
	    {
	        //There is no solution
	        x1 = double.NaN;
	        x2 = double.NaN;
	    }
	    else
	    {
	        //Compute the value of each x
	        //if there is only one solution, both x's will be the same
	        double sqrt = Math.Sqrt(insideSquareRoot);
	        x1 = (-b + sqrt) / (2 * a);
	        x2 = (-b - sqrt) / (2 * a);
	    }
	}
		
	//This is used to calculate our current Y increment based on
	//how long we have been jumping for
	protected float GetJumpHeight(float deltaTime){
		
		currentJumpTime += deltaTime;
				
		float y = -Mathf.Pow(
		                     (currentJumpTime*(0.5f*gravity)
		                      -
		                      (Mathf.Sqrt(currentJumpHeight)))
		                      ,2.0f
		                      ) + currentJumpHeight;
		   
		float retValue = (y - lastRawJump);
		
		lastRawJump = y;
		
		return retValue;
		
	}
	
	#endregion
	
}

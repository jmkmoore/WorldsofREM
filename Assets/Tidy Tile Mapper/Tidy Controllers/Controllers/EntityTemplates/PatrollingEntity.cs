using System;
using UnityEngine;
using DopplerInteractive.TidyTileMapper.Utilities;

public abstract class PatrollingEntity : Entity
{
	public enum PatrolAxis{
		Vertical,
		Horizontal
	};
	
	public PatrolAxis patrolAxis;
	
	public float timeToMoveSingleTile = 1.0f;
	protected float currentLerpAmount = 0.0f;
	
	protected int currentDirection = 0;
	
	public int initialDirection = 1;
	public bool randomiseInitialDirection = false;
	
	Vector3 sourcePosition;
	Vector3 targetPosition;
	Vector3 targetCoords;
	
	//Relative to map tiling
	public static Vector3 leftDirection = new Vector3(-1.0f,0.0f,0.0f);
	public static Vector3 rightDirection = new Vector3(1.0f,0.0f,0.0f);
	public static Vector3 frontDirection = new Vector3(0.0f,0.0f,-1.0f);
	public static Vector3 backDirection = new Vector3(0.0f,0.0f,1.0f);
	public static Vector3 upDirection = new Vector3(0.0f,1.0f,0.0f);
	public static Vector3 downDirection = new Vector3(0.0f,-1.0f,0.0f);
	
	public float idleTime = 0.0f;
	float currentIdleTime = 0.0f;
		
	public override void OnInitializeEntity ()
	{
		if(initialDirection != 1 && initialDirection != -1 || randomiseInitialDirection){
			
			int r = UnityEngine.Random.Range(0,2);
			
			if(r == 0){
				currentDirection = 1;
			}
			else{
				currentDirection = -1;
			}
			
		}
		else{
			currentDirection = initialDirection;			
		}
		
		eTransform.parent = parentMap.transform;
		eTransform.localPosition = BlockUtilities.GetMathematicalPosition(parentMap,x,y,z);
		
		ProcessPatrolNode();
	}
	
	public override void OnUpdateEntity (float deltaTime)
	{
		if(!isIdle){
			UpdateMovement(deltaTime);	
		}
		else{
			UpdateIdle(deltaTime);
		}
	}
	
	protected void UpdateMovement(float deltaTime){
		
		currentLerpAmount += deltaTime;
		float n = currentLerpAmount / timeToMoveSingleTile;
		
		if(n >= 1.0f){
			n = 1.0f;
		}
		
		transform.localPosition = Vector3.Lerp(sourcePosition,targetPosition,n);
		
		if(n == 1.0f){
			OnReachBlockCenter(x,y,z);
			currentLerpAmount = 0.0f;
		}
	}
	
	protected bool CanMoveTo(int x, int y, int z){
		
		if(!BlockUtilities.IsWithinMapBounds(parentMap,x,y,z)){
			return false;
		}
		
		if(!controller.CanMoveTo(x,y,z)){
			return false;
		}
		
		Block b = BlockUtilities.GetBlockAt(parentMap,x,y,z);
		
		if(b == null || b.isNullBlock || b.actAsEmptyBlock){
			return true;
		}
		
		return false;
		
	}
	
	public void ProcessPatrolNode(){
		
		sourcePosition = eTransform.localPosition;
				
		if(IsStuck()){
			OnCannotMove(x,y,z);
			return;
		}
		
		if(patrolAxis == PatrolAxis.Horizontal){
			targetCoords = new Vector3(x + currentDirection,y,z);
			targetPosition = BlockUtilities.GetMathematicalPosition(parentMap,x + currentDirection,y,z);
						
		}
		else if(patrolAxis == PatrolAxis.Vertical){
			targetCoords = new Vector3(x,y+currentDirection,z);
			targetPosition = BlockUtilities.GetMathematicalPosition(parentMap,x,y+currentDirection,z);
			
		}
		
		if(!CanMoveTo((int)targetCoords.x,(int)targetCoords.y,(int)targetCoords.z)){
						
			ReachEndOfPatrol(x,y,z);
			
			return;
		}
		controller.ReserveBlock(entityType,targetCoords);
		
		//Now decide where we're facing
		int x_dir = x - (int)targetCoords.x;
		int y_dir = (int)targetCoords.y - y;
		
		if(x_dir > 0){
			eTransform.up = upDirection;
			eTransform.forward = rightDirection;
		}
		else if(x_dir < 0){
			eTransform.up = upDirection;
			eTransform.forward = leftDirection;
		}
		else if(y_dir > 0){
			
			if(parentMap.growthAxis == BlockMap.GrowthAxis.Forward){
				eTransform.forward = frontDirection;	
			}
			else{
				eTransform.forward = downDirection;
				Vector3 rot = eTransform.localRotation.eulerAngles;
				rot.y = 0.0f;
				eTransform.localRotation = Quaternion.Euler(rot);
			}
			
		}
		else if(y_dir < 0){
			
			if(parentMap.growthAxis == BlockMap.GrowthAxis.Forward){
				eTransform.forward = backDirection;	
			}
			else{
				eTransform.forward = upDirection;
			}
			
		}
		
		SetIsMoving(true);
		
	}
	
	protected void Idle(){
		
		SetIsIdle(true);
		SetIsMoving(false);
		currentIdleTime = 0.0f;
	}
	
	protected void UpdateIdle(float deltaTime){
		
		currentIdleTime += deltaTime;
		
		if(currentIdleTime >= idleTime){
			
			SetIsIdle(false);
			currentIdleTime = 0.0f;
			ProcessPatrolNode ();
		}
		
	}
	
	public bool IsStuck(){
		
		if(patrolAxis == PatrolAxis.Horizontal){
			if(!CanMoveTo(x + currentDirection,y,z) && !CanMoveTo(x - currentDirection,y,z)){
				return true;
			}
		}
		else if(patrolAxis == PatrolAxis.Vertical){
			if(!CanMoveTo(x,y+ currentDirection,z) && !CanMoveTo(x,y- currentDirection,z)){
				return true;
			}
		}
		
		return false;
	}
	
	public override void OnReachBlockCenter (int x, int y, int z)
	{
		ProcessPatrolNode();
	}
	
	protected void ReachEndOfPatrol(int x, int y, int z){
		
		OnReachEndOfPatrol(x,y,z);
				
	}
		
	public abstract void OnCannotMove(int x, int y, int z);
	
	public abstract void OnReachEndOfPatrol(int x, int y, int z);
}



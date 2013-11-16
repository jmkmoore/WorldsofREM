using UnityEngine;
using System.Collections;
using DopplerInteractive.TidyTileMapper.Utilities.Pathing;
using System.Collections.Generic;
using DopplerInteractive.TidyTileMapper.Utilities;

public abstract class PathingEntity : Entity {
	
	public List<Vector3> currentPath = new List<Vector3>();
		
	protected Vector3 startPosition = Vector3.zero;
	
	protected Vector3 targetCoords = Vector3.zero;
	protected Vector3 targetPosition = Vector3.zero;
	
	public int pathRadius;
	
	public float idleTime;
	protected float currentIdleTime;
	
	public float timeToMoveSingleTile = 1.0f;
	protected float currentLerpAmount = 0.0f;
		
	//Relative to map tiling
	public static Vector3 leftDirection = new Vector3(-1.0f,0.0f,0.0f);
	public static Vector3 rightDirection = new Vector3(1.0f,0.0f,0.0f);
	public static Vector3 frontDirection = new Vector3(0.0f,0.0f,-1.0f);
	public static Vector3 backDirection = new Vector3(0.0f,0.0f,1.0f);
	public static Vector3 upDirection = new Vector3(0.0f,1.0f,0.0f);
	public static Vector3 downDirection = new Vector3(0.0f,-1.0f,0.0f);
		
	public void ReachBlockCenter(int x, int y, int z){
				
		ProcessPathing();
		
		OnReachBlockCenter(x,y,z);
	}
		
	public abstract void OnReachPathEnd(int x, int y, int z);
	
	public void PathTo(int x, int y, int z){
		
		currentPath.Clear();
		
		currentPath = BlockUtilities.GetPath(parentMap,this.x,this.y,(int)x,(int)y,z,false);
		
		if(currentPath == null || currentPath.Count <= 0){
			currentPath = new List<Vector3>();
		}
		else{
			currentPath.Add(new Vector3(x,y,z));
		}
		
		SetIsIdle(true);
	}
	
	void ProcessPathing(){
				
		//Debug.Log(name + " Processing pathing at: " + x + "," + y + "," + z);
		
		if(currentPath.Count <= 0){
						
			OnReachPathEnd(x,y,z);
			
			return;
		}
		
		if(currentPath.Count > 0){
			
			Vector3 targetNode = currentPath[0];
						
			currentPath.RemoveAt(0);
			
			if(!CanWalkTo((int)targetNode.x,(int)targetNode.y,z)){
								
				currentPath.Clear();
								
				Idle ();
				
				return;
			}
			
			targetCoords = new Vector3(targetNode.x,targetNode.y,z);
			targetPosition = BlockUtilities.GetMathematicalPosition(parentMap,(int)targetNode.x,(int)targetNode.y,z);
			
			controller.ReserveBlock(entityType,targetCoords);
			
			startPosition = eTransform.localPosition;
			
			//Debug.Log(name + " Moving to: " + targetCoords.ToString());
			
			currentLerpAmount = 0.0f;
			
			isMoving = true;
			
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
			
		}
		else{
						
			Idle();
		}
	}
	
	void Idle(){
	
		Debug.Log("Idle");
		
		isMoving = false;
		isIdle = true;
		currentIdleTime = 0.0f;
	}
	
	void UpdateIdle(float deltaTime){
				
		currentIdleTime += deltaTime;
		if(currentIdleTime >= idleTime){
			isIdle = false;
			
			ProcessPathing ();
		}
	}
	
	void DebugPath(){
				
		string s = name + " Path:[";
		
		for(int i = 0; i < currentPath.Count; i++){
			s += currentPath[i].ToString() + ":";
		}
		
		s += "]";
		
		Debug.Log(s);
		
	}
	
	public bool CanWalkTo(int x, int y, int z){
		
		if(!BlockUtilities.IsWithinMapBounds(parentMap,x,y,z)){
			//Debug.Log("Cannot walk to "+ x + "," + y + "," + z+": bounds");
			return false;
		}
		
		if(!controller.CanMoveTo(x,y,z)){
			//Debug.Log("Cannot walk to "+ x + "," + y + "," + z+": controller");
			return false;
		}
		
		Block b = BlockUtilities.GetBlockAt(parentMap,x,y,z);
		
		if(b == null || b.isNullBlock || b.actAsEmptyBlock){
			//Debug.Log("Can walk to "+ x + "," + y + "," + z+": not null");
			return true;
		}
		
		return false;
		
	}
	
	public override void OnUpdateEntity (float deltaTime)
	{
		if(isIdle){
			UpdateIdle(deltaTime);	
		}
		else{
			UpdateMovement(deltaTime);
		}
		
	}
	
	void UpdateMovement(float deltaTime){
		
		currentLerpAmount += deltaTime;
		
		float n = currentLerpAmount / timeToMoveSingleTile;
		
		if(n >= 1.0f){
			n = 1.0f;
		}

		eTransform.localPosition = Vector3.Lerp(startPosition,targetPosition,n);
		
		if(n >= 1.0f){
			currentLerpAmount = 0.0f;
			startPosition = targetPosition;
			ReachBlockCenter(x,y,z);
		}
	}
			
	public override void OnBlockEntry (int x, int y, int z)
	{
	}
	
	public override void OnBlockExit (int x, int y, int z)
	{
	}
	
	public override void OnDestroyEntity ()
	{
		
	}
	
	public override void OnInitializeEntity ()
	{
		InitializePosition ();
		ProcessPathing();
	}
	
	void InitializePosition(){
		
		eTransform.localPosition = BlockUtilities.GetMathematicalPosition(parentMap,x,y,z);
		
	}
	
	void OnDrawGizmos(){
		
		BlockMap map = parentMap;
		
		List<Vector3> v = currentPath;
		
		Gizmos.color = new Color(0.0f,0.0f,1.0f,0.5f);
		
		for(int i = 0; i < v.Count; i++){
			
			Vector3 pos = BlockUtilities.GetMathematicalPosition(map,(int)v[i].x,(int)v[i].y,(int)v[i].z);
			
			pos = map.transform.TransformPoint(pos);
			
			Gizmos.DrawSphere (pos,0.5f);
			
		}
	}
	
}

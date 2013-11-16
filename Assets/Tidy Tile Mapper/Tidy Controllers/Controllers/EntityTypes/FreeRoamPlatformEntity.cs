using UnityEngine;
using System.Collections;

public class FreeRoamPlatformEntity : PlatformerEntity {
	
	public override bool EvaluateAction(){
		if(isFalling || isJumping){
			return true;
		}
		
		Vector3 cBlock = new Vector3(x,y,z);
		
		//Just move forward
		for(int i = 0; i < reservedBlocks.Count; i++){
			
			if(reservedBlocks[i] == cBlock){
				continue;
			}
			
			controller.UnreserveBlock(entityType,reservedBlocks[i]);
		}
				
		reservedBlocks.Clear();
				
		bool belowBlock = !BlockIsEmpty(x,y+1,z);
		
		if(!belowBlock){
			return true;
		}
		
		if(CanIdle()){
			if(UnityEngine.Random.value <= chanceToIdle){
				Idle ();
				return false;
			}
		}
		
		//We'll be using this a lot. Let's just do it once.
		int next_x = x + currentDirection;
		
		//Step One: Can we just walk forward?
		bool forwardBlock = CanMoveToBlock(next_x,y,z);
				
		if(forwardBlock){
			
			//Step One-point-five: Is there a supporting block somewhere onto which we can fall / walk?
			bool supportingBlock = false;
			
			reservedBlocks.Add(new Vector3(next_x,y,z));
			
			//We'll iterate from the block at our feet, to the block at the furthest reaches
			//of our ability to fall
			for(int yb = y+1; yb <= y+maximumFallAllowance+1; yb++){
				
				if(!controller.CanMoveTo(next_x,yb,z)){
					break;
				}
				
				if(!CanMoveToBlock(next_x,yb,z)){
					supportingBlock = true;
					break;
				}
				
				reservedBlocks.Add(new Vector3(next_x,yb,z));
				
			}
			
			if(!supportingBlock){
				//We can't just move forward.
				//Cancel this experiment
				reservedBlocks.Clear();
			}
			else{
				
				//Just move forward
				for(int i = 0; i < reservedBlocks.Count; i++){
					controller.ReserveBlock(entityType,reservedBlocks[i]);
				}
				
				SetIsMoving(true);
											
				return true;
			}
			
		}
				
		bool canJump = false;
		
		if(!BlockIsEmpty(next_x,y,z) && controller.CanMoveTo(next_x,y,z) && controller.CanMoveTo(next_x,y-1,z) && controller.CanMoveTo(next_x,y+1,z)){
			//Step Two: Okay. We can't move forward. But can we jump forward?
			for(int yb = y-1; yb >= (y-jumpHeight_Blocks); yb--){
				
				//If we can't move to this block owing to a reservation or a current entity,
				//we won't be able to jump here. 
				if(!controller.CanMoveTo(next_x,yb,z)){
					break;
				}
				
				if(!CanMoveToBlock(x,yb,z)){
					break;
				}
				
				reservedBlocks.Add(new Vector3(x,yb,z));
				
				if(CanMoveToBlock(next_x,yb,z)){
					
					//Now that we know we can jump to here, set our jumpheight
					
					int blockCount = y - yb;
					float n = ((float)blockCount / (float)jumpHeight);
					currentJumpHeight = n * jumpHeight;
										
					canJump = true;
					reservedBlocks.Add(new Vector3(next_x,yb,z));
					break;
				}
			}
			
			if(!canJump){
								
				reservedBlocks.Clear ();
			}
			else{
				//Jump!
				for(int i = 0; i < reservedBlocks.Count; i++){
					controller.ReserveBlock(entityType,reservedBlocks[i]);
				}
				
				SetIsMoving(true);
				
				Jump ();
				
				return true;
			}
		}
		
		//Step Three: We can't do anything. Turn around, and re-evaluate.
		currentDirection *= -1;
				
		SetIsMoving(true);
		
		return false;
	}
}

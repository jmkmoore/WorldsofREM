using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomPathingEntity : PathingEntity {
		
	public override void OnReachPathEnd (int x, int y, int z)
	{
		RePath ();
	}
	
	void RePath(){
			
		List<Vector3> b = new List<Vector3>();
		
		for(int px = x - pathRadius; px <= x + pathRadius; px++){
			for(int py = y - pathRadius; py <= y + pathRadius; py++){
				if(CanWalkTo(px,py,z)){
					b.Add(new Vector3(px,py,z));
				}
			}	
		}
		
		if(b.Count <= 0){
						
			return;
		}
		
		
		Vector3 target = b[UnityEngine.Random.Range (0,b.Count)];
		
		PathTo((int)target.x,(int)target.y,z);
				
	}
}

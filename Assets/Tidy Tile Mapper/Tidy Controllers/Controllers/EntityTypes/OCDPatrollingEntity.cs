using UnityEngine;
using System.Collections;

public class OCDPatrollingEntity : PatrollingEntity {
	
	public override void OnReachEndOfPatrol (int x, int y, int z)
	{
		currentDirection *= -1;
		ProcessPatrolNode();
	}
	
	public override void OnCannotMove (int x, int y, int z)
	{
		Idle();
	}
}

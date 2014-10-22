using UnityEngine;
using System.Collections;

public class DefaultEnemy : Enemy
{
	override public void PlaceEnemyAtCoordinates(Vector2 Coords){
		transform.position = Coords;
	}

	void Start(){
		//SetStartAndEndPoints(Vector3.left);
	}

	// Update is called once per frame
	void Update ()
	{
		//BackAndForthMovement();
	}

	public Vector3 startMarker;
	public Vector3 endMarker;
	private float lerpPos = 0.0f;
	private float lerpTime = 1.0f;
	private Vector3 vectorDirection = Vector3.left;

	void BackAndForthMovement()
	{
		if(lerpPos > 1){
			lerpPos = 0;
			//SwitchEndPos();
			SetStartAndEndPoints(vectorDirection);
		}
		lerpPos += Time.deltaTime/lerpTime;
		transform.position = Vector3.Lerp(startMarker, endMarker,lerpPos);
	}

	void OnCollisionEnter(Collision collision){
		/*foreach (ContactPoint contact in collision.contacts) {
			//print();
			if((transform.position - contact.point).y > 0.5){
				print("hit the following object: " + contact.otherCollider.name);
				SwitchDirection();				
			}
		}*/
	}

	void SetStartAndEndPoints(Vector3 endVectorDirection){
		startMarker = transform.position;
		endMarker = startMarker + endVectorDirection;
	}

	void SwitchDirection(){
		if((startMarker - endMarker).x > 0){
			vectorDirection = Vector3.right;
			SetStartAndEndPoints(vectorDirection);
		}
		else{
			vectorDirection = Vector3.left;
			SetStartAndEndPoints(vectorDirection);
		}
	}

}

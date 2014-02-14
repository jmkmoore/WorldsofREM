using UnityEngine;
using System.Collections;

public class LightOrb : MonoBehaviour {
	
	float timeRemaining;
	bool movePlayer = false;
	GameObject player;
	PlatformerPlayer platformVars;
	// Use this for initialization
	void Start () {
		timeRemaining = Settings.getInstance().OrbTime;		
		player = GameObject.FindGameObjectWithTag("Player");
		platformVars = player.GetComponent<PlatformerPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		timeRemaining -= Time.deltaTime;
		Color color = renderer.material.color;
		color.a = timeRemaining / Settings.getInstance().OrbTime;;
		renderer.material.color = color;
		if(timeRemaining < 0){
			DestroySelf();
		}
		if(movePlayer){
			MoveUserTowardsTarget(transform.position);
		}
	}

	//For when right clicking on the orb
	void OnMouseOver(){
		if(Input.GetMouseButtonDown(1)){
			movePlayer = true;
			StopGravity();
		}
	}

	void DestroySelf(){
		LightController.OrbCount--;
		DestroyImmediate(this.gameObject);	
	}
	
	void StartGravity(){
		platformVars.gravity = 9.8f;
	}

	void StopGravity(){
		platformVars.gravity = 0.0f;
	}

	void MoveUserTowardsTarget(Vector3 target){
		var cc = player.GetComponent<CharacterController>();
		var offset = target - player.transform.position;
		//Get the difference.
		if(offset.magnitude > .1f) {			
			offset = offset.normalized * 6;
			cc.Move(offset * Time.deltaTime);
		}
		else{
			StartGravity();
			DestroySelf();
		}
	}

}

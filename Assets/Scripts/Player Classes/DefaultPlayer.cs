using UnityEngine;
using System.Collections;

public class DefaultPlayer : Player {
	
	bool grounded = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		ApplyInputs();
		SetAnimationParameters();

		if(rigidbody.velocity.x < 0) transform.localScale = new Vector3(-1,1,1);
		else if (rigidbody.velocity.x > 0) transform.localScale = new Vector3(1,1,1);
	}

	override protected void ApplyInputs(){

	}

	override protected void SetAnimationParameters(){
		//GetComponent<Animator>().SetBool("Running",rigidbody.velocity.x != 0);
		//GetComponent<Animator>().SetInteger("VerticalMovement",Mathf.RoundToInt(rigidbody.velocity.y));
		/*
		if(currSpeed.y > 0){
			GetComponent<Animator>().SetInteger("VerticalMovement",1);
		}else if (currSpeed.y < 0){
			GetComponent<Animator>().SetInteger("VerticalMovement",-1);
		}else{
			GetComponent<Animator>().SetInteger("VerticalMovement",0);
		}
		*/
	}

	override public void JumpAction(){
		if(grounded)
			rigidbody.AddForce(transform.up * PropertyManager.getInstance().JumpHeight);
	}

	override public void UseAction(){

	}

	override public void SetGrounded(bool ground){
		grounded = ground;
	}

	override public void KillPlayer(){
		//GetComponent<Animator>().SetBool("Alive",false);
		rigidbody.isKinematic = true;
		IsAlive = false;
	}

	public void OnDeath(){
		LevelLoader.getInstance().ReloadCurrentLevel();
	}
}

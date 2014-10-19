using UnityEngine;
using System.Collections;

public class DefaultPlayer : Player
{
	
		bool grounded = false;
		bool isRightFacing = true;
		bool hitWall	= false;
		bool isDashing = false;
	public Rigidbody Pellet; 

		// Use this for initialization
		void Start ()
		{

		}

	void FixedUpdate(){
		ApplyInputs ();
	}
		
		
		// Update is called once per frame
		void Update ()
		{

			//	ApplyInputs ();
				SetAnimationParameters ();

				if (rigidbody.velocity.x < 0)
						transform.localScale = new Vector3 (-1, 1, 1);
				else if (rigidbody.velocity.x > 0)
						transform.localScale = new Vector3 (1, 1, 1);
		}

		override protected void ApplyInputs ()
		{

		}

		override protected void SetAnimationParameters ()
		{
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

		override public void JumpAction ()
		{
				if (grounded)
						rigidbody.AddForce (transform.up * PropertyManager.getInstance ().JumpHeight);
		Debug.Log ("Grounded is: " + grounded);
		}

		override public void DashAction ()
		{

			if (isRightFacing) {
				rigidbody.AddForce (Vector3.right * PropertyManager.getInstance ().JumpHeight * 10, ForceMode.Acceleration);
			}
			else{
				rigidbody.AddForce (Vector3.left * PropertyManager.getInstance ().JumpHeight * 10, ForceMode.Acceleration);
			}
		}

		override public void UseAction ()
		{

		}

		override public void SetGrounded (bool ground)
		{
				grounded = ground;
		}

		override public void KillPlayer ()
		{
				//GetComponent<Animator>().SetBool("Alive",false);
				rigidbody.isKinematic = true;
				IsAlive = false;
		}

		public void OnDeath ()
		{
				LevelLoader.getInstance ().ReloadCurrentLevel ();
		}

		void OnCollisionEnter(Collision col){
			if(col.gameObject.name == "tile_34(Clone)"){
				hitWall = true;
			DestroyObject(col.gameObject);
			}
			
		Debug.Log ("Is Grounded on collision: " + grounded);
		}


		override public void setDirection (bool isRight)
		{
				isRightFacing = isRight;
		}

		override public void lightGunAction (){
		Rigidbody bullet = Instantiate (Pellet, transform.position, transform.rotation) as Rigidbody;
		}
}

using UnityEngine;
using System.Collections;

public class DefaultPlayer : Player
{
	
		public bool grounded = false;
		bool isRightFacing = true;
		bool hitWall = false;
		bool isDashing = false;
	private int downVelocity;
	private int previousYVelocity;
		bool ignorePlat;
        private float runSpeed;
        private Rigidbody2D thisRigidbody;
        private Transform thisTransform;
    

	public Rigidbody Pellet; 

		// Use this for initialization
		void Start ()
		{
            thisRigidbody = rigidbody2D;
            thisTransform = transform;
            runSpeed = PropertyManager.getInstance().RunSpeed;
		}

	void FixedUpdate(){
		ApplyInputs ();
	}
		
		
		void Update ()
		{
				//if (!grounded) {
					//	if (rigidbody.velocity.y <= 0 && !grounded)
						//		Physics.IgnoreLayerCollision (9, 8, false);
						//else
						//		Physics.IgnoreLayerCollision (9, 8, true);

						//	ApplyInputs ();
						SetAnimationParameters ();

				
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
				if (grounded) {
                    Debug.Log("Jumping");
						//rigidbody2D.AddForce (transform.up * PropertyManager.getInstance ().JumpHeight, ForceMode.Impulse);
                    rigidbody2D.AddForce(transform.up * PropertyManager.getInstance().JumpHeight, ForceMode2D.Impulse);
						grounded = false;
				}
				
		}

		override public void DashAction ()
		{

			if (isRightFacing) {
                Vector3 movementDirection = Vector3.right * runSpeed * runSpeed;
                thisRigidbody.MovePosition(thisTransform.position + (movementDirection * Time.deltaTime));
			}
			else{
                Vector3 movementDirection = Vector3.left * runSpeed * runSpeed;
                thisRigidbody.MovePosition(thisTransform.position + (movementDirection * Time.deltaTime));
				//rigidbody.AddForce (Vector3.left * PropertyManager.getInstance ().JumpHeight, ForceMode.Impulse);
			}
		}

		override public void UseAction ()
		{

		}

		override public void SetGrounded (bool ground)
		{
				grounded = ground;
		}

		override public bool GetGrounded(){
				return grounded;
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


		override public void setDirection (bool isRight)
		{
				isRightFacing = isRight;
		}

		override public void lightGunAction (){
		Rigidbody bullet = Instantiate (Pellet, transform.position, transform.rotation) as Rigidbody;
		}
}

using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{

		private static InputManager _instance;
		private Player player;
		public float ButtonDelay;
		float lastJump = 0;
		float lastUse = 0;

		//Button variables
		public bool LeftButton = false;
		public bool RightButton = false;
		public bool JumpButton = false;
		public bool UseButton = false;
		public bool AttackButton = false;
		public bool DashButton = false;
	private Rigidbody2D thisRigidbody;
	//private Transform thisTransform;
	private float runSpeed;
	public LayerMask layer;
    public Vector2 velocity;

	public bool GunButton = false;

		void Awake ()
		{
			_instance = this;
			if (player == null) {
				GameObject go = GameObject.FindGameObjectWithTag ("Player");
				if (go != null)
					player = go.GetComponent<Player> ();
				}
			//thisRigidbody = player.rigidbody2D;
			//thisTransform = player.transform;
			runSpeed = PropertyManager.getInstance ().RunSpeed;
		}

		public static InputManager getInstance ()
		{
				return _instance;
		}
	
		// Update is called once per frame
	void FixedUpdate ()
	{

		if (player.IsAlive) {
			if (LeftButton) {
				Vector2 movementDirection = Vector2.right * -1 * runSpeed;
                thisRigidbody.MovePosition(thisRigidbody.position + (movementDirection * Time.deltaTime));
				player.setDirection (false);
			} else if (RightButton) {
				Vector2 movementDirection = Vector2.right * runSpeed;
                thisRigidbody.MovePosition(thisRigidbody.position + (movementDirection * Time.deltaTime));
				player.setDirection (true);
			}

			if (JumpButton && Time.time > lastJump + ButtonDelay) {
					player.JumpAction ();
					lastJump = Time.time;
				}
				if (UseButton && Time.time > lastUse + ButtonDelay) {
					player.UseAction ();
					lastUse = Time.time;
				}
				if (DashButton && Time.time > lastUse + ButtonDelay) {
					player.DashAction ();
					lastUse = Time.time;
				}
						if(GunButton && Time.time > lastUse + ButtonDelay){
				player.lightGunAction ();
				lastUse = Time.time;
						}
				}


				if (Input.GetKey (KeyCode.LeftArrow)) {
						LeftButton = true;
						RightButton = false;
				} else if (Input.GetKey (KeyCode.RightArrow)) {
						LeftButton = false;
						RightButton = true;
				} else {
						LeftButton = false;
						RightButton = false;
				}
		
				if (Input.GetKeyDown (KeyCode.Space)) {
						JumpButton = true;
				} else {
						JumpButton = false;
				}
		
				if (Input.GetKeyDown (KeyCode.LeftShift)) {
						DashButton = true;
				} else {
						DashButton = false;
				}
			if(Input.GetKeyDown (KeyCode.Z)){
				GunButton = true;
			}else{
				GunButton = false;
			}

		}

		void Update(){

		}
}

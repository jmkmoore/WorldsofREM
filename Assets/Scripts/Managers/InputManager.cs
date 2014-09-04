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
	public bool GunButton = false;

		void Awake ()
		{
				_instance = this;

		}

		public static InputManager getInstance ()
		{
				return _instance;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (player == null) {
						GameObject go = GameObject.FindGameObjectWithTag ("Player");
						if (go != null)
								player = go.GetComponent<Player> ();
				} else if (player.IsAlive) {
						if (LeftButton) {
								player.rigidbody.velocity = new Vector2 (-1 * PropertyManager.getInstance ().RunSpeed, player.rigidbody.velocity.y);
								player.setDirection (false);
						} else if (RightButton) {
								player.rigidbody.velocity = new Vector2 (PropertyManager.getInstance ().RunSpeed, player.rigidbody.velocity.y);
								player.setDirection (true);
						} else {
								player.rigidbody.velocity = new Vector2 (0, player.rigidbody.velocity.y);
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

		void FixedUpdate(){

		}
}

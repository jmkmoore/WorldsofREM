using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
		float projectileSpeed = 1;
		bool fireRight = true;

		// Use this for initialization
		void Start ()
		{
				transform.localEulerAngles = new Vector3 (90, 90, 0);
		}
	
		// Update is called once per frame
		void Update ()
		{
			float movementAmt = projectileSpeed * Time.deltaTime;
			//	if (fireRight) {
			//			rigidbody.AddForce(Vector3.up * movementAmt, ForceMode.Force);
			transform.Translate (Vector3.up * movementAmt);
			//	} else {
			//			transform.Translate (Vector3.up * movementAmt * -1);
			//	}
		}

		void FixedUpdate(){
				rigidbody.AddForce (Vector3.up * 10);
		}

		void OnTriggerEnter (Collider other)
		{
				Debug.Log ("bullet collision");
				if (other.CompareTag ("Enemy")) {
						DestroyObject (other.gameObject);
						DestroyObject (gameObject);
				} 
				if (other.CompareTag ("Platform")) {
						DestroyObject (other.gameObject);
						DestroyObject (gameObject);
				}
				
		}
}


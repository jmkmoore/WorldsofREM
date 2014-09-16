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
				if (fireRight) {
						transform.Translate (Vector3.up * movementAmt);
				} else {
						transform.Translate (Vector3.up * movementAmt * -1);
				}
		}

		void OnCollisionEnter (Collision other)
		{
				Debug.Log ("bullet collision");
				if (other.gameObject.name == ("tile_34(Clone)")) {
						DestroyObject (other.gameObject);
						DestroyObject (gameObject);
				}
		}
}


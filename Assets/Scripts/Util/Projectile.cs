using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float projectileSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float movementAmt = projectileSpeed * Time.deltaTime;
		transform.Translate (Vector3.up * movementAmt);
	}
}

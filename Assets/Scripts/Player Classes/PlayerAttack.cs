using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	private int attackValue = 10;
	private int superAttackValue = 20;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
				if (Input.GetKeyDown (KeyCode.X)) {
						Attack();
				}
		}

	void Attack(){
		collider.enabled = true;
		Debug.Log ("enabled" + collider.enabled);
		collider.enabled = false;
		}

	void OnCollisionEnter(Collision other){
				Debug.Log (other.collider.CompareTag ("Player"));
				Debug.Log (other.collider.CompareTag ("Enemy"));
				if (other.collider.CompareTag ("Enemy")) {
						other.rigidbody.AddForce (Vector3.up * 200);
				}
		}
}

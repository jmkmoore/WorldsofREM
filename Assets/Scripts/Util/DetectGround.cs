using UnityEngine;
using System.Collections;

public class DetectGround : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Ground Collision");
        transform.parent.GetComponent<Player>().SetGrounded(true);
        Physics2D.IgnoreLayerCollision(9, 8, false);

	}
	void OnTriggerStay2D(Collider2D other){
		transform.parent.GetComponent<Player>().SetGrounded(true);        
	}
	void OnTriggerExit2D(Collider2D other){
		transform.parent.GetComponent<Player>().SetGrounded(false);
	}
}

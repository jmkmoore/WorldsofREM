using UnityEngine;
using System.Collections;

public class DetectGround : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		transform.parent.GetComponent<Player>().SetGrounded(true);
	}
	void OnTriggerStay(Collider other){
		transform.parent.GetComponent<Player>().SetGrounded(true);
	}
	void OnTriggerExit(Collider other){
		transform.parent.GetComponent<Player>().SetGrounded(false);
	}
}

using UnityEngine;
using System.Collections;

public class DetectGround : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		transform.parent.GetComponent<Player>().SetGrounded(true);
        if (transform.parent.GetComponent<Player>().GetGrounded() && other.collider.gameObject.layer == LayerMask.NameToLayer("OnewayPass"))
        {
            Physics.IgnoreLayerCollision(9, 8, true);
        }
	}
	void OnTriggerStay(Collider other){
		transform.parent.GetComponent<Player>().SetGrounded(true);        
	}
	void OnTriggerExit(Collider other){
		transform.parent.GetComponent<Player>().SetGrounded(false);
	}
}

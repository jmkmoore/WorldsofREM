using UnityEngine;
using System.Collections;

public class DetectGround : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		transform.parent.GetComponent<Player>().SetGrounded(true);
        if (transform.parent.GetComponent<Player>().GetGrounded() && other.collider2D.gameObject.layer == LayerMask.NameToLayer("OnewayPass"))
        {
            Physics.IgnoreLayerCollision(9, 8, false);
        }
        else
        {
            Physics.IgnoreLayerCollision(9, 8, true);
        }
	}
	void OnTriggerStay2D(Collider2D other){
		transform.parent.GetComponent<Player>().SetGrounded(true);        
	}
	void OnTriggerExit2D(Collider2D other){
		transform.parent.GetComponent<Player>().SetGrounded(false);
	}
}

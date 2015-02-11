using UnityEngine;
using System.Collections;

public class DiveTrigger : MonoBehaviour {

    private GameObject myParent;
    private DollMovement myDollMovement;

    void awake()
    {
        myParent = transform.parent.gameObject;
        DollMovement myDollMovement = transform.parent.GetComponent<DollMovement>();
    }

	void OnColliderEnter2D(Collider2D other)
    {
        if (other.name.Equals("TienHitBox"))
        {
            Debug.Log("asdfoasdfasdf");
            myParent.GetComponent<DollMovement>().updateAttack(true);
        }
    }

    void OnColliderExit2D(Collider2D other)
    {
        if (other.name.Equals("TienHitBox"))
        {
            myParent.GetComponent<DollMovement>().updateAttack(false);
        }
    }


}

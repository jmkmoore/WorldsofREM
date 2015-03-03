using UnityEngine;
using System.Collections;

public class EnemyAttackUpdate : MonoBehaviour {

    public string attackName = "";

    private GameObject myParent;
    private ColossusController myController;


	// Use this for initialization
	void Start () {
        myParent = myParent = transform.parent.parent.gameObject;
        myController = (ColossusController)myParent.GetComponent<ColossusController>();

	}

    void Awake()
    {
        myParent = myParent = transform.parent.parent.gameObject;
        myController = (ColossusController)myParent.GetComponent<ColossusController>();
    }
	
	// Update is called once per frame
	void Update () {
       	    
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("TienHitBox"))
        {
            myController.updateCanAttack(attackName, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("TienHitBox"))
        {
            myController.updateCanAttack(attackName, false);
        }
    }
}

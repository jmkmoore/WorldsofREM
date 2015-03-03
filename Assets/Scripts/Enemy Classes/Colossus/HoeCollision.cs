using UnityEngine;
using System.Collections;

public class HoeCollision : MonoBehaviour {

    public int damage = 0;
    private ColossusController myController;
    private GameObject myParent;

    public float collisionTimer;


	// Use this for initialization
	void Start () {
        myParent = transform.parent.parent.parent.gameObject;
        myController = (ColossusController)myParent.GetComponent<ColossusController>();
	}

    void Awake()
    {
        myParent = transform.parent.parent.parent.gameObject;
        myController = (ColossusController)myParent.GetComponent<ColossusController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (collisionTimer != 0)
        {
            damage = 0;
        }
        else
        {
            updateAttackDamage(myController.getCurrentAttackValue());
        }
        if (collisionTimer != 0)
        {
            collisionTimer += Time.deltaTime;
        }
        if (collisionTimer > 0.5f)
        {
            collisionTimer = 0;
        }
    }

    void updateAttackDamage(int newDamage)
    {
        damage = newDamage;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player") && collisionTimer == 0)
        {
            PlayerHealth ph = (PlayerHealth)other.transform.parent.GetComponent<PlayerHealth>();
            ph.adjustCurrentHealth(-damage);
            collisionTimer += Time.deltaTime;
        }
    }


}

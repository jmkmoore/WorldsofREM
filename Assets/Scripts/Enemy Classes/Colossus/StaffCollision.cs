using UnityEngine;
using System.Collections;

public class StaffCollision : MonoBehaviour {

    public int damage = 0;
    private ColossusController myController;
    private GameObject myParent;


    // Use this for initialization
    void Start()
    {
        myParent = transform.parent.parent.parent.gameObject;
        myController = (ColossusController)myParent.GetComponent<ColossusController>();
    }

    void Awake()
    {
        myParent = transform.parent.parent.parent.gameObject;
        myController = (ColossusController)myParent.GetComponent<ColossusController>();
    }

    // Update is called once per frame
    void Update()
    {
        updateAttackDamage(myController.getCurrentAttackValue());
    }

    void updateAttackDamage(int newDamage)
    {
        damage = newDamage;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.tag.Equals("Player"))
        //{
        //    PlayerHealth ph = (PlayerHealth)other.transform.parent.GetComponent<PlayerHealth>();
         //   ph.adjustCurrentHealth(-damage/2);
        //}
    }

}

using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	public int attackValue = 10;
	public int superAttackValue = 20;
	public GameObject target;
	public float attackTimer;
	public float cooldown;
	// Use this for initialization
	void Start () {
		attackTimer = 0;
		cooldown = 3.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null)
        {
            attackTimer = 0;
        }
	if (attackTimer > 0)
		attackTimer -= Time.deltaTime;
	if (attackTimer < 0)
		attackTimer = 0;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject.name);
        if (other.name.Equals("TienHitBox"))
        {
            target = other.gameObject.transform.parent.gameObject;
            if(attackTimer == 0)
                Attack();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        target = null;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (attackTimer == 0)
        {
            Attack();
        }
    }
		
	void Attack(){
		PlayerHealth ph = (PlayerHealth)target.GetComponent("PlayerHealth");
		ph.adjustCurrentHealth(-attackValue);
        attackTimer = cooldown;
	}
}

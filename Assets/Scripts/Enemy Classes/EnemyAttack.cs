using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	private int attackValue = 10;
	private int superAttackValue = 20;
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
		if (attackTimer > 0) 
			attackTimer -= Time.deltaTime;
		if (attackTimer < 0)
			attackTimer = 0;
		float distance = Vector3.Distance (target.transform.position, transform.position);
		Vector3 dir = (target.transform.position - transform.position).normalized;
		float direction = Vector3.Dot (dir, transform.forward);
		if (distance <= 4.0f) {
			if (direction > 0) {
				if (attackTimer == 0) {
					Attack ();
					attackTimer = cooldown;
				}
			}
		}
	}
	
	void Attack(){
		PlayerHealth ph = (PlayerHealth)target.GetComponent("PlayerHealth");
		ph.adjustCurrentHealth(-attackValue);
	}
}

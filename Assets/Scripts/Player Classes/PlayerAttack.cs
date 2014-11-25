using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
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
		if(attackTimer < 0)
			attackTimer = 0;

		if (Input.GetKeyDown (KeyCode.X))
				if (attackTimer == 0) {
						Attack ();
				}
		}

	void Attack(){
		attackTimer = cooldown;
		float distance = Vector3.Distance (target.transform.position, transform.position);
		Vector3 dir = (target.transform.position - transform.position).normalized;
		Debug.Log (distance);
		float direction = Vector3.Dot (dir, transform.forward);
		if (distance < 5.0f) {
			if (direction > 0) {
				EnemyHealth eh = (EnemyHealth)target.GetComponent("EnemyHealth");
				PlayerMode pm  = (PlayerMode)gameObject.GetComponent ("PlayerMode");
				if(pm.Equals("Melee")){
					eh.adjustCurrentHealth(-superAttackValue);
				}
				else
					eh.adjustCurrentHealth(-attackValue);
				target.rigidbody.AddForce(Vector3.up * 100);
				}
			}
		}
}

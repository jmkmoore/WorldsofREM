using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	private int attackValue = 10;
	private int superAttackValue = 2;
	public GameObject target;

	public float attackTimer;
    public float aliveTimer;
    public float maxTime = .01f;
	// Use this for initialization
	void Start () {
		attackTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (aliveTimer > maxTime)
        {
            DestroyObject(gameObject);
        }
        aliveTimer += Time.deltaTime;
		
		}

	void Attack(GameObject target){
        EnemyHealth eh = (EnemyHealth)target.GetComponent<EnemyHealth>();
        eh.adjustCurrentHealth(-attackValue);
        DestroyObject(gameObject);
		}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy"))
        {
            Debug.Log("Hit Enemy: " + other.name + " " + gameObject.layer);
            Attack(other.transform.parent.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Trigger on " + other.name);
        Attack(other.transform.parent.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit trigger");
    }

    public void setAttackDamage(int Damage)
    {
        attackValue = Damage;
    }
}

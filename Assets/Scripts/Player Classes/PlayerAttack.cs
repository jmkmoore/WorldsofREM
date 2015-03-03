using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	public int attackValue = 10;
	private int superAttackValue = 2;
	public GameObject target;

	public float attackTimer;
    public float aliveTimer;
    public float maxTime = .1f;
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
        EnemyHealth eh = findEnemyHealth(target);
        if (eh != null)
        {
            eh.adjustCurrentHealth(-attackValue);
            DestroyObject(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy"))
        {
            Attack(other.transform.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }

    public void setAttackDamage(int Damage)
    {
        attackValue = Damage;
    }

    public EnemyHealth findEnemyHealth(GameObject obj){
        EnemyHealth eh = (EnemyHealth)obj.transform.GetComponent<EnemyHealth>();
        if (eh == null)
        {
            eh = obj.transform.parent.GetComponent<EnemyHealth>();
        }
        if (eh == null)
        {
            eh = obj.transform.parent.parent.GetComponent<EnemyHealth>();
        }
        return eh;
    }
}

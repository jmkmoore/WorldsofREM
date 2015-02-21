using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	float projectileSpeed = 15;
	bool fireRight = true;
	public int bulletDamage;
    float direction;
    public string myType;

    public float timeAlive;
    public float MaxTime;
		// Use this for initialization
	void Start ()
	{
			transform.localEulerAngles = new Vector3 (90, 90, 0);
            Transform parentTrans = transform.parent;
            direction = parentTrans.localScale.x;
            timeAlive = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float movementAmt = projectileSpeed * Time.deltaTime;

		transform.Translate (direction * Vector3.up * movementAmt);

        timeAlive += Time.deltaTime;
        if (timeAlive > MaxTime)
        {
            DestroyObject(gameObject);
        }
	}
    
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Enemy")) {
		DestroyObject(gameObject);
		EnemyHealth eh = (EnemyHealth)other.GetComponent ("EnemyHealth");
		eh.adjustCurrentHealth(-bulletDamage);
		other.transform.rigidbody.AddForce(Vector3.forward * -1 * 100);
		} 
		else if (other.CompareTag ("Platform")) {
					DestroyObject (other.gameObject);
					DestroyObject (gameObject);
			}
        else if (other.CompareTag("Player"))
        {
            DestroyObject(gameObject);
            GameObject go = other.gameObject.transform.parent.gameObject;
            PlayerHealth ph = (PlayerHealth)go.GetComponent ("PlayerHealth");
            ph.adjustCurrentHealth(-bulletDamage);
                
        }
	}
}


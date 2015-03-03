using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	public int maxHealth = 40;
	public int currentHealth = 40;
	private Transform myTransform;
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void adjustCurrentHealth(int adj){
		currentHealth += adj;
		if (currentHealth > maxHealth)
			currentHealth = maxHealth;
		if(currentHealth < 1)
			currentHealth = 0;
		if (currentHealth == 0) {
            if (!transform.gameObject.tag.Equals("Boss"))
    			Destroy(gameObject);
		}
	}
}

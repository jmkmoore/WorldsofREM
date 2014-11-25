using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public int maxHealth = 100;
	public int currentHealth = 100;
	private GameObject myself;
	// Use this for initialization
	void Start () {
		myself = gameObject;
	
	}
	
	// Update is called once per frame
	void Update () {
		//if (currentHealth == 0)
						
	}

	public void adjustCurrentHealth(int adj){
				currentHealth += adj;
				if (currentHealth > 100)
						currentHealth = maxHealth;
				if(currentHealth < 1)
						currentHealth = 0;
		}
	}

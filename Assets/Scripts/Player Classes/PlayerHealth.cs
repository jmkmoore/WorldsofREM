using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public int maxHealth = 100;
	public int currentHealth = 100;
	private GameObject myself;

    public float invulnTime = .5f;
    private float invulnTimer = 0f;
    public bool invuln = false;
    // Use this for initialization
	void Start () {
		myself = gameObject;
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (invuln)
        {
            invulnTimer += Time.deltaTime;
        }
        if (invulnTimer > invulnTime)
        {
            invuln = false;
        }
		
						
	}

	public void adjustCurrentHealth(int adj){
        if (adj < 0)
        {
            if(!invuln){
                currentHealth += adj;
                if (currentHealth > 100)
                    currentHealth = maxHealth;
                if (currentHealth < 1)
                    currentHealth = 0;
                invuln = true;
                invulnTimer += Time.deltaTime;
            }
        }
        if (adj > 0)
        {
            currentHealth += adj;
            if (currentHealth > 100)
                currentHealth = maxHealth;
        }
		

		TienGUI.getInstance().LifeBar = ((float)currentHealth / (float)maxHealth);
		}
		

	}

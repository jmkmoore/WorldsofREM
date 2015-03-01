using UnityEngine;
using System.Collections;

public class FallApart : MonoBehaviour {

    public float destroyTriggerTime = 3f;
    public float destroyTimer = 0;

    public CharacterController2D controller;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (destroyTimer != 0)
        {
            destroyTimer += Time.deltaTime;
        }
        
        if (destroyTimer >= destroyTriggerTime)
        {
            DestroyObject(gameObject);
        }

	}

    void OnColliderEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        destroyTimer += 1;
    }

    void OnColliderExit2D(Collider2D other)
    {
        controller = null;
    }
}


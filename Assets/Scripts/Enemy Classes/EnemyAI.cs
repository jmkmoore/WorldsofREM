using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public Transform target;
	public int moveSpeed;
	public int rotationSpeed;
	public int maxDistance;
	public int minDistance;

    // movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;

    private Animator animator;

	private Transform myTransform;
    private Vector3 _velocity;



	void Awake(){
		myTransform = transform;

        animator = GetComponent<Animator>();

	}

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag ("Player");
		target = go.transform;

		maxDistance = 10;
		minDistance = 5;
	}
	
	// Update is called once per frame
	void Update () {
        



	}
}

using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public Transform target;
	public int moveSpeed;
	public int rotationSpeed;
	public int maxDistance;
	public int minDistance;

	private Transform myTransform;

	void Awake(){
		myTransform = transform;
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
//		Debug.DrawLine (target.transform.position, myTransform.position, Color.red);

		//Look at target
		if (Vector2.Distance (target.position, myTransform.position) < maxDistance) {
						myTransform.rotation = Quaternion.Slerp (myTransform.rotation, Quaternion.LookRotation (target.position - myTransform.position), rotationSpeed * Time.deltaTime);

						if (Vector2.Distance (target.position, myTransform.position) > minDistance) {
								//Move towards target
								myTransform.position += myTransform.forward * 4 * moveSpeed * Time.deltaTime;
						}
				} else {
			myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
				}
	}
}

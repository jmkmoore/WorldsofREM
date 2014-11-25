using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targetting : MonoBehaviour {
	public List<Transform> targets;
	public Transform selectedTarget;
	private Transform myTransform;
	// Use this for initialization
	void Start () {
		targets = new List<Transform>();
		addAllEnemies ();
		selectedTarget = null;
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Tab)){
			TargetEnemy();
		}
	}

	public void addAllEnemies(){
		GameObject[] go = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach(GameObject enemy in go){
			AddTarget(enemy.transform);
		}
	}

	private void sortTargetsDistance(){
		targets.Sort(delegate(Transform x, Transform y) {
			return Vector3.Distance(x.position, myTransform.position).CompareTo (Vector3.Distance (y.position, myTransform.position));});
	}

	public void AddTarget(Transform enemy){
		targets.Add (enemy);
		}

	private void TargetEnemy(){
				if (selectedTarget == null) {
						sortTargetsDistance ();
						selectedTarget = targets [0];
				} else {
						int index = targets.IndexOf (selectedTarget);
						if (index < targets.Count - 1)
								index++;
						else {
								index = 0;
						}
			DeselectTarget ();
			selectedTarget = targets [index];
				}
		SelectTarget ();
		}
	private void SelectTarget(){
				selectedTarget.renderer.material.color = Color.blue;
		PlayerAttack pa = (PlayerAttack)GetComponent ("PlayerAttack");
		pa.target = selectedTarget.gameObject;
		}

	private void DeselectTarget(){
		selectedTarget.renderer.material.color = Color.grey;
		selectedTarget = null;
	}

}


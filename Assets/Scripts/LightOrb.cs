using UnityEngine;
using System.Collections;

public class LightOrb : MonoBehaviour {
	
	float timeRemaining;

	// Use this for initialization
	void Start () {
		timeRemaining = Settings.getInstance().OrbTime;
	}
	
	// Update is called once per frame
	void Update () {

		timeRemaining -= Time.deltaTime;
		Color color = renderer.material.color;
		color.a = timeRemaining / Settings.getInstance().OrbTime;;
		renderer.material.color = color;
		if(timeRemaining < 0){
			LightController.OrbCount--;
			DestroyImmediate(this.gameObject);
		}

	}

	void OnMouseOver(){
		if(Input.GetMouseButtonDown(1)){
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			player.GetComponent<CharacterController>().Move(transform.position - player.transform.position);
			LightController.OrbCount--;
			DestroyImmediate(this.gameObject);
		}
	}
}

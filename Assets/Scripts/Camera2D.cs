using UnityEngine;
using System.Collections;

public class Camera2D : MonoBehaviour {

	public float CamSpeed;

	public float xMin;
	public float xMax;

	public float yMin;
	public float yMax;

	public float NeutralZoneWidth;
	public float NeutralZoneHeight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(GameObject.FindGameObjectWithTag("Player").transform.position.x - transform.position.x) > NeutralZoneWidth){
			//Camera needs to be panned on the x axis
			if(GameObject.FindGameObjectWithTag("Player").transform.position.x > transform.position.x && transform.position.x + CamSpeed <= xMax){
				transform.position += new Vector3(CamSpeed,0,0);
			}else if(GameObject.FindGameObjectWithTag("Player").transform.position.x < transform.position.x && transform.position.x - CamSpeed >= xMin){
				transform.position -= new Vector3(CamSpeed,0,0);
			}
		}

		if(Mathf.Abs(GameObject.FindGameObjectWithTag("Player").transform.position.y - transform.position.y) > NeutralZoneHeight){
			//Camera needs to be panned on the y axis
			if(GameObject.FindGameObjectWithTag("Player").transform.position.y > transform.position.y && transform.position.y + CamSpeed <= yMax){
				transform.position += new Vector3(0,CamSpeed,0);
			}else if(GameObject.FindGameObjectWithTag("Player").transform.position.y < transform.position.y && transform.position.y - CamSpeed >= yMin){
				transform.position -= new Vector3(0,CamSpeed,0);
			}
		}
	}
}

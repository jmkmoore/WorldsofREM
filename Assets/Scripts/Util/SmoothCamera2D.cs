using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	public Vector2 Camera_Min;
	public Vector2 Camera_Max;

	private Vector3 point;
	private Vector3 delta;
	private Vector3 destination;
	

	// Update is called once per frame
	void Update () 
	{
		if (target)
		{
			point = camera.WorldToViewportPoint(target.position);
			delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.4f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			destination = transform.position + delta;
			CheckBounds();
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}else{
			target = GameObject.FindGameObjectWithTag("Player").transform;
			if (target)
			{
				point = camera.WorldToViewportPoint(target.position);
				delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
				destination = transform.position + delta;
				CheckBounds();
				transform.position = destination;
			}
		}
	}

	void CheckBounds(){
		if(destination.x > Camera_Max.x) destination.x = Camera_Max.x;
		else if(destination.x < Camera_Min.x) destination.x = Camera_Min.x;
		if(destination.y > Camera_Max.y) destination.y = Camera_Max.y;
		else if(destination.y < Camera_Min.y) destination.y = Camera_Min.y;
	}
}
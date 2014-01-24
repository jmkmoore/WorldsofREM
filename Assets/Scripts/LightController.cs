using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {


	public static int OrbCount = 0;

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) && OrbCount < Settings.getInstance().MaxOrbs){
			Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			newPos.z = 0;
			if(!Physics.CheckSphere(newPos,.5f,1)){
				OrbCount++;
				Instantiate(Resources.Load("Prefab/Light"),newPos,Quaternion.identity);
			}
		}
	}
}

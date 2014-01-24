using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour{

	public float OrbTime;
	
	public float DashSpeed;

	public int MaxOrbs;

	static Settings instance;

	void Awake(){
		instance = this;
	}

	public static Settings getInstance ()
	{
		return instance;
	}



}

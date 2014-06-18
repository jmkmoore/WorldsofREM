using UnityEngine;
using System.Collections;

public class PropertyManager : MonoBehaviour{

	private static PropertyManager _instance;


	public float RunSpeed;
	public float JumpHeight;
	

	void Awake ()
	{
		_instance = this;
	}

	public static PropertyManager getInstance ()
	{
		return _instance;
	}

}

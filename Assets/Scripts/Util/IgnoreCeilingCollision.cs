using UnityEngine;
using System.Collections;

public class IgnoreCeilingCollision : MonoBehaviour {

	public bool oneWay = false;
	public EdgeCollider2D platform;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		platform.enabled = !oneWay;	
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		oneWay = true;
	}
	
	
	void OnTriggerExit2D(Collider2D other)
	{
		oneWay = false;
	}
}﻿
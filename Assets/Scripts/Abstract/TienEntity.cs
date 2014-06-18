using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TienEntity : MonoBehaviour {

	public int EID;
	public Dictionary<string, string> properties;

	public bool IsAlive = true;
	public bool TouchingPlayer = false;

}

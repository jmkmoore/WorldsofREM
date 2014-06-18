using UnityEngine;
using System.Collections;

public class DeathTile : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player") other.GetComponent<Player>().KillPlayer();
	}
}

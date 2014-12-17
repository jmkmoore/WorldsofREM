using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Player : TienEntity {


	public static TienEntity CreatePlayer(Dictionary<string,string> props){
		GameObject entity = GameObject.Instantiate(Resources.Load("Prefab/Objects/Player")) as GameObject;
		TienEntity player = entity.GetComponent<TienEntity>();
		player.properties = props;
		player.IsAlive = true;

		GameObject bullet = GameObject.Instantiate (Resources.Load ("Prefab/AttackObjects/bullet")) as GameObject;


		GameManager.getInstance().RegisterEntity(player);
		return player;
	}

	abstract protected void ApplyInputs();
	abstract protected void SetAnimationParameters();
	abstract public void JumpAction();
	abstract public void UseAction();
	abstract public void DashAction();
	abstract public void SetGrounded(bool ground);
	abstract public bool GetGrounded();
	abstract public void KillPlayer();
	abstract public void setDirection(bool rightOrLeft);
	abstract public void lightGunAction();
}

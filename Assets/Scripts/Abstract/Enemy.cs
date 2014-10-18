using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : TienEntity {
	
	
	public static TienEntity CreateEnemy(Dictionary<string,string> props){
		Debug.Log (props.ContainsKey("EnemyType"));
		GameObject entity = GameObject.Instantiate(Resources.Load("Prefab/Objects/Enemy/"+props["EnemyType"])) as GameObject; 
		TienEntity enemy = entity.GetComponent<TienEntity>();
		enemy.properties = props;
		enemy.IsAlive = true;

		GameManager.getInstance().RegisterEntity(enemy);
		return enemy;
	}
	

	abstract public void PlaceEnemyAtCoordinates(Vector2 Coords);

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Static : TienEntity {
	
	
	public static TienEntity CreateStatic(Dictionary<string,string> props){
		GameObject entity = GameObject.Instantiate(Resources.Load("Prefab/Objects/Static/"+props["StaticType"])) as GameObject; 
		TienEntity stat = entity.GetComponent<TienEntity>();
		stat.properties = props;
		stat.IsAlive = true;

		GameManager.getInstance().RegisterEntity(stat);
		return stat;
	}
}

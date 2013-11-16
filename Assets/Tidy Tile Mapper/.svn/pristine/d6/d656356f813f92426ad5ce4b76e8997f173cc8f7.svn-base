using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DopplerInteractive.TidyTileMapper.Utilities;

public class EntityController : MonoBehaviour {
	
	//We will keep a single reference to this bad-boy
	//It will be very handy to know this
	public Entity playerEntity;
	
	//We keep these in a list (instead of a dictionary by position)
	//Until we've finished our grand "Entity project"
	//At that stage, we can make decisions about how to store these nicely
	public List<Entity> entities = new List<Entity>();
	
	//We register entity positions by Entity category and position
	//Thus no two entities of the same type can share a position
	Dictionary<string,HashSet<Vector3>> reservationRegister = new Dictionary<string, HashSet<Vector3>>();
		
	//Register the player - we keep a single reference to this as we'll 
	//likely be querying against it quite frequently
	public void RegisterPlayer(Entity playerEntity){
		this.playerEntity = playerEntity;
	}
	
	public bool HasCollidedWithPlayer(Vector3 coords){
		
		Vector3 playerCoords = new Vector3(playerEntity.x,playerEntity.y,playerEntity.z);
		
		return (playerCoords == coords);
		
	}
	
	void CheckRegister(string entityType){
		
		if(reservationRegister.ContainsKey(entityType)){
			return;
		}
		
		reservationRegister.Add(entityType,new HashSet<Vector3>());
		
	}
	
	//Reserve this position for the entity
	public void ReserveBlock(string entityType, Vector3 coords){
		
		CheckRegister(entityType);
		
		reservationRegister[entityType].Add(coords);
	}
	
	//Is this position reserved by anything at all?
	//Useful for basic pathfinding
	public bool IsReservedByAny(Vector3 coords){
		
		foreach(string s in reservationRegister.Keys){
			
			if(IsReserved(s,coords)){
				return true;
			}
			
		}
		
		return false;
		
	}
	
	//Is this position reserved by the entity type specified?
	public bool IsReserved(string entityType, Vector3 coords){
		
		CheckRegister(entityType);
		
		return reservationRegister[entityType].Contains(coords);
	}
	
	//Unreserve the position currently reserved by this entity type
	public void UnreserveBlock(string entityType, Vector3 coords){
		
		CheckRegister(entityType);
		
		reservationRegister[entityType].Remove(coords);
	}
		
	//Return all entities at the given position
	public List<Entity> GetEntitiesAt(int x, int y, int z){
					
		List<Entity> ce = new List<Entity>();
		
		for(int i = 0; i < entities.Count; i++){
			
			Entity e = entities[i];
			
			if(x == e.x && y == e.y && z == e.z){
				ce.Add(e);
			}
			
		}
		
		return ce;
		
	}
	
	//Subscribe an entity as being 'alive'
	public void SubscribeEntity(Entity e){
		entities.Add(e);
	}
	
	//Unsubscribe an entity
	public void UnSubscribeEntity(Entity e){
		entities.Remove(e);
	}
	
	//Singleton-esque behaviour allowing entities to retrieve this controller statically
	//It will also create a new controller if one does not currently exist in the map
	
	static EntityController instance = null;
	
	public static EntityController GetInstance(){
		
		if(instance == null){
			instance = GameObject.FindObjectOfType(typeof(EntityController)) as EntityController;
		}
		
		if(instance == null){		
			//We'll need to attach one
			GameObject o = new GameObject("Entity Controller");
			instance = o.AddComponent<EntityController>();
		}
		
		return instance;
		
	}
	
	//A high-level movement query - useful for basic pathfinding
	public bool CanMoveTo(int x, int y, int z){
		
		Vector3 mVector = new Vector3(x,y,z);
				
		if(IsReservedByAny(mVector)){
			return false;
		}
		
		return true;
		
	}
	
	void OnDrawGizmos(){
		
		BlockMap map = GameObject.FindObjectOfType(typeof(BlockMap)) as BlockMap;
		
		foreach(string s in reservationRegister.Keys){
		
			List<Vector3> v = new List<Vector3>(reservationRegister[s]);
			
			Gizmos.color = new Color(0.0f,1.0f,0.0f,0.5f);
			
			for(int i = 0; i < v.Count; i++){
				
				Vector3 pos = BlockUtilities.GetMathematicalPosition(map,(int)v[i].x,(int)v[i].y,(int)v[i].z);
				
				pos = map.transform.TransformPoint(pos);
				
				Gizmos.DrawSphere (pos,0.5f);
				
			}
		}
	}
	
	
}

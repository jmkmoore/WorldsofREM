using UnityEngine;
using System;
using System.Collections;
using DopplerInteractive.TidyTileMapper.Utilities;
using System.Collections.Generic;

//Entity is the fundamental base class for moving entities
public class Entity : TidyMapBoundObject {
		
	//Entity type provides a 'category' of entity. You can use this for... entity-specific logic and such
	public string entityType = "Default";
	
	//We will host the core animations in this class, as all deriving classes will 
	//utilise these
	public AnimationPackage anim_idle;
	public AnimationPackage anim_moving;
	
	public  bool isMoving = false;
	public  bool isIdle = false;
	
	//Should we automatically reserve positions as we move around?
	public bool automaticallyReservePosition = true;
	
	//Should we initialize this character on Awake, or at some arbitrary time?
	//This is useful when you are adding a character to a procedural map, and need it to be initialized once the map is generated
	public bool initializeOnAwake = true;
	
	//This handles gravity for platforms
	protected float arbitraryGroundPoint = 0.0f;
	protected bool arbitraryGroundPointEnabled = false;
	protected float yGroundOffset = 0.0f;
		
	void Awake(){
		
		if(initializeOnAwake){
			InitializeEntity();
		}
	}
	
	#region Minor state handling
	
	public bool IsMoving(){
		return isMoving;
	}
	
	public bool IsIdle(){
		return isIdle;
	}
	
	public void SetIsMoving(bool isMoving){
		this.isMoving = isMoving;	
	}
	
	public void SetIsIdle(bool isIdle){
		this.isIdle = isIdle;
	}
	
	#endregion
		
	//The current coordinates of the entity
	//These are public for debugging purposes
	public int x,y,z;
	
	//A link to the entity controller
	protected EntityController controller;
		
	//The cached transform of the entity
	protected Transform eTransform;
		
	//We must initialize the entity to the map that we wish to use, if it's not been
	//done via parameters
	public void InitializeEntity(BlockMap map){
		
		this.parentMap = map;
		
		InitializeEntity();
		
		OnInitializeEntity();
		
	}
	
	//If we've already done this, it's OK. It is still nice to have a standard initialization function
	//from which we can derive
	//We won't do this on Awake or Start, however - we will leave this to the discretion of the developer
	//It depends on when they wish to create their map and bind their entities
	public void InitializeEntity(){
						
		//We'll cache our transform, as we'll be calling it frequently in order to check our position
		eTransform = transform;
				
		eTransform.parent = parentMap.transform;
		
		this.controller = EntityController.GetInstance();
				
		this.controller.SubscribeEntity(this);
		
		//Initialize our ground position
		Collider c = gameObject.collider;
		
		if(c != null){
			
			yGroundOffset = c.bounds.center.y - c.bounds.min.y;
			
		}
		
		InitializeMapPosition();
		
		OnInitializeEntity();
	}
	
	//Called on initialization
	public virtual void OnInitializeEntity(){}
	
	public void DestroyEntity(){
		
		this.controller.UnSubscribeEntity(this);
		
		OnDestroyEntity();
		
		AssetPool.Destroy(gameObject);
	}
	
	public virtual void OnDestroyEntity(){}
	
	void Update(){
		
		UpdateEntity (Time.deltaTime);
		
	}
	
	public void UpdateEntity(float deltaTime){
		
		UpdateAnimation(deltaTime);
		
		OnUpdateEntity(deltaTime);
		
		UpdateMapPosition();
	}
	
	public void UpdateAnimation(float deltaTime){
		
		if(IsIdle()){
			anim_idle.Play();
		}
		else if(IsMoving()){
			anim_moving.Play();
		}
		
		OnUpdateAnimation(deltaTime);
	}
	
	//Deriving classes should override these functions in order to extend this class
	//E.g: Add their own animation states
	public virtual void OnUpdateAnimation(float deltaTime){}
	
	public virtual void OnUpdateEntity(float deltaTime){}
	
	//We need to check if we have changed position
	public void UpdateMapPosition(){
				
		Vector3 pos = BlockUtilities.GetMathematicalCoordinates(parentMap,eTransform.position);
		
		//We temporarily store these values and will check against changes
		int t_x = (int)pos.x;
		int t_y = (int)pos.y;
		int t_z = (int)pos.z;
		
		//If our position has changed at all
		if(t_x != x || t_y != y || t_z != z){
			
			HandleBlockExit(x,y,z);
						
			HandleBlockEntry(t_x,t_y,t_z);
			
		}
		
	}
	
	//We wish to set the coordinates without triggering any Entry/Exit events
	protected void InitializeMapPosition(){
		
		Vector3 pos = BlockUtilities.GetMathematicalCoordinates(parentMap,eTransform.position);
		
		this.x = (int)pos.x;
		this.y = (int)pos.y;
		this.z = (int)pos.z;
		
		controller.ReserveBlock(entityType,new Vector3(x,y,z));
		
	}
	
	#region Block events
	
	//Trigger-styled events that will send Block exit / Block entry messages
	public void HandleBlockExit(int x, int y, int z){
		
		Block b = BlockUtilities.GetBlockAt(parentMap,x,y,z);
		
		if(b != null){
			
			b.ObjectExitedBlock(this);
			
		}
		
		if(automaticallyReservePosition){
			controller.UnreserveBlock(entityType,new Vector3(x,y,z));
		}
		
		OnBlockExit(x,y,z);
	}
	
	public void HandleBlockEntry(int x, int y, int z){
		
		Block b = BlockUtilities.GetBlockAt(parentMap,x,y,z);
		
		if(b != null){
			
			b.ObjectEnteredBlock(this);
			
		}
		
		List<Entity> ce = controller.GetEntitiesAt(x,y,z);
		
		for(int i = 0; i < ce.Count; i++){
			ce[i].OnEntityCollision(this);
			OnEntityCollision(ce[i]);
		}
		
		this.x = x;
		this.y = y;
		this.z = z;
		
		if(automaticallyReservePosition){
			controller.ReserveBlock(entityType,new Vector3(x,y,z));
		}
		
		OnBlockEntry(x,y,z);
	}
	
	//Called on entry into a block
	public virtual void OnBlockEntry(int x, int y, int z){}
	//Called on exit from a block
	public virtual void OnBlockExit(int x, int y, int z){}
	//Called by the child class somewhere down the line
	//I like to keep it here because it's core to the whole affair
	public virtual void OnReachBlockCenter(int x, int y, int z){}
	
	#endregion
	
	//Called when this entity and another entity share a coordinate on update map position
	public virtual void OnEntityCollision(Entity e){
			
	}
	
	#region Alternative gravity
	
	public void EnableArbitraryGroundPoint(float groundPoint){
		arbitraryGroundPointEnabled = true;
		this.arbitraryGroundPoint = groundPoint;
	}
	
	public void DisableArbitraryGroundPoint(){
		arbitraryGroundPointEnabled = false;
	}
	
	#endregion
	
	#region implemented abstract members of TidyMapBoundObject
	public override void OnObjectEnterBlock (Block b, TidyMapBoundObject e){
		
	}

	public override void OnObjectExitBlock (Block b, TidyMapBoundObject e){
		
	}
	#endregion
}

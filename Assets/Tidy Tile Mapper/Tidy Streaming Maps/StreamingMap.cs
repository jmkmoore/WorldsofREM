using UnityEngine;
using System.Collections;
using DopplerInteractive.TidyTileMapper.Utilities;
using System.Collections.Generic;

public class StreamingMap : MonoBehaviour{
	
	//A 2D map (faux 2D) holding the prefabs of blocks in the map
	//If we wish to change a block, we should change this
	[HideInInspector]
	public Block[] prefabMap;
	
	//The width/height/depth of the prefab map
	//[HideInInspector]
	public int width;
//	[HideInInspector]
	public int height;
	//[HideInInspector]
	public int depth;
	
	//The blockmap that is created at runtime
	public BlockMap blockMap;
	
	//The current point of focus (the character's coordinates)
	public int focus_x;
	public int focus_y;
	public int focus_z;
	
	//I really can't remember what this does right now.
	Vector3 vCoords;
	
	//The radius within which blocks will be drawn
	public int drawRadius;
	
	//We use HashSets for our lists of blocks to be created and destroyed
	//and the list of blocks that is currently in focus
	//They have O(1) access time, which is blazingly fast
	HashSet<Vector3> currentlyInFocus = new HashSet<Vector3>();
	
	HashSet<Vector3> toBeCreated = new HashSet<Vector3>();
	HashSet<Vector3> toBeDestroyed = new HashSet<Vector3>();
	
	//We spread our destroy/create functions over multiple frames
	//in order to make it even more delicious and efficient
	
	//This indicates how often the delete / create function should be called.
	//0.0f = every frame
	float deleteRate = 0.0f;
	float createRate = 0.0f;
	float lastDelete = 0.0f;
	float lastCreate = 0.0f;
	
	//This indicates how many times the delete / create function should be called per call
	int actionsPerPass = 3;
	
	public bool generateMapOnAwake = false;
		
	//We sort our lists according to proximity to the player
	//In this manner we do all we can to avoid "Popping"
	void SortAllCollections(){
		
		List<Vector3> tbc = new List<Vector3>(toBeCreated);
		tbc.Sort(new CreateComparer(this));
		toBeCreated = new HashSet<Vector3>(tbc);
		
		List<Vector3> tbd = new List<Vector3>(toBeDestroyed);
		tbd.Sort(new DestroyComparer(this));
		toBeDestroyed = new HashSet<Vector3>(tbd);
	}
	
	//Initialize the prefab map (create the array)
	public void InitializePrefabMap(int width, int height, int depth){

		if(prefabMap == null || prefabMap.Length == 0){
	
			this.width = width;
			this.height = height;
			this.depth = depth;
			
			prefabMap = new Block[width * height * depth];
		}
	}
	
	public void ClearPrefabMap(){
		prefabMap = null;
	}
	
	//Initialize the streaming map (create the prefab map, enable pooling)
	void InitializeStreamingMap(int width, int height, int depth){
		
		InitializePrefabMap(width,height,depth);
		
		AssetPool.EnablePooling();
	}
	
	public Vector3 GetItemFromSet(HashSet<Vector3> hashSet){
		foreach(Vector3 v in hashSet){
			return v;
		}
		
		return Vector3.zero;
	}
	
	//Retrieve the prefab object at the given coordinates
	//We need to generate our index as it is a 1D array
	public Block GetBlockPrefabAt(int x, int y, int z){
		
		int index =x+y*width+(z*width*depth);
		
		if(index < 0 || index >= prefabMap.Length){
			return null;
		}
		
		return prefabMap[index];
		
	}
	
	//Is this coordinate within our radius?
	bool IsWithinRadius(Vector3 focus, Vector3 target){
		
		float dist = Mathf.Abs(Vector3.Distance(target,focus));
							
		float n = 1.0f - ((dist) / (float)(drawRadius+1.0f));
		
		if(n < 0.0f){
			return false;
		}
		
		return true;
		
	}
	
	//Set the prefab at this coordinate to... the desired prefab
	public void SetBlockPrefabAt(Block blockPrefab, int x, int y, int z){
				
		int index =x+y*width+(z*width*depth);
		
		if(index < 0 || index >= prefabMap.Length){
						
			return;
		}
		
		prefabMap[index] = blockPrefab;
		
	}
	
	public void OutputPrefabMap(){
		
		string s = "";
		
		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				
				Block b = GetBlockPrefabAt(x,y,1);
				
				s += (b != null ? b.name : "Null")[0] + ",";
			}
			
			s += "\n";
		}
		
		Debug.Log(s);
	}
	
	//Generate our map
	public void GenerateMap(int width, int height, int depth,
		string mapName, Vector3 tileScale, 
		int chunkWidth, int chunkHeight, 
		BlockMap.GrowthAxis growthAxis,
		float deleteUpdateRate,float createUpdateRate,int actionsPerPass){
				
		this.deleteRate = deleteUpdateRate;
		this.createRate = createUpdateRate;
		this.actionsPerPass = actionsPerPass;
		
		if(this.blockMap == null){
			
			InitializeStreamingMap (width,height,depth);
			
			this.blockMap = BlockUtilities.CreateBlockMap(mapName,tileScale,chunkWidth,chunkHeight,growthAxis);
						
		}
		
	}
	
	//This is the meat of the function
	//It indicates to the map that it is time to redraw given the current focus
	//within the given radius
	//If bypass queue is true - it will not add the action to the staggered queue of actions,
	//but instead just do it right now (call this on the first frame from your character)
	public void DrawMap(int focus_x, int focus_y, int focus_z, int radius, bool bypassQueue){
		
		if(blockMap == null){
			Debug.LogWarning("No block map: aborting");
			return;
		}
		
		this.focus_x = focus_x;
		this.focus_y = focus_y;
		this.focus_z = focus_z;
		
		this.vCoords = new Vector3(focus_x,focus_y,focus_z);
		
		this.drawRadius = radius;
		
		Vector3 focus = new Vector3(focus_x,focus_y,focus_z);
				
		HashSet<Vector3> newFocus = new HashSet<Vector3>();
		
		for(int x1 = focus_x - radius; x1 <= focus_x + radius; x1++){
			
			if(x1 < 0 || x1 >= width){
				continue;
			}
			
			for(int y1 = focus_y - radius; y1 <= focus_y + radius; y1++){
				
				if(y1 < 0 || y1 >= height){
					continue;
				}
				
				for(int z1 = focus_z - radius; z1 <= focus_z + radius; z1++){
					
					if(z1 < 0 || z1 >= depth){
						continue;
					}
					
					Vector3 f = new Vector3(x1,y1,z1);
					
					if(IsWithinRadius(focus,f)){
						
						newFocus.Add (f);
						
					}
				}	
			}
		}
				
		foreach(Vector3 v in currentlyInFocus){
			if(!newFocus.Contains(v)){
				
				if(bypassQueue){
					DestroyBlock(v);
				}
				else{
					DeleteBlock(v);
				}
			}
		}
				
		foreach(Vector3 v in newFocus){
					
			if(!currentlyInFocus.Contains(v)){
								
				if(bypassQueue){
					InstantiateBlock(v);
				}
				else{
					AddBlock(v);
				}
				
			}
		}
		
		currentlyInFocus = newFocus;
		
		SortAllCollections();
		
	}
	
	//Update all of our queues!
	void Update(){
		UpdateDeletion (Time.deltaTime);
		UpdateCreation(Time.deltaTime);
	}
	
	//Update our delete queue, delete if our timer says to
	void UpdateDeletion(float deltaTime){
		lastDelete += deltaTime;

		if(lastDelete >= deleteRate){
		
			lastDelete = lastDelete - deleteRate;
			
			for(int i = 0; i < actionsPerPass; i++){
				if(toBeDestroyed.Count > 0){
								
					Vector3 v = GetItemFromSet(toBeDestroyed);
					
					DestroyBlock(v);
					toBeDestroyed.Remove(v);
				}
			}
			
		}

	}
	
	//Update our create queue, create if our timer says to
	void UpdateCreation(float deltaTime){
		lastCreate += deltaTime;
	
		if(lastCreate >= createRate){
		
			lastCreate = lastCreate - createRate;
			
			for(int i = 0; i < actionsPerPass; i++){
				if(toBeCreated.Count > 0){
					
					Vector3 v = GetItemFromSet(toBeCreated);
					
					InstantiateBlock(v);
					toBeCreated.Remove(v);
				}
			}
		}
	
	
	}
		
	//Add this block to the destroy queue
	void DeleteBlock(Vector3 coords){
		
		if(toBeDestroyed.Contains(coords)){
			return;
		}
		
		if(toBeCreated.Contains(coords)){
			toBeCreated.Remove(coords);
		}
		
		toBeDestroyed.Add(coords);
	}
	
	//Add this block to the create queue
	void AddBlock(Vector3 coords){
		
		if(toBeCreated.Contains(coords)){
			return;
		}
		
		if(toBeDestroyed.Contains(coords)){
			toBeDestroyed.Remove(coords);
		}
		
		toBeCreated.Add(coords);
	}
	
	void DestroyBlock(Vector3 coords){
      		BlockUtilities.AddBlockToMap(blockMap,null,false,0,true,(int)coords.x,(int)coords.y,(int)coords.z,false,false);
	}
	
	//Instantiate our block! Wrap the AssetPool functions nicely
	void InstantiateBlock(Vector3 coords){
		
		int x = (int)coords.x;
		int y = (int)coords.y;
		int z = (int)coords.z;
		
		Block b = GetBlockPrefabAt(x,y,z);
				
		Block toAdd = null;
		
		if(b != null){
			
			GameObject o = AssetPool.Instantiate(b.gameObject) as GameObject;
			
			o.SetActiveRecursively (true);
			
			toAdd = o.GetComponent<Block>();
			
		}
				
		BlockUtilities.AddBlockToMap(blockMap,toAdd,false,0,true,x,y,z,false,false);
		
	}
	
	public BlockMap GetMap(){
		return blockMap;
	}
	
	public void SetMap(BlockMap map){
		this.blockMap = map;
		
		InitializeStreamingMap(
					BlockUtilities.GetMapWidth(map),
					BlockUtilities.GetMapHeight(map),
					BlockUtilities.GetMapUpperDepth(map) - BlockUtilities.GetMapLowerDepth(map) + 1
					);
	}
	
	//A few custom comparers that help us be ultra-efficient
	
	public class CreateComparer : IComparer<Vector3>{
	
		StreamingMap map;
		
		public CreateComparer(StreamingMap map){
			this.map = map;
		}
		
		#region IComparer[Vector3] implementation
		public int Compare (Vector3 x, Vector3 y)
		{
			float a = Mathf.Abs (Vector3.Distance(x,map.vCoords));
			float b = Mathf.Abs (Vector3.Distance(y,map.vCoords));
			
			return (int)(a - b);
		}
		#endregion
		
	}
	
	public class DestroyComparer : IComparer<Vector3>{
	
		StreamingMap map;
		
		public DestroyComparer(StreamingMap map){
			this.map = map;
		}
		
		#region IComparer[Vector3] implementation
		public int Compare (Vector3 x, Vector3 y)
		{
			float a = Mathf.Abs (Vector3.Distance(x,map.vCoords));
			float b = Mathf.Abs (Vector3.Distance(y,map.vCoords));
			
			return (int)(b - a);
		}
		#endregion
		
	}
}

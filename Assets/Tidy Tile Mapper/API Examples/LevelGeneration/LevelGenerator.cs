using UnityEngine;
using System.Collections;
using DopplerInteractive.TidyTileMapper.Utilities;

public abstract class LevelGenerator : MonoBehaviour {
	
	//In this episode, we're going to generate a single level
	//By populating an array of boolean values, and then
	//instantiating them
	
	//We'll use the same paramaters as we used in the Runtime_Demo script
	
	#region Map Creation Parameters
	
	//These parameters are identical to those used in the Tidy Tile Mapper UI
	
	//The name of the created map
	public string mapName = "Demo Level";
		
	//The dimensions of the blocks to be used
	public Vector3 tileSize = new Vector3(1.0f,1.0f,1.0f);
	
	//The dimensions of the chunks that will be created
	public int chunkWidth = 5;
	public int chunkHeight = 5;
	
	public BlockMap.GrowthAxis growthAxis;
	
	#endregion
	
	#region Map Population Parameters
	
	//The dimensions of the map to be created
	public int levelWidth = 10;
	public int levelHeight = 10;
	public int levelDepth = 1;
	
	//The block prefab that we will use to create our demonstration map
	public Block blockPrefab;
		
	//This will randomise the variants of the blocks that we add to the map
	public bool randomiseInitialMap = false;
	
	//Will we use empty blocks, or no block at all, in empty slots?
	public bool useEmptyBlocks = false;
	
	#endregion
	
	protected BlockMap createdMap;
	
	// Use this for initialization
	void Start () {
	
		ConstructLevel();
		
	}
	
	//We'll construct the map here
	void ConstructLevel(){
		
		bool[,] map = GetLevelMap(levelWidth, levelHeight);
				
		//We'll go ahead and create our map now
		createdMap = BlockUtilities.CreateBlockMap(mapName,tileSize,chunkWidth,chunkHeight,growthAxis);
		
		//We're just going to iterate through the level we got back from the
		//function (trusting that it's correctly sized)
		for(int x = 0; x < levelWidth; x++){
			for(int y = 0; y < levelHeight; y++){
				
				if(map[x,y]){
					
					Block b = GameObject.Instantiate(blockPrefab) as Block;
					
					//We'll add our instantiated block here
					//And we will not refresh just yet -
					//it's better to refresh and clean at the end
					BlockUtilities.AddBlockToMap(createdMap,b,false,0,false,x,y,0,false,false);
					
				}
				else{
					
					if(useEmptyBlocks){
						BlockUtilities.AddBlockToMap(createdMap,null,false,0,false,x,y,0,false,true);
					}
					
				}
				
			}
		}
		
		//And now we refresh our map
		BlockUtilities.RefreshMap(createdMap,randomiseInitialMap);
				
		//Done!
		//Enjoy!
		
	}
	
	//And split our bool[,] generation to here
	//If you want to see your level created, replace this!
	public abstract bool[,] GetLevelMap(int levelWidth, int levelHeight);
}

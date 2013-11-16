using UnityEngine;
using System.Collections;
using DopplerInteractive.TidyTileMapper.Utilities;

public class Runtime_Demo : MonoBehaviour {
	
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
	
	#endregion
	
	#region Map Modification Parameters
	
	public float modificationRate = 0.5f;
	float lastModification = 0.0f;
	
	BlockMap createdMap;
	
	#endregion
	
	void Awake(){
		
		//First we will create our map using the parameters we have set
		createdMap = BlockUtilities.CreateBlockMap(mapName,tileSize,chunkWidth,chunkHeight,growthAxis);
		
		//Now we will create a big old block of cubes, according to the dimensions we've set above
		for(int x = 0; x < levelWidth; x++){
			for(int y = 0; y < levelWidth; y++){
				for(int z = 0; z < levelDepth; z++){
					
					//We instantiate the block outside of the BlockUtilites library
					//I've made it this way to allow you to pool GameObjects or anything you like 
					//So you have control over where your blocks come from
					Block b = GameObject.Instantiate(blockPrefab) as Block;
					
					//Now we add the block to the map at the desired coordinates
					//We won't randomise or refresh at this point, as we're creating a large map
					//We will do that as one call later
					BlockUtilities.AddBlockToMap(createdMap,b,false,0,false,x,y,z,false,true);
				}
			}
		}
		
		//Now that our map has been created, we will refresh it
		//Refreshing your map sets your blocks to orient correctly
		//And gives you an opportunity to randomise your variants en'masse
		BlockUtilities.RefreshMap(createdMap,randomiseInitialMap);
		
		
	}
	
	void Update(){
		
		//For demonstration purposes, we're going to modify this map at runtime
		//By randomly adding and removing blocks from the map
		//It's not an entirely meaningful function, but it demonstrates the core functions of 
		//the BlockUtilites class
		lastModification += Time.deltaTime;
		
		if(lastModification >= modificationRate){
			
			lastModification = 0.0f;
			
			RandomlyModifyMap();
			
		}
		
	}
	
	void RandomlyModifyMap(){
		
		//We'll retrieve the bounds of our map in order to pick a nice random block
		int x = UnityEngine.Random.Range(0,BlockUtilities.GetMapWidth(createdMap));
		int y = UnityEngine.Random.Range(0,BlockUtilities.GetMapHeight(createdMap));
		
		//The depth is a little different from the width/height, as depth may be negative
		int z = UnityEngine.Random.Range(BlockUtilities.GetMapLowerDepth(createdMap),BlockUtilities.GetMapUpperDepth(createdMap));
		
		//Here we'll flip a figurative coin to decide if we'll add or remove
		if(UnityEngine.Random.Range (0,2) == 1){
			
			//We'll remove a block in this case
			BlockUtilities.RemoveBlockFromMap(createdMap,x,y,z,true,false);
		}
		else{
			
			//Again, we'll create a block
			Block b = GameObject.Instantiate(blockPrefab) as Block;
			
			//And then add it to the map
			//We'll randomise on addition
			//And pending our CleanMap parameter, we'll refresh the surrounding blocks
			BlockUtilities.AddBlockToMap(createdMap,b,true,0,true,x,y,z,false,true);
			
		}
		
	}
	
}

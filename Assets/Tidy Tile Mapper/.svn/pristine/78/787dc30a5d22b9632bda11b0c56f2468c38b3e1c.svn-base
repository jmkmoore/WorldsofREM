using UnityEngine;
using System.Collections;

public class PerlinGenerator : LevelGenerator {
	
	//An example level generator
	//using Perlin noise
	
	//What we are doing is:
	//
	//Given our level width, we're sampling the Perlin Noise value (between 0.0 and 1.0)
	//and setting this as our 'height cap' for this column.
	//Everything below this on the map will be a block
	//Everything above this will be empty
	//It will result in a nice, rolling level
	
	#region implemented abstract members of LevelGenerator
	public override bool[,] GetLevelMap (int levelWidth, int levelHeight)
	{
		bool[,] map = new bool[levelWidth,levelHeight];
		
		//We will randomise the y value that we pass to the perlin function
		//This will result in a random level everytime
		float yOffset = UnityEngine.Random.value;
		
		for(int x =0; x < levelWidth; x++){
			
			//We'll get our perlin noise value -
			//normalizing our x to pass it as a parameter
			float p = Mathf.PerlinNoise((float)x/(float)levelHeight,yOffset);
			
			//Given this value, we'll get our height cap
			//And set it to allow for an empty column at the top, and 
			//assure it doesn't leave gaps in the bottom
			int ny = (int)(p * (float)levelHeight-2)+1;
			
			//Given this, we'll set all values below this number to be 'true'
			for(int y = ny; y < levelHeight; y++){
				
				map[x,y] = true;
				
			}
			
		}
		
		return map;
		
	}
	#endregion
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamingMapColorProfile : MonoBehaviour {
	
	//Look at all that hiding! It's like living behind the iron curtain
	[HideInInspector]
	public int width;
	[HideInInspector]
	public int height;
	[HideInInspector]
	public int depth = 1;
	
	public string mapName = "Streaming Map";
	public BlockMap.GrowthAxis growthAxis;
	public Vector3 tileScale = new Vector3(1.0f,1.0f,1.0f);
	
	public float deleteUpdateRate = 0.0f;
	public float createUpdateRate = 0.0f;
	
	public int actionsPerPass = 3;
	
	public int chunkWidth = 5;
	public int chunkHeight = 5;
	
	[HideInInspector]
	public float pixelScale = 1.0f;
	[HideInInspector]
	public Texture2D texture = null;
	[HideInInspector]
	public float colorDifferenceBias = 0.05f;
	
	[HideInInspector]
	public List<Color> colors = new List<Color>();
	[HideInInspector]
	public List<Block> blocks = new List<Block>();
	
	public StreamingMap map;
		
	public void SampleColorsFromTexture(){
		
		if(texture != null && pixelScale > 0.0f){
						
			List<Color> newColors = new List<Color>();
			
			//we'll say the interval is pixelscale is the interval for now
			int interval = (int)pixelScale;
			int start_interval = (int)(pixelScale * 0.5f);
			
			int xc = 0,yc = 0;
			
			this.width = texture.width / interval;
			this.height = texture.height / interval;
			
			this.map.ClearPrefabMap();
			this.map.InitializePrefabMap(width,height,1);
			
			for(int x = start_interval; x < texture.width; x+=interval){
				
				yc = 0;
				
				for(int y = start_interval; y < texture.height; y+=interval){
					
					Color c = texture.GetPixel (x,y);
					
					if(!newColors.Contains(c)){
						newColors.Add(c);
					}
					
					yc++;
				}
				
				xc++;
			}
			
			//Sampled, now time to check against our original
			//In this manner, we retain our original links
			
			//Compile please
			
			for(int i = 0; i < newColors.Count; i++){
			
				if(!ColorContains(colors,newColors[i])){
					colors.Add(newColors[i]);
					blocks.Add(null);
				}
				
			}
			
			for(int i = 0; i < colors.Count; i++){
				
				if(!ColorContains(newColors,colors[i])){
					
					colors.RemoveAt(i);
					blocks.RemoveAt(i);
				}
				
			}
			
			//And now rationalise the colors that remain
			//Required for when we change our bias
			
			int upperBound = colors.Count;
			
			for(int i = 0; i < upperBound; i++){
				
				for(int j = 0; j < upperBound; j++){
					if(i == j){
						continue;
					}
					
					if(ColorsEqual (colors[i],colors[j])){
						colors.RemoveAt(j);
						j--;
						upperBound = colors.Count;
					}
					
				}
				
			}
			
		}
		else{
			
			Debug.LogWarning("No dice kemosabe - I cannot sample a texture without a non-null texture and a greater-than-zero pixel scale.");
			
		}
		
	}
	
	bool ColorContains(List<Color> colors, Color color){
		
		//Wow, i've written Color so many times it has lost all meaning
		
		for(int i = 0; i < colors.Count; i++){
			if(ColorsEqual(colors[i],color)){
				return true;
			}
		}
		
		return false;
		
	}
	
	bool ColorsEqual(Color a, Color b){
		
		//At this stage, we use our bias
		//to decide if two colors are similar
		float r_dif = Mathf.Abs(a.r - b.r);
		float g_dif = Mathf.Abs(a.g - b.g);
		float b_dif = Mathf.Abs(a.b - b.b);
		
		float total_dif = (r_dif + g_dif + b_dif) / 3.0f;
				
		//Debug.Log(a.ToString() + " vs " + b.ToString() + " : " + total_dif);
		
		if(total_dif <= colorDifferenceBias){
			
			//Debug.Log(a.ToString() + " equal to " + b.ToString() + " total dif " + total_dif);
			
			return true;
		}
		
		return false;
	}
	
	public int GetColorIndex(Color c){
		
		for(int i = 0; i < colors.Count; i++){
			
			if(ColorsEqual(colors[i],c)){
				return i;
			}
			
		}
		
		Debug.LogWarning("No color found for entry! That's strange! You likely need to resample your texture");
		
		return -1;
		
	}
	
	public void GenerateMapFromTexture(){
		
		if(map == null){
			Debug.LogWarning("Cannot generate a map without first... having a map! Be sure you've sampled a texture.");
			return;
		}
		
		if(pixelScale <= 0.0f){
			Debug.LogWarning("Cannot generate a map without a greater-than-zero pixel scale. Be sure you've sampled a texture.");
			return;
		}
		
		if(texture == null){
			Debug.LogWarning("Cannot generate a map without a texture. Sample a texture, my friend.");
			return;
		}
		
		//Yeah, we sample the texture twice.
		//I'm not happy about it either,
		//But otherwise it'd be like...
		//Make an index map up there ^
		//And then shuffle the indices around when they change
		//
		//And dude.. It's like 5.32pm here. That's clocking-off time.
		//
		//My elephant teapot is almost ready
		//I'll drink tea
		//But i'll leave this.
		//Deal? Deal.
		
		int interval = (int)pixelScale;
		int start_interval = (int)(pixelScale * 0.5f);
					
		int xc = 0,yc = 0;
				
		for(int x = start_interval; x < texture.width; x+=interval){
			
			yc = 0;
			
			for(int y = start_interval; y < texture.height; y+=interval){
				
				Color c = texture.GetPixel (x,y);
				
				int i = GetColorIndex(c);
									
				if(i == -1){
					continue;
				}
				
				map.SetBlockPrefabAt(blocks[i],xc,map.height - yc,1);
								
				yc++;
			}
			
			xc++;
		}
		
		map.generateMapOnAwake = true;
	}
	
		
}

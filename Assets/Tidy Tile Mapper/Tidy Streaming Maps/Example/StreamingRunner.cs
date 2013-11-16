using UnityEngine;
using System.Collections;

public class StreamingRunner : MonoBehaviour {
	
	//This is an example script, demonstrating the drawing of a focus point on a streaming map
	
	public StreamingMap streamingMap;
	public int drawRadius;
	
	public int x = 0;
	public int y = 0;
	public int z = 0;
	
	int width;
	int depth;
	int height;
	
	public int runnerStep = 5;
	
	public float updateRate = 0.2f;
	float lastUpdate = 0.0f;
	
	public bool bypassQueue = false;
	
	void Start(){
		
		this.width = streamingMap.width;
		this.height = streamingMap.height;
		this.depth = streamingMap.depth;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//This is really a big, delayed for loop, iterating through all of the 
		//nodes of the streaming map
		
		lastUpdate += Time.deltaTime;
		
		if(lastUpdate >= updateRate){
			
			lastUpdate = 0.0f;
			
			x+=runnerStep;
			
			if(x >= width){
				 
				x = 0;
				
				y+=runnerStep;
				
				if(y >= height){
					
					y = 0;
					
					z+=runnerStep;
					
					if(z >= depth){
						
						z = 0;
						
					}
					
				}
				
			}
			
			streamingMap.DrawMap(x,y,z,drawRadius,bypassQueue);
			
		}
		
	}
}

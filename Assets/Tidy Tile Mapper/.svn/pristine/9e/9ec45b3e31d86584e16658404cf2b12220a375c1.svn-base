using UnityEngine;
using System.Collections;
using DopplerInteractive.TidyTileMapper.Editors;
using UnityEditor;

public class AssetPoolWindow : EditorWindow
{
	TidyAssetPoolMonitor assetPoolMonitor;
	
	//This is the function that will be called when editing an existing block
	//Via the map window
	[MenuItem("Window/Tidy Asset Pool")]
	public static void Init () {
		EditorWindow.GetWindow(typeof(AssetPoolWindow),false,"Tidy Asset Pool",true); 
	}
	
	void OnGUI(){
				
		assetPoolMonitor.DrawWindow();
		
	}
	
	void OnEnable(){
		
		if(assetPoolMonitor == null){
			
			assetPoolMonitor = new TidyAssetPoolMonitor();
						
			assetPoolMonitor.Initialize(this);
			
		}
		
	}
	
	void OnDestroy(){
		assetPoolMonitor.Destroy();
	}
	
	void OnDisable(){
		
	}
	
	void Update(){
		
		assetPoolMonitor.Update();
		
	}
}

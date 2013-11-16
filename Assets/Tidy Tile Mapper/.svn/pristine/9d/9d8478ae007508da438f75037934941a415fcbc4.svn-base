using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using DopplerInteractive.TidyTileMapper.Editors;

public class MapCreatorWindow : EditorWindow
{
	public TidyBlockMapCreator mapPainter;
	
	//This is the function that will be called when editing an existing block
	//Via the map window
	[MenuItem("Window/Tidy Tile Mapper")]
	public static void Init () {
		EditorWindow.GetWindow(typeof(MapCreatorWindow),false,"Tidy Tile Mapper",true); 
	}
	
	void OnGUI(){
		
		mapPainter.DrawWindow();
		
	}
	
	void OnInspectorUpdate(){
		mapPainter.InspectorUpdate();
	}
	
	void OnEnable(){
		
		//UpdateWindow.Init();
		
		if(mapPainter == null){
			
			mapPainter = new TidyBlockMapCreator();
			
			mapPainter.Initialize(this);
			
			SceneView.onSceneGUIDelegate += mapPainter.DrawScene;
			
		}
		
	}
	
	void OnDestroy(){
		
		mapPainter.Destroy();
		
		SceneView.onSceneGUIDelegate -= mapPainter.DrawScene;
	}
	
	void Update(){
				
		mapPainter.Update();
		
	}
	
	void OnSelectionChange(){
		
		if(mapPainter == null){
			Debug.LogWarning("Map Painter is null!");
			return;
		}
		
		mapPainter.OnSelectionChange();
		
	}
}


using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using DopplerInteractive.TidyTileMapper.Editors;

public class BlockEditorWindow : EditorWindow
{
	TidyBlockEditor blockEditor;
	public static TidyBlockMapCreator blockMapCreator;
	
	//This is the function that will be called when editing an existing block
	//Via the map window
	[MenuItem("Window/Tidy Block Editor")]
	public static void Init () {
		EditorWindow.GetWindow(typeof(BlockEditorWindow),true,"Tidy Block Editor",true); 
	}
	
	void OnGUI(){
				
		blockEditor.DrawWindow();
		
	}
	
	
	void OnEnable(){
		
		if(blockEditor == null){
			
			blockEditor = new TidyBlockEditor();
									
			MapCreatorWindow w = EditorWindow.GetWindow<MapCreatorWindow>();
			
			blockEditor.Initialize(this,w.mapPainter);
			
		}
		
	}
	
	void OnDestroy(){
		blockEditor.Destroy();
	}
	
	void OnDisable(){
		
		//blockEditor.Destroy();
		
	}
	
	void Update(){
		
		blockEditor.Update();
		
	}
}


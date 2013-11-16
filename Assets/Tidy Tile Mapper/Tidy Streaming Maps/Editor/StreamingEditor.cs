using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(StreamingMap))]
public class StreamingEditor : Editor {
			
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		
		if(GUILayout.Button("Add Color Profile")){
			
			StreamingMap map = target as StreamingMap;
			
			StreamingMapColorProfile mp = map.gameObject.GetComponentInChildren<StreamingMapColorProfile>();
			
			if(mp != null){
				Debug.LogWarning("No dice, there's already a color profile attached to the map. Look down a little in the inspector!");
			}
			else{
				
				//We add this to a new gameobject to allow the user to create a prefab from this color profile
				//
				//In this way, they can transfer color profiles between maps
				
				GameObject colorRoot = new GameObject("Color Profile");
				
				colorRoot.transform.parent = map.transform;
				
				colorRoot.transform.localPosition = Vector3.zero;
				
				mp = colorRoot.AddComponent<StreamingMapColorProfile>();
				mp.map = map;
				
				Selection.activeGameObject = colorRoot.gameObject;
			}
			
		}

		if(EditorApplication.isPlaying){
			
			//Warning! This will take a long time for larger maps
			//
			//Use this function at your own discretion. It's for debugging purposes ONLY
			
			if(GUILayout.Button("Debug: Draw Map")){
				
				StreamingMap map = target as StreamingMap;
				
				int radius = map.width;
				
				if(radius < map.height){
					radius = map.height;
				}
				
				int x= (int)((float)map.width * 0.5f);
				int y= (int)((float)map.height * 0.5f);
				
				map.DrawMap(x,y,1,radius,true);
				
			}
			
		}
		
	}
	
}

using UnityEngine;
using UnityEditor;

public class UpdateWindow : EditorWindow
{
	//Hello friend!
	//
	//I've put this window into your project 
	//so that when you get an update, and it has important information
	//E.g: something may break your project
	//It will inform you so that you know to make provisions
	//
	//I realised a lot of people may not be on the forums
	//And I don't want to rudely shock you by breaking your project
	//
	//So hopefully this will make things a little easier
	
	public string updateInformation = "" +
		"Welcome to Tidy TileMapper v1.4\n"+
			"This version has fundamentally changed the way Tidy TileMapper works internally\n"+
			"in order to take advantage of the updated prefab system of Unity 3.5.\n"+
			"This will result in faster load times and a tighter link between prefab and block.\n\n"+
			"However:\n\n"+
			"This update WILL break your existing blocks and maps. I am very very sorry about this!\n\n"+
			"As blocks are now links to prefabs, and not links to existing objects,\n"+
			"you will need to recreate your custom blocks and maps.\n\n"+
			"But don't fret! In order to sweeten the deal a little bit, I will soon be releasing\n"+
			"Character Controllers, AI and 'Streaming Tilemaps' for your development pleasure.\n\n"+
			"If you find any issues with this new update, please email me at support@dopplerinteractive.com\n\n"+
			"Thankyou for using Tidy TileMapper! The future is looking excellent!\n\n"+
			"-Joshua McGrath\n\n"+
			"(Hit OK to stop this message appearing again)";
	 
	public static string UPDATE_VERSION_KEY = "TTM_V_1_4";

	public static void Init () {
		
		if(EditorPrefs.HasKey(UPDATE_VERSION_KEY)){
			return;
		}
		
		EditorWindow w = EditorWindow.GetWindow(typeof(UpdateWindow),true,"Update: Tidy Tile Mapper",true); 
		w.position = new Rect(Screen.width*0.5f,Screen.height*0.5f,750.0f,300.0f);
	}
	
	Vector2 scrollPos = Vector2.zero;
	
	void OnGUI(){
				
		EditorGUILayout.BeginVertical();
		
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		
		EditorGUILayout.TextArea(updateInformation);
		
		EditorGUILayout.EndScrollView();
		
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginHorizontal();
		
		GUILayout.FlexibleSpace();
		
		if(GUILayout.Button("OK")){
			EditorPrefs.SetBool(UPDATE_VERSION_KEY,true);
			Close ();
		}
		
		EditorGUILayout.EndHorizontal();
		
	}
}

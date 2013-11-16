using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(StreamingMapColorProfile))]
public class ColorProfileEditor : Editor {
		
	//What a long class name!
	StreamingMapColorProfile profile;
	
	void OnEnable(){
		
		profile = target as StreamingMapColorProfile;
		
	}
	
	public override void OnInspectorGUI ()
	{
		bool isDirty = false;
		
		base.OnInspectorGUI ();
		
		GUILayout.Space(10.0f);			
		
		GUILayout.Label("Map-from-texture:");
		
		GUILayout.Space(10.0f);
		
		profile.texture = EditorGUILayout.ObjectField("Source Image:",profile.texture,typeof(Texture2D),false) as Texture2D;
		
		profile.pixelScale = EditorGUILayout.FloatField("Pixel Scale:",profile.pixelScale);
		
		profile.colorDifferenceBias = EditorGUILayout.FloatField("Color Bias:",profile.colorDifferenceBias);
		
		EditorGUILayout.BeginHorizontal();
		
		GUILayout.FlexibleSpace();
		
		if(GUILayout.Button("Sample Texture")){
			
			string path = AssetDatabase.GetAssetPath(profile.texture);
									
			TextureImporter importer = TextureImporter.GetAtPath(path) as TextureImporter;
			
			if(importer != null){
				importer.isReadable = true;
				AssetDatabase.ImportAsset(path,ImportAssetOptions.Default);
			}
			else{
				Debug.LogWarning("Hmm... I couldn't find the texture importer for that texture. I hope it's able to be read!");
			}
			
			profile.SampleColorsFromTexture();
		}
		
		EditorGUILayout.EndHorizontal();
		
		GUILayout.Space(10.0f);
		
		GUILayout.Label("Color-to-Block Mapping");
		
		GUILayout.Space(10.0f);
				
		for(int i = 0; i < profile.colors.Count; i++){
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Color:");
			EditorGUILayout.ColorField(profile.colors[i]);
			GUILayout.Label("Block:");
			Block b = EditorGUILayout.ObjectField(profile.blocks[i],typeof(Block),false) as Block;
			
			if(b != profile.blocks[i]){
				profile.blocks[i] = b;
				isDirty = true;
			}
						
			EditorGUILayout.EndHorizontal();
			
		}
		
		EditorGUILayout.BeginHorizontal();
		
		GUILayout.FlexibleSpace();
		
		if(GUILayout.Button("Populate Map")){

			if(profile.width <= 0){
				Debug.LogWarning("Streaming Map width is zero! You must have a non-zero width, friend.");
				return;
			}
			
			if(profile.height <= 0){
				Debug.LogWarning("Streaming Map height is zero! You must have a non-zero width, friend.");
				return;
			}
			
			if(profile.chunkWidth <= 0){
				Debug.LogWarning("Streaming Map chunk width is zero! You must have a non-zero width, friend.");
				return;
			}
			
			if(profile.chunkHeight <= 0){
				Debug.LogWarning("Streaming Map chunk height is zero! You must have a non-zero width, friend.");
				return;
			}
						
			if(profile.tileScale.x <= 0 || profile.tileScale.y <= 0 || profile.tileScale.z <= 0){
				Debug.LogWarning("Tile scale includes a zero or less-than-zero (Ellis!) value. Assure this is positive");
			}
			
			if(profile.map.GetMap() != null){
				GameObject.DestroyImmediate(profile.map.GetMap().gameObject);
			}
			
			profile.GenerateMapFromTexture();
			
			profile.map.GenerateMap(profile.width,profile.height,profile.depth,profile.mapName,profile.tileScale,profile.chunkWidth,profile.chunkHeight,profile.growthAxis,profile.deleteUpdateRate,profile.createUpdateRate,profile.actionsPerPass				);
			
			isDirty = true;
		}
		
		EditorGUILayout.EndHorizontal();
		
		if(isDirty){
			EditorUtility.SetDirty(profile);
			EditorUtility.SetDirty(profile.map);
		}
		
	}
}

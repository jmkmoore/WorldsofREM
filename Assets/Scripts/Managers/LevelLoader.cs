using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class LevelLoader : MonoBehaviour {

	//Variables for storing level info
	public int[,] TileList;
	int MapWidth;
	int MapHeight;
	string LevelName;

	//Tick this if we want to just load a hard coded level for debug purposes
	public bool DebugLoad = false;

	private static LevelLoader _instance;
	
	void Awake ()
	{
		_instance = this;
	}
	

	public static LevelLoader getInstance ()
	{
		return _instance;
	}

	public void LoadLevel(string levelName){
		LevelName = levelName;
		PopulateTileList();
		RenderLevel();
		PopulateLevel();
	}

	public void ReloadCurrentLevel(){
		foreach(GameObject go in FindObjectsOfType(typeof(GameObject)) as GameObject[]){
			if(go.tag != "Persistent") Destroy(go);
		}
		RenderLevel();
		PopulateLevel();
	}

	void PopulateTileList(){
		//Load up XML from Levels folder
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml((Resources.Load("Levels/XML/"+LevelName) as TextAsset).text);

		//Parse the XML
		XmlNode Map = xmlDoc.SelectSingleNode("map");
		MapWidth = int.Parse(Map.Attributes.GetNamedItem("width").Value);
		MapHeight = int.Parse(Map.Attributes.GetNamedItem("height").Value);
		TileList = new int[MapWidth,MapHeight+1];

		//Fill up the tile list!
		int x = 0;
		int y = 0;
		XmlNodeList Tiles = xmlDoc.GetElementsByTagName("tile");
		foreach(XmlNode tile in Tiles){
			//Debug.Log("Mapping tile "+x+","+y);
			if(y>= MapHeight){
				Debug.LogWarning("More tiles than should fit in the map! ("+x+","+y+")");
			}
			TileList[x,y] = int.Parse(tile.Attributes.GetNamedItem("gid").Value);

			x++;
			if(x>=MapWidth){
				x=0;
				y++;
			}
		}
		//Fill bottom row with DEATH TILES
		for( int z = 0; z < MapWidth; z++){
			TileList[z,MapHeight] = 666;
		}

		//Find the objects
		XmlNodeList Objects = xmlDoc.GetElementsByTagName("objectgroup");
		foreach(XmlNode obj in Objects){


		}
	}

	void RenderLevel(){
		GameObject LevelTiles = new GameObject();
		LevelTiles.name = "Level Tiles";
		for(int x = 0; x < TileList.GetLength(0); x++){
			for(int y = 0; y < TileList.GetLength(1); y++){
				//Debug.Log("Instantiating tile "+TileList[x,y]);
				GameObject tilePrefab = (GameObject)Resources.Load ("Prefab/Blocks/tile_"+TileList[x,y]);
				if(tilePrefab != null){
					GameObject tile = (GameObject)Instantiate(tilePrefab);
					tile.transform.parent = LevelTiles.transform;
					tile.transform.position = new Vector3(x,-1*y,0);
				}
			}
		}
	}

	public void PopulateLevel(){
		//Debug!
		//TienEntity player = GameManager.getInstance().CreateEntity("Player");
		//player.transform.position = new Vector3(2.5f,-57.925f,0.0f);

		/*
		 * For each object in properties:
		 * 1) Generate a property dictionary
		 * 2) Create an entity based on type property
		 * 3) Do whatever we do with every other property
		 * 4) Store that property dict back to the entity
		*/

		//Load up XML from Levels folder
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml((Resources.Load("Levels/XML/"+LevelName) as TextAsset).text);

		
		//Find the objects
		XmlNodeList Objects = xmlDoc.GetElementsByTagName("object");
		foreach(XmlNode obj in Objects){
			//Parse out the entity
			Dictionary<string,string> props = new Dictionary<string,string>();
			Vector2 pos = new Vector2(int.Parse(obj.Attributes.GetNamedItem("x").Value),int.Parse(obj.Attributes.GetNamedItem("y").Value));
			props.Add("Type",obj.Attributes.GetNamedItem("type").Value);
			Debug.Log ("Type" + obj.Attributes.GetNamedItem("type").Value);
			foreach(XmlNode proplist in obj.ChildNodes){
				foreach(XmlNode prop in proplist.ChildNodes){
					//Debug.Log("Found prop for " + obj.Attributes.GetNamedItem("type").Value);
					props.Add(prop.Attributes.GetNamedItem("name").Value, prop.Attributes.GetNamedItem("value").Value);
				}
			}

			//Create the entity
			TienEntity entity;
			switch(props["Type"]){
			case "Player":
				entity = Player.CreatePlayer(props);
				entity.transform.position = new Vector3(pos.x/32f,-1f*pos.y/32f + ((102f-38f)/32f),0);
				break;
			case "Enemy":
				props.Add("EnemyType",obj.Attributes.GetNamedItem("name").Value);
				entity = Enemy.CreateEnemy(props);
				entity.GetComponent<Enemy>().PlaceEnemyAtCoordinates(pos);
				break;
			case "Static":
				//TODO: IMPLEMENT
				//entity = Static.CreateStatic(props);
				//entity.transform.position = new Vector3(pos.x/32f,-1f*pos.y/32f + ((102f-38f)/32f),0);
				break;

			}

		}
	}
	

}

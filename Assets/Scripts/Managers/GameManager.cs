using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public enum GameState {
		Menu,
		Paused,
		Running,
		Cutscene
	};

	private GameState state;
	public GameState State{
		get{
			return state;
		}
		set{
			SetGameState(value,state);
			state = value;
		}
	}

	private int levelState;
	public int LevelState{
		get{
			return levelState;
		}
		set{
			levelState = value;
			SetLevelState(levelState);
		}
	}

	public Dictionary<int,TienEntity> entities = new Dictionary<int,TienEntity>();
	public static int EntityID = 0;

	private static GameManager _instance;
	
	void Awake ()
	{
		_instance = this;
		//TEST CODE
		SetLevelState(0);
	}
	
	public static GameManager getInstance ()
	{
		return _instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void RegisterEntity(TienEntity entity){
		entity.EID = EntityID;
		entities.Add(EntityID,entity);
		EntityID++;

		//GameObject entity = GameObject.Instantiate(Resources.Load("Prefabs/Objects/"+entityName)) as GameObject;
		//return entity.GetComponent<OffswitchEntity>();
	}


	//Essentially hard coded state IDs for all of my levels and cutscenes.
	/*
	 * 0 = menu
	 * 1 = level select
	 * 100+ = world 1
	 * 200+ = world 2
	 * 300+ = world 3
	 * 400+ = world 4
	 */
	void SetLevelState(int state){
		switch(state){
		case 0:
			//GetComponent<LevelLoader>().LoadLevel("TestMap");
			GetComponent<LevelLoader>().LoadLevel("Level 1 refinished");
			break;
		case 100:

			break;
		case 101:

			break;
		default:

			break;
		}
	}

	void SetGameState(GameState currState, GameState prevState){







	}
	
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TienGUI : MonoBehaviour {

	private static TienGUI _instance;

	public Image GUILifeBar;
	public Image GUIPowerBar;
	public Image GUILightBar;
	public Image[] GUIOrbs;

	void Awake ()
	{
		_instance = this;
	}
	
	public static TienGUI getInstance()
	{
		return _instance;
	}


	private float lifeBar;
	public float LifeBar{
		get{
			return lifeBar;
		}
		set{
			lifeBar = value;
			GUILifeBar.fillAmount = value;
		}
	}

	private float powerBar;
	public float PowerBar{
		get{
			return powerBar;
		}
		set{
			powerBar = value;
			GUIPowerBar.fillAmount = value;
		}
	}

	private float lightBar;
	public float LightBar{
		get{
			return lightBar;
		}
		set{
			lightBar = value;
			GUILightBar.fillAmount = value;
		}
	}

	private int orbs;
	public int Orbs{
		get{
			return orbs;
		}
		set{
			if(value > GUIOrbs.Length){
				orbs = GUIOrbs.Length;
			}else if(value < 0){
				orbs = 0; 
			}else{
				orbs = value;
			}
			for(int i=0;i<orbs;i++){
				GUIOrbs[i].fillAmount = 1;
			}
			for(int j=orbs;j<GUIOrbs.Length;j++){
				GUIOrbs[j].fillAmount = 0;
			}
		}
	}




}

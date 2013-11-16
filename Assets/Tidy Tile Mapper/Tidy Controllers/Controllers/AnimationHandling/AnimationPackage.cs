using UnityEngine;
using System;

[Serializable]
public class AnimationPackage{
	public GameObject animationTarget = null;
	public string animationName = "";
	public float normalizedAnimationSpeed = 1.0f;
	
	public void Play(){
		
		if(animationTarget == null){
			return;
		}
					
		animationTarget.animation[animationName].normalizedSpeed = normalizedAnimationSpeed;
		animationTarget.animation.Play(animationName);
	}
}
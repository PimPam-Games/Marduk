using UnityEngine;
using System;

public class ReSkinAnimation : MonoBehaviour {
	

	public void ReSkinArmour(string sprite){

		if (sprite == null)
			return;
		var subSprites = Resources.LoadAll<Sprite> ("Character/Armours/" + sprite);

		foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
		{
			if(renderer.sprite !=null){
				string spriteName = renderer.sprite.name;
				var newSprite = Array.Find(subSprites, item => item.name == spriteName);
				if (newSprite)
					renderer.sprite = newSprite;
			}
		}
	}

	public void ReSkinHelmet(string sprite){
		
		if (sprite == null)
			return;
		var subSprites = Resources.LoadAll<Sprite> ("Character/heads/" + sprite);
		
		foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
		{
			if(renderer.sprite !=null){

				string spriteName = renderer.sprite.name;
				var newSprite = Array.Find(subSprites, item => item.name == spriteName);				
				if (newSprite)
					renderer.sprite = newSprite;
			}			
		}
	}

	public void ReSkinShield(string sprite){

		if (sprite == null)
			return;
		var subSprites = Resources.LoadAll<Sprite> ("Character/shields/" + sprite);
		foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
		{
			if(renderer.sprite !=null){

				string spriteName = renderer.sprite.name;

				var newSprite = Array.Find(subSprites, item => item.name == spriteName);

				if (newSprite){

					renderer.sprite = newSprite;
				}
			}			
		}
	}

}

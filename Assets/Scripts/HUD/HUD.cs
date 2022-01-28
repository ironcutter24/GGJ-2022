using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.Patterns;

[ExecuteInEditMode]
public class HUD : Singleton<HUD>
{
	[Header("Portrait Sprites")]
	[SerializeField] Sprite spritePrey;
	[SerializeField] Sprite spriteHunter;
	[SerializeField] Image[] portraits;
	private bool isPrey_;
	public bool isPrey 
	{ 
		get { return isPrey_; }
		set { isPrey_ = value; SwitchSprite(value);  } 
	}

	[Header("Health Bar Values")]
	[SerializeField] Image[] healthBubbles;
	public int maxHealth { get; private set; }
	
	[Header("Resource Bar Values")]
	[SerializeField] int minimum;
	[SerializeField] int maximum;
	[SerializeField] int current;
	[SerializeField] bool clampHalfMax;
	
	[Header("Resource Bar Images")]
	[SerializeField] Image mask;
	[SerializeField] Image fill;
	[SerializeField] Color color;

	void Update()
	{
		GetCurrentFill();
	    // NB: va agganciato allo stato del player
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	    	SwitchSprite(true);
	    }
    }
    
	public void SwitchSprite(bool isActivePreySprite)
	{
		for (int i = 0; i < portraits.Length; i++) {
			portraits[i].sprite = isActivePreySprite ? spritePrey : spriteHunter;
		}	
	}
	
	public void GetCurrentFill()
	{
		float currentOffset = current - minimum;
		float maximumOffset = maximum - minimum;
		float fillAmount = currentOffset / maximumOffset;
		fill.fillAmount = clampHalfMax ? (float)(fillAmount * 0.5f) : fillAmount;
		fill.color = color;
	}
	
	public void SetHealthBar(float health) {
		for (int i = 0; i < healthBubbles.Length; i++) {
			if ( i + 1 > (int) health ) {
				Color fade = healthBubbles[i].color;
				fade.a = 0;
				healthBubbles[i].color = fade;
			}
		}
	}
	
}

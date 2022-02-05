using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.Patterns;

public class HUD : Singleton<HUD>
{
    [Header("Portrait Sprites")]
    [SerializeField] Sprite spritePrey;
    [SerializeField] Sprite spriteHunter;
    [SerializeField] Image[] portraits;

    [SerializeField] Text enemiesLeft;

    private bool isPrey_;
    public bool isPrey
    {
        get { return isPrey_; }
	    set 
	    { 
	    	isPrey_ = value;
			SwitchSprite(value); 
	    }
    }

    [Header("Health Bar Values")]
    [SerializeField] Slider healthBar;

    [Header("Resource Bar Values")]
    [SerializeField] int resourceMin;
	[SerializeField] int resourceMax;
	[SerializeField] int resourceCurrent;
    [SerializeField] bool clampHalfMax;

    [Header("Resource Bar Images")]
    [SerializeField] Image mask;
    [SerializeField] Image fill;
    [SerializeField] Color color;

    void Update()
    {
	    GetCurrentFill();
    }

    public void SwitchSprite(bool isActivePreySprite)
    {
        for (int i = 0; i < portraits.Length; i++)
        {
            portraits[i].sprite = isActivePreySprite ? spritePrey : spriteHunter;
        }
	    if (isActivePreySprite) {
		    ResetResourceBar();
	    }
    }
    
	private void ResetResourceBar()
	{
		resourceMax = 0;
		resourceCurrent = 0;
	}

    public void GetCurrentFill()
    {
        float currentOffset = resourceCurrent - resourceMin;
        float maximumOffset = resourceMax - resourceMin;
	    float fillAmount = currentOffset / maximumOffset;
	    if(resourceMax == 0 && resourceMin== 0 ){
	    	fillAmount = 0;
	    }
        fill.fillAmount = clampHalfMax ? (float)(fillAmount * 0.5f) : fillAmount;
	    fill.color = color;
    }

    public void SetHealthBar(float health)
    {
	    healthBar.value = health;
    }
    
	public void setResourceCurrent(int resCurrent) {
		resourceCurrent = resCurrent;
	}
	
	public void setResourceMax(int resMax) {
		resourceMax = resMax;
		resourceCurrent = resMax;
	}

    public static void SetEnemiesLeft(int amount)
    {
        _instance.enemiesLeft.text = amount.ToString();
    }
}

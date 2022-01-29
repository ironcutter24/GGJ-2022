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
        set { isPrey_ = value; SwitchSprite(value); }
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
    }

    public void GetCurrentFill()
    {
        float currentOffset = resourceCurrent - resourceMin;
        float maximumOffset = resourceMax - resourceMin;
        float fillAmount = currentOffset / maximumOffset;
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
}

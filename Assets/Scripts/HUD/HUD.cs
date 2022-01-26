using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
	public Sprite activeSprite { get; private set;}
	[SerializeField] Sprite spritePrey;
	[SerializeField] Sprite spriteHunter;
	[SerializeField] Image[] portraits;
	
	private bool isPrey;
    
	void Start()
    {
	    activeSprite = spriteHunter;
    }

    // Update is called once per frame
    void Update()
    {
	    // NB: va agganciato allo stato del player
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	    	isPrey = !isPrey;
	    	activeSprite = !isPrey ? spriteHunter : spritePrey;
	    	SwitchSprite();
	    }
    }
    
	void SwitchSprite()
	{
		for (int i = 0; i < portraits.Length; i++) {
			portraits[i].sprite = activeSprite;
		}	
	}
}

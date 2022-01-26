using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ResourceBar : MonoBehaviour
{
	[Header("Values")]
	public int minimum;
	public int maximum;
	public int current;
	public bool clampHalfMax;
	[Header("ImageUI")]
	public Image mask;
	public Image fill;
	public Color color;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    GetCurrentFill();
    }
    
	void GetCurrentFill()
	{
		float currentOffset = current - minimum;
		float maximumOffset = maximum - minimum;
		float fillAmount = currentOffset / maximumOffset;
		fill.fillAmount = clampHalfMax ? (float)(fillAmount * 0.5f) : fillAmount;
		//Cambia il colore della barra
		fill.color = color;
	}
}

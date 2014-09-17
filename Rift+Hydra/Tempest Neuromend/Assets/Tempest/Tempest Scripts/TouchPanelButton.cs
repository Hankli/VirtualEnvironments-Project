using UnityEngine;
using System.Collections;

public class TouchPanelButton : MonoBehaviour 
{
	public int buttonID=0;
	private Color colour1=Color.white;
	private Color colour2=Color.green;
	private Color colour3=Color.red;
	private Color buttonColour;
	private float fade = 1.0f;
	private TouchPanel panelScript;
	private bool b_correct=true;
	private bool b_canTouch=true;
	
	void Start()
	{
		buttonColour=colour1;
	}
	
	void Awake() 
	{
		panelScript = transform.parent.GetComponent<TouchPanel>();
	}
	
	void Update() 
	{
		fade+=0.1f*Time.deltaTime*20.0f;
		if(fade>=1.1f)	fade=1.1f;
		
		//
		if(fade<=1.1f)
		{
			//check if correct ID...			
			if(b_correct)
			{	
				buttonColour=Color.Lerp(colour2, colour1, fade);//flash green if correct in sequence
				//set texture to inactive/correct sequence number texture
			}
			else
			{	
				buttonColour=Color.Lerp(colour3, colour1, fade);//flash red if incorrect in sequence
			}
		}
		renderer.material.color = buttonColour;
	}
	
	public void OnTriggerEnter()
	{
		OnClick();
	}
	
	
	public void OnClick()
	{
		if(b_canTouch)
		{
			fade=0.0f;

			//check if correct ID...			
			if(buttonID==panelScript.GetCurrentSequenceNumber())
			{	
				b_canTouch=false;
				panelScript.SetInactivetexture(this);
				panelScript.NextIndex();
				b_correct=true;
			}
			else
			{
				panelScript.ErrorCount();
				b_correct=false;
			}
		}
	}
	
	public void SetbuttonID(int number)
	{
		buttonID=number;
	}	
	
	public void SetTexture(Texture2D theTexture)
	{
		renderer.material.SetTexture("_MainTex", theTexture);
	}
	
	public int GetID()
	{
		return buttonID;
	}
	
	public void SetTouchable(bool choice=true)
	{
		b_canTouch=choice;
	}
}

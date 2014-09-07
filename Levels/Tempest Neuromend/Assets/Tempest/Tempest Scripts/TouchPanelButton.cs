using UnityEngine;
using System.Collections;

public class TouchPanelButton : MonoBehaviour 
{
	public string name="";
	public int buttonID=0;
	public bool b_touched=false;
	Color colour1=Color.white;
	Color colour2=Color.green;
	Color buttonColour;
	float fade = 1.0f;
	
	void Start() 
	{
		buttonColour=colour1;
	}
	

	void Update() 
	{
		fade+=0.1f*Time.deltaTime*10.0f;
		if(fade>=1.0f)	fade=1.0f;
		//buttonColour=Color.Lerp(colour2, colour1, fade*Time.deltaTime);
		buttonColour=Color.Lerp(colour2, colour1, fade);
		renderer.material.color = buttonColour;
	}
	
	public void onClick()
	{
		b_touched=!b_touched;
		fade=0.0f;
	}
}

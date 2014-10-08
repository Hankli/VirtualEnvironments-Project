using UnityEngine;
using System.Collections;

//only allows single trigger of each hint type in the level.
public class WFHintManager : MonoBehaviour 
{

	public enum HintType
	{
		None,
		JumpPad,
		Animals
	};
	
	bool b_jumpPadTriggered=false;
	bool b_animalsTriggered=false;
	
	
	void Start() 
	{
	
	}
	
	void Update() 
	{
	
	}
	
	public bool CanHint(HintType hintType)
	{
		bool b_return=false;
		switch(hintType)
		{
			case HintType.None:
				b_return=true;
				break;
			case HintType.JumpPad:
				if(b_jumpPadTriggered)
				{
					b_return=false;
				}
				else
				{
					b_jumpPadTriggered=true;
					b_return=true;
				}
				break;
			case HintType.Animals:
				if(b_animalsTriggered)
				{
					b_return=false;
				}
				else
				{
					b_animalsTriggered=true;
					b_return=true;
				}
				break;
				
		}
		
		return b_return;
	}
}

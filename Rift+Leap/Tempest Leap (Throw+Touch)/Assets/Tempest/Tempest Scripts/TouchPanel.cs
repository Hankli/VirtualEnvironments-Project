using UnityEngine;
using System.Collections;

public class TouchPanel : MonoBehaviour 
{
	private int a=0;//misc counter
	private int numButtons=0;//number of buttons... auto updated in start
	private int errorCount = 0;//number of errors made (may not be used for score)
	private int sequenceCount = 0;//current sequences
	private int currentSequenceIndex = 0;//current index of current sequence
	private int maxSequence = 3;//maybe should be higher? need to test
	public int[] theSequence;//the sequence (public for debug)
	public Texture2D[] numberTextures;
	
	void Start() 
	{
		a=0;
		//set button IDs		
		foreach(Transform child in transform)
		{
			if(child.GetComponent<TouchPanelButton>())
			{
				TouchPanelButton buttonScript = child.GetComponent<TouchPanelButton>();		
				buttonScript.setbuttonID(a+1);
				a++;
			}
		}
		numButtons=a;	
		LoadTextures();
		resetSequence();
	}
	
	void Update() 
	{
		
	}
	
	//increase sequence count, generate new button sequence
	public void resetSequence()
	{
		sequenceCount++;
		//if sequenceCount >= maxSequence... end task...
		if(sequenceCount>=maxSequence)
		{
		}
		
		//if sequenceCount > 1 generate random sequence, otherwise ordered sequence...
		if(sequenceCount>1)
			GenSeq(theSequence,1,numButtons,true);
		else 
			GenSeq(theSequence,1,numButtons);
		
		//set button textures... need to optimise this maybe...
		
		a=0;
		for(int i=0;i<numButtons;i++)
		{
			foreach(Transform child in transform)
			{
				if(child.GetComponent<TouchPanelButton>())
				{
					TouchPanelButton buttonScript = child.GetComponent<TouchPanelButton>();	
					if(buttonScript.GetID()==theSequence[i])
					{
						buttonScript.SetTexture(GetNumberTexture(a));
						a++;
						break;
					}	
				}
			}
		}
	}	
	
	//generate integer sequnce for number of buttons in panel... optional randomness
	void GenSeq(int[] sequence, int low, int high, bool random=false)
	{
		theSequence=new int[numButtons];
		for(int i=0;i<numButtons;i++)
		{
			theSequence[i]=i+1;
		}
		if(random)
		{		
			int n = theSequence.Length;
			while (n > 1)
			{
				n--;
				int k = Random.Range(0,n+1);
				int value = theSequence[k];
				theSequence[k] = theSequence[n];
				theSequence[n] = value;
			}
		}
	}
	
	public int GetCurrentSequenceNumber()
	{
		return theSequence[currentSequenceIndex];
	}
	
	public void NextIndex()
	{
		currentSequenceIndex++;
		
		if(currentSequenceIndex>=theSequence.Length)
		{
			currentSequenceIndex=0;
			resetSequence();
		}
	}
	
	public void ErrorCount()
	{
		errorCount++;
	}
	
	public void LoadTextures()
	{
		numberTextures = new Texture2D[numButtons];
		for(int i=0; i<numButtons; i++)
		{
			numberTextures[i]=Resources.Load<Texture2D>("TouchPanelButton0"+(i+1));
		}
	}
	
	public Texture2D GetNumberTexture(int sequenceNum)
	{
		return numberTextures[sequenceNum];
	}
}

/*
Tempest/Tempest Assets/Max/Materials/
*/
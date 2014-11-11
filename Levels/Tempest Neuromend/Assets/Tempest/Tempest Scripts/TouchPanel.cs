using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class TouchPanel : MonoBehaviour 
{
	private int a=0;//misc counter
	private int numButtons=0;//number of buttons... auto updated in start
	private int errorCount = 0;//number of errors made (may not be used for score)
	private int sequenceCount = 0;//current sequences
	private int currentSequenceIndex = 0;//current index of current sequence
	private int maxSequence = 4;//(1 higher than actual maximum sequences) maybe should be higher? need to test...
	[Tooltip("The number of sequences the player must complete to continue")]
	public int numberOfSequences = 3;
	private int[] theSequence;//the sequence (public for debug)
	private Texture2D[] numberTextures;
	private Texture2D[] numberTexturesInactive;
	
	private GameObject gameControl=null;
	private GameControl gameControlScript=null;

	private GameObject levelControl=null;
	private LevelControl levelControlScript=null;


	
	private string objectiveText="Objective:\nTouch the numbers in the correct sequence";
	private string objectiveTextUpdated="";
	
	private float vanishingHeight=-0.25f;
	private float vanishingZ=0.8f;
	private bool b_destructionImminent=false;
	public GameObject nextObjective=null;


	//private AudioClip noteA = null;
	//private AudioClip noteB = null;
	//private AudioClip noteC = null;
	//private AudioClip noteD = null;
	//private AudioClip noteE = null;

	private AudioClip[] notes;


	void Awake() 
	{
		if(levelControl=GameObject.FindWithTag("Level"))
		{
			levelControlScript=levelControl.GetComponent<LevelControl>();
		}	
		if(gameControl=GameObject.FindWithTag("Game"))
		{
			gameControlScript=gameControl.GetComponent<GameControl>();
		}	
	}
	
	void Start()
	{
		a=0;
		//set button IDs		
		foreach(Transform child in transform)
		{
			if(child.GetComponent<TouchPanelButton>())
			{
				TouchPanelButton buttonScript = child.GetComponent<TouchPanelButton>();		
				buttonScript.SetbuttonID(a+1);
				a++;
			}
		}
		maxSequence=numberOfSequences+1;
		objectiveTextUpdated=objectiveText;
		//objectiveTextUpdated+="\n(x"+(maxSequence-sequenceCount)+")";
		objectiveTextUpdated+="\n("+(sequenceCount-1)+"/"+numberOfSequences+")";

		numButtons=a;	
		LoadTextures();
		LoadAudio();
		ResetSequence();

		if(levelControlScript!=null)
		{
			levelControlScript.SetCurrentObjective(objectiveTextUpdated);
		}
		
	}
	
	void Update() 
	{
		if(b_destructionImminent)
		{
			if(this.gameObject.transform.position.y>vanishingHeight)
			{
				if(this.gameObject.transform.position.z<vanishingZ)
				{
					this.gameObject.transform.Translate(Vector3.forward*0.6f*Time.deltaTime,Space.World);
					this.gameObject.transform.Translate(Vector3.up*0.1f*Time.deltaTime,Space.World);
				}
				else
				{
					this.gameObject.transform.Translate(Vector3.up*-0.4f*Time.deltaTime,Space.World);
					this.gameObject.transform.Rotate(Vector3.left*-15.0f*Time.deltaTime,Space.World);
				}
			}
			else
			{
			
			
				//need to remove this from this script and find a less cohesive way to trigger next objective...
				if(nextObjective!=null)
				{
					if(nextObjective.GetComponent<ThrowingObjective>())
					{
						ThrowingObjective blah= nextObjective.GetComponent<ThrowingObjective>();
						blah.SetActive();
					}
					if(nextObjective.transform.GetChild(0))
					{
						Transform spawner=nextObjective.transform.GetChild(0);
						if(spawner.GetComponent<ThrowableSpawner>())
						{
							ThrowableSpawner bleh = spawner.GetComponent<ThrowableSpawner>();
							bleh.SetActive();
						}
					}
				}
				//yea.. this ^ bleh...
				
				
				//kill this thing
				//Destroy(this.gameObject);
			}
		}
	}
	
	//increase sequence count, generate new button sequence
	public void ResetSequence()
	{
		sequenceCount++;
		
		objectiveTextUpdated=objectiveText;
		//objectiveTextUpdated+="\n(x"+(maxSequence-sequenceCount)+")";
		objectiveTextUpdated+="\n("+(sequenceCount-1)+"/"+numberOfSequences+")";
		if(levelControlScript!=null)
			levelControlScript.SetCurrentObjective(objectiveTextUpdated);
			
		//if sequenceCount >= maxSequence... end task...
		if(sequenceCount<maxSequence)
		{
		
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
							buttonScript.SetTouchable();
							a++;
							break;
						}	
					}
				}
			}
		}
		else
		{
			if(levelControlScript!=null)
			{
				levelControlScript.SetCurrentObjective("");
				levelControlScript.ObjectiveCompleted();
			}
				
			b_destructionImminent=true;
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
		return theSequence[currentSequenceIndex];//will return null reference on collision after exercise completed... need to fix!
	}
	
	public void NextIndex()
	{
		currentSequenceIndex++;
		
		if(currentSequenceIndex>=theSequence.Length)
		{
			currentSequenceIndex=0;
			ResetSequence();
		}
	}
	
	public void ErrorCount()
	{
		errorCount++;
	}
	
	public void LoadTextures()
	{
		numberTextures = new Texture2D[numButtons];
		numberTexturesInactive = new Texture2D[numButtons];
		for(int i=0; i<numButtons; i++)
		{
			numberTextures[i]=Resources.Load<Texture2D>("TouchPanel/TouchPanelButton0"+(i+1));
		}
		for(int i=0; i<numButtons; i++)
		{
			numberTexturesInactive[i]=Resources.Load<Texture2D>("TouchPanel/TouchPanelButtonInactive0"+(i+1));
		}
	}

	public void LoadAudio()
	{
		notes = new AudioClip[5];
		notes[0] = Resources.Load<AudioClip>("Audio/Notes2/Note2b");
		notes[1] = Resources.Load<AudioClip>("Audio/Notes2/Note1b");
		notes[2] = Resources.Load<AudioClip>("Audio/Notes2/Note3b");
		notes[3] = Resources.Load<AudioClip>("Audio/Notes2/Note4b");
		notes[4] = Resources.Load<AudioClip>("Audio/Notes2/Note5b");
	}
	
	public Texture2D GetNumberTexture(int sequenceNum)
	{
		return numberTextures[sequenceNum];
	}
	
	public Texture2D GetNumberTextureInactive(int sequenceNum)
	{
		return numberTexturesInactive[sequenceNum];
	}
	
	public void SetInactivetexture(TouchPanelButton buttonScript)
	{
		buttonScript.SetTexture(GetNumberTextureInactive(currentSequenceIndex),false);
	}

	public void PlayNote()
	{
		if(gameControlScript)
		{
			if(gameControlScript.b_sound)
			{
				if(currentSequenceIndex<=notes.Length)
				{
					audio.volume = gameControlScript.audioVolume;
					audio.clip = notes [currentSequenceIndex];
					if(audio.clip)
					{
						audio.Play();
					}
				}
			}
		}

	}
}

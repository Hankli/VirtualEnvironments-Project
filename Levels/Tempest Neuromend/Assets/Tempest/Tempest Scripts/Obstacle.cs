using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour 
{
	//public?
	public enum ObstacleType
	{
		Stationary,
		Moving,
		MoveTo,
		FadeIn
		//Rotate, Projectile...
	};
	
	public enum ObstacleDirection
	{
		None,
		Up,
		Down,
		Left,
		Right
		//Forward, Random...
	};
	
	public ObstacleType obstacleType;
	public ObstacleDirection obstacleDirection;
	public bool b_HasTrigger=false;//will this obstacle need a trigger to activate movement?
	public Vector3 passageDimensions=Vector3.zero;//to calculate trigger size etc...
	public Vector3 MoveToStart=Vector3.zero;//if this obstacle is of type MoveTo..
	public Vector3 MoveToEnd=Vector3.zero;//if this obstacle is of type MoveTo..
	
	public float defaultTriggerSize = 3.0f;
	public BoxCollider triggerBox;//if this obstacle is to have a trigger...
	//model
	public Mesh bleh;
	//texture
	
	void Start() 
	{
		//no mesh assigned so load default
		if(bleh==null)
		{
			//Debug.Log("herpDerp");
			LoadDefaultModel();
		}
		
		if(obstacleType==ObstacleType.MoveTo||obstacleType==ObstacleType.FadeIn)
		{
			SetTriggerDimensions(passageDimensions);
		}
	}
	
	void Update() 
	{
	}
	
	//load default mesh
	public void LoadDefaultModel()
	{
		//bleh=Resources.Load<Mesh>("");
	}
	
	public void LoadTextures()
	{
	}
	
	public void SetTriggerDimensions(Vector3 dimensions)
	{
		if(dimensions.x==0.0f)
			dimensions.x=defaultTriggerSize;
		if(dimensions.y==0.0f)
			dimensions.y=defaultTriggerSize;
		if(dimensions.z==0.0f)
			dimensions.z=defaultTriggerSize;
			
		triggerBox=gameObject.AddComponent<BoxCollider>();
		triggerBox.size=dimensions;
		triggerBox.isTrigger=true;
		//move triggerBox up in line with obstacle base...
		Vector3 pos=Vector3.zero;
		pos.y+=dimensions.y/2.0f;
		triggerBox.center=pos;

	}
	
	
	
}

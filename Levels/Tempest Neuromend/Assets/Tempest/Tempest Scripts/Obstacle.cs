using UnityEngine;
using System.Collections;

//at the moment this script is only used for stationary signs in obstacle avoidance level...
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
	
	//public ObstacleType obstacleType;
	public ObstacleDirection obstacleDirection;
	private ObstacleDirection previousObstacleDirection;
	//public bool b_HasTrigger=false;//will this obstacle need a trigger to activate movement?
	//public Vector3 passageDimensions=Vector3.zero;//to calculate trigger size etc...
	
	private float defaultTriggerSize = 3.0f;
	private BoxCollider triggerBox;//if this obstacle is to have a trigger...
	
	//public Vector3 startPosition=Vector3.zero;//if this obstacle is of type MoveTo..
	//public Vector3 endPosition=Vector3.zero;//if this obstacle is of type MoveTo..
	//model
	//public Mesh bleh;
	//texture
	private bool b_animated=true;
	public float animationSpeed=1.0f;
	//private Mesh mesh = null;
	//private Vector2[] originalUVs;
	
	void Awake()
	{
		//mesh = GetComponent<MeshFilter>().sharedMesh;
	}
	
	void Start() 
	{
		//originalUVs=GetUVs();
		previousObstacleDirection=ObstacleDirection.None;
		
		/*
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
		*/		
		switch(obstacleDirection)
		{
			case ObstacleDirection.None:
				b_animated=false;
				break;
			case ObstacleDirection.Up:
				//RotateUVs(-90);
				b_animated=true;
				break;
			case ObstacleDirection.Down:
				//RotateUVs(90);
				b_animated=true;
				break;
			case ObstacleDirection.Left:
				//RotateUVs(180);
				b_animated=true;
				break;
			case ObstacleDirection.Right:
				//RotateUVs(0);
				b_animated=true;
				break;
		}
	}
	
	void Update() 
	{
		if(obstacleDirection==ObstacleDirection.None)
		{
			ScreenTextureCheck("None");
			b_animated=false;
		}
		else
			b_animated=true;
			
		if(b_animated)
		{
			AnimateTexture();
		}
	}
	
	public void AnimateTexture()
	{
		float slideSpeedU=-1.0f;
		float slideSpeedV=0.0f;
		
		
		switch(obstacleDirection)
		{
			case ObstacleDirection.None:
				ScreenTextureCheck("None");
				slideSpeedU=0.0f;
				slideSpeedV=0.0f;
				break;
			case ObstacleDirection.Up:
				ScreenTextureCheck("Up");
				slideSpeedU=0.0f;
				slideSpeedV=-1.0f;
				break;
			case ObstacleDirection.Down:
				ScreenTextureCheck("Down");
				slideSpeedU=0.0f;
				slideSpeedV=1.0f;
				break;
			case ObstacleDirection.Left:
				ScreenTextureCheck("Left");
				slideSpeedU=1.0f;
				slideSpeedV=0.0f;
				break;
			case ObstacleDirection.Right:
				ScreenTextureCheck("Right");
				slideSpeedU=-1.0f;
				slideSpeedV=0.0f;
				break;
		}
	

		Vector2 offset = new Vector2((Time.time * slideSpeedU * animationSpeed),(Time.time * slideSpeedV * animationSpeed));
		if(renderer)
		{
			renderer.materials[1].SetTextureOffset("_MainTex", offset);
		}
	}
	
	public void ScreenTextureCheck(string textureName)
	{
		if(previousObstacleDirection!=obstacleDirection)
		{
			Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
			textureName = mesh.name + textureName;
			SetScreenTexture(textureName);
			previousObstacleDirection=obstacleDirection;
		}
	}
	
	public void SetScreenTexture(string textureName)
	{
		renderer.materials[1].SetTexture("_MainTex", Resources.Load<Texture2D>(textureName));
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
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		//...
	}
	
	
	
	/*
	//===============================================================Ary still needs to fix these bits..=======
	//not working as intended... scrapping for the moment...
	//rotates 2nd uv set in obstacle for animation etc.
	public void RotateUVs(float angle=0.0f)
	{
		if(mesh)
		{
		
			Vector3[] vertices = mesh.vertices;
			Vector2[] uvs = new Vector2[vertices.Length];
			int i = 0;
			while(i < uvs.Length) 
			{
				uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
				//Debug.Log(vertices[i].x + ", " + vertices[i].z);
				i++;
			}
			
			while(i < uvs.Length) 
			{
				uvs[i] = Quaternion.Euler(0,0,angle) * uvs[i];
				i++;
			}
			
			mesh.uv=uvs;
			

			//Vector2[] vertices= GetUVs();
			//int i = 0;
			//while(i < vertices.Length) 
			//{
			//	vertices[i] = Quaternion.Euler(0,0,180) * vertices[i];
			//	i++;
			//}
			//mesh.uv2=vertices;


		}
	}
	
	//does this return copy or ref? I don't have time for this yo!
	private Vector2[] GetUVs()
	{
		Vector3[] vertices = mesh.vertices;
		Vector2[] uvs = new Vector2[vertices.Length];
		int i = 0;
		while(i < uvs.Length) 
		{
			uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
			//Debug.Log(vertices[i].x + ", " + vertices[i].z);
			i++;
		}
		return uvs;
	}
	*/	
}

using UnityEngine;
using System.Collections;

public class ThrowableSpawner : MonoBehaviour 
{

	public bool b_objectOnTop=false;//is there an object sitting on top of this spawner?
	public bool b_canSpawn=true;
	public GameObject[] someObject;
	public float spawnWaitTime=2.0f;
	public float lastExitTime=0.0f;
	public float timeCheck=0.0f;
	private Vector3 pos;//used for spawn position
	
	private float awakeHeight=1.4f;//active y position
	private bool b_isAwake=false;
	private bool b_awaken=false;
	
	void Start() 
	{
		LoadObjects();
	}
	
	void Update() 
	{
		AwakenSpawner();
			
		timeCheck=Time.time-lastExitTime;
		if(b_isAwake)
		{
			if(!b_objectOnTop&&timeCheck>2.0f)
			{
				b_canSpawn=true;
			}
			if(!b_objectOnTop&&b_canSpawn)
			{
				SpawnNewObject();
			}
		}
	}
	
	void OnTriggerStay(Collider theObject)
	{
		b_objectOnTop=true;		
		b_canSpawn=false;
	}
	
	void OnTriggerExit(Collider theObject)
	{
		b_objectOnTop=false;
		lastExitTime=Time.time;
	}
	
	//randomly selects objects from the array of objects and spawns one if possible.
	private void SpawnNewObject()
	{
		if(b_isAwake)
		{
			if(!b_objectOnTop&&b_canSpawn)
			{
				b_canSpawn=false;
				lastExitTime=Time.time;
				Vector3 pos=transform.position;
				pos.y+=0.3f;
				int x = Random.Range(0,someObject.Length);
				Instantiate(someObject[x], pos, Quaternion.identity);
			}
		}
	}
	
	//need to make dynamic...
	private void LoadObjects()
	{
		someObject=new GameObject[3];
		someObject[0]=Resources.Load<GameObject>("Prefabs/ThrowableSphere");
		someObject[1]=Resources.Load<GameObject>("Prefabs/ThrowableCube");
		someObject[2]=Resources.Load<GameObject>("Prefabs/ThrowablePyramid");
	}

	private void AwakenSpawner()
	{
		if(b_awaken&&!b_isAwake)
		{
			if(this.gameObject.transform.position.y<awakeHeight)
			{
				this.gameObject.transform.Translate(Vector3.up*0.4f*Time.deltaTime,Space.World);
			}
			else
			{
				b_isAwake=true;
			}
		}
	}
	
	public void SetActive(bool choice=true)
	{
		b_awaken=choice;
	}
}

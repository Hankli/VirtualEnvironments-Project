using UnityEngine;
using System.Collections;

public class TextureSlide : MonoBehaviour 
{
	public float slideSpeedU=0.0f;
	public float slideSpeedV=0.0f;

	void Start() {}
	
	void Update() 
	{
		Vector2 offset = new Vector2((Time.time * slideSpeedU),(Time.time * slideSpeedV));
		renderer.material.SetTextureOffset("_MainTex", offset);
	}
}

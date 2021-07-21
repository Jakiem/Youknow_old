using UnityEngine;
using System.Collections;

public class Click_Sound : MonoBehaviour {
	
	public bool Play_Sound = false;

	void Awake()
	{

	}
	void Update () 
	{
		if (Play_Sound == true) 
		{
			audio.Play();
			Play_Sound = false;
		}
	}
	
	public void Play()
	{
		Play_Sound = true;
	}
}

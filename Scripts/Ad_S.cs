using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class Ad_S : MonoBehaviour {
	
	void Awake () 
	{
			Advertisement.Initialize ("25613");
	}
	public void Ad_Show() 
	{
			if(Advertisement.isReady())
		{ 
			Advertisement.Show(); 
		}
		if(!Advertisement.isReady())
		{ 
			Time.timeScale = 1;
		}
	}
}

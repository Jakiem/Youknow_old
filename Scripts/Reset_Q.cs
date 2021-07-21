using UnityEngine;
using System.Collections;
using System.IO;

public class Reset_Q : MonoBehaviour {
	
	public void Reset()
	{
		PlayerPrefs.SetInt ("Save_Q", 0);
		PlayerPrefs.SetInt ("Save_S", 0);
		PlayerPrefs.SetInt ("Save_R", 0);
	}
}

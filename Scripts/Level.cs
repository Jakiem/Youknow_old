using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
	 
	public void Game()
	{
		Application.LoadLevel (1);
	}
	public void Menu()
	{
		Application.LoadLevel (0);
		Time.timeScale = 1;
	}

}

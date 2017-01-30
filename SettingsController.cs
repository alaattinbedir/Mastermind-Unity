using UnityEngine;
using System.Collections;

public class SettingsController : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void BackButtonClicked()
	{
		Debug.Log("Back button Clicked");
		this.gameObject.SetActive (false);
//		Application.LoadLevel("MainMenu");
		
	}

	public void AIPlayersClicked()
	{
		Debug.Log("AI button Clicked");
		
		Application.LoadLevel("PlayersScene");
		
	}
}

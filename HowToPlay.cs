using UnityEngine;
using System.Collections;

public class HowToPlay : MonoBehaviour {

	public AudioClip menuClip;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBackButtonClicked()
	{
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);
		
		Debug.Log("Back button Clicked");
		
		Application.LoadLevel("MainMenu");
		
	}
}

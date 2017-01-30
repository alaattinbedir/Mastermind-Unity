using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackButtonClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown()
	{
		Debug.Log("Back button MouseDown");
		
		SceneManager.LoadScene("MainMenu");

	}

	public void BackButtonClicked()
	{
		Debug.Log("Back button Clicked");


		GamePlay.Instance.isSetFirstNumber = false;
		GamePlay.Instance.isSetSecondNumber = false;
		GamePlay.Instance.isSetThirdNumber = false;
		
		GamePlay.Instance.firstNumber = 0;
		GamePlay.Instance.secondNumber = 0;
		GamePlay.Instance.thirdNumber = 0;
		
		GamePlay.Instance.FirstNumberObject = null;
		GamePlay.Instance.SecondNumberObject = null;
		GamePlay.Instance.ThirdNumberObject = null;

		SceneManager.LoadScene("MainMenu");
		
	}

}

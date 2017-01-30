using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SetButtonClick : MonoBehaviour {

	public AudioClip menuClip;

	// Use this for initialization
	void Start () {
		DOTween.Init(false, false, LogBehaviour.Default);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown()
	{
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		Debug.Log("Set button MouseDown");

		if (GameData.Instance.appModeType == 3) {
			if (GamePlay.Instance.isSetFirstNumber &&
			    GamePlay.Instance.isSetSecondNumber &&
			    GamePlay.Instance.isSetThirdNumber) {

				GameController gameLogic = GameController.Instance ();

				string number = GamePlay.Instance.firstNumber.ToString () + GamePlay.Instance.secondNumber.ToString () + GamePlay.Instance.thirdNumber.ToString ();
			
				Number myNumber = new Number ();
				myNumber.FirstNumber = GamePlay.Instance.firstNumber;
				myNumber.SecondNumber = GamePlay.Instance.secondNumber;
				myNumber.ThirdNumber = GamePlay.Instance.thirdNumber;
				NumberGameAI.Instance.myNumber = myNumber;

				gameLogic.setMyNumber (NumberGameAI.Instance.myNumber);

				GamePlay.Instance.resetNumbers ();

				this.gameObject.GetComponent<CanvasGroup> ().DOFade (0, 1);
			
				this.gameObject.SetActive (false);

				int result = int.Parse (number);

				if (GamePlay.Instance.multiplayerGame) {
					MultiplayerController.Instance.SendChooseNumber (result);
				} else {

					NumberGameAI.Instance.createRandomComputerNumber3 ();

					if (GameData.Instance.appModeType == 3) { 
						gameLogic.sendButton.SetActive (true);
					} else {
						gameLogic.sendButton4.SetActive (true);
					}

				}

			}
		} else {
			if (GamePlay.Instance.isSetFirstNumber &&
				GamePlay.Instance.isSetSecondNumber &&
				GamePlay.Instance.isSetThirdNumber &&
				GamePlay.Instance.isSetFourthNumber) {

				GameController gameLogic = GameController.Instance ();

				string number = GamePlay.Instance.firstNumber.ToString () + GamePlay.Instance.secondNumber.ToString () + GamePlay.Instance.thirdNumber.ToString () + GamePlay.Instance.fourthNumber.ToString ();

				Number myNumber = new Number ();
				myNumber.FirstNumber = GamePlay.Instance.firstNumber;
				myNumber.SecondNumber = GamePlay.Instance.secondNumber;
				myNumber.ThirdNumber = GamePlay.Instance.thirdNumber;
				myNumber.FourthNumber = GamePlay.Instance.fourthNumber;
				NumberGameAI.Instance.myNumber = myNumber;

				gameLogic.setMyNumber (NumberGameAI.Instance.myNumber);

				GamePlay.Instance.resetNumbers ();

				this.gameObject.GetComponent<CanvasGroup> ().DOFade (0, 1);

				this.gameObject.SetActive (false);

				int result = int.Parse (number);

				if (GamePlay.Instance.multiplayerGame) {
					MultiplayerController.Instance.SendChooseNumber (result);
				} else {

					NumberGameAI.Instance.createRandomComputerNumber4 ();

					if (GameData.Instance.appModeType == 3) { 
						gameLogic.sendButton.SetActive (true);
					} else {
						gameLogic.sendButton4.SetActive (true);
					}

				}

			}
		}
		
		
	}
}

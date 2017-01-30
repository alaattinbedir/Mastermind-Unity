using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SendButtonClick : MonoBehaviour {

	private GameController gameLogic;
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

		Debug.Log("Send button MouseDown");

		gameLogic = GameController.Instance();

		if (GameData.Instance.appModeType == 3) {
			if (GamePlay.Instance.isSetFirstNumber &&
			    GamePlay.Instance.isSetSecondNumber &&
			    GamePlay.Instance.isSetThirdNumber) {

				this.gameObject.GetComponent<CanvasGroup> ().DOFade (0, 1);

				string number = GamePlay.Instance.firstNumber.ToString () + GamePlay.Instance.secondNumber.ToString () + GamePlay.Instance.thirdNumber.ToString ();

				int result = int.Parse (number);

				if (GamePlay.Instance.multiplayerGame) {
//				[self stopTimer];
//				self.multiGameStarted = YES;
					gameLogic.isStoped = true;
					MultiplayerController.Instance.SendEstimatingNumber (result, 0, 0);
				} else {

					Number myNumberEstimate = new Number ();
					myNumberEstimate.FirstNumber = GamePlay.Instance.firstNumber;
					myNumberEstimate.SecondNumber = GamePlay.Instance.secondNumber;
					myNumberEstimate.ThirdNumber = GamePlay.Instance.thirdNumber;
					myNumberEstimate.PozitiveResult = 0;
					myNumberEstimate.NegativeResult = 0;


					gameLogic.myMoveNumberSingle (myNumberEstimate);
				}

			}
		} else {
			if (GamePlay.Instance.isSetFirstNumber &&
				GamePlay.Instance.isSetSecondNumber &&
				GamePlay.Instance.isSetThirdNumber &&
				GamePlay.Instance.isSetFourthNumber) {

				this.gameObject.GetComponent<CanvasGroup> ().DOFade (0, 1);

				string number = GamePlay.Instance.firstNumber.ToString () + GamePlay.Instance.secondNumber.ToString () + GamePlay.Instance.thirdNumber.ToString () + GamePlay.Instance.fourthNumber.ToString ();

				int result = int.Parse (number);

				if (GamePlay.Instance.multiplayerGame) {
					//				[self stopTimer];
					//				self.multiGameStarted = YES;
					gameLogic.isStoped = true;
					MultiplayerController.Instance.SendEstimatingNumber (result, 0, 0);
				} else {

					Number myNumberEstimate = new Number ();
					myNumberEstimate.FirstNumber = GamePlay.Instance.firstNumber;
					myNumberEstimate.SecondNumber = GamePlay.Instance.secondNumber;
					myNumberEstimate.ThirdNumber = GamePlay.Instance.thirdNumber;
					myNumberEstimate.FourthNumber = GamePlay.Instance.fourthNumber;
					myNumberEstimate.PozitiveResult = 0;
					myNumberEstimate.NegativeResult = 0;


					gameLogic.myMoveNumberSingle (myNumberEstimate);
				}

			}
		}


		gameLogic.isMyTurn = false;

	}
}

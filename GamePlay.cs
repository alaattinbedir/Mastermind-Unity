using UnityEngine;
using System.Collections;

/**
 * Used just to pass information from one scene to the next
 */

public class GamePlay {

	public enum GameMode
	{
		SinglePlayer,
		MultiPlayer,
		Tournament,
		Training,
		HowToPlay
	}

	public bool multiplayerGame;
	public bool singleplayerGame;
	public bool trainingGame;
	public bool howtoplayGame;

	public bool isSetFirstNumber;
	public bool isSetSecondNumber;
	public bool isSetThirdNumber;
	public bool isSetFourthNumber;

	public int firstNumber;
	public int secondNumber;
	public int thirdNumber;
	public int fourthNumber;

	public GameObject FirstNumberObject;
	public GameObject SecondNumberObject;
	public GameObject ThirdNumberObject;
	public GameObject FourthNumberObject;

	public GameObject sendButtonObject;
	public GameObject setButtonObject;

	public bool acceptedRematch;


	private static GamePlay _instance = null;
	
	private GamePlay() {
		// Anything to init would go here
	}
	
	public static GamePlay Instance {
		get {
			if (_instance == null) {
				_instance = new GamePlay();
			}
			return _instance;
		}
	}

	public void resetNumbers(){

		if (GameData.Instance.appModeType == 3) {
			if (GamePlay.Instance.isSetFirstNumber &&
			    GamePlay.Instance.isSetSecondNumber &&
			    GamePlay.Instance.isSetThirdNumber) {

				FirstNumberObject.GetComponent<NumberClass> ().snapToBase ();
				SecondNumberObject.GetComponent<NumberClass> ().snapToBase ();
				ThirdNumberObject.GetComponent<NumberClass> ().snapToBase ();

			}
		} else {
			if (GamePlay.Instance.isSetFirstNumber &&
				GamePlay.Instance.isSetSecondNumber &&
				GamePlay.Instance.isSetThirdNumber &&
				GamePlay.Instance.isSetFourthNumber) {

				FirstNumberObject.GetComponent<NumberClass> ().snapToBase ();
				SecondNumberObject.GetComponent<NumberClass> ().snapToBase ();
				ThirdNumberObject.GetComponent<NumberClass> ().snapToBase ();
				FourthNumberObject.GetComponent<NumberClass> ().snapToBase ();

			}
		}
	}

	public void resetNumbers2(){
		if (GameData.Instance.appModeType == 3) {
			if (GamePlay.Instance.isSetFirstNumber &&
			    GamePlay.Instance.isSetSecondNumber &&
			    GamePlay.Instance.isSetThirdNumber) {

				FirstNumberObject.GetComponent<NumberClass> ().snapToBase ();
				SecondNumberObject.GetComponent<NumberClass> ().snapToBase ();
				ThirdNumberObject.GetComponent<NumberClass> ().snapToBase ();

			}
		} else {
			if (GamePlay.Instance.isSetFirstNumber &&
				GamePlay.Instance.isSetSecondNumber &&
				GamePlay.Instance.isSetThirdNumber &&
				GamePlay.Instance.isSetFourthNumber) {

				FirstNumberObject.GetComponent<NumberClass> ().snapToBase ();
				SecondNumberObject.GetComponent<NumberClass> ().snapToBase ();
				ThirdNumberObject.GetComponent<NumberClass> ().snapToBase ();
				FourthNumberObject.GetComponent<NumberClass> ().snapToBase ();

			}
		}
	}
	
}

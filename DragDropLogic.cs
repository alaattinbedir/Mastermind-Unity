using UnityEngine;
using System.Collections;

public class DragDropLogic : MonoBehaviour {

	public GameObject TempPlanetSelected;

	private void OnTriggerEnter2D(Collider2D objectThatEnters)
	{

		if (GameData.Instance.appModeType == 3) { 
			// eger sayi paneline numara tasiniyorsa 
			if (objectThatEnters.gameObject.CompareTag ("FirstNumber") ||
			    objectThatEnters.gameObject.CompareTag ("SecondNumber") ||
			    objectThatEnters.gameObject.CompareTag ("ThirdNumber")) {


				if (objectThatEnters.gameObject.CompareTag ("FirstNumber")) {

					if (!GamePlay.Instance.isSetFirstNumber) {
						this.gameObject.GetComponent<NumberClass> ().isFirstNumber = true;
						GamePlay.Instance.isSetFirstNumber = true;
						GamePlay.Instance.firstNumber = int.Parse (this.gameObject.tag);
						GamePlay.Instance.FirstNumberObject = this.gameObject;
					} else {
						this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
						objectThatEnters.GetComponent<NumberClass> ().CollisionDetected = false;
						objectThatEnters.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatEnters.GetComponent<NumberClass> ().BaseNumber;

						return;
					}
				}

				if (objectThatEnters.gameObject.CompareTag ("SecondNumber")) {

					if (!GamePlay.Instance.isSetSecondNumber) {
						this.gameObject.GetComponent<NumberClass> ().isSecondNumber = true;
						GamePlay.Instance.isSetSecondNumber = true;
						GamePlay.Instance.secondNumber = int.Parse (this.gameObject.tag);
						GamePlay.Instance.SecondNumberObject = this.gameObject;
					} else {
						this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
						objectThatEnters.GetComponent<NumberClass> ().CollisionDetected = false;
						objectThatEnters.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatEnters.GetComponent<NumberClass> ().BaseNumber;
					
						return;
					}


				}

				if (objectThatEnters.gameObject.CompareTag ("ThirdNumber")) {

					if (!GamePlay.Instance.isSetThirdNumber) {
						this.gameObject.GetComponent<NumberClass> ().isThirdNumber = true;
						GamePlay.Instance.isSetThirdNumber = true;
						GamePlay.Instance.thirdNumber = int.Parse (this.gameObject.tag);
						GamePlay.Instance.ThirdNumberObject = this.gameObject;
					} else {
						this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
						objectThatEnters.GetComponent<NumberClass> ().CollisionDetected = false;
						objectThatEnters.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatEnters.GetComponent<NumberClass> ().BaseNumber;
					
						return;
					}


				}

				// sayi panel yaklasinca snap ediliyor
				this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = false;
				objectThatEnters.GetComponent<NumberClass> ().CollisionDetected = true;
				objectThatEnters.GetComponent<NumberClass> ().TempPanelToMoveTo = this.gameObject;
				this.gameObject.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatEnters.gameObject;

			} 
		} else {
			// eger sayi paneline numara tasiniyorsa 
			if (objectThatEnters.gameObject.CompareTag ("FirstNumber") ||
				objectThatEnters.gameObject.CompareTag ("SecondNumber") ||
				objectThatEnters.gameObject.CompareTag ("ThirdNumber") ||
				objectThatEnters.gameObject.CompareTag ("FourthNumber") )  {


				if (objectThatEnters.gameObject.CompareTag ("FirstNumber")) {

					if (!GamePlay.Instance.isSetFirstNumber) {
						this.gameObject.GetComponent<NumberClass> ().isFirstNumber = true;
						GamePlay.Instance.isSetFirstNumber = true;
						GamePlay.Instance.firstNumber = int.Parse (this.gameObject.tag);
						GamePlay.Instance.FirstNumberObject = this.gameObject;
					} else {
						this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
						objectThatEnters.GetComponent<NumberClass> ().CollisionDetected = false;
						objectThatEnters.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatEnters.GetComponent<NumberClass> ().BaseNumber;

						return;
					}
				}

				if (objectThatEnters.gameObject.CompareTag ("SecondNumber")) {

					if (!GamePlay.Instance.isSetSecondNumber) {
						this.gameObject.GetComponent<NumberClass> ().isSecondNumber = true;
						GamePlay.Instance.isSetSecondNumber = true;
						GamePlay.Instance.secondNumber = int.Parse (this.gameObject.tag);
						GamePlay.Instance.SecondNumberObject = this.gameObject;
					} else {
						this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
						objectThatEnters.GetComponent<NumberClass> ().CollisionDetected = false;
						objectThatEnters.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatEnters.GetComponent<NumberClass> ().BaseNumber;

						return;
					}
				}

				if (objectThatEnters.gameObject.CompareTag ("ThirdNumber")) {

					if (!GamePlay.Instance.isSetThirdNumber) {
						this.gameObject.GetComponent<NumberClass> ().isThirdNumber = true;
						GamePlay.Instance.isSetThirdNumber = true;
						GamePlay.Instance.thirdNumber = int.Parse (this.gameObject.tag);
						GamePlay.Instance.ThirdNumberObject = this.gameObject;
					} else {
						this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
						objectThatEnters.GetComponent<NumberClass> ().CollisionDetected = false;
						objectThatEnters.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatEnters.GetComponent<NumberClass> ().BaseNumber;

						return;
					}
				}

				if (objectThatEnters.gameObject.CompareTag ("FourthNumber")) {

					if (!GamePlay.Instance.isSetFourthNumber) {
						this.gameObject.GetComponent<NumberClass> ().isFourthNumber = true;
						GamePlay.Instance.isSetFourthNumber = true;
						GamePlay.Instance.fourthNumber = int.Parse (this.gameObject.tag);
						GamePlay.Instance.FourthNumberObject = this.gameObject;
					} else {
						this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
						objectThatEnters.GetComponent<NumberClass> ().CollisionDetected = false;
						objectThatEnters.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatEnters.GetComponent<NumberClass> ().BaseNumber;

						return;
					}
				}

				// sayi panel yaklasinca snap ediliyor
				this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = false;
				objectThatEnters.GetComponent<NumberClass> ().CollisionDetected = true;
				objectThatEnters.GetComponent<NumberClass> ().TempPanelToMoveTo = this.gameObject;
				this.gameObject.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatEnters.gameObject;

			} 
		}

	}
	
	private void OnTriggerExit2D(Collider2D objectThatExited)
	{
		if (GameData.Instance.appModeType == 3) { 
			if (objectThatExited.gameObject.CompareTag ("FirstNumber") ||
			    objectThatExited.gameObject.CompareTag ("SecondNumber") ||
			    objectThatExited.gameObject.CompareTag ("ThirdNumber")) {


				if (objectThatExited.gameObject.CompareTag ("FirstNumber")) {
					if (GamePlay.Instance.isSetFirstNumber && this.gameObject.GetComponent<NumberClass> ().isFirstNumber) {
						GamePlay.Instance.isSetFirstNumber = false;
						this.gameObject.GetComponent<NumberClass> ().isFirstNumber = false;
						GamePlay.Instance.firstNumber = 0;
						GamePlay.Instance.FirstNumberObject = null;
					}
				}
			
				if (objectThatExited.gameObject.CompareTag ("SecondNumber")) {
					if (GamePlay.Instance.isSetSecondNumber && this.gameObject.GetComponent<NumberClass> ().isSecondNumber) {
						GamePlay.Instance.isSetSecondNumber = false;
						this.gameObject.GetComponent<NumberClass> ().isSecondNumber = false;
						GamePlay.Instance.secondNumber = 0;
						GamePlay.Instance.SecondNumberObject = null;
					}
				}
			
				if (objectThatExited.gameObject.CompareTag ("ThirdNumber")) {
					if (GamePlay.Instance.isSetThirdNumber && this.gameObject.GetComponent<NumberClass> ().isThirdNumber) {
						GamePlay.Instance.isSetThirdNumber = false;
						this.gameObject.GetComponent<NumberClass> ().isThirdNumber = false;
						GamePlay.Instance.thirdNumber = 0;
						GamePlay.Instance.ThirdNumberObject = null;
					}
				}

				if (!this.gameObject.GetComponent<NumberClass> ().isFirstNumber &&
				    !this.gameObject.GetComponent<NumberClass> ().isSecondNumber &&
				    !this.gameObject.GetComponent<NumberClass> ().isThirdNumber) {

					this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
				}

			} else {

//			this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
//			objectThatExited.GetComponent<NumberClass> ().CollisionDetected = false;
//			objectThatExited.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatExited.GetComponent<NumberClass> ().BaseNumber;
			}
		} else {
			if (objectThatExited.gameObject.CompareTag ("FirstNumber") ||
				objectThatExited.gameObject.CompareTag ("SecondNumber") ||
				objectThatExited.gameObject.CompareTag ("ThirdNumber") || 
				objectThatExited.gameObject.CompareTag ("FourthNumber")) {


				if (objectThatExited.gameObject.CompareTag ("FirstNumber")) {
					if (GamePlay.Instance.isSetFirstNumber && this.gameObject.GetComponent<NumberClass> ().isFirstNumber) {
						GamePlay.Instance.isSetFirstNumber = false;
						this.gameObject.GetComponent<NumberClass> ().isFirstNumber = false;
						GamePlay.Instance.firstNumber = 0;
						GamePlay.Instance.FirstNumberObject = null;
					}
				}

				if (objectThatExited.gameObject.CompareTag ("SecondNumber")) {
					if (GamePlay.Instance.isSetSecondNumber && this.gameObject.GetComponent<NumberClass> ().isSecondNumber) {
						GamePlay.Instance.isSetSecondNumber = false;
						this.gameObject.GetComponent<NumberClass> ().isSecondNumber = false;
						GamePlay.Instance.secondNumber = 0;
						GamePlay.Instance.SecondNumberObject = null;
					}
				}

				if (objectThatExited.gameObject.CompareTag ("ThirdNumber")) {
					if (GamePlay.Instance.isSetThirdNumber && this.gameObject.GetComponent<NumberClass> ().isThirdNumber) {
						GamePlay.Instance.isSetThirdNumber = false;
						this.gameObject.GetComponent<NumberClass> ().isThirdNumber = false;
						GamePlay.Instance.thirdNumber = 0;
						GamePlay.Instance.ThirdNumberObject = null;
					}
				}

				if (objectThatExited.gameObject.CompareTag ("FourthNumber")) {
					if (GamePlay.Instance.isSetFourthNumber && this.gameObject.GetComponent<NumberClass> ().isFourthNumber) {
						GamePlay.Instance.isSetFourthNumber = false;
						this.gameObject.GetComponent<NumberClass> ().isFourthNumber = false;
						GamePlay.Instance.fourthNumber = 0;
						GamePlay.Instance.FourthNumberObject = null;
					}
				}


				if (!this.gameObject.GetComponent<NumberClass> ().isFirstNumber &&
					!this.gameObject.GetComponent<NumberClass> ().isSecondNumber &&
					!this.gameObject.GetComponent<NumberClass> ().isThirdNumber &&
					!this.gameObject.GetComponent<NumberClass> ().isFourthNumber) {

					this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
				}

			} else {

				//			this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel = true;
				//			objectThatExited.GetComponent<NumberClass> ().CollisionDetected = false;
				//			objectThatExited.GetComponent<NumberClass> ().TempPanelToMoveTo = objectThatExited.GetComponent<NumberClass> ().BaseNumber;
			}
		}
	}
}

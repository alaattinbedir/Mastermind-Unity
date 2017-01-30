using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Threading;
using Heyzap;


//  This script will be updated in Part 2 of this 2 part series.
public class DialogueManager : MonoBehaviour {
	public Sprite icon;
	private ModalPanel modalPanel;
	private GameController gameLogic;
	public AudioClip menuClip;

	private static DialogueManager dialogueManager;
	
	public static DialogueManager Instance () {
		if (!dialogueManager) {
			dialogueManager = FindObjectOfType(typeof (DialogueManager)) as DialogueManager;
			if (!dialogueManager)
				Debug.LogError ("There needs to be one active DialogueManager script on a GameObject in your scene.");
		}
		
		return dialogueManager;
	}
	


	void Awake () {
		modalPanel = ModalPanel.Instance ();
	}


	void Start(){

	}

	public void showDialog(string title, string message,int tag){
		modalPanel = ModalPanel.Instance ();
		modalPanel.Choice (title, message, () => {PostOkFunction (tag);});
	}

	public void showConfirmDialog(string title, string message,int tag,bool multi){
		modalPanel = ModalPanel.Instance ();
		if (multi) {
			modalPanel.Choice (title,message, () => {
				PostYesFunction (tag);}, () => {
				PostNoFunction (tag);});
		} else {
			modalPanel.Choice2 (title,message, () => {
				PostYesFunction (tag);}, () => {
				PostNoFunction (tag);});
		}

	}

	void PostYesFunction(int tag){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		switch (tag) {
		case 0:
			gameLogic = GameController.Instance();
			gameLogic.restartGame();
			break;
		case 10:{
			MultiplayerController.Instance.LeaveGame();
			gameLogic = GameController.Instance();
			gameLogic.BackButtonClicked();
//			if(gameLogic.iStarted){
//
//				if (GameData.Instance.multiRemainingGameCount <= 0) {
//					string title = "Info";
//					string message = "You don't have enough credit to play online game. You can get 5 credits by watching rewarded videos. In addition, You can get more credits by purchasing game packages from the shopping list";
//					dialogueManager = DialogueManager.Instance();
//					dialogueManager.showDialog(title,message,0);
//					
//					return;
//				}else{
//					MultiplayerController.Instance.SendRematch(true);
//				}
//
//			}
		}
			break;
		case 33:{
			// rematch istegi geldi ve kullanici kabul etti..
//			MultiplayerController.Instance.SendAcceptedRematch(true);
		}
			break;
		case 77:
			break;
		case 88:
			{
				MultiplayerController.Instance.SignInAndStartMPGame (InvitationManager.Instance.Invitation);
			}
			break;
		case 90:{
				Application.Quit ();
			}
			break;
		default:{
			gameLogic = GameController.Instance();
			gameLogic.restartGame();
		}
			break;
		}
		
	}

	void PostNoFunction(int tag){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		gameLogic = GameController.Instance();
		if (GameData.Instance.appModeType == 3) { 
			gameLogic.sendButton.SetActive (false);
		} else {
			gameLogic.sendButton4.SetActive (false);
		}
		switch (tag) {
		case 0:
			break;
		case 10:{
			GamePlay.Instance.acceptedRematch = false;
			MultiplayerController.Instance.LeaveGame();
		}
			break;
		case 33:{
			// rematch istegi geldi ve kullanici reddetti..
//			MultiplayerController.Instance.SendAcceptedRematch(false);
		}
			break;
		case 77:{
			gameLogic.BackButtonClicked();
		}
			break;
		case 88:{
			InvitationManager.Instance.DeclineInvitation();
		}
			break;
		case 90:
			break;
		default:
			gameLogic.BackButtonClicked();
			break;
		}
	}

	void PostOkFunction(int tag){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		switch (tag) {
		case 50:{

			#if UNITY_ANDROID
			Application.OpenURL(GameData.Instance.googlePlayLink);
			#elif UNITY_IPHONE
			Application.OpenURL(GameData.Instance.iTunesLink);
			#endif

			GameObject.Find("MainCamera").SetActive(false);

			Thread.Sleep(200);
			Application.Quit(); 
		}
			break;

		case 100:{
			gameLogic = GameController.Instance();
			gameLogic.isMyTurn = true;
				if (GameData.Instance.appModeType == 3) { 
					gameLogic.setButton.SetActive (false);
					gameLogic.setButton4.SetActive (false);
					gameLogic.sendButton.SetActive (true);
					gameLogic.sendButton4.SetActive (false);
				} else {
					gameLogic.setButton4.SetActive (false);
					gameLogic.setButton.SetActive (false);
					gameLogic.sendButton.SetActive (false);
					gameLogic.sendButton4.SetActive (true);
				}
			GamePlay.Instance.resetNumbers();
		}
			break;
		case 500:{
			gameLogic = GameController.Instance();
			gameLogic.computerMoveAfterDelay ();
		}
			break;
		case 501:{
			GamePlay.Instance.resetNumbers();
		}
			break;
		case 600:{
			gameLogic = GameController.Instance();
			gameLogic.isMyTurn = true;
				if (GameData.Instance.appModeType == 3) { 
					gameLogic.setButton4.SetActive (false);
					gameLogic.setButton.SetActive (false);
					gameLogic.sendButton.SetActive (true);
					gameLogic.sendButton4.SetActive (false);
				} else {
					gameLogic.setButton4.SetActive (false);
					gameLogic.setButton.SetActive (false);
					gameLogic.sendButton4.SetActive (true);
					gameLogic.sendButton.SetActive (false);
				}
			GamePlay.Instance.resetNumbers();
		}
			break;
		case 700:{
			gameLogic = GameController.Instance();
			gameLogic.isMyTurn = true;
				if (GameData.Instance.appModeType == 3) {
					gameLogic.setButton.SetActive (false);
					gameLogic.setButton4.SetActive (false);
					gameLogic.sendButton.SetActive (true);
					gameLogic.sendButton4.SetActive (false);
				} else {
					gameLogic.setButton4.SetActive (false);
					gameLogic.setButton.SetActive (false);
					gameLogic.sendButton4.SetActive (true);
					gameLogic.sendButton.SetActive (false);
				}
			GamePlay.Instance.resetNumbers();
		}
			break;
		
		default:
			break;
		}
	}
}

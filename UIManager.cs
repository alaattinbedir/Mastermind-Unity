using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Heyzap;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour, MPLobbyListener {

	private float remainingTime = 65;
	public bool isStoped;
	private float minutes;
	private float seconds;

	public Toggle master;
	public Toggle rand;
	public Toggle lui;
	public Toggle alice;
	public Toggle jack;
	public Toggle marcia;
	public Toggle sally;
	public Toggle tanya;
	public Toggle digitThree;
	public Toggle digitFour;
	public GoogleAnalyticsV4 googleAnalytics;

	public GUISkin guiSkin;
	public Text versionLabel; 
	private bool _showLobbyDialog;
	private string _lobbyMessage;
	private GameController gameController;
	bool mInMatch = false;
	private string iTunesLink;
	private DialogueManager dialogueManager;
	// state info, to avoid processing events multiple times.
	public bool processed = false;
	private Invitation inv;
	public GameObject settingsObject;
	public GameObject playersObject;
	public GameObject rulesObject;
	public GameObject music;
	public GameObject musicBan;
	public GameObject sound;
	public GameObject soundBan;
	public AudioClip menuClip;


	public void SetLobbyStatusMessage(string message) {
		_lobbyMessage = message;
	}

	void Awake(){
//		ParseAnalytics.TrackAppOpenedAsync();
//		HeyzapAds.ShowMediationTestSuite();
	}

	void CheckForceUpdate ()
	{
		if (GameData.Instance.config != null) {
			
			bool forceUpdate = false;
			GameData.Instance.config.TryGetValue ("ForceUpdate", out forceUpdate);
			if (forceUpdate) {
				int serverVersion = 99999999;
				GameData.Instance.config.TryGetValue ("ForceUpdateVersion", out serverVersion);
				int localVersion = GameData.Instance.versionCode;
				if (localVersion < serverVersion) {
					GameData.Instance.iTunesLink = "";
					GameData.Instance.config.TryGetValue ("ForceUpdateUrlApple", out GameData.Instance.iTunesLink);
					GameData.Instance.googlePlayLink = "";
					GameData.Instance.config.TryGetValue ("ForceUpdateUrlAndroid", out GameData.Instance.googlePlayLink);
					string message = "There is a new available version to update\n Please, install new version";
					dialogueManager = DialogueManager.Instance ();
					dialogueManager.showDialog ("Info",message, 50);
				}
			}
		}
	}

	void Start() {

		GameData.Instance.dailyLimit = 2;


		GameData.Instance.Get();
//		GameData.Instance.multiRemainingGameCount = 20;


		if (GameData.Instance.isSoundON != 0) {
			sound.SetActive (true);
			soundBan.SetActive (false);
			MusicManager.play ("NumbersGameMusic", 1.0f, 2.0f);
		} else {
			sound.SetActive (false);
			soundBan.SetActive (true);
			MusicManager.stop(1.0f);
		}

		if (GameData.Instance.isMusicON != 0) {
			music.SetActive (true);
			musicBan.SetActive (false);
		} else {
			music.SetActive (false);
			musicBan.SetActive (true);
		}

		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		GamePlay.Instance.multiplayerGame = false;
		GamePlay.Instance.singleplayerGame = false;
		GamePlay.Instance.trainingGame = false;
		GamePlay.Instance.howtoplayGame = false;



//		if(GameData.Instance.noAd != 99){
//			HZBannerShowOptions showOptions = new HZBannerShowOptions();
//			showOptions.Position = HZBannerShowOptions.POSITION_BOTTOM;
//			HZBannerAd.ShowWithOptions(showOptions);
//		}

	} 


	public void startTimer(float from){
		
		remainingTime = from;
		isStoped = true;
		Update();

	}

	private IEnumerator updateCoroutine(){
		yield return new WaitForSeconds(3.2f);
	}

	// Handle detecting incoming invitations.
	public void UpdateInvitation()
	{
		
		if (InvitationManager.Instance == null)
		{
			return;
		}
		
		// if an invitation arrived, switch to the "invitation incoming" GUI
		// or directly to the game, if the invitation came from the notification
		Invitation inv = InvitationManager.Instance.Invitation;
		if (inv != null)
		{
			if (InvitationManager.Instance.ShouldAutoAccept)
			{
				// jump straight into the game, since the user already indicated
				// they want to accept the invitation!
				InvitationManager.Instance.Clear();
				MultiplayerController.Instance.SignInAndStartMPGame(inv);
//				MultiplayerController.AcceptInvitation(inv.InvitationId);
//				NavigationUtil.ShowPlayingPanel();
			}
			else
			{
				// show the "incoming invitation" screen
//				NavigationUtil.ShowInvitationPanel();

				inv = (inv != null) ? inv : InvitationManager.Instance.Invitation;
				if (inv == null && !processed)
				{
					Debug.Log("No Invite -- back to main");
					return;
				}

				// show the popup
				string who = (inv.Inviter != null && inv.Inviter.DisplayName != null) ? inv.Inviter.DisplayName : "Someone";
					
				string message = who + " is challenging you to a Number Game!";
				string title = "Info";
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showConfirmDialog(title,message,88,true);

			}
		}
	}

	public void MasterChanged(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if (master.isOn)
			GameData.Instance.playerId = 1;
		
	}

	public void DigitsThreeChanged(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if (digitThree.isOn)
			GameData.Instance.appModeType = 3;

		GameData.Instance.Save();
		
	}

	public void DigitsFourChanged(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if (digitFour.isOn)
			GameData.Instance.appModeType = 4;

		GameData.Instance.Save();
	}
	
	public void RandChanged(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if (rand.isOn)
			GameData.Instance.playerId = 2;
		
	}
	
	public void LuiChanged(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if (lui.isOn)
			GameData.Instance.playerId = 3;
		
	}
	
	public void AliceChanged(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if (alice.isOn)
			GameData.Instance.playerId = 4;
		
	}
	
	public void JackChanged(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if (jack.isOn)
			GameData.Instance.playerId = 5;
		
	}
	
	public void MarciaChanged(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if (marcia.isOn)
			GameData.Instance.playerId = 6;
		
	}
	
	public void SallyChanged(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if (sally.isOn)
			GameData.Instance.playerId = 7;
		
	}
	
	public void TanyaChanged(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if (tanya.isOn)
			GameData.Instance.playerId = 8;		
	}
	
	public void PlayerBackClicked()
	{
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		Debug.Log("Back button Clicked");

		if(GameData.Instance.noAd != 99){
			HZBannerShowOptions showOptions = new HZBannerShowOptions();
			showOptions.Position = HZBannerShowOptions.POSITION_BOTTOM;
			HZBannerAd.ShowWithOptions(showOptions);
		}

		playersObject.SetActive (false);

	}

	void Update()
	{
		CheckForceUpdate ();


		UpdateInvitation ();




		switch (GameData.Instance.playerId) {
		case 1:
			master.isOn = true;
			break;
		case 2:
			rand.isOn = true;
			break;
		case 3:
			lui.isOn = true;
			break;
		case 4:
			alice.isOn = true;
			break;
		case 5:
			jack.isOn = true;
			break;
		case 6:
			marcia.isOn = true;
			break;
		case 7:
			sally.isOn = true;
			break;
		case 8:
			tanya.isOn = true;
			break;
			
		default:
			break;
		}

		#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
			
		#endif

		switch (GameData.Instance.appModeType) {
		case 3:
			{
				digitThree.isOn = true;
				digitFour.isOn = false;
			}
			break;
		case 4:
			{
				digitFour.isOn = true;
				digitThree.isOn = false;
			}
			break;
		default:
			break;
		}

//		if (GamePlay.Instance.multiplayerGame) {
//
//			if (!isStoped)
//				return;
//			
//			remainingTime -= Time.deltaTime;
//
//			if (remainingTime <= 0) {
//				isStoped = false;
//				_lobbyMessage = "No players found.";
//				_showLobbyDialog = true;
//
//				MultiplayerController.Instance.LeaveGame();
//
//				StartCoroutine(ExecuteAfterTime(5));
//
//
//
//				//		Thread thread = new Thread(() => computerMove(computerEstimate));
//				//		thread.Start();
//			
//
//			}
//		}



	}


	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);

		HideLobby ();

	}

//	IEnumerator HideLobbyAfterTime(float time)
//	{
//		yield return new WaitForSeconds(time);
//
//		_lobbyMessage = "";
//		_showLobbyDialog = false;
//
//	}

	void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			// Application came back to the fore; 

			CheckForceUpdate ();

//			Social.localUser.Authenticate (ProcessAuthentication);
		}
	}

	public void BackButtonClicked()
	{
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		Debug.Log("Back button Clicked");
		settingsObject.SetActive (false);
	}
	
	public void AIPlayersClicked()
	{
		googleAnalytics.LogScreen(new AppViewHitBuilder()
			.SetScreenName("AI Players Screen"));
		
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		Debug.Log("AI button Clicked");
		HZBannerAd.Hide();
		playersObject.SetActive (true);
	}

	public void OnGameRulesClicked()
	{
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		HZBannerAd.Hide();
		Debug.Log("AI button Clicked");
		rulesObject.SetActive (true);
	}

	public void OnGameRulesOkClicked()
	{
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		if(GameData.Instance.noAd != 99){
			HZBannerShowOptions showOptions = new HZBannerShowOptions();
			showOptions.Position = HZBannerShowOptions.POSITION_BOTTOM;
			HZBannerAd.ShowWithOptions(showOptions);
		}

		rulesObject.SetActive (false);
	}

	public void OnResetGamePowerClicked(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		GameData.Instance.ResetGamePower ();
		string message = "Career has been reset";
		dialogueManager = DialogueManager.Instance ();
		dialogueManager.showDialog ("Info",message, 0);
	}

	public void OnMusicClicked(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		GameData.Instance.isMusicON = 0;
		GameData.Instance.Save ();

		music.SetActive (false);
		musicBan.SetActive (true);
	}

	public void OnMusicBanClicked(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		GameData.Instance.isMusicON = 1;
		GameData.Instance.Save ();

		music.SetActive (true);
		musicBan.SetActive (false);
	}

	public void OnSoundClicked(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		GameData.Instance.isSoundON = 0;
		GameData.Instance.Save ();

		sound.SetActive (false);
		soundBan.SetActive (true);
		MusicManager.stop(1.0f);
	}
	
	public void OnSoundBanClicked(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		GameData.Instance.isSoundON = 1;
		GameData.Instance.Save ();

		sound.SetActive (true);
		soundBan.SetActive (false);
		MusicManager.play("NumbersGameMusic", 1.0f, 2.0f);
	}


	public void StartMultiplayerGame()
	{
		#if UNITY_IOS
		MultiplayerController.Instance.TrySilentSignIn();
		#endif

		googleAnalytics.LogScreen(new AppViewHitBuilder()
			.SetScreenName("Multiplayer Screen"));
		
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);



		if (GameData.Instance.multiRemainingGameCount > 0) {
			MultiplayerController.Instance.LeaveGame();
			MusicManager.stop(1.0f);
			GamePlay.Instance.multiplayerGame = true;
			GamePlay.Instance.singleplayerGame = false;
			GamePlay.Instance.trainingGame = false;
			GamePlay.Instance.howtoplayGame = false;
			_lobbyMessage = "Starting an online game...";
			_showLobbyDialog = true;
			MultiplayerController.Instance.lobbyListener = this;
//			startTimer(65);
			MultiplayerController.Instance.showingWaitingRoom = false;
			#if UNITY_ANDROID
			MultiplayerController.Instance.StartMPGame();
			#endif
			
			#if UNITY_IOS
			MultiplayerController.Instance.SignInAndStartMPGame();
			#endif


		} else {
			string title = "Info";
			string message = "You don't have enough credit to play online game. You can get 5 credits by watching rewarded videos. In addition, You can get more credits by purchasing game packages from the shopping list";
			dialogueManager = DialogueManager.Instance();
			dialogueManager.showDialog(title,message,0);

		}

	}


	public void StartSingleplayerGame()
	{
		googleAnalytics.LogScreen(new AppViewHitBuilder()
			.SetScreenName("Singleplayer Screen"));
		
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);


		MusicManager.stop(1.0f);
		GamePlay.Instance.singleplayerGame = true;
		GamePlay.Instance.multiplayerGame = false;
		GamePlay.Instance.trainingGame = false;
		GamePlay.Instance.howtoplayGame = false;
		SceneManager.LoadScene("GameScene");
	}

	public void ShowLeaderboard()
	{
		#if UNITY_IOS
		MultiplayerController.Instance.TrySilentSignIn();
		#endif

		googleAnalytics.LogScreen(new AppViewHitBuilder()
			.SetScreenName("Top Player Screen"));
		
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);
		// show leaderboard UI
		Social.ShowLeaderboardUI();
	}

	public void ShowMarket()
	{
		googleAnalytics.LogScreen(new AppViewHitBuilder()
			.SetScreenName("Extras Screen"));
		
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		HZBannerAd.Hide();
		SceneManager.LoadScene("MarketScene");

	}

	public void ShowSettings()
	{
		googleAnalytics.LogScreen(new AppViewHitBuilder()
			.SetScreenName("Settings Screen"));
		
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		settingsObject.SetActive (true);
	}

	public void StartTrainingGame()
	{
		googleAnalytics.LogScreen(new AppViewHitBuilder()
			.SetScreenName("Training Screen"));
		
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		MusicManager.stop(1.0f);
		GamePlay.Instance.trainingGame = true;
		GamePlay.Instance.multiplayerGame = false;
		GamePlay.Instance.singleplayerGame = false;
		GamePlay.Instance.howtoplayGame = false;
		SceneManager.LoadScene("GameScene");
	}

	public void StartHowToPlayGame()
	{
		googleAnalytics.LogScreen(new AppViewHitBuilder()
			.SetScreenName("HowToPlay Screen"));
		
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		
		HZBannerAd.Hide();

		GamePlay.Instance.howtoplayGame = true;
		GamePlay.Instance.multiplayerGame = false;
		GamePlay.Instance.singleplayerGame = false;
		GamePlay.Instance.trainingGame = false;
		SceneManager.LoadScene("HowToPlayScene");
	}

	public void OnGUI()
	{
//		if (_showLobbyDialog) {
//			GUI.skin = guiSkin;
//			GUI.Box(new Rect(Screen.width * 0.1f, Screen.height * 0.4f, Screen.width * 0.8f, Screen.height * 0.25f), _lobbyMessage);
//		}
	}

	public void HideLobby() {
		_lobbyMessage = "";
		_showLobbyDialog = false;
	}


}

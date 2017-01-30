using UnityEngine;
using System.Collections;
using Parse;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Threading;
using UnityEngine.UI;
using Heyzap;
using Soomla.Store;
using Soomla;


public class PushBehaviour : MonoBehaviour {

	bool mInMatch = false;
	public Text versionCode;
	public GoogleAnalyticsV4 googleAnalytics;

	void Awake() {

		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
				// registers a callback to handle game invitations received while the game is not running.
			.WithInvitationDelegate(InvitationManager.Instance.OnInvitationReceived)
			.Build();
		
		PlayGamesPlatform.InitializeInstance(config);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();







		#if UNITY_ANDROID
		MultiplayerController.Instance.TrySilentSignIn();
		#endif

		#if UNITY_IOS

		GameData.Instance.Get();
		if (GameData.Instance.loggedIn > 0) {
			MultiplayerController.Instance.TrySilentSignIn();
		}

		UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert |
		                                                        UnityEngine.iOS.NotificationType.Badge |
		                                                        UnityEngine.iOS.NotificationType.Sound);
		#endif

		ParsePush.ParsePushNotificationReceived += (sender, args) => {
			#if UNITY_ANDROID
			AndroidJavaClass parseUnityHelper = new AndroidJavaClass("com.parse.ParseUnityHelper");
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			
			// Call default behavior.
			parseUnityHelper.CallStatic("handleParsePushNotificationReceived", currentActivity, args.StringPayload);
			#endif
		};

		// Your Publisher ID is: 1f256a74c0178cb111a2cde78a93d2a2
		HeyzapAds.Start("1f256a74c0178cb111a2cde78a93d2a2", HeyzapAds.FLAG_NO_OPTIONS);

		googleAnalytics.LogEvent(new EventHitBuilder()
			.SetEventCategory("app_action")
			.SetEventAction("launch")
			.SetEventLabel("launched"));
	

	}

	private void fetchConfig() {
		ParseConfig.GetAsync().ContinueWith(t => {
			if (t.IsFaulted || t.IsCanceled) {
				GameData.Instance.config = ParseConfig.CurrentConfig;
				Debug.LogError("Failed to retrieve config.");
			} else {
				GameData.Instance.config = t.Result;
				Debug.Log("Config retrieval successful.");
			}});
	}


	// Use this for initialization
	void Start () {
		fetchConfig ();
		GameData.Instance.versionCode = int.Parse(versionCode.text.Trim());
		GameData.Instance.playerId = 1;
		StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
		SoomlaStore.Initialize (new NumberGameAssets ());

		Thread.Sleep (500);
		Application.LoadLevel("MainMenu");

	}

	public void onSoomlaStoreInitialized(){
		//		noadsVG = (VirtualGood) StoreInfo.GetItemByItemId("no_ads_item_id");
	}

	// Update is called once per frame
	void Update () {
	
	}
}

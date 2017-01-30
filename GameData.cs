using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;


public class GameData {


	public int multiGameCount;
	public int multiRemainingGameCount;
	public int score;
	public int multiAvarageTime;
	public float trainingAvarageSteps;
	public float trainingAvarageSteps4;
	public int singleGameCount;
	public int trainingGameCount;
	public int trainingGameCount4;
	public int rewardedGameCount;
	public string limitDate;
	public int dailyLimit;
	public int noAd;
	public string objectId;
	ParseObject gameDataUpdate;
	public ParseConfig config;
	public string iTunesLink;
	public string googlePlayLink;
	public int versionCode;
	public Invitation mIncomingInvitation;
	public int playerId;
	public int isSoundON;
	public int isMusicON;
	public int limitExceeded;
	public int appModeType;
	public int loggedIn;


	private static GameData _instance = null;
	
	private GameData() {
//		gameDataUpdate = new ParseObject ("GameData");
	}
	
	public static GameData Instance {
		get {
			if (_instance == null) {
				_instance = new GameData();
			}
			return _instance;
		}
	}


	public void ResetGamePower(){

		PlayerPrefs.SetFloat("trainingAvarageSteps",0);
		PlayerPrefs.SetInt("trainingGameCount",0);

	}

	public void ResetGamePower4(){

		PlayerPrefs.SetFloat("trainingAvarageSteps4",0);
		PlayerPrefs.SetInt("trainingGameCount4",0);

	}

	public void Save(){

		PlayerPrefs.SetInt("multiGameCount",multiGameCount);
		PlayerPrefs.SetInt("multiRemainingGameCount",multiRemainingGameCount);
		PlayerPrefs.SetInt("score",score);
		PlayerPrefs.SetInt("multiAvarageTime",multiAvarageTime);
		PlayerPrefs.SetFloat("trainingAvarageSteps",trainingAvarageSteps);
		PlayerPrefs.SetFloat("trainingAvarageSteps4",trainingAvarageSteps4);
		PlayerPrefs.SetInt("singleGameCount",singleGameCount);
		PlayerPrefs.SetInt("trainingGameCount",trainingGameCount);
		PlayerPrefs.SetInt("trainingGameCount4",trainingGameCount4);
		PlayerPrefs.SetInt("rewardedGameCount",rewardedGameCount);
		PlayerPrefs.SetString("limitDate",limitDate);
		PlayerPrefs.SetInt("noAd",noAd);
		PlayerPrefs.SetInt("isMusicON",isMusicON);
		PlayerPrefs.SetInt("isSoundON",isSoundON);
		PlayerPrefs.SetInt("limitExceeded",limitExceeded);
		PlayerPrefs.SetInt("appModeType",appModeType);
		PlayerPrefs.SetInt("loggedIn",loggedIn);
	}

	public void Get(){

		multiGameCount = PlayerPrefs.HasKey("multiGameCount") ? PlayerPrefs.GetInt("multiGameCount"):0;
		multiRemainingGameCount = PlayerPrefs.HasKey("multiRemainingGameCount") ? PlayerPrefs.GetInt("multiRemainingGameCount"):0;
		score = PlayerPrefs.HasKey("score") ?PlayerPrefs.GetInt("score"):0;
		multiAvarageTime = PlayerPrefs.HasKey("multiAvarageTime") ? PlayerPrefs.GetInt("multiAvarageTime"):0;
		trainingAvarageSteps = PlayerPrefs.HasKey("trainingAvarageSteps") ? PlayerPrefs.GetFloat("trainingAvarageSteps"):0;
		trainingAvarageSteps4 = PlayerPrefs.HasKey("trainingAvarageSteps4") ? PlayerPrefs.GetFloat("trainingAvarageSteps4"):0;
		singleGameCount = PlayerPrefs.HasKey("singleGameCount") ? PlayerPrefs.GetInt("singleGameCount"):0;
		trainingGameCount = PlayerPrefs.HasKey("trainingGameCount") ? PlayerPrefs.GetInt("trainingGameCount"):0;
		trainingGameCount4 = PlayerPrefs.HasKey("trainingGameCount4") ? PlayerPrefs.GetInt("trainingGameCount4"):0;
		rewardedGameCount = PlayerPrefs.HasKey("rewardedGameCount") ? PlayerPrefs.GetInt("rewardedGameCount"):0;
		limitDate = PlayerPrefs.HasKey("limitDate") ? PlayerPrefs.GetString("limitDate"):"";
		noAd = PlayerPrefs.HasKey("noAd") ? PlayerPrefs.GetInt("noAd"):0;
		isMusicON = PlayerPrefs.HasKey("isMusicON") ? PlayerPrefs.GetInt("isMusicON"):1;
		isSoundON = PlayerPrefs.HasKey("isSoundON") ? PlayerPrefs.GetInt("isSoundON"):1;
		limitExceeded = PlayerPrefs.HasKey("limitExceeded") ? PlayerPrefs.GetInt("limitExceeded"):1;
		appModeType = PlayerPrefs.HasKey("appModeType") ? PlayerPrefs.GetInt("appModeType"):3;
		loggedIn = PlayerPrefs.HasKey("loggedIn") ? PlayerPrefs.GetInt("loggedIn"):0;

	}

}

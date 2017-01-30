using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;
using Soomla;
using UnityEngine.UI;
using System;
using Heyzap;

public class Market : MonoBehaviour {

	public Text purchasedLabel; 
	public Button noAds;
	public AudioClip menuClip;
	private bool active;
	private DialogueManager dialogueManager;

	// Use this for initialization
	void Start () {
		StoreEvents.OnMarketPurchase += onMarketPurchase;
		StoreEvents.OnUnexpectedStoreError += onUnexpectedStoreError;

		HZIncentivizedAd.AdDisplayListener listener = delegate(string adState, string adTag){
			if ( adState.Equals("incentivized_result_complete") ) {
				// The user has watched the entire video and should be given a reward.
				GameData.Instance.multiRemainingGameCount += 5;
				GameData.Instance.rewardedGameCount += 5;
				GameData.Instance.Save();
			}
			if ( adState.Equals("incentivized_result_incomplete") ) {
				// The user did not watch the entire video and should not be given a   reward.
			}
		};
		
		HZIncentivizedAd.SetDisplayListener(listener);

		HZIncentivizedAd.Fetch();

//		if (GameData.Instance.multiRemainingGameCount <= 0) {
//			active = true;
////			checkDailyLimitExceed ();
//		} else {
//			active = false;
//		}

//		if (StoreInventory.GetItemBalance (NumberGameAssets.NO_ADS_ITEM_ID) > 0) {
//			noAds.enabled = false;
//			noAds.GetComponentInChildren<Text>().text = "All ads removed";
//		}

	}



	public void onUnexpectedStoreError(int errorCode) {
		SoomlaUtils.LogError ("ExampleEventHandler", "error with code: " + errorCode);
	}

	public void onMarketPurchase(PurchasableVirtualItem pvi, string payload,
	                             Dictionary<string, string> extra) {
		// pvi - the PurchasableVirtualItem that was just purchased
		// payload - a text that you can give when you initiate the purchase operation and
		//    you want to receive back upon completion
		// extra - contains platform specific information about the market purchase
		//    Android: The "extra" dictionary will contain: 'token', 'orderId', 'originalJson', 'signature', 'userId'
		//    iOS: The "extra" dictionary will contain: 'receiptUrl', 'transactionIdentifier', 'receiptBase64', 'transactionDate', 'originalTransactionDate', 'originalTransactionIdentifier'
		
		// ... your game specific implementation here ...


		if(pvi.ItemId.Equals(NumberGameAssets.NO_ADS_ITEM_ID)){
			GameData.Instance.noAd = 99;
			GameData.Instance.Save();
		}else if(pvi.ItemId.Equals(NumberGameAssets.PACKET1_ITEM_ID)){
			GameData.Instance.multiRemainingGameCount += 1000;
			GameData.Instance.Save();
		}else if(pvi.ItemId.Equals(NumberGameAssets.PACKET2_ITEM_ID)){
			GameData.Instance.multiRemainingGameCount += 3000;
			GameData.Instance.Save();
		}else if(pvi.ItemId.Equals(NumberGameAssets.PACKET3_ITEM_ID)){
			GameData.Instance.multiRemainingGameCount += 5000;
			GameData.Instance.Save();
		}

		purchasedLabel.text = string.Format ("Purchased {0}", pvi.ItemId);

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void purchaseNoAd(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		try {
			Debug.Log(StoreInventory.GetItemBalance(NumberGameAssets.NO_ADS_ITEM_ID));
			StoreInventory.BuyItem(NumberGameAssets.NO_ADS_ITEM_ID);
		} catch (System.Exception ex) {
			Debug.Log("SOOMLA/UNITY" + ex.Message);
		}
	}


	public void purchasePacket1(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		try {
			Debug.Log(StoreInventory.GetItemBalance(NumberGameAssets.PACKET1_ITEM_ID));
			StoreInventory.BuyItem(NumberGameAssets.PACKET1_ITEM_ID);
		} catch (System.Exception ex) {
			Debug.Log("SOOMLA/UNITY" + ex.Message);
		}
	}


	public void purchasePacket2(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		try {
			Debug.Log(StoreInventory.GetItemBalance(NumberGameAssets.PACKET2_ITEM_ID));
			StoreInventory.BuyItem(NumberGameAssets.PACKET2_ITEM_ID);
		} catch (System.Exception ex) {
			Debug.Log("SOOMLA/UNITY" + ex.Message);
		}
	}


	public void purchasePacket3(){
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		try {
			Debug.Log(StoreInventory.GetItemBalance(NumberGameAssets.PACKET3_ITEM_ID));
			StoreInventory.BuyItem(NumberGameAssets.PACKET3_ITEM_ID);
		} catch (System.Exception ex) {
			Debug.Log("SOOMLA/UNITY" + ex.Message);
		}
	}

	public void requestUTCTimeFromServer() 
	{
		string url = "http://www.timeapi.org/utc/now";
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.data);

//			2016-02-23T20:12:30+00:00
			DateTime dateParsed = DateTime.Now; 
			if ( DateTime.TryParseExact( www.data, "yyyy-MM-ddTHH:mm:ss+00:00", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out dateParsed ) ) {
				GameData.Instance.limitDate = www.data;
				GameData.Instance.rewardedGameCount = 0;
				GameData.Instance.limitExceeded = 1;
				GameData.Instance.Save();
				Debug.Log(string.Format("Parsing done: {0:MM/dd/yyyy @ hh:mm}", dateParsed ) );

				// add remaining time
				string message = string.Format("Unfortunately your credit limit exeeded. {0} hours later you can gain credit watching rewarded videos. " +
				                               "You can gain more credits by purchasing game packages from the shopping list", remainingTime());

				dialogueManager = DialogueManager.Instance ();
				dialogueManager.showDialog ("Info",message, 0);


			} else {
				Debug.Log("no result");
			}

		} else {
			active = false;
			string message = www.error;
			dialogueManager = DialogueManager.Instance ();
			dialogueManager.showDialog ("Error",message, 0);
			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	IEnumerator WaitForRequestCheck(WWW www)
	{
		yield return www;
		// check for errors
		if (www.error == null) {

			DateTime currentDate = DateTime.Now;
			if (DateTime.TryParseExact (www.data, "yyyy-MM-ddTHH:mm:ss+00:00", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out currentDate)) {
				

			}

			DateTime dateLocal = DateTime.Now;
			if (!GameData.Instance.limitDate.Equals (string.Empty)) {
				DateTime.TryParseExact (GameData.Instance.limitDate, "yyyy-MM-ddTHH:mm:ss+00:00", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out dateLocal);
			} else {
				//ilk kullanim 
				GameData.Instance.limitExceeded = 0;
				GameData.Instance.Save();
//tableview reload
				yield break;
			}

			TimeSpan span = currentDate.Subtract (dateLocal);
			if (span.TotalMinutes > 240) {
				
				GameData.Instance.limitExceeded = 0;
				GameData.Instance.Save();
				//tableview reload

			} else {
				GameData.Instance.limitExceeded = 1;
				GameData.Instance.Save();
				//tableview reload

//				string message = "Unfortunately your credit limit exeeded. 4 hours later you can gain credit watching rewarded videos. You can gain more credits by purchasing game packages from the shopping list";
//				dialogueManager = DialogueManager.Instance ();
//				dialogueManager.showDialog ("Info",message, 0);
			}

		}else {
			string message = "Connection problem";
			dialogueManager = DialogueManager.Instance ();
			dialogueManager.showDialog ("Error",message, 0);
			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	public void BackButtonClicked()
	{
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		Debug.Log("Back button Clicked");

		Application.LoadLevel("MainMenu");
		
	}

	public string timeFormatted(long totalSeconds)
	{
		
		long seconds = totalSeconds % 60;
		long minutes = (totalSeconds / 60) % 60;
		long hours = totalSeconds / 3600;

		string time = string.Format ("{0:00}:{1:00}:{2:00}",hours, minutes, seconds);

		return time;
	}

	public string remainingTime(){
		DateTime utcLocal = DateTime.Now;
//		NSDate *utcLocal = nil;
		if (!GameData.Instance.limitDate.Equals (string.Empty)) {
			DateTime.TryParseExact (GameData.Instance.limitDate, "yyyy-MM-ddTHH:mm:ss+00:00", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out utcLocal);
		}
		
		DateTime utcCurrent = DateTime.Now;
//		long totalSeconds = 
		TimeSpan span = utcCurrent.Subtract (utcLocal);
		long totalSeconds = Convert.ToInt64(span.TotalSeconds);
		string remainingTime = string.Empty;
		if (!GameData.Instance.limitDate.Equals (string.Empty) && (240*60 - totalSeconds > 0)) {
			remainingTime = timeFormatted(240*60-totalSeconds);
		}
		
		return remainingTime;
	}

	public void checkDailyLimitExceed(){

		string url = "http://www.timeapi.org/utc/now";
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequestCheck(www));
	}


	public void showHZRewardedAdVideos(){

//		if (GameData.Instance.multiRemainingGameCount <= 0) {

//			if (GameData.Instance.rewardedGameCount >= GameData.Instance.dailyLimit * 5) {

//				requestUTCTimeFromServer ();

//				GameData.Instance.rewardedGameCount = 0;
//				GameData.Instance.Save ();
//
//				string message = "Unfortunately your credit limit exeeded. 4 hours later you can gain credit watching rewarded videos. You can gain more credits by purchasing game packages from the shopping list";
//				dialogueManager = DialogueManager.Instance ();
//				dialogueManager.showDialog ("Info",message, 0);

//				return;
//			} 

			if (HZIncentivizedAd.IsAvailable ()) {
				HZIncentivizedAd.Show ();
			}
			
			HZIncentivizedAd.Fetch ();

//		} else {

//			string message = "Please try again when your credit runs out. In case of no credits left you can gain again";
//			dialogueManager = DialogueManager.Instance ();
//			dialogueManager.showDialog ("Info",message, 0);
//		}
	}

	public void OnRewardedVideoClicked()
	{
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

//		if (GameData.Instance.limitExceeded == 0) {
			GameData.Instance.multiRemainingGameCount += 5;
			GameData.Instance.rewardedGameCount += 5;
			GameData.Instance.Save();
			showHZRewardedAdVideos ();
//		} else {
//			string message = string.Format("Unfortunately your credit limit exeeded. {0} hours later you can gain credit watching rewarded videos. " +
//				"You can gain more credits by purchasing game packages from the shopping list", remainingTime());
//			dialogueManager = DialogueManager.Instance ();
//			dialogueManager.showDialog ("Info",message, 0);
//		}
	}
}

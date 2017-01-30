using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Tacticsoft;
using UnityEngine.UI;
using System.Timers;
using Parse;
using Heyzap;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour,ITableViewDataSource,MPUpdateListener {

	public GuessResultCell m_cellPrefab;
	public TableView m_tableView;
	public AudioClip menuClip;
	public int m_numRows;
	private int m_numInstancesCreated = 0;
	public GoogleAnalyticsV4 googleAnalytics;
	public List<Number> computerMovesHistory;
	private bool isFindNumber = false;
	private bool isFoundedInMultiplayer = false;
	public bool iStarted = false;
	private bool isCompFindNumber = false;
	public bool isMyTurn;
	public bool multiPlaying;
	public GameObject sendButton;
	public GameObject setButton;
	public GameObject sendButton4;
	public GameObject setButton4;
	public GameObject numberPlace;
	public GameObject numberPlace4;
	public GameObject blankFirst;
	public GameObject blankSecond;
	public GameObject blankThird;
	public GameObject blankFirst4;
	public GameObject blankSecond4;
	public GameObject blankThird4;
	public GameObject blankFourth4;

	private DialogueManager dialogueManager;

	private List<ResultNumber> resultNumbers;

	public GamePlay.GameMode gameMode;
	private static GameController gameLogic;

	public Image LifeImage;
	public Image PointsImage;

	public Image myFirstLabel;
	public Image mySecondLabel;
	public Image myThirdLabel;
	public Image myFourthLabel;

	public Image oppFirstLabel;
	public Image oppSecondLabel;
	public Image oppThirdLabel;
	public Image oppFourthLabel;

	public Image powerFirstImage;
	public Image powerFirstImage2;
	public Image powerDotImage;
	public Image powerSecondImage;
	public Image powerThirdImage;
	public Image powerFourthImage;

	public Image minutesImage;
	public Image ColumnImage;
	public Image seconds1Image;
	public Image seconds2Image;


	public Image AIAvatarImage;
	public Text oppHeader;
	public Text myHeader;

	public Text elapsedTimeLabel;
	public Text pointsLabel;
	public Text multiGameCount;
	public Text singleGameCount;
	public Image singleCountImage;

	private static System.Timers.Timer timer;
	private static int currMinute;
	private static int currSeconds;

	private const int  kWinPoints = 30;
	private const int  kDrawPoints = 10;
	private const int  kGameDuration = 3;

	int tryLoad = 0;

	private float remainingTime = 300;
	public bool isStoped;
	private float minutes;
	private float seconds;


	public static GameController Instance () {
		if (!gameLogic) {
			gameLogic = FindObjectOfType(typeof (GameController)) as GameController;
			if (!gameLogic)
				Debug.LogError ("There needs to be one active GameController script on a GameObject in your scene.");
		}
		
		return gameLogic;
	}
	
	
	
	void Awake () {
//		gameLogic = this;
	}

	
	//Register as the TableView's delegate (required) and data source (optional)
	//to receive the calls
	void Start() {
		m_tableView.dataSource = this;
		HZIncentivizedAd.Fetch ();
//		HZVideoAd.Fetch();
		HZBannerAd.Hide();
		initializeGame ();
	} 

	public void startTimer(float from){
		isStoped = false;
		remainingTime = from;
		Update();
		StartCoroutine(updateCoroutine());
	}

	private IEnumerator updateCoroutine(){
		while(!isStoped){


			string time =  string.Format("{0:0}:{1:00}", minutes, seconds);
			
			string first = time.Substring(0,1); 
			string second = time.Substring(2,1); 
			string third = time.Substring(3,1); 

			minutesImage.sprite = Resources.Load <Sprite>("Power"+first);
			ColumnImage.sprite = Resources.Load <Sprite>("PowerColumn");
			seconds1Image.sprite = Resources.Load <Sprite>("Power"+second);
			seconds2Image.sprite = Resources.Load <Sprite>("Power"+third);

//			elapsedTimeLabel.text = string.Format("{0:0}:{1:00}", minutes, seconds);

			yield return new WaitForSeconds(0.2f);
		}
	}

	void Update() {

		#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (HeyzapAds.OnBackPressed())
				return;
			else
				BackButtonClicked ();
		}
		#endif


		if (gameMode == GamePlay.GameMode.MultiPlayer) {

			if(isStoped) return;
			remainingTime -= Time.deltaTime;
			
			minutes = Mathf.Floor(remainingTime / 60);
			seconds = remainingTime % 60;
			if(seconds > 59) seconds = 59;
			if(minutes < 0) {
				isStoped = true;
				minutes = 0;
				seconds = 0;

				//TODO: number nesnesi integer sayi degerine cevrilecek
				string number = string.Empty;
				if (GameData.Instance.appModeType == 3) { 
					number = string.Format ("{0}{1}{2}", NumberGameAI.Instance.opponentNumber.FirstNumber, NumberGameAI.Instance.opponentNumber.SecondNumber, NumberGameAI.Instance.opponentNumber.ThirdNumber);
				} else {
					number = string.Format ("{0}{1}{2}{3}", NumberGameAI.Instance.opponentNumber.FirstNumber, NumberGameAI.Instance.opponentNumber.SecondNumber, NumberGameAI.Instance.opponentNumber.ThirdNumber,NumberGameAI.Instance.opponentNumber.FourthNumber);
				}
				int iNumber = (int.Parse(number));
				 

				MultiplayerController.Instance.SendGameOver(iNumber, NumberGameAI.Instance.opponentNumber.PozitiveResult, NumberGameAI.Instance.opponentNumber.NegativeResult,false); 
				
				// maci lokal oyuncu zamandan kaybeder
				calculateScoreTablesForMulti(false);
				
				string title = "Info";
				string message = "Unfortunatelly you lost! Would you like to play again? \n";
				
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showConfirmDialog(title,message,10,true);
			}


//			if (!isStoped) // has the level been completed
//			{
//				remainingTime -= Time.deltaTime; // I need timer which from a particular time goes to zero
//			} 
//
//			var minutes = remainingTime / 60;  //Divide the guiTime by sixty to get the minutes.
//			var seconds = remainingTime % 60;  //Use the euclidean division for the seconds.
//			var fraction = (remainingTime * 100) % 100;
//
//			if (remainingTime > 0)
//			{
//				//update the label value
//				elapsedTimeLabel.text = string.Format ("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
////				guiText.text = timer.ToString("F0");
//			}else{
//				isStoped = true;
//				MultiplayerController.Instance.SendGameOver(NumberGameAI.Instance.opponentNumber.FirstNumber, NumberGameAI.Instance.opponentNumber.PozitiveResult, NumberGameAI.Instance.opponentNumber.NegativeResult,false); 
//				
//				// maci lokal oyuncu zamandan kaybeder
//				calculateScoreTablesForMulti(false);
//
//				string title = "Info";
//				string message = "Unfortunatelly you lost! Would you like to play again? \n";
//				
//				dialogueManager = DialogueManager.Instance();
//				dialogueManager.showConfirmDialog(title,message,10,true);
//
//			}



		}

	}

	#region ITableViewDataSource
	
	//Will be called by the TableView to know how many rows are in this table
	public int GetNumberOfRowsForTableView(TableView tableView) {
		return resultNumbers.Count;
	}
	
	//Will be called by the TableView to know what is the height of each row
	public float GetHeightForRowInTableView(TableView tableView, int row) {
		return (m_cellPrefab.transform as RectTransform).rect.height;
	}
	
	//Will be called by the TableView when a cell needs to be created for display
	public TableViewCell GetCellForRowInTableView(TableView tableView, int row) {

		ResultNumber rowData = null;
		GuessResultCell cell = tableView.GetReusableCell(m_cellPrefab.reuseIdentifier) as GuessResultCell;
//		if (cell == null) {
			cell = (GuessResultCell)GameObject.Instantiate(m_cellPrefab);
			cell.name = "VisibleCounterCellInstance_" + (++m_numInstancesCreated).ToString();

//		}


		if(resultNumbers.Count > 0){
			rowData = (ResultNumber)resultNumbers[row];

			if (rowData.myNumber != null && rowData.myNumber.FirstNumber !=-1) {
				cell.SetRowNumber(row+1);
				cell.SetPozitiveImage();
				cell.SetNegativeImage();
				string number = string.Empty;
				if (GameData.Instance.appModeType == 3) { 
					number = string.Format ("{0}{1}{2}", rowData.myNumber.FirstNumber, rowData.myNumber.SecondNumber, rowData.myNumber.ThirdNumber);
				} else {
					number = string.Format ("{0}{1}{2}{3}", rowData.myNumber.FirstNumber, rowData.myNumber.SecondNumber, rowData.myNumber.ThirdNumber,rowData.myNumber.FourthNumber);
				}
				cell.SetNumber (int.Parse(number));
				cell.SetPozitive (rowData.myNumber.PozitiveResult);
				cell.SetNegative (rowData.myNumber.NegativeResult);
			}

			if (rowData.estimatedNumber != null && rowData.estimatedNumber.FirstNumber !=-1) {
				cell.SetPozitiveImage2();
				cell.SetNegativeImage2();
				cell.SetRowNumber2(row+1);
				string number = string.Empty;
				if (GameData.Instance.appModeType == 3) { 
					number = string.Format ("{0}{1}{2}", rowData.estimatedNumber.FirstNumber, rowData.estimatedNumber.SecondNumber, rowData.estimatedNumber.ThirdNumber);
				} else {
					number = string.Format ("{0}{1}{2}{3}", rowData.estimatedNumber.FirstNumber, rowData.estimatedNumber.SecondNumber, rowData.estimatedNumber.ThirdNumber, rowData.estimatedNumber.FourthNumber);
				}
				cell.SetNumber2 (int.Parse(number));
				cell.SetPozitive2 (rowData.estimatedNumber.PozitiveResult);
				cell.SetNegative2 (rowData.estimatedNumber.NegativeResult);
			}

		}


		return cell;
	}
	
	#endregion

	#region Table View event handlers
	
	//Will be called by the TableView when a cell's visibility changed
	public void TableViewCellVisibilityChanged(int row, bool isVisible) {

		if (isVisible) {
			GuessResultCell cell = (GuessResultCell)m_tableView.GetCellAtRow(row);
		}
	}
	
	#endregion


	public void setMyNumber(Number myNumbers) {

		Sprite first =  Resources.Load <Sprite>(myNumbers.FirstNumber.ToString ());
		Sprite second =  Resources.Load <Sprite>(myNumbers.SecondNumber.ToString ());
		Sprite third =  Resources.Load <Sprite>(myNumbers.ThirdNumber.ToString ());
		Sprite fourth =  Resources.Load <Sprite>(myNumbers.FourthNumber.ToString ());

		if (GameData.Instance.appModeType == 3) { 
			oppSecondLabel.sprite = first;
			oppThirdLabel.sprite = second;
			oppFourthLabel.sprite = third;
		} else {
			oppFirstLabel.sprite = first;
			oppSecondLabel.sprite = second;
			oppThirdLabel.sprite = third;
			oppFourthLabel.sprite = fourth;
		}

	}
	
	public void setMyNumber() {

		Sprite first =  Resources.Load <Sprite>(NumberGameAI.Instance.myNumber.FirstNumber.ToString ());
		Sprite second =  Resources.Load <Sprite>(NumberGameAI.Instance.myNumber.SecondNumber.ToString ());
		Sprite third =  Resources.Load <Sprite>(NumberGameAI.Instance.myNumber.ThirdNumber.ToString ());
		Sprite fourth =  Resources.Load <Sprite>(NumberGameAI.Instance.myNumber.FourthNumber.ToString ());
		Sprite questionMark =  Resources.Load <Sprite>("_");

		if (GameData.Instance.appModeType == 3) { 
			oppSecondLabel.sprite = first;
			oppThirdLabel.sprite = second;
			oppFourthLabel.sprite = third;
		} else {
			oppFirstLabel.sprite = first;
			oppSecondLabel.sprite = second;
			oppThirdLabel.sprite = third;
			oppFourthLabel.sprite = fourth;
		}

		myFirstLabel.sprite = questionMark;
		mySecondLabel.sprite = questionMark;
		myThirdLabel.sprite =  questionMark;
		if (GameData.Instance.appModeType == 4) { 
			myFourthLabel.sprite = questionMark;
		}

	}
	
	private void initNumbersLabel() {

		Sprite questionMark =  Resources.Load <Sprite>("_");

		if (GameData.Instance.appModeType == 3) { 
			oppSecondLabel.sprite = questionMark;
			oppThirdLabel.sprite = questionMark;
			oppFourthLabel.sprite = questionMark;
		} else {
			oppFirstLabel.sprite = questionMark;
			oppSecondLabel.sprite = questionMark;
			oppThirdLabel.sprite = questionMark;
			oppFourthLabel.sprite = questionMark;
		}


		myFirstLabel.sprite = questionMark;
		mySecondLabel.sprite = questionMark;
		myThirdLabel.sprite = questionMark;
		if (GameData.Instance.appModeType == 4) { 
			myFourthLabel.sprite = questionMark;
		}
		
	}
	
	private void setOppenentsNumber(Number foundNumber) {

		Sprite first =  Resources.Load <Sprite>(foundNumber.FirstNumber.ToString ());
		Sprite second =  Resources.Load <Sprite>(foundNumber.SecondNumber.ToString ());
		Sprite third =  Resources.Load <Sprite>(foundNumber.ThirdNumber.ToString ());
		Sprite fourth =  Resources.Load <Sprite>(foundNumber.FourthNumber.ToString ());

		myFirstLabel.sprite = first;
		mySecondLabel.sprite = second;
		myThirdLabel.sprite = third;
		if (GameData.Instance.appModeType == 4) { 
			myFourthLabel.sprite = fourth;
		}
		
	}
	
	private void setOppenentsNumberComp(Number compNumber){

		Sprite first =  Resources.Load <Sprite>(compNumber.FirstNumber.ToString ());
		Sprite second =  Resources.Load <Sprite>(compNumber.SecondNumber.ToString ());
		Sprite third =  Resources.Load <Sprite>(compNumber.ThirdNumber.ToString ());
		Sprite fourth =  Resources.Load <Sprite>(compNumber.FourthNumber.ToString ());

		myFirstLabel.sprite = first;
		mySecondLabel.sprite = second;
		myThirdLabel.sprite = third;
		if (GameData.Instance.appModeType == 4) { 
			myFourthLabel.sprite = fourth;
		}
	}

	public void LeftRoomConfirmed() {
		MultiplayerController.Instance.updateListener = null;
		SceneManager.LoadScene ("MainMenu");
	}

	public void PlayerLeftRoom(string participantId) {
//		if (_finishTimes[participantId] < 0) {
//			_finishTimes[participantId] = 999999.0f;
//			if (_opponentScripts[participantId] != null) {
//				_opponentScripts[participantId].HideCar();
//			}
//			CheckForMPGameOver();
//		}
	}

	private void reportScore(long points) {

		Social.ReportScore(points, "CgkIs4j8s8wCEAIQAQ", (bool success) => {
			// handle success or failure
		});
	}
	
	
	private void reportSingleAvarageSteps(double steps) {

		//game power
		long lsteps = (long)steps * 1000;
		Social.ReportScore(lsteps, "CgkIs4j8s8wCEAIQAw", (bool success) => {
			// handle success or failure
		});

		//master club
		long lstepsm = (long)steps * 1000;
		Social.ReportScore(lstepsm, "CgkIs4j8s8wCEAIQAg", (bool success) => {
			// handle success or failure
		});

	}
	
	private void reportAvarageTimes(long time) {
//		GKScore *score = [[GKScore alloc] initWithLeaderboardIdentifier:kScoreTimeLevelID];
//		score.value = time;
//		
//		[GKScore reportScores:@[score] withCompletionHandler:^(NSError *error) {
//			if (error != nil) {
//				NSLog(@"%@", [error localizedDescription]);
//			}
//		 }];
	}

	void SetGameMode ()
	{
		if (GamePlay.Instance.singleplayerGame)
			gameMode = GamePlay.GameMode.SinglePlayer;
		else
			if (GamePlay.Instance.multiplayerGame)
				gameMode = GamePlay.GameMode.MultiPlayer;
			else
				if (GamePlay.Instance.trainingGame)
					gameMode = GamePlay.GameMode.Training;
				else
					if (GamePlay.Instance.howtoplayGame)
						gameMode = GamePlay.GameMode.HowToPlay;
	}

	void SetMultiplayerTopPanel ()
	{
		LifeImage.GetComponent<CanvasGroup> ().alpha = 1;
		PointsImage.GetComponent<CanvasGroup> ().alpha = 1;
		elapsedTimeLabel.GetComponent<CanvasGroup> ().alpha = 1;
		multiGameCount.GetComponent<CanvasGroup> ().alpha = 1;
		pointsLabel.GetComponent<CanvasGroup> ().alpha = 1;
		powerFirstImage.GetComponent<CanvasGroup> ().alpha = 0;
		powerFirstImage2.GetComponent<CanvasGroup> ().alpha = 0;
		powerSecondImage.GetComponent<CanvasGroup> ().alpha = 0;
		powerThirdImage.GetComponent<CanvasGroup> ().alpha = 0;
		powerFourthImage.GetComponent<CanvasGroup> ().alpha = 0;
		powerDotImage.GetComponent<CanvasGroup> ().alpha = 0;
		singleGameCount.GetComponent<CanvasGroup> ().alpha = 0;
		singleCountImage.GetComponent<CanvasGroup> ().alpha = 0;
		oppHeader.GetComponent<CanvasGroup> ().alpha = 1;
		oppSecondLabel.GetComponent<CanvasGroup> ().alpha = 1;
		oppThirdLabel.GetComponent<CanvasGroup> ().alpha = 1;
		oppFourthLabel.GetComponent<CanvasGroup> ().alpha = 1;
		if (GameData.Instance.appModeType == 4) { 
			oppFirstLabel.GetComponent<CanvasGroup> ().alpha = 1;
		} else {
			oppFirstLabel.GetComponent<CanvasGroup> ().alpha = 0;
		}

		myHeader.GetComponent<CanvasGroup> ().alpha = 1;
		myFirstLabel.GetComponent<CanvasGroup> ().alpha = 1;
		mySecondLabel.GetComponent<CanvasGroup> ().alpha = 1;
		myThirdLabel.GetComponent<CanvasGroup> ().alpha = 1;
		if (GameData.Instance.appModeType == 4) { 
			myFourthLabel.GetComponent<CanvasGroup> ().alpha = 1;
		} else {
			myFourthLabel.GetComponent<CanvasGroup> ().alpha = 0;
		}
		minutesImage.GetComponent<CanvasGroup> ().alpha = 1;
		ColumnImage.GetComponent<CanvasGroup> ().alpha = 1;
		seconds1Image.GetComponent<CanvasGroup> ().alpha = 1;
		seconds2Image.GetComponent<CanvasGroup> ().alpha = 1;
	}

	void SetSingleplayerTopPanel ()
	{
		LifeImage.GetComponent<CanvasGroup> ().alpha = 1;
		PointsImage.GetComponent<CanvasGroup> ().alpha = 1;
		elapsedTimeLabel.GetComponent<CanvasGroup> ().alpha = 0;
		multiGameCount.GetComponent<CanvasGroup> ().alpha = 1;
		pointsLabel.GetComponent<CanvasGroup> ().alpha = 1;
		powerFirstImage.GetComponent<CanvasGroup> ().alpha = 0;
		powerFirstImage2.GetComponent<CanvasGroup> ().alpha = 1;
		powerSecondImage.GetComponent<CanvasGroup> ().alpha = 1;
		powerThirdImage.GetComponent<CanvasGroup> ().alpha = 1;
		powerFourthImage.GetComponent<CanvasGroup> ().alpha = 1;
		powerDotImage.GetComponent<CanvasGroup> ().alpha = 1;
		singleGameCount.GetComponent<CanvasGroup> ().alpha = 0;
		singleCountImage.GetComponent<CanvasGroup> ().alpha = 0;
		myHeader.GetComponent<CanvasGroup> ().alpha = 1;
		myFirstLabel.GetComponent<CanvasGroup> ().alpha = 1;
		mySecondLabel.GetComponent<CanvasGroup> ().alpha = 1;
		myThirdLabel.GetComponent<CanvasGroup> ().alpha = 1;
		if (GameData.Instance.appModeType == 4) { 
			myFourthLabel.GetComponent<CanvasGroup> ().alpha = 1;
		} else {
			myFourthLabel.GetComponent<CanvasGroup> ().alpha = 0;
		}
		oppHeader.GetComponent<CanvasGroup> ().alpha = 1;
		oppFirstLabel.GetComponent<CanvasGroup> ().alpha = 1;
		oppSecondLabel.GetComponent<CanvasGroup> ().alpha = 1;
		oppThirdLabel.GetComponent<CanvasGroup> ().alpha = 1;
		oppFourthLabel.GetComponent<CanvasGroup> ().alpha = 1;

		if (GameData.Instance.appModeType == 4) { 
			oppFirstLabel.GetComponent<CanvasGroup> ().alpha = 1;
		} else {
			oppFirstLabel.GetComponent<CanvasGroup> ().alpha = 0;
		}
		minutesImage.GetComponent<CanvasGroup> ().alpha = 0;
		ColumnImage.GetComponent<CanvasGroup> ().alpha = 0;
		seconds1Image.GetComponent<CanvasGroup> ().alpha = 0;
		seconds2Image.GetComponent<CanvasGroup> ().alpha = 0;
	}

	void SetTrainingTopPanel ()
	{
		LifeImage.GetComponent<CanvasGroup> ().alpha = 1;
		PointsImage.GetComponent<CanvasGroup> ().alpha = 1;
		elapsedTimeLabel.GetComponent<CanvasGroup> ().alpha = 0;
		multiGameCount.GetComponent<CanvasGroup> ().alpha = 1;
		pointsLabel.GetComponent<CanvasGroup> ().alpha = 1;
		powerFirstImage.GetComponent<CanvasGroup> ().alpha = 0;
		powerFirstImage2.GetComponent<CanvasGroup> ().alpha = 1;
		powerSecondImage.GetComponent<CanvasGroup> ().alpha = 1;
		powerThirdImage.GetComponent<CanvasGroup> ().alpha = 1;
		powerFourthImage.GetComponent<CanvasGroup> ().alpha = 1;
		powerDotImage.GetComponent<CanvasGroup> ().alpha = 1;
		singleGameCount.GetComponent<CanvasGroup> ().alpha = 1;
		singleCountImage.GetComponent<CanvasGroup> ().alpha = 1;
		myHeader.GetComponent<CanvasGroup> ().alpha = 1;
		myFirstLabel.GetComponent<CanvasGroup> ().alpha = 1;
		mySecondLabel.GetComponent<CanvasGroup> ().alpha = 1;
		myThirdLabel.GetComponent<CanvasGroup> ().alpha = 1;
		if (GameData.Instance.appModeType == 4) { 
			myFourthLabel.GetComponent<CanvasGroup> ().alpha = 1;
		} else {
			myFourthLabel.GetComponent<CanvasGroup> ().alpha = 0;
		}
		oppHeader.GetComponent<CanvasGroup> ().alpha = 0;
		oppFirstLabel.GetComponent<CanvasGroup> ().alpha = 0;
		oppSecondLabel.GetComponent<CanvasGroup> ().alpha = 0;
		oppThirdLabel.GetComponent<CanvasGroup> ().alpha = 0;
		oppFourthLabel.GetComponent<CanvasGroup> ().alpha = 0;
		minutesImage.GetComponent<CanvasGroup> ().alpha = 0;
		ColumnImage.GetComponent<CanvasGroup> ().alpha = 0;
		seconds1Image.GetComponent<CanvasGroup> ().alpha = 0;
		seconds2Image.GetComponent<CanvasGroup> ().alpha = 0;
	}

	void SetGameData ()
	{
		if (GameData.Instance.appModeType == 3) { 
			this.blankFirst.SetActive (true);
			this.blankSecond.SetActive (true);
			this.blankThird.SetActive (true);
			this.numberPlace.SetActive (true);
			this.blankFirst4.SetActive (false);
			this.blankSecond4.SetActive (false);
			this.blankThird4.SetActive (false);
			this.blankFourth4.SetActive (false);
			this.numberPlace4.SetActive (false);
		} else {
			this.blankFirst.SetActive (false);
			this.blankSecond.SetActive (false);
			this.blankThird.SetActive (false);
			this.numberPlace.SetActive (false);
			this.blankFirst4.SetActive (true);
			this.blankSecond4.SetActive (true);
			this.blankThird4.SetActive (true);
			this.blankFourth4.SetActive (true);
			this.numberPlace4.SetActive (true);
		}
		elapsedTimeLabel.text = "3:00";
		multiGameCount.text = GameData.Instance.multiRemainingGameCount.ToString();
		pointsLabel.text = GameData.Instance.score.ToString();


		string power = string.Empty;
		if (GameData.Instance.appModeType == 3) { 
			power = string.Format ("{0:#,0.000}", GameData.Instance.trainingAvarageSteps);
		} else {
			power = string.Format ("{0:#,0.000}", GameData.Instance.trainingAvarageSteps4);
		}

		if (power.Length == 5) {
			string first = power.Substring (0, 1); 
			string second = power.Substring (2, 1); 
			string third = power.Substring (3, 1); 
			string fourth = power.Substring (4, 1); 
			powerFirstImage.GetComponent<CanvasGroup> ().alpha = 0;
			powerFirstImage2.sprite = Resources.Load <Sprite> ("Power" + first);
			powerDotImage.sprite = Resources.Load <Sprite> ("PowerDot");
			powerSecondImage.sprite = Resources.Load <Sprite> ("Power" + second);
			powerThirdImage.sprite = Resources.Load <Sprite> ("Power" + third);
			powerFourthImage.sprite = Resources.Load <Sprite> ("Power" + fourth);

			singleGameCount.text = GameData.Instance.trainingGameCount.ToString ();
		} else {
			string first = power.Substring (0, 1); 
			string first2 = power.Substring (1, 1); 
			string second = power.Substring (3, 1); 
			string third = power.Substring (4, 1); 
			string fourth = power.Substring (5, 1); 

			powerFirstImage.GetComponent<CanvasGroup> ().alpha = 1;

			powerFirstImage.sprite = Resources.Load <Sprite> ("Power" + first);
			powerFirstImage2.sprite = Resources.Load <Sprite> ("Power" + first2);
			powerDotImage.sprite = Resources.Load <Sprite> ("PowerDot");
			powerSecondImage.sprite = Resources.Load <Sprite> ("Power" + second);
			powerThirdImage.sprite = Resources.Load <Sprite> ("Power" + third);
			powerFourthImage.sprite = Resources.Load <Sprite> ("Power" + fourth);

			singleGameCount.text = GameData.Instance.trainingGameCount.ToString ();
		}





		switch (GameData.Instance.playerId) {
		case 1:
			oppHeader.text = "Master Guesses";
			AIAvatarImage.sprite = Resources.Load <Sprite>("master@3x");
			break;
		case 2:
			oppHeader.text = "Lucy Guesses";
			AIAvatarImage.sprite = Resources.Load <Sprite>("Lucy@3x");
			break;
		case 3:
			oppHeader.text = "Maze Guesses";
			AIAvatarImage.sprite = Resources.Load <Sprite>("maze@3x");
			break;
		case 4:
			oppHeader.text = "Aveline Guesses";
			AIAvatarImage.sprite = Resources.Load <Sprite>("aveline@3x");
			break;
		case 5:
			oppHeader.text = "Vincent Guesses";
			AIAvatarImage.sprite = Resources.Load <Sprite>("vincent@3x");
			break;
		case 6:
			oppHeader.text = "Elina Guesses";
			AIAvatarImage.sprite = Resources.Load <Sprite>("elina@3x");
			break;
		case 7:
			oppHeader.text = "Rhonin Guesses";
			AIAvatarImage.sprite = Resources.Load <Sprite>("rhonin@3x");
			break;
		case 8:
			oppHeader.text = "Barret Guesses";
			AIAvatarImage.sprite = Resources.Load <Sprite>("barret@3x");
			break;

		default:
			oppHeader.text = "Master Guesses";
			AIAvatarImage.sprite = Resources.Load <Sprite>("master@3x");
			break;
		}

	}

	public void initializeGame(){

		SetGameMode ();

		SetGameData ();

		AIAvatarImage.gameObject.SetActive (false);
		switch (gameMode) {
		case GamePlay.GameMode.SinglePlayer:{
			SetSingleplayerTopPanel();
				if (GameData.Instance.appModeType == 3) { 
					setButton.SetActive (true);
					setButton4.SetActive (false);
					sendButton.SetActive (false);
					sendButton4.SetActive (false);
				} else {
					setButton4.SetActive (true);
					setButton.SetActive (false);
					sendButton.SetActive (false);
					sendButton4.SetActive (false);
				}
			AIAvatarImage.gameObject.SetActive (true);
				googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("game_action")
					.SetEventAction("single_game_started")
					.SetEventLabel("single_start"));
		}
			break;
		case GamePlay.GameMode.HowToPlay:
		case GamePlay.GameMode.Training:{
			SetTrainingTopPanel();
				if (GameData.Instance.appModeType == 3) { 
					setButton.SetActive (false);
					setButton4.SetActive (false);
					sendButton.SetActive (true);
					sendButton4.SetActive (false);
					NumberGameAI.Instance.createRandomComputerNumber3();

				} else {
					setButton4.SetActive (false);
					setButton.SetActive (false);
					sendButton4.SetActive (true);
					sendButton.SetActive (false);
					NumberGameAI.Instance.createRandomComputerNumber4();
				}
				googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("game_action")
					.SetEventAction("training_game_started")
					.SetEventLabel("training_start"));
		}
			break;
		case GamePlay.GameMode.MultiPlayer:{
			isStoped = true;
			MultiplayerController.Instance.matchStarted();
			SetMultiplayerTopPanel();
				if (GameData.Instance.appModeType == 3) { 
					setButton.SetActive (true);
					setButton4.SetActive (false);
					sendButton.SetActive (false);
					sendButton4.SetActive (false);
				} else {
					setButton4.SetActive (true);
					setButton.SetActive (false);
					sendButton4.SetActive (false);
					sendButton.SetActive (false);
				}
				googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("game_action")
					.SetEventAction("multi_game_started")
					.SetEventLabel("multi_start"));
		}
			break;
		default:
			break;
		}


		initNumbersLabel();

		
		isFindNumber = false;
		isCompFindNumber = false;
		isMyTurn = false;

		if (GamePlay.Instance.singleplayerGame) {
			iStarted = true;
		}else{
			iStarted = false;
		}

		isFoundedInMultiplayer = false;

		computerMovesHistory = new List<Number> ();
		resultNumbers = new List<ResultNumber> ();

		NumberGameAI.Instance.doubleDigits = new ArrayList();
		NumberGameAI.Instance.doubleDigits.Add (0);
		NumberGameAI.Instance.doubleDigits.Add (1);
		NumberGameAI.Instance.doubleDigits.Add (2);
		NumberGameAI.Instance.doubleDigits.Add (3);
		NumberGameAI.Instance.doubleDigits.Add (4);
		NumberGameAI.Instance.doubleDigits.Add (5);
		NumberGameAI.Instance.doubleDigits.Add (6);
		NumberGameAI.Instance.doubleDigits.Add (7);
		NumberGameAI.Instance.doubleDigits.Add (8);
		NumberGameAI.Instance.doubleDigits.Add (9);

		if (GameData.Instance.appModeType == 3) { 
			NumberGameAI.Instance.createNumbersPool3 ();
			NumberGameAI.Instance.createMyNumbersPool3 ();
		} else {
			NumberGameAI.Instance.createNumbersPool4 ();
			NumberGameAI.Instance.createMyNumbersPool4 ();
		}

//		reloadTableView ();
	}

	void reloadTableView ()
	{
		m_tableView.ReloadData ();
//		int tryCount = 0;
//		while (true) {
//
//			if(tryCount == 3)
//				return;
//
//			try {
//				m_tableView.ReloadData ();
//				return;
//			}
//			catch {
//				Debug.Log("reloadTableView exeption");
//			}
//
//			tryCount++;
//		}
	}

	public void restartMultiGame(bool rematch){


		if (!rematch)
			return;

		if (GameData.Instance.multiRemainingGameCount <= 0) {
			string title = "Info";
			string message = "You don't have enough credit to play online game. You can get 5 credits by watching rewarded videos. In addition, You can get more credits by purchasing game packages from the shopping list";
			dialogueManager = DialogueManager.Instance();
			dialogueManager.showDialog(title,message,0);
			
			return;
		}


		GamePlay.Instance.resetNumbers2();

		SetMultiplayerTopPanel();
		if (GameData.Instance.appModeType == 3) { 
			setButton.SetActive (false);
			setButton4.SetActive (false);
			sendButton.SetActive (false);
			sendButton4.SetActive (false);
		} else {
			setButton.SetActive (false);
			setButton4.SetActive (false);
			sendButton4.SetActive (false);
			sendButton.SetActive (false);
		}

		
		SetGameData ();
		
		initNumbersLabel();
		isFindNumber = false;
		isCompFindNumber = false;
		isMyTurn = false;

		
		if (!GamePlay.Instance.multiplayerGame) {
			iStarted = true;
		}else{
			iStarted = false;
		}
		
		isFoundedInMultiplayer = false;
		
		computerMovesHistory.Clear();
		computerMovesHistory = null;
		computerMovesHistory = new List<Number> ();
		
		
		resultNumbers.Clear ();
		resultNumbers = null;
		resultNumbers = new List<ResultNumber> ();
		
		
		NumberGameAI.Instance.doubleDigits = new ArrayList();
		NumberGameAI.Instance.doubleDigits.Add (0);
		NumberGameAI.Instance.doubleDigits.Add (1);
		NumberGameAI.Instance.doubleDigits.Add (2);
		NumberGameAI.Instance.doubleDigits.Add (3);
		NumberGameAI.Instance.doubleDigits.Add (4);
		NumberGameAI.Instance.doubleDigits.Add (5);
		NumberGameAI.Instance.doubleDigits.Add (6);
		NumberGameAI.Instance.doubleDigits.Add (7);
		NumberGameAI.Instance.doubleDigits.Add (8);
		NumberGameAI.Instance.doubleDigits.Add (9);
		

		if (GameData.Instance.appModeType == 3) {
			NumberGameAI.Instance.createNumbersPool3 ();
			NumberGameAI.Instance.createMyNumbersPool3 ();
		} else {
			NumberGameAI.Instance.createNumbersPool4 ();
			NumberGameAI.Instance.createMyNumbersPool4 ();
		}
		
		
		GamePlay.Instance.isSetFirstNumber = false;
		GamePlay.Instance.isSetSecondNumber = false;
		GamePlay.Instance.isSetThirdNumber = false;
		GamePlay.Instance.isSetFourthNumber = false;
		
		GamePlay.Instance.firstNumber = 0;
		GamePlay.Instance.secondNumber = 0;
		GamePlay.Instance.thirdNumber = 0;
		GamePlay.Instance.fourthNumber = 0;
		
		GamePlay.Instance.FirstNumberObject = null;
		GamePlay.Instance.SecondNumberObject = null;
		GamePlay.Instance.ThirdNumberObject = null;
		GamePlay.Instance.FourthNumberObject = null;

		reloadTableView ();


		// tekrar mac istegi mesaji gonder

		MultiplayerController.Instance.matchRestarted();

	}

	public void restartGame(){

		switch (gameMode) {
		case GamePlay.GameMode.SinglePlayer:{
			SetSingleplayerTopPanel();
				if (GameData.Instance.appModeType == 3) { 
					setButton.SetActive (true);
					setButton4.SetActive (false);
					sendButton.SetActive (false);
					sendButton4.SetActive (false);
				} else {
					setButton.SetActive (false);
					setButton4.SetActive (true);
					sendButton4.SetActive (false);
					sendButton.SetActive (false);
				}
				googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("game_action")
					.SetEventAction("single_game_started")
					.SetEventLabel("single_restart"));
				
		}
			break;
		case GamePlay.GameMode.Training:{
			SetTrainingTopPanel();
				if (GameData.Instance.appModeType == 3) { 
					setButton.SetActive (false);
					setButton4.SetActive (false);
					sendButton.SetActive (true);
					sendButton4.SetActive (false);
					NumberGameAI.Instance.createRandomComputerNumber3();
				} else {
					setButton.SetActive (false);
					setButton4.SetActive (false);
					sendButton4.SetActive (true);
					sendButton.SetActive (false);
					NumberGameAI.Instance.createRandomComputerNumber4();
				}
			
				googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("game_action")
					.SetEventAction("training_game_started")
					.SetEventLabel("training_restart"));
		}
			break;
		case GamePlay.GameMode.MultiPlayer:{
			SetMultiplayerTopPanel();
				if (GameData.Instance.appModeType == 3) { 
					setButton.SetActive (true);
					setButton4.SetActive (false);
					sendButton.SetActive (false);
					sendButton4.SetActive (false);
				} else {
					setButton.SetActive (false);
					setButton4.SetActive (true);
					sendButton4.SetActive (false);
					sendButton.SetActive (false);
				}
				googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("game_action")
					.SetEventAction("multi_game_started")
					.SetEventLabel("multi_restart"));
		}
			break;
		default:
			break;
		}


		SetGameData ();

		initNumbersLabel();
		isFindNumber = false;
		isCompFindNumber = false;
		isMyTurn = false;


		GamePlay.Instance.resetNumbers2();

		if (!GamePlay.Instance.multiplayerGame) {
			iStarted = true;
		}else{
			iStarted = false;
		}
		
		isFoundedInMultiplayer = false;
		
		computerMovesHistory.Clear();
		computerMovesHistory = new List<Number> ();


		resultNumbers.Clear ();
		resultNumbers = null;
		resultNumbers = new List<ResultNumber> ();

		
		NumberGameAI.Instance.doubleDigits = new ArrayList();
		NumberGameAI.Instance.doubleDigits.Add (0);
		NumberGameAI.Instance.doubleDigits.Add (1);
		NumberGameAI.Instance.doubleDigits.Add (2);
		NumberGameAI.Instance.doubleDigits.Add (3);
		NumberGameAI.Instance.doubleDigits.Add (4);
		NumberGameAI.Instance.doubleDigits.Add (5);
		NumberGameAI.Instance.doubleDigits.Add (6);
		NumberGameAI.Instance.doubleDigits.Add (7);
		NumberGameAI.Instance.doubleDigits.Add (8);
		NumberGameAI.Instance.doubleDigits.Add (9);

		if (GameData.Instance.appModeType == 3) { 
			NumberGameAI.Instance.createNumbersPool3 ();
			NumberGameAI.Instance.createMyNumbersPool3 ();
		} else {
			NumberGameAI.Instance.createNumbersPool4 ();
			NumberGameAI.Instance.createMyNumbersPool4 ();
		}


		GamePlay.Instance.isSetFirstNumber = false;
		GamePlay.Instance.isSetSecondNumber = false;
		GamePlay.Instance.isSetThirdNumber = false;
		GamePlay.Instance.isSetFourthNumber = false;

		GamePlay.Instance.firstNumber = 0;
		GamePlay.Instance.secondNumber = 0;
		GamePlay.Instance.thirdNumber = 0;
		GamePlay.Instance.fourthNumber = 0;
		
		GamePlay.Instance.FirstNumberObject = null;
		GamePlay.Instance.SecondNumberObject = null;
		GamePlay.Instance.ThirdNumberObject = null;
		GamePlay.Instance.FourthNumberObject = null;

		reloadTableView ();
	}

	private void LeaveMPGame() {
		MultiplayerController.Instance.LeaveGame();
	}

	public void BackButtonClicked()
	{
		if (GameData.Instance.isMusicON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		Debug.Log("Back button Clicked");

		GamePlay.Instance.isSetFirstNumber = false;
		GamePlay.Instance.isSetSecondNumber = false;
		GamePlay.Instance.isSetThirdNumber = false;
		GamePlay.Instance.isSetFourthNumber = false;
		
		GamePlay.Instance.firstNumber = 0;
		GamePlay.Instance.secondNumber = 0;
		GamePlay.Instance.thirdNumber = 0;
		GamePlay.Instance.fourthNumber = 0;
		
		GamePlay.Instance.FirstNumberObject = null;
		GamePlay.Instance.SecondNumberObject = null;
		GamePlay.Instance.ThirdNumberObject = null;
		GamePlay.Instance.FourthNumberObject = null;

		if (gameMode == GamePlay.GameMode.MultiPlayer) {
			isStoped = true;
			LeaveMPGame ();
		}

		SceneManager.LoadScene("MainMenu");

	}

	private void calculateRankingPoints(int point, int minute) {
		
		long time = minute*60 - (currMinute * 60 + currSeconds);
		
		
		if (time <= 40) {
			point += 20;
		}else if( time > 40 && time <= 80){
			point *= 10;
		}
		
		// beraberlikte de score gonderiliyor numara bulundu.
		GameData.Instance.score += point;
		GameData.Instance.Save();
		
	}

	private void calculateScoreTablesForMulti(bool didWin){
		
		//oyun sayisini bir artir
		GameData.Instance.multiGameCount++;

		// kalan oyun sayisini bir azalt
		GameData.Instance.multiRemainingGameCount--;

		reportScore(GameData.Instance.score);
		
		int time = kGameDuration*60 - (currMinute * 60 + currSeconds);
		GameData.Instance.multiAvarageTime = (GameData.Instance.multiAvarageTime * GameData.Instance.multiGameCount + time) / (GameData.Instance.multiGameCount + 1);
		
		
		if (GameData.Instance.multiGameCount >= 20) {
			reportAvarageTimes(GameData.Instance.multiAvarageTime);
		}
		
		GameData.Instance.Save();

		string power = string.Empty;
		if (GameData.Instance.appModeType == 3) { 
			power = string.Format ("{0:#,0.000}", GameData.Instance.trainingAvarageSteps);
		} else {
			power = string.Format ("{0:#,0.000}", GameData.Instance.trainingAvarageSteps4);
		}
		if (power.Length == 5) {
			string first = power.Substring (0, 1); 
			string second = power.Substring (2, 1); 
			string third = power.Substring (3, 1); 
			string fourth = power.Substring (4, 1); 
			powerFirstImage2.sprite = Resources.Load <Sprite> ("Power" + first);
			powerDotImage.sprite = Resources.Load <Sprite> ("PowerDot");
			powerSecondImage.sprite = Resources.Load <Sprite> ("Power" + second);
			powerThirdImage.sprite = Resources.Load <Sprite> ("Power" + third);
			powerFourthImage.sprite = Resources.Load <Sprite> ("Power" + fourth);


		} else {
			string first = power.Substring (0, 1); 
			string first2 = power.Substring (1, 1); 
			string second = power.Substring (3, 1); 
			string third = power.Substring (4, 1); 
			string fourth = power.Substring (5, 1); 

			powerFirstImage.sprite = Resources.Load <Sprite> ("Power" + first);
			powerFirstImage2.sprite = Resources.Load <Sprite> ("Power" + first2);
			powerDotImage.sprite = Resources.Load <Sprite> ("PowerDot");
			powerSecondImage.sprite = Resources.Load <Sprite> ("Power" + second);
			powerThirdImage.sprite = Resources.Load <Sprite> ("Power" + third);
			powerFourthImage.sprite = Resources.Load <Sprite> ("Power" + fourth);


		}


		pointsLabel.text = string.Format("{0}",GameData.Instance.score);
		multiGameCount.text = string.Format("{0}",GameData.Instance.multiRemainingGameCount);
		
		currMinute=kGameDuration;
		currSeconds=0;
		elapsedTimeLabel.text = "3:00";
		multiPlaying = false;
//		[GameKitHelper sharedGameKitHelper].remotePlayerPhoto = nil;
//		[GameKitHelper sharedGameKitHelper].localPlayerPhoto = nil;
		
	}

	private void calculateScoreTablesForSingle(bool didWin) {
		
		
		if (gameMode ==  GamePlay.GameMode.Training) {
			float step = resultNumbers.Count;

			if (GameData.Instance.appModeType == 3) { 
				if (GameData.Instance.trainingAvarageSteps > 0) {
				
					// sadece training modunda oyun gucu hesaplanacak
					// onceki ortalama oyun sayisi ile carpilip yeni basamak eklendikten sonra oyun sayisi + 1 bolunerek son basamak ortalami bulunuyor
					GameData.Instance.trainingAvarageSteps = (GameData.Instance.trainingAvarageSteps * GameData.Instance.trainingGameCount + step) / (GameData.Instance.trainingGameCount + 1);
				
				} else {
					GameData.Instance.trainingAvarageSteps = step;
				}
			
				//oyun sayisini bir artir
				GameData.Instance.trainingGameCount++;
			} else {
				if (GameData.Instance.trainingAvarageSteps4 > 0) {

					// sadece training modunda oyun gucu hesaplanacak
					// onceki ortalama oyun sayisi ile carpilip yeni basamak eklendikten sonra oyun sayisi + 1 bolunerek son basamak ortalami bulunuyor
					GameData.Instance.trainingAvarageSteps4 = (GameData.Instance.trainingAvarageSteps4 * GameData.Instance.trainingGameCount4 + step) / (GameData.Instance.trainingGameCount4 + 1);

				} else {
					GameData.Instance.trainingAvarageSteps4 = step;
				}

				//oyun sayisini bir artir
				GameData.Instance.trainingGameCount4++;
			}
			
		}
		
		//oyun sayisini bir artir
		GameData.Instance.singleGameCount++;
		
		GameData.Instance.Save();


		if (GameData.Instance.appModeType == 3) { 
			if (GameData.Instance.trainingGameCount >= 10) {
				reportSingleAvarageSteps (GameData.Instance.trainingAvarageSteps);
			}
		} else {
			if (GameData.Instance.trainingGameCount4 >= 10) {
				reportSingleAvarageSteps (GameData.Instance.trainingAvarageSteps4);
			}
		}

		string power = string.Empty;
		if (GameData.Instance.appModeType == 3) { 
			power = string.Format ("{0:#,0.000}", GameData.Instance.trainingAvarageSteps);
		} else {
			power = string.Format ("{0:#,0.000}", GameData.Instance.trainingAvarageSteps4);
		}
		if (power.Length == 5) {
			string first = power.Substring (0, 1); 
			string second = power.Substring (2, 1); 
			string third = power.Substring (3, 1); 
			string fourth = power.Substring (4, 1); 
			powerFirstImage2.sprite = Resources.Load <Sprite> ("Power" + first);
			powerDotImage.sprite = Resources.Load <Sprite> ("PowerDot");
			powerSecondImage.sprite = Resources.Load <Sprite> ("Power" + second);
			powerThirdImage.sprite = Resources.Load <Sprite> ("Power" + third);
			powerFourthImage.sprite = Resources.Load <Sprite> ("Power" + fourth);


		} else {
			string first = power.Substring (0, 1); 
			string first2 = power.Substring (1, 1); 
			string second = power.Substring (3, 1); 
			string third = power.Substring (4, 1); 
			string fourth = power.Substring (5, 1); 

			powerFirstImage.sprite = Resources.Load <Sprite> ("Power" + first);
			powerFirstImage2.sprite = Resources.Load <Sprite> ("Power" + first2);
			powerDotImage.sprite = Resources.Load <Sprite> ("PowerDot");
			powerSecondImage.sprite = Resources.Load <Sprite> ("Power" + second);
			powerThirdImage.sprite = Resources.Load <Sprite> ("Power" + third);
			powerFourthImage.sprite = Resources.Load <Sprite> ("Power" + fourth);


		}


		if (gameMode == GamePlay.GameMode.Training) {
			if (GameData.Instance.appModeType == 3) { 
				singleGameCount.text = string.Format ("{0}", GameData.Instance.trainingGameCount);
			} else {
				singleGameCount.text = string.Format ("{0}", GameData.Instance.trainingGameCount4);
			}
		}else{
			singleGameCount.text = string.Format("{0}",GameData.Instance.singleGameCount);
		}

		if (GameData.Instance.appModeType == 3) {
			if (GameData.Instance.trainingGameCount >= 99) {
				GameData.Instance.ResetGamePower ();
			} 
		} else {
			if (GameData.Instance.trainingGameCount4 >= 99) {
				GameData.Instance.ResetGamePower4 ();
			}
		}
		
	}

	public void chooseNumber(int number){
		string sNumber = string.Empty;
		Number estimatedNumber = new Number();
		if (GameData.Instance.appModeType == 3) { 
			sNumber = number.ToString ().PadLeft (3, '0');
			estimatedNumber.FirstNumber =  int.Parse(sNumber.Substring(0,1)); 
			estimatedNumber.SecondNumber = int.Parse(sNumber.Substring(1,1)); 
			estimatedNumber.ThirdNumber = int.Parse(sNumber.Substring(2,1)); 
		} else {
			sNumber = number.ToString ().PadLeft (4, '0');
			estimatedNumber.FirstNumber =  int.Parse(sNumber.Substring(0,1)); 
			estimatedNumber.SecondNumber = int.Parse(sNumber.Substring(1,1)); 
			estimatedNumber.ThirdNumber = int.Parse(sNumber.Substring(2,1)); 
			estimatedNumber.FourthNumber = int.Parse(sNumber.Substring(3,1)); 
		}

		NumberGameAI.Instance.opponentNumber = estimatedNumber;

		if (iStarted) {
			// gelen numara mesajla gosteriliyor
			string title = "Info";
			string message = "Your opponent chose her/his code and you can start to play now";
			dialogueManager = DialogueManager.Instance();
			dialogueManager.showDialog(title,message,0);

			if (GameData.Instance.appModeType == 3) { 
				sendButton.SetActive (true);
				sendButton4.SetActive (false);
				setButton.SetActive (false);
				setButton4.SetActive (false);
			} else {
				sendButton.SetActive (false);
				sendButton4.SetActive (true);
				setButton4.SetActive (false);
				setButton.SetActive (false);
			}
			
			elapsedTimeLabel.text = "3:00";
//			isStoped = false;
			startTimer(remainingTime);
			
		}else
		{
			string title = "Info";
			string message = "Please create your secret code and click on choose button";
			dialogueManager = DialogueManager.Instance();
			dialogueManager.showDialog(title,message,0);

			if (GameData.Instance.appModeType == 3) { 
				sendButton.SetActive (false);
				sendButton4.SetActive (false);
				setButton.SetActive (true);
				setButton4.SetActive (false);
			} else {
				sendButton4.SetActive (false);
				sendButton.SetActive (false);
				setButton4.SetActive (true);
				setButton.SetActive (false);
			}

		}
		
	}

	public void showStartGameMessage(bool invite) {
		
//		if(invite){
//			
//			if (![self checkRemotePlayerIds:[GameKitHelper sharedGameKitHelper].remotePlayerID]) {
//				// bu pyuncuyu kaydet tekrar davet edildiginde isleme alinmasin
//				[[RWGameData sharedGameData].playerIDs addObject:[GameKitHelper sharedGameKitHelper].remotePlayerID];
//				
//				// davet edilen oyuncuya 10 bonus oyun ver
//				[RWGameData shardGameData].multiRemainingGameCount +=10;
//				
//				[[RWGameData sharedGameData] save];
//				[self restartGame];
//			}
//			
//			//        [GameKitHelper sharedGameKitHelper].invitedGame = NO;
//			[[NSNotificationCenter defaultCenter] postNotificationName: SSGameDataUpdatedFromiCloud object:nil];
//			[self didUpdateGameData:nil];
//		}

		Debug.Log ("Game started! Your opponent will begin to play.");

		restartGame ();

		string title = "Info";
		string message = "Game started! Your opponent will begin to play.";
		dialogueManager = DialogueManager.Instance();
		dialogueManager.showDialog(title,message,0);


		googleAnalytics.LogEvent(new EventHitBuilder()
			.SetEventCategory("game_action")
			.SetEventAction("multi_game_started")
			.SetEventLabel("start"));
		
		isMyTurn = false;
		iStarted = false;

		if (GameData.Instance.appModeType == 3) { 
			sendButton.SetActive (false);
			sendButton4.SetActive (false);
			setButton.SetActive (false);
			setButton4.SetActive (false);
		} else {
			sendButton4.SetActive (false);
			sendButton.SetActive (false);
			setButton4.SetActive (false);
			setButton.SetActive (false);
		}

		multiPlaying = true;
		
		reloadTableView ();
		
	}


	public void matchFirstStarted(bool invite){

//		string sNumber = number.ToString().PadLeft(3,'0');
//		
//		Number estimatedNumber = new Number();
//		estimatedNumber.FirstNumber =  int.Parse(sNumber.Substring(0,1)); 
//		estimatedNumber.SecondNumber = int.Parse(sNumber.Substring(1,1)); 
//		estimatedNumber.ThirdNumber = int.Parse(sNumber.Substring(2,1)); 
//		
//		NumberGameAI.Instance.opponentNumber = estimatedNumber;
		
//		if(invite){
//			
//			if (![self checkRemotePlayerIds:[GameKitHelper sharedGameKitHelper].remotePlayerID]) {
//				//        bu pyuncuyu kaydet tekrar davet edildiginde isleme alinmasin
//				[[RWGameData sharedGameData].playerIDs addObject:[GameKitHelper sharedGameKitHelper].remotePlayerID];
//				
//				// davet edilen oyuncuya 10 bonus oyun ver
//				[RWGameData sharedGameData].multiRemainingGameCount += 10;
//				//        [GameKitHelper sharedGameKitHelper].invitedGame = NO;
//				[[RWGameData sharedGameData] save];
//				[self restartGame];
//			}
//			
//			
//			[[NSNotificationCenter defaultCenter] postNotificationName: SSGameDataUpdatedFromiCloud object:nil];
//			[self didUpdateGameData:nil];
//		}


//		restartGame();

		string title = "Info";
		string message = "Game started! Your turn to start the game.";
		dialogueManager = DialogueManager.Instance();
		dialogueManager.showDialog(title,message,0);

		googleAnalytics.LogEvent(new EventHitBuilder()
			.SetEventCategory("game_action")
			.SetEventAction("multi_game_started")
			.SetEventLabel("start"));
		
		isMyTurn = true;
		iStarted = true;
		if (GameData.Instance.appModeType == 3) { 
			sendButton.SetActive (false);
			sendButton4.SetActive (false);
			setButton.SetActive (true);
			setButton4.SetActive (false);
		} else {
			sendButton4.SetActive (false);
			sendButton.SetActive (false);
			setButton4.SetActive (true);
			setButton.SetActive (false);
		}
		multiPlaying = true;

		reloadTableView ();

	}


	public void rematchFirstStarted(bool rematch){

		if (GameData.Instance.multiRemainingGameCount <= 0) {
			string title = "Info";
			string message = "You don't have enough credit to play online game. You can get 5 credits by watching rewarded videos. In addition, You can get more credits by purchasing game packages from the shopping list";
			dialogueManager = DialogueManager.Instance();
			dialogueManager.showDialog(title,message,0);

			return;
		} else {
			string title = "Info";
			string message = "Your opponent wants to rematch. Whould you like to accept it?";
			dialogueManager = DialogueManager.Instance();
			dialogueManager.showConfirmDialog(title,message,33,true);
		}



		
//		isMyTurn = true;
//		iStarted = true;
//		sendButton.SetActive(false);
//		setButton.SetActive(true);
//		multiPlaying = true;
//		
//		reloadTableView ();
		
	}


//	private void startTimer()
//	{
//		// Create a timer with a two second interval.
//		timer = new System.Timers.Timer(1000);
//		// Hook up the Elapsed event for the timer. 
//		timer.Elapsed += timerFired;;
//		timer.AutoReset = true;
//		timer.Enabled = true;
//	}

//	void timerFired (object sender, ElapsedEventArgs e)
//	{
//		if((currMinute>0 || currSeconds>=0) && currMinute>=0)
//		{
//			if(currSeconds==0)
//			{
//				currMinute-=1;
//				currSeconds=59;
//			}
//			else if(currSeconds>0)
//			{
//				currSeconds-=1;
//			}
//			
//			if(currMinute==0 && currSeconds < 30){
//				elapsedTimeLabel.color = Color.red;
//			}
//			
//			if(currMinute>-1)
//				elapsedTimeLabel.text = string.Format("{0}{1}{2}{3}","",currMinute,":",currSeconds);
//		}
//		else
//		{
//			timer.Stop ();
////			MultiplayerController.Instance.SendGameOver(NumberGameAI.Instance.opponentNumber.FirstNumber, NumberGameAI.Instance.opponentNumber.PozitiveResult, NumberGameAI.Instance.opponentNumber.NegativeResult,true); 
//
//			// maci lokal oyuncu zamandan kaybeder
//			calculateScoreTablesForMulti(false);
//
////			string message = "Unfortunatelly you loose! Would you like to play again? \n";
////			
////			dialogueManager = DialogueManager.Instance();
////			dialogueManager.showConfirmDialog(message,10);
//
//		}
//	}
	
//	private void stopTimer()
//	{
//		timer.Stop ();
//		timer.Dispose ();
//	}
	

	/// <summary>
	/// singleplayerdaki oyuncunun tahmin sonucunun gosterildigi fonksiyon
	/// bir sonraki hamle bilgisayarda olacagi icin send butonu gizleniyor
	/// </summary>
	/// <param name="result">Result.</param>
	private void showMyPlayerResultSingle(Number result)  {

		string totalResult = string.Empty;
		if (GameData.Instance.appModeType == 3) { 
			totalResult = string.Format ("{0}{1}{2} ({3}) ({4})", result.FirstNumber, result.SecondNumber, result.ThirdNumber, result.PozitiveResult, result.NegativeResult);

			if (result.PozitiveResult == 0 && result.NegativeResult == 1) {
				totalResult = string.Format ("{0} \n One digit exists but its place is wrong.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n Three digits don't exist in the code.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 2) {
				totalResult = string.Format ("{0} \n Two digits exist but their place are wrong.", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 1) {
				totalResult = string.Format ("{0} \n Two digits exist one is at correct place but the other is at wrong place.", totalResult);
			} else if (result.PozitiveResult == 2 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n Two digits exist and both of them at correct place.", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n One digit exists and its place is correct.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 3) {
				totalResult = string.Format ("{0} \n Three digits exist but all of them at wrong place.", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 2) {
				totalResult = string.Format ("{0} \n Three digits exist but one of them is at correct place the other two are at wrong place.", totalResult);
			} else if (result.PozitiveResult == 3 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n You found the opponents secret code. CONGRATULATIONS!", totalResult);
			}
		} else {
			totalResult = string.Format ("{0}{1}{2}{3} ({4}) ({5})", result.FirstNumber, result.SecondNumber, result.ThirdNumber,result.FourthNumber, result.PozitiveResult, result.NegativeResult);
			if (result.PozitiveResult == 0 && result.NegativeResult == 1) {
				totalResult = string.Format ("{0} \n One digit exists but its place is wrong.", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n One digit exists and its place is correct.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 2) {
				totalResult = string.Format ("{0} \n Two digits exist but their place are wrong.", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 1) {
				totalResult = string.Format ("{0} \n Two digits exist one is at correct place the other is at wrong place.", totalResult);
			} else if (result.PozitiveResult == 2 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n Two digits exist and both of them at correct place.", totalResult);
			} else if (result.PozitiveResult == 3 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n Three digits exist and all of them are at correct place.", totalResult);
			} else if (result.PozitiveResult == 2 && result.NegativeResult == 1) {
				totalResult = string.Format ("{0} \n Three digits exist two of them are at correct place and the other is at wrong place.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 3) {
				totalResult = string.Format ("{0} \n Three digits exist all of them are at wrong place.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 4) {
				totalResult = string.Format ("{0} \n Four digits exist but all of them are at wrong place.", totalResult);
			}else if (result.PozitiveResult == 1 && result.NegativeResult == 3) {
				totalResult = string.Format ("{0} \n Four digits exist but one is at correct place the other three are at wrong place.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n Four digits don't exist in the code.", totalResult);
			} else if (result.PozitiveResult == 4 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n You found your opponent's secret code. CONGRATULATIONS!", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 2) {
				totalResult = string.Format ("{0} \n Three digits exist but one is at correct place and the other two are at wrong places.", totalResult);
			}else if (result.PozitiveResult == 2 && result.NegativeResult == 2) {
				totalResult = string.Format ("{0} \n Four digits exist but two of them are at correct places the other two are at wrong places.", totalResult);
			}  

		}  


		Debug.Log(string.Format("Result:{0}",totalResult));

		dialogueManager = DialogueManager.Instance();

		switch (gameMode) {
		case GamePlay.GameMode.HowToPlay:
		case GamePlay.GameMode.Training:{

			dialogueManager.showDialog ("Information",totalResult, 501);
				if (GameData.Instance.appModeType == 3) { 
					sendButton.SetActive (true);
					sendButton4.SetActive (false);
				} else {
					sendButton4.SetActive (true);
					sendButton.SetActive (false);
				}
		}
			break;
		case GamePlay.GameMode.SinglePlayer:{
			string title = "My guess result";
			dialogueManager.showDialog (title,totalResult, 500);
				if (GameData.Instance.appModeType == 3) { 
					sendButton.SetActive (false);
				} else {
					sendButton4.SetActive (false);
				}
		}
			break;
		default:
			break;
		}

	}

	/// <summary>
	/// Singleplayerdaki oyuncunun tahmininin islendigi fonksiyon
	/// </summary>
	/// <param name="tahmin">Tahmin.</param>
	public void myMoveNumberSingle(Number tahmin) {

		Number result = null;

		if (GameData.Instance.appModeType == 3) { 
			result = NumberGameAI.Instance.analyzeMyNumber3 (NumberGameAI.Instance.computerNumber, tahmin);
		} else {
			result = NumberGameAI.Instance.analyzeMyNumber4 (NumberGameAI.Instance.computerNumber, tahmin);
		}

		// gonderdigim sayi tabloya ekleniyoer
		if (resultNumbers.Count > 0) {
			ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
			if(tmpResultNumber.myNumber == null){
				tmpResultNumber.myNumber = result;
				resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
			}
			else{
				ResultNumber resultNumber = new ResultNumber();
				resultNumber.myNumber = result;
				// add mine estimated number to my numbers array
				resultNumbers.Add(resultNumber);
			}
			
		}else{
			ResultNumber resultNumber = new ResultNumber();
			resultNumber.myNumber = result;
			// add mine estimated number to my numbers array
			resultNumbers.Add(resultNumber);
		}
		
		reloadTableView ();


		// antreman modunda oyunu kaydediliyor oyun birakilsada sonradan kaldigi yerden devam edebiliyor.
//		if (gameMode == GamePlay.GameMode.HowToPlay) {

//			NSMutableArray *archiveArray = [NSMutableArray arrayWithCapacity:resultNumbers.count];
//			for (ResultNumber *numberObject in resultNumbers) {
//				NSData *personEncodedObject = [NSKeyedArchiver archivedDataWithRootObject:numberObject];
//				[archiveArray addObject:personEncodedObject];
//			}
//			
//			NSData *computerNumberData = [NSKeyedArchiver archivedDataWithRootObject:computerNumber];
//			
//			NSArray *array = [archiveArray copy];
//			[[NSUserDefaults standardUserDefaults] setObject:array forKey:@"trainingNumbers"];
//			[[NSUserDefaults standardUserDefaults] setObject:computerNumberData forKey:@"computerNumber"];
//			[[NSUserDefaults standardUserDefaults] synchronize];
//		}

		int pozitifResult;
		if (GameData.Instance.appModeType == 3) {
			pozitifResult = 3;
		} else {
			pozitifResult = 4;
		}
			
		if (result.PozitiveResult == pozitifResult) {
			string messsage = null;	
			isFindNumber = true;
				
			switch (gameMode) {
			case GamePlay.GameMode.HowToPlay:
			{
				messsage = "You found the secret code. CONGRATULATIONS! \n Would you like to play again?";

				// show messages
				string title = "Game Over";
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showConfirmDialog(title,messsage,0,false);

				calculateScoreTablesForSingle(true);
				setOppenentsNumber(result);

			}
				break;
			case GamePlay.GameMode.Training:{

				// antranman tamamlandi kayit resetleniyor
//				[[NSUserDefaults standardUserDefaults] setObject:nil forKey:@"trainingNumbers"];
//				[[NSUserDefaults standardUserDefaults] setObject:nil forKey:@"computerNumber"];
//				[[NSUserDefaults standardUserDefaults] synchronize];
				string title = "Game Over";
				messsage = "You found the secret code. CONGRATULATIONS! \n Would you like to play again?";
				
				// show messages
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showConfirmDialog(title,messsage,0,false);
				
				calculateScoreTablesForSingle(true);
				setOppenentsNumber(result);

			}
				break;
			case GamePlay.GameMode.SinglePlayer:{

				if (iStarted && !isCompFindNumber) {
					// bu durumda local oyuncu numarayi buldu. 
					messsage = "You found the secret code! \n Last chance for computer to draw. \n You will win if it can't find your code";
					
					// show messages
					dialogueManager = DialogueManager.Instance();
					dialogueManager.showDialog("",messsage,500);
					
					
				}else if (!iStarted && isCompFindNumber){
					// ben baslamadim bilgisayar numarayi buldu ve tahminimde numarami buldum. beraberlik
					
						if (GameData.Instance.appModeType == 3) {
							messsage = string.Format ("Your number: {0}{1}{2} \n Draw! Would you like to play again? \n", result.FirstNumber, result.SecondNumber, result.ThirdNumber);
						} else {
							messsage = string.Format ("Your number: {0}{1}{2}{3} \n Draw! Would you like to play again? \n", result.FirstNumber, result.SecondNumber, result.ThirdNumber,result.FourthNumber);
						}
					
					string title = "Game Over";
					// show messages
					dialogueManager = DialogueManager.Instance();
					dialogueManager.showConfirmDialog(title,messsage,0,false);
					
					
				}else if (!iStarted && !isCompFindNumber){
					string title = "Game Over";
					//ben baslamadim bilgisayarda numarayi bulamadi bu durumda kazandim
					messsage = "You won! Would you like to play again? \n";
					// show messages
					dialogueManager = DialogueManager.Instance();
					dialogueManager.showConfirmDialog(title,messsage,0,false);
				}
				
				setOppenentsNumberComp(NumberGameAI.Instance.computerNumber);

			}
				break;
			default:
			break;
			}

				
		}else
		{
			
			if (isCompFindNumber && !iStarted) {
				// bilgisayar numarami buldu ve oyuna ben baslamadim o zaman numarayi bulamamissam oyun kaybettim bilgisayar kazandi
				// computer player kazandi local oyuncu kaybetti
				calculateScoreTablesForSingle(false);

				string title = "Game Over";
				string aiPlayer = "Master";
				switch (GameData.Instance.playerId) {
				case 1:
					aiPlayer = "Master";
					break;
				case 2:
					aiPlayer = "Lucy";
					break;
				case 3:
					aiPlayer = "Maze";
					break;
				case 4:
					aiPlayer = "Aveline";
					break;
				case 5:
					aiPlayer = "Vincent";
					break;
				case 6:
					aiPlayer = "Elina";
					break;
				case 7:
					aiPlayer = "Rhonin";
					break;
				case 8:
					aiPlayer = "Barret";
					break;
					
				default:
					aiPlayer = "Master";
					break;
				}

				string messsage = string.Format("{0} won \n Would you like to play again?",aiPlayer);
//				NSString *message = [NSString stringWithFormat:JALocalizedString(@"KEY22", @""), [RWGameData sharedGameData].playerName];
				
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showConfirmDialog(title,messsage,0,false);
				
				
			}else{
				
				if(result.NegativeResult == -3)
				{
//					[self setUnavailableNumber:result];
				}

				if (GameData.Instance.appModeType == 3) {
					// kullanici muhtemel sayi havuzunu olusutur.
					NumberGameAI.Instance.createMyPossibleNumbersPool3(result);
				} else {
					// kullanici muhtemel sayi havuzunu olusutur.
					NumberGameAI.Instance.createMyPossibleNumbersPool4(result);
				} 

				this.showMyPlayerResultSingle(result);
				
			}
			
		}
	}


	private void showComputerPlayerResultSingle(Number result)  {

		string totalResult = string.Empty;

		if (GameData.Instance.appModeType == 3) { 
			totalResult = string.Format ("{0}{1}{2} ({3}) ({4})", result.FirstNumber, result.SecondNumber, result.ThirdNumber, result.PozitiveResult, result.NegativeResult);

			if (result.PozitiveResult == 0 && result.NegativeResult == 1) {
				totalResult = string.Format ("{0} \n One digit exists but its place is wrong.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n Three digits don't exist in the code.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 2) {
				totalResult = string.Format ("{0} \n Two digits exist but their place are wrong.", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 1) {
				totalResult = string.Format ("{0} \n Two digits exist one is at correct place but the other is at wrong place.", totalResult);
			} else if (result.PozitiveResult == 2 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n Two digits exist and both of them at correct place.", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n One digit exists and its place is correct.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 3) {
				totalResult = string.Format ("{0} \n Three digits exist but all of them at wrong place.", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 2) {
				totalResult = string.Format ("{0} \n Three digits exist but one of them is at correct place the other two are at wrong place.", totalResult);
			} else if (result.PozitiveResult == 3 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n You found the opponents secret code. CONGRATULATIONS!", totalResult);
			}
		}else {
			totalResult = string.Format ("{0}{1}{2}{3} ({4}) ({5})", result.FirstNumber, result.SecondNumber, result.ThirdNumber,result.FourthNumber, result.PozitiveResult, result.NegativeResult);
			if (result.PozitiveResult == 0 && result.NegativeResult == 1) {
				totalResult = string.Format ("{0} \n One digit exists but its place is wrong.", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n One digit exists and its place is correct.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 2) {
				totalResult = string.Format ("{0} \n Two digits exist but their place are wrong.", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 1) {
				totalResult = string.Format ("{0} \n Two digits exist one is at correct place the other is at wrong place.", totalResult);
			} else if (result.PozitiveResult == 2 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n Two digits exist and both of them at correct place.", totalResult);
			} else if (result.PozitiveResult == 3 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n Three digits exist and all of them are at correct place.", totalResult);
			} else if (result.PozitiveResult == 2 && result.NegativeResult == 1) {
				totalResult = string.Format ("{0} \n Three digits exist two of them are at correct place and the other is at wrong place.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 3) {
				totalResult = string.Format ("{0} \n Three digits exist all of them are at wrong place.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 4) {
				totalResult = string.Format ("{0} \n Four digits exist but all of them are at wrong place.", totalResult);
			}else if (result.PozitiveResult == 1 && result.NegativeResult == 3) {
				totalResult = string.Format ("{0} \n Four digits exist but one is at correct place the other three are at wrong place.", totalResult);
			} else if (result.PozitiveResult == 0 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n Four digits don't exist in the code.", totalResult);
			} else if (result.PozitiveResult == 4 && result.NegativeResult == 0) {
				totalResult = string.Format ("{0} \n You found your opponent's secret code. CONGRATULATIONS!", totalResult);
			} else if (result.PozitiveResult == 1 && result.NegativeResult == 2) {
				totalResult = string.Format ("{0} \n Three digits exist but one is at correct place and the other two are at wrong places.", totalResult);
			}else if (result.PozitiveResult == 2 && result.NegativeResult == 2) {
				totalResult = string.Format ("{0} \n Four digits exist but two of them are at correct places the other two are at wrong places.", totalResult);
			}  

		}

		Debug.Log(string.Format("Result:{0}",totalResult));
		string title = "Master guess";
		switch (GameData.Instance.playerId) {
		case 1:
			title = "Master guess";
			break;
		case 2:
			title = "Lucy guess";
			break;
		case 3:
			title = "Maze guess";
			break;
		case 4:
			title = "Aveline guess";
			break;
		case 5:
			title = "Vincent guess";
			break;
		case 6:
			title = "Elina guess";
			break;
		case 7:
			title = "Rhonin guess";
			break;
		case 8:
			title = "Barret guess";
			break;
			
		default:
			title = "Master guess";
			break;
		}

		dialogueManager = DialogueManager.Instance();
		dialogueManager.showDialog(title,totalResult,600);

		if (GameData.Instance.appModeType == 3) { 
			sendButton.SetActive (true);
			sendButton4.SetActive (false);
		} else {
			sendButton4.SetActive (true);
			sendButton.SetActive (false);
		}
		
	}

	private void computerMoveNumberSingle(Number tahmin) {
		
		tahmin.PozitiveResult = 0;
		tahmin.NegativeResult = 0;


		Number result = null;
		if (GameData.Instance.appModeType == 3) { 	
			result = NumberGameAI.Instance.analyzeMyNumber3 (NumberGameAI.Instance.myNumber, tahmin);
		} else {
			result = NumberGameAI.Instance.analyzeMyNumber4 (NumberGameAI.Instance.myNumber, tahmin);
		}

		this.computerMovesHistory.Add(result);


		int pozitifResult;
		if (GameData.Instance.appModeType == 3) {
			pozitifResult = 3;
		} else {
			pozitifResult = 4;
		}
		if (result.PozitiveResult == pozitifResult) {
			
			isCompFindNumber = true;

			if (resultNumbers.Count > 0) {
				ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
				if(tmpResultNumber.estimatedNumber == null){
					tmpResultNumber.estimatedNumber = result;
					resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
				}
				else{
					ResultNumber resultNumber = new ResultNumber();
					resultNumber.estimatedNumber = result;
					// add mine estimated number to my numbers array
					resultNumbers.Add(resultNumber);
				}
				
			}else{
				ResultNumber resultNumber = new ResultNumber();
				resultNumber.estimatedNumber = result;
				// add mine estimated number to my numbers array
				resultNumbers.Add(resultNumber);
			}
			
			reloadTableView ();

			
			string messsage = null;
			string title = null;
			
			if (iStarted && isFindNumber) {
				
				// beraberlikte de score gonderiliyor numara bulundu.
				calculateRankingPoints(kDrawPoints ,kGameDuration);
				
				setOppenentsNumberComp(NumberGameAI.Instance.computerNumber);
				
				// calculate points and avarage steps
				calculateScoreTablesForSingle(true);


				// bu durumda local oyuncu numarayi buldu. 
				if (GameData.Instance.appModeType == 3) {
					messsage = string.Format ("Your number: {0}{1}{2} \n Draw! Would you like to play again? \n", result.FirstNumber, result.SecondNumber, result.ThirdNumber);
				} else {
					messsage = string.Format ("Your number: {0}{1}{2}{3} \n Draw! Would you like to play again? \n", result.FirstNumber, result.SecondNumber, result.ThirdNumber,result.FourthNumber);
				}
				// show messages
				title = "Game Over";
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showConfirmDialog(title,messsage,0,false);
				
			}else if(iStarted && !isFindNumber){
				
				setOppenentsNumberComp(NumberGameAI.Instance.computerNumber);
				
				// computer player kazandi local oyuncu kaybetti
				calculateScoreTablesForSingle(false);

				title = "Game Over";
				if (GameData.Instance.appModeType == 3) {
					messsage = string.Format ("Your number: {0}{1}{2} \n Would you like to play again? \n", result.FirstNumber, result.SecondNumber, result.ThirdNumber);
				} else {
					messsage = string.Format ("Your number: {0}{1}{2}{3} \n Would you like to play again? \n", result.FirstNumber, result.SecondNumber, result.ThirdNumber,result.FourthNumber);
				}

				dialogueManager = DialogueManager.Instance();
				dialogueManager.showConfirmDialog(title,messsage,0,false);
				
			}else if(!iStarted && !isFindNumber){
				
				// computer player numarayi buldu local oyuncu bir hamle daha yapacak
				string aiPlayer = "Master";
				switch (GameData.Instance.playerId) {
				case 1:
					aiPlayer = "Master";
					break;
				case 2:
					aiPlayer = "Lucy";
					break;
				case 3:
					aiPlayer = "Maze";
					break;
				case 4:
					aiPlayer = "Aveline";
					break;
				case 5:
					aiPlayer = "Vincent";
					break;
				case 6:
					aiPlayer = "Elina";
					break;
				case 7:
					aiPlayer = "Rhonin";
					break;
				case 8:
					aiPlayer = "Barret";
					break;
					
				default:
					aiPlayer = "Master";
					break;
				}
				messsage = string.Format("{0} found your secret code! \n Last chance to draw. \n {1} will win if you can't find its number",aiPlayer,aiPlayer);

				dialogueManager = DialogueManager.Instance();
				dialogueManager.showDialog("",messsage,100);
				
			}
			
			
			
		}else
		{
			
			// Bilgisayar rakibinden sonucu aldi ve buna gore muhtemel sayi havuzunu olusuturuyor.

			if (GameData.Instance.appModeType == 3) { 
				if (GameData.Instance.playerId == 1) {
					NumberGameAI.Instance.createPossibleNumbersPool3 (result);
				} else if (GameData.Instance.playerId == 2) {
					NumberGameAI.Instance.createPossibleNumbersPool3 (result);
				} else if (GameData.Instance.playerId == 3 && resultNumbers.Count > 0) {
					NumberGameAI.Instance.createPossibleNumbersPool3 (result);
				} else if (GameData.Instance.playerId == 4 && resultNumbers.Count > 1) {
					NumberGameAI.Instance.createPossibleNumbersPool3 (result);
				} else if (GameData.Instance.playerId == 5 && resultNumbers.Count > 3) {
					NumberGameAI.Instance.createPossibleNumbersPool3 (result);
				} else if (GameData.Instance.playerId == 6 && resultNumbers.Count > 5) {
					NumberGameAI.Instance.createPossibleNumbersPool3 (result);
				} else if (GameData.Instance.playerId == 7 && resultNumbers.Count > 7) {
					NumberGameAI.Instance.createPossibleNumbersPool3 (result);
				} else if (GameData.Instance.playerId == 8 && resultNumbers.Count > 9) {
					NumberGameAI.Instance.createPossibleNumbersPool3 (result);
				} else {
					NumberGameAI.Instance.createPossibleNumbersPool3 (result);
				}
			} else {
				if (GameData.Instance.playerId == 1) {
					NumberGameAI.Instance.createPossibleNumbersPool4 (result);
				} else if (GameData.Instance.playerId == 2) {
					NumberGameAI.Instance.createPossibleNumbersPool4 (result);
				} else if (GameData.Instance.playerId == 3 && resultNumbers.Count > 0) {
					NumberGameAI.Instance.createPossibleNumbersPool4 (result);
				} else if (GameData.Instance.playerId == 4 && resultNumbers.Count > 1) {
					NumberGameAI.Instance.createPossibleNumbersPool4 (result);
				} else if (GameData.Instance.playerId == 5 && resultNumbers.Count > 3) {
					NumberGameAI.Instance.createPossibleNumbersPool4 (result);
				} else if (GameData.Instance.playerId == 6 && resultNumbers.Count > 5) {
					NumberGameAI.Instance.createPossibleNumbersPool4 (result);
				} else if (GameData.Instance.playerId == 7 && resultNumbers.Count > 7) {
					NumberGameAI.Instance.createPossibleNumbersPool4 (result);
				} else if (GameData.Instance.playerId == 8 && resultNumbers.Count > 9) {
					NumberGameAI.Instance.createPossibleNumbersPool4 (result);
				} else {
					NumberGameAI.Instance.createPossibleNumbersPool4 (result);
				}
			}

			if (resultNumbers.Count > 0) {
				ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
				if(tmpResultNumber.estimatedNumber == null){
					tmpResultNumber.estimatedNumber = result;
					resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
				}
				else{
					ResultNumber resultNumber = new ResultNumber();
					resultNumber.estimatedNumber = result;
					// add mine estimated number to my numbers array
					resultNumbers.Add(resultNumber);
				}
				
			}else{
				ResultNumber resultNumber = new ResultNumber();
				resultNumber.estimatedNumber = result;
				// add mine estimated number to my numbers array
				resultNumbers.Add(resultNumber);
			}

			reloadTableView ();

			if (isFindNumber) {
				
				calculateRankingPoints(kWinPoints ,kGameDuration);
				
				// calculate points and avarage steps
				calculateScoreTablesForSingle(true);
				
				string message = "You won! Would you like to play again? \n";
				string title = "Game Over";
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showConfirmDialog(title,message,0,false);
				
			}else{


				//Run some code on the main thread
				showComputerPlayerResultSingle(result);	
			}
				
				
		}
	}

	public void computerMoveAfterDelay(){

		Number computerEstimate = null;

		if (GameData.Instance.appModeType == 3) { 
			computerEstimate = NumberGameAI.Instance.createNumberByVeryHardAI3 (computerMovesHistory);
		} else {
			computerEstimate = NumberGameAI.Instance.createNumberByVeryHardAI4 (computerMovesHistory);
		}

		StartCoroutine(ExecuteAfterTime(2,computerEstimate));

//		Thread thread = new Thread(() => computerMove(computerEstimate));
//		thread.Start();
	}


	IEnumerator ExecuteAfterTime(float time,Number computerEstimate)
	{
		yield return new WaitForSeconds(time);

//		Loom.RunAsync(()=>{
			//Loop through the vertices
			
			computerMove(computerEstimate);
			
//		});


	}

	private void computerMove(Number computerEstimate){

//		Loom.QueueOnMainThread (() => {
			computerMoveNumberSingle (computerEstimate);
			
//		});

	}
	
	// ====================== MULTIPLAYER ===================== 

	/// <summary>
	/// Multiplayer oyunda rakip tahminini yaptiginda tetiklenen fonksiyon
	/// rakibin tahminin analiz ediliyor eger +6 sayimi bulmussa oyun sonu gonderiliyor.
	/// 
	/// Eger ilk tahmin sirasi bende ise ve bir onceki hamlede sayiyi bulmus isem
	/// rakib bu tahmininde sayimi bulursa beraberlik oyun sonu gonderilecek eger sayimi bulmaz ise oyun sonu gonderilecek
	/// 
	/// Eger ilk tahmin sirasi bende degil ve rakip sayimi bulmus ise oyun sonu gonderilecek
	/// </summary>
	/// <param name="number">number rakibin gonderdigi tahmin sayisi</param>
	public void movePlayerNumber(int number) {

		Number result = null;
		int pozitifResult;
		if (GameData.Instance.appModeType == 3) { 
			string sNumber = number.ToString ().PadLeft (3, '0');
		
			Number estimatedNumber = new Number ();
			estimatedNumber.FirstNumber = int.Parse (sNumber.Substring (0, 1)); 
			estimatedNumber.SecondNumber = int.Parse (sNumber.Substring (1, 1)); 
			estimatedNumber.ThirdNumber = int.Parse (sNumber.Substring (2, 1)); 
		
			result = NumberGameAI.Instance.analyzeMyNumber3 (NumberGameAI.Instance.myNumber, estimatedNumber);
			pozitifResult = 3;
		} else {
			string sNumber = number.ToString ().PadLeft (4, '0');

			Number estimatedNumber = new Number ();
			estimatedNumber.FirstNumber = int.Parse (sNumber.Substring (0, 1)); 
			estimatedNumber.SecondNumber = int.Parse (sNumber.Substring (1, 1)); 
			estimatedNumber.ThirdNumber = int.Parse (sNumber.Substring (2, 1)); 
			estimatedNumber.FourthNumber = int.Parse (sNumber.Substring (3, 1)); 

			result = NumberGameAI.Instance.analyzeMyNumber4 (NumberGameAI.Instance.myNumber, estimatedNumber);
			pozitifResult = 4;
		}

		computerMovesHistory.Add (result);
		if (GameData.Instance.appModeType == 3) { 
			sendButton.SetActive (true);
			sendButton4.SetActive (false);
		} else {
			sendButton4.SetActive (true);
			sendButton.SetActive (false);
		}

		if (result.PozitiveResult == pozitifResult) {
			
			
			if (!iStarted) {
				// maca ben baslamadim
				// rakibim sayimi buldu
				// tahmin icin beraberlik icin bir sansim daha var
				isFoundedInMultiplayer = true;

				string message = "Your opponent found your secret code! \n Last chance to draw. \n Your opponent will win if you can't find her/his code";

				dialogueManager = DialogueManager.Instance();
				dialogueManager.showDialog("",message,100);
				
				// sonucu yinede rakibe gonder son hamlesi icin
				MultiplayerController.Instance.SendEstimatingResult(number, result.PozitiveResult, result.NegativeResult); 

				
				if (resultNumbers.Count > 0) {
					ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
					if(tmpResultNumber.estimatedNumber == null){
						tmpResultNumber.estimatedNumber = result;
						resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
					}
					else{
						ResultNumber resultNumber = new ResultNumber();
						resultNumber.estimatedNumber = result;
						// add mine estimated number to my numbers array
						resultNumbers.Add(resultNumber);
					}
					
				}else{
					ResultNumber resultNumber = new ResultNumber();
					resultNumber.estimatedNumber = result;
					// add mine estimated number to my numbers array
					resultNumbers.Add(resultNumber);
				}

				reloadTableView ();
				
			}else{
				// maca ben basladim
				// rakibim sayimi buldu
				// oyun sonu gonder
				
				// eger sayi daha once bulmus ve rakibim sayimi bulmus ise beraberlik olacak
				if(isFoundedInMultiplayer){
					Debug.Log(@"Beraberlik gonderilecek");
					
					calculateRankingPoints(kDrawPoints ,kGameDuration);
					
					// calculate points and avarage steps
					calculateScoreTablesForMulti(true);
					
					
					MultiplayerController.Instance.SendGameDraws(number, result.PozitiveResult, result.NegativeResult,true); 


					if(iStarted){
						string title = "Game Over";
						string message = "Draw! \n Would you like to play again? \n";
						dialogueManager = DialogueManager.Instance();
						dialogueManager.showConfirmDialog(title,message,10,true);
					}
//					else{
//						string message = "Draw! \n Your opponent may invite you to rematch soon.. Please wait.. \n";
//						dialogueManager = DialogueManager.Instance();
//						dialogueManager.showDialog("",message,10);
//					}
					

					if (resultNumbers.Count > 0) {
						ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
						if(tmpResultNumber.estimatedNumber == null){
							tmpResultNumber.estimatedNumber = result;
							resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
						}
						else{
							ResultNumber resultNumber = new ResultNumber();
							resultNumber.estimatedNumber = result;
							// add mine estimated number to my numbers array
							resultNumbers.Add(resultNumber);
						}
						
					}else{
						ResultNumber resultNumber = new ResultNumber();
						resultNumber.estimatedNumber = result;
						// add mine estimated number to my numbers array
						resultNumbers.Add(resultNumber);
					}

					reloadTableView ();

				}else{
					// maci local oyuncu kaybeder. remote oyuncu kazandi
					MultiplayerController.Instance.SendGameOver(number, result.PozitiveResult, result.NegativeResult,true); 

					calculateScoreTablesForMulti(false);
					
					// oyunu kaybettiginde de karsi tarafta numarasi yazilacak
					setOppenentsNumber(NumberGameAI.Instance.opponentNumber);


					if(iStarted){
						string title = "Game Over";
						string message = "Unfortunatelly you lost! Would you like to play again? \n";
						dialogueManager = DialogueManager.Instance();
						dialogueManager.showConfirmDialog(title,message,10,true);
					}
//					else{
//						string message = "Unfortunatelly you lost! \n Your opponent may invite you to rematch soon.. Please wait.. \n";
//						dialogueManager = DialogueManager.Instance();
//						dialogueManager.showDialog("",message,10);
//					}

					if (resultNumbers.Count > 0) {
						ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
						if(tmpResultNumber.estimatedNumber == null){
							tmpResultNumber.estimatedNumber = result;
							resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
						}
						else{
							ResultNumber resultNumber = new ResultNumber();
							resultNumber.estimatedNumber = result;
							// add mine estimated number to my numbers array
							resultNumbers.Add(resultNumber);
						}
						
					}else{
						ResultNumber resultNumber = new ResultNumber();
						resultNumber.estimatedNumber = result;
						// add mine estimated number to my numbers array
						resultNumbers.Add(resultNumber);
					}

					reloadTableView ();

				}
			}
			
		}else
		{
			if(iStarted && isFoundedInMultiplayer){
				
				calculateRankingPoints(kWinPoints ,kGameDuration);
				
				// calculate points and avarage steps
				calculateScoreTablesForMulti(true);
				
				setOppenentsNumber(result);
				
				// maci local oyuncu kazanir
				MultiplayerController.Instance.SendGameOver(number, result.PozitiveResult, result.NegativeResult,false); 

				if(iStarted){
					string message = string.Format("Opponent's secret code:{0} \n You won! Would you like to play again? \n",number);
					string title = "Game Over";
					dialogueManager = DialogueManager.Instance();
					dialogueManager.showConfirmDialog(title,message,10,true);
				}
//				else{
//					string message = string.Format("Opponent's secret code:{0} \n Your opponent may invite you to rematch soon.. Please wait.. \n",number);
//					dialogueManager = DialogueManager.Instance();
//					dialogueManager.showDialog("",message,10);
//				}
				
				if (resultNumbers.Count > 0) {
					ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
					if(tmpResultNumber.estimatedNumber == null){
						tmpResultNumber.estimatedNumber = result;
						resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
					}
					else{
						ResultNumber resultNumber = new ResultNumber();
						resultNumber.estimatedNumber = result;
						// add mine estimated number to my numbers array
						resultNumbers.Add(resultNumber);
					}
					
				}else{
					ResultNumber resultNumber = new ResultNumber();
					resultNumber.estimatedNumber = result;
					// add mine estimated number to my numbers array
					resultNumbers.Add(resultNumber);
				}
				
				reloadTableView ();

				
			}else{
				

				MultiplayerController.Instance.SendEstimatingResult(number, result.PozitiveResult, result.NegativeResult); 
				
				if (resultNumbers.Count > 0) {
					ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
					if(tmpResultNumber.estimatedNumber == null){
						tmpResultNumber.estimatedNumber = result;
						resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
					}
					else{
						ResultNumber resultNumber = new ResultNumber();
						resultNumber.estimatedNumber = result;
						// add mine estimated number to my numbers array
						resultNumbers.Add(resultNumber);
					}
					
				}else{
					ResultNumber resultNumber = new ResultNumber();
					resultNumber.estimatedNumber = result;
					// add mine estimated number to my numbers array
					resultNumbers.Add(resultNumber);
				}
				
				reloadTableView ();

				string title = "Information";
				string message = "Your turn for guess";
				
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showDialog(title,message,700);
				
				startTimer(remainingTime);
			}
			
		}
		
	}


	/// <summary>
	/// Rakibime gonderdigim tahmin sayisinin sonucu burda isleniyor. Eger +6 sonuc gelirse sayiyi buldum demektir
	/// Egere maca ben baslamissam sayiyi bulma durumda rakibe bir sans daha verilecek
	/// Eger rakibimde benim sayiyi bulursa o zaman oyun beraberlik ile bitecek. Berabaelik oyun sonu gonderilecek
	/// Eger maca ben baslamamissam o zaman oyun sonu gonderilecek cunku macu kazanmis oluyorum
	/// 
	/// </summary>
	/// <param name="number">rakibe gonderile tahmin sayisi.</param>
	/// <param name="positive">pozitif sonucu.</param>
	/// <param name="negative">negatif sonucu.</param>
	public void showPlayerResult(int number, int positive, int negative) {

		string tahmin = string.Empty;
		int pozitifResult;
		if (GameData.Instance.appModeType == 3) { 
			tahmin = number.ToString ().PadLeft (3, '0');
			pozitifResult = 3;
		} else {
			tahmin = number.ToString ().PadLeft (4, '0');
			pozitifResult = 4;
		}

		if (GameData.Instance.appModeType == 3) { 
			sendButton.SetActive (false);
		} else {
			sendButton4.SetActive (false);
		}
		// rakibinden tahmininin sonucu burda isleniyor
		if (positive == pozitifResult) {
			
			if (iStarted) {
				isFoundedInMultiplayer = true;
				
				string message = string.Format("You found the secret code! {0} \n Last chance for opponent to draw. \n You will win if he doesn't find your number",tahmin);
				Number estimatedNumber_ = new Number ();
				if (GameData.Instance.appModeType == 3) { 
					estimatedNumber_.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
					estimatedNumber_.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
					estimatedNumber_.ThirdNumber = int.Parse (tahmin.Substring (2, 1)); 
				} else {
					estimatedNumber_.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
					estimatedNumber_.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
					estimatedNumber_.ThirdNumber = int.Parse (tahmin.Substring (2, 1)); 
					estimatedNumber_.FourthNumber = int.Parse (tahmin.Substring (3, 1)); 
				}

				setOppenentsNumber(estimatedNumber_);

				dialogueManager = DialogueManager.Instance();
				dialogueManager.showDialog("",message,0);

			}else{
				// maca ben baslamamisim ve rakip tahmini tutmussa. beraberlik oyun sonu gonder
				
				calculateRankingPoints(kDrawPoints ,kGameDuration);
				
				// calculate points and avarage steps
				calculateScoreTablesForMulti(true);
				
				Number estimatedNumber_ = new Number();
				if (GameData.Instance.appModeType == 3) {
					estimatedNumber_.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
					estimatedNumber_.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
					estimatedNumber_.ThirdNumber = int.Parse (tahmin.Substring (2, 1)); 
				} else {
					estimatedNumber_.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
					estimatedNumber_.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
					estimatedNumber_.ThirdNumber = int.Parse (tahmin.Substring (2, 1)); 
					estimatedNumber_.FourthNumber = int.Parse (tahmin.Substring (3, 1)); 
				}
				
				setOppenentsNumber(estimatedNumber_);

				MultiplayerController.Instance.SendGameDraws(number, positive, negative,true); 

				if(iStarted){
					string message = "Draw! \n Would you like to play again? \n";
					string title = "Game Over";
					dialogueManager = DialogueManager.Instance();
					dialogueManager.showConfirmDialog(title,message,10,true);
				}
//				else{
//					string message = "Draw! \n Your opponent may invite you to rematch soon.. Please wait.. \n";
//					dialogueManager = DialogueManager.Instance();
//					dialogueManager.showDialog("",message,10);
//				}

			}
		}else{
			
			// ege sonucu bilememisse oyunu oyun sonu katbetti gonderilecek        
			if(!iStarted && isFoundedInMultiplayer){
				
				calculateRankingPoints(kWinPoints ,kGameDuration);
				
				// calculate points and avarage steps
				calculateScoreTablesForMulti(true);
				
				
				Number estimatedNumber_ = new Number();
				if (GameData.Instance.appModeType == 3) {
					estimatedNumber_.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
					estimatedNumber_.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
					estimatedNumber_.ThirdNumber = int.Parse (tahmin.Substring (2, 1)); 
				} else {
					estimatedNumber_.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
					estimatedNumber_.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
					estimatedNumber_.ThirdNumber = int.Parse (tahmin.Substring (2, 1)); 
					estimatedNumber_.FourthNumber = int.Parse (tahmin.Substring (3, 1)); 
				}
				
				setOppenentsNumber(estimatedNumber_);
				
				//maci local oyuncu kazanir
				MultiplayerController.Instance.SendGameOver(number, positive, negative,false); 

				if(iStarted){
					string message = string.Format("Opponent's number:{0} \n You won! Would you like to play again? \n",number);
					string title = "Game Over";
					dialogueManager = DialogueManager.Instance();
					dialogueManager.showConfirmDialog(title,message,10,true);
				}
//				else{
//					string message = string.Format("Opponent's code:%@ \n You won! \n Your opponent may invite you to rematch soon.. Please wait.. \n",number);
//					dialogueManager = DialogueManager.Instance();
//					dialogueManager.showDialog("",message,10);
//				}

			}else{
				
				if (isFoundedInMultiplayer) {
					
					calculateRankingPoints (kDrawPoints, kGameDuration);
					
					// calculate points and avarage steps
					calculateScoreTablesForMulti (true);
					
					Number estimatedNumber_ = new Number ();
					if (GameData.Instance.appModeType == 3) {
						estimatedNumber_.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
						estimatedNumber_.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
						estimatedNumber_.ThirdNumber = int.Parse (tahmin.Substring (2, 1)); 
					} else {
						estimatedNumber_.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
						estimatedNumber_.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
						estimatedNumber_.ThirdNumber = int.Parse (tahmin.Substring (2, 1)); 
						estimatedNumber_.FourthNumber = int.Parse (tahmin.Substring (3, 1));
					}

					MultiplayerController.Instance.SendGameDraws (number, positive, negative, true); 

					setOppenentsNumber (estimatedNumber_);

					if (iStarted) {
						string message = "Draw! \n Would you like to play again? \n";
						string title = "Game Over";
						dialogueManager = DialogueManager.Instance ();
						dialogueManager.showConfirmDialog (title, message, 10, true);
					} 
//					else {
//						string message = "Draw! \n Your opponent may invite you to rematch soon.. Please wait.. \n";
//						dialogueManager = DialogueManager.Instance ();
//						dialogueManager.showDialog ("", message, 10);
//					}

					Debug.Log (@"Beraberlik gonderilecek");
				} else {

					Number result = new Number ();
					if (GameData.Instance.appModeType == 3) {
						result.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
						result.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
						result.ThirdNumber = int.Parse (tahmin.Substring (2, 1)); 
					} else {
						result.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
						result.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
						result.ThirdNumber = int.Parse (tahmin.Substring (2, 1)); 
						result.ThirdNumber = int.Parse (tahmin.Substring (3, 1));
					}

					string totalResult = string.Empty;

				if (GameData.Instance.appModeType == 3) {
					totalResult = string.Format ("{0}{1}{2} ({3}) ({4})", result.FirstNumber, result.SecondNumber, result.ThirdNumber, positive, negative);

					if (result.PozitiveResult == 0 && result.NegativeResult == 1) {
						totalResult = string.Format ("{0} \n One digit exists but its place is wrong.", totalResult);
					} else if (result.PozitiveResult == 0 && result.NegativeResult == 0) {
						totalResult = string.Format ("{0} \n Three digits don't exist in the code.", totalResult);
					} else if (result.PozitiveResult == 0 && result.NegativeResult == 2) {
						totalResult = string.Format ("{0} \n Two digits exist but their place are wrong.", totalResult);
					} else if (result.PozitiveResult == 1 && result.NegativeResult == 1) {
						totalResult = string.Format ("{0} \n Two digits exist one is at correct place but the other is at wrong place.", totalResult);
					} else if (result.PozitiveResult == 2 && result.NegativeResult == 0) {
						totalResult = string.Format ("{0} \n Two digits exist and both of them at correct place.", totalResult);
					} else if (result.PozitiveResult == 1 && result.NegativeResult == 0) {
						totalResult = string.Format ("{0} \n One digit exists and its place is correct.", totalResult);
					} else if (result.PozitiveResult == 0 && result.NegativeResult == 3) {
						totalResult = string.Format ("{0} \n Three digits exist but all of them at wrong place.", totalResult);
					} else if (result.PozitiveResult == 1 && result.NegativeResult == 2) {
						totalResult = string.Format ("{0} \n Three digits exist but one of them is at correct place the other two are at wrong place.", totalResult);
					} else if (result.PozitiveResult == 3 && result.NegativeResult == 0) {
						totalResult = string.Format ("{0} \n You found the opponent secret code. CONGRATULATIONS!", totalResult);
					}
				} else {
					totalResult = string.Format ("{0}{1}{2}{3} ({4}) ({5})", result.FirstNumber, result.SecondNumber, result.ThirdNumber, result.FourthNumber, result.PozitiveResult, result.NegativeResult);
					if (result.PozitiveResult == 0 && result.NegativeResult == 1) {
						totalResult = string.Format ("{0} \n One digit exists but its place is wrong.", totalResult);
					} else if (result.PozitiveResult == 1 && result.NegativeResult == 0) {
						totalResult = string.Format ("{0} \n One digit exists and its place is correct.", totalResult);
					} else if (result.PozitiveResult == 0 && result.NegativeResult == 2) {
						totalResult = string.Format ("{0} \n Two digits exist but their place are wrong.", totalResult);
					} else if (result.PozitiveResult == 1 && result.NegativeResult == 1) {
						totalResult = string.Format ("{0} \n Two digits exist one is at correct place the other is at wrong place.", totalResult);
					} else if (result.PozitiveResult == 2 && result.NegativeResult == 0) {
						totalResult = string.Format ("{0} \n Two digits exist and both of them at correct place.", totalResult);
					} else if (result.PozitiveResult == 3 && result.NegativeResult == 0) {
						totalResult = string.Format ("{0} \n Three digits exist and all of them are at correct place.", totalResult);
					} else if (result.PozitiveResult == 2 && result.NegativeResult == 1) {
						totalResult = string.Format ("{0} \n Three digits exist two of them are at correct place and the other is at wrong place.", totalResult);
					} else if (result.PozitiveResult == 0 && result.NegativeResult == 3) {
						totalResult = string.Format ("{0} \n Three digits exist all of them are at wrong place.", totalResult);
					} else if (result.PozitiveResult == 0 && result.NegativeResult == 4) {
						totalResult = string.Format ("{0} \n Four digits exist but all of them are at wrong place.", totalResult);
					} else if (result.PozitiveResult == 1 && result.NegativeResult == 3) {
						totalResult = string.Format ("{0} \n Four digits exist but one is at correct place the other three are at wrong place.", totalResult);
					} else if (result.PozitiveResult == 0 && result.NegativeResult == 0) {
						totalResult = string.Format ("{0} \n Four digits don't exist in the code.", totalResult);
					} else if (result.PozitiveResult == 4 && result.NegativeResult == 0) {
						totalResult = string.Format ("{0} \n You found your opponent's secret code. CONGRATULATIONS!", totalResult);
					} else if (result.PozitiveResult == 1 && result.NegativeResult == 2) {
						totalResult = string.Format ("{0} \n Three digits exist but one is at correct place and the other two are at wrong places.", totalResult);
					} else if (result.PozitiveResult == 2 && result.NegativeResult == 2) {
						totalResult = string.Format ("{0} \n Four digits exist but two of them are at correct places the other two are at wrong places.", totalResult);
					}  

				}
					
					Debug.Log(string.Format("Result:{0}",totalResult));
					
					dialogueManager = DialogueManager.Instance();
					dialogueManager.showDialog ("My guess result",totalResult, 0);

				}

			}
			
		}
		
		
		Number estimatedNumber = new Number();
		if (GameData.Instance.appModeType == 3) {
			estimatedNumber.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
			estimatedNumber.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
			estimatedNumber.ThirdNumber = int.Parse (tahmin.Substring (2, 1));
			estimatedNumber.PozitiveResult = positive;
			estimatedNumber.NegativeResult = negative;
		} else {
			estimatedNumber.FirstNumber = int.Parse (tahmin.Substring (0, 1)); 
			estimatedNumber.SecondNumber = int.Parse (tahmin.Substring (1, 1)); 
			estimatedNumber.ThirdNumber = int.Parse (tahmin.Substring (2, 1));
			estimatedNumber.FourthNumber = int.Parse (tahmin.Substring (3, 1));
			estimatedNumber.PozitiveResult = positive;
			estimatedNumber.NegativeResult = negative;
		}


		if (resultNumbers.Count > 0) {
			ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
			if(tmpResultNumber.myNumber == null){
				tmpResultNumber.myNumber = estimatedNumber;
				resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
			}
			else{
				ResultNumber resultNumber = new ResultNumber();
				resultNumber.myNumber = estimatedNumber;
				// add mine estimated number to my numbers array
				resultNumbers.Add(resultNumber);
			}
			
		}else{
			ResultNumber resultNumber = new ResultNumber();
			resultNumber.myNumber = estimatedNumber;
			// add mine estimated number to my numbers array
			resultNumbers.Add(resultNumber);
		}
		
		reloadTableView ();

		
	}


	public void matchEnded() {
		
		if (gameMode == GamePlay.GameMode.MultiPlayer && multiPlaying == true) {

			string message = "Player disconnected! Whould you like to continue playing aganist AI";
			string title = "Game Over";
			dialogueManager = DialogueManager.Instance();
			dialogueManager.showConfirmDialog(title,message,77,false);

			elapsedTimeLabel.GetComponent<CanvasGroup> ().alpha = 0;
			isStoped = true;
		}
		
		multiPlaying = false;
		
	}

	public void gameOver(bool player1Won ,int number, int positive, int negative) {
		
		// player1won YES gelmesi remote oyucunun kaybettigi
		// local oyucunun ise kazandigini gosterir
		
		currMinute=(int)kGameDuration;
		currSeconds=0;
		elapsedTimeLabel.text = "3:00";
		
		if (player1Won) {
			calculateRankingPoints(kWinPoints ,kGameDuration);
		}
		
		// local oyuncu kazandi puanlarini ekle
		calculateScoreTablesForMulti(true);
		
		bool didLocalPlayerWin = true;
		if (player1Won) {
			didLocalPlayerWin = false;
		}

		string sNumber = string.Empty;
		Number estimatedNumber_ = new Number();
		if (GameData.Instance.appModeType == 3) { 
			sNumber = number.ToString ().PadLeft (3, '0');
			estimatedNumber_ = new Number();
			estimatedNumber_.FirstNumber =  int.Parse(sNumber.Substring(0,1)); 
			estimatedNumber_.SecondNumber = int.Parse(sNumber.Substring(1,1)); 
			estimatedNumber_.ThirdNumber = int.Parse(sNumber.Substring(2,1)); 
			estimatedNumber_.PozitiveResult = positive;
			estimatedNumber_.NegativeResult = negative;
		} else {
			sNumber = number.ToString ().PadLeft (4, '0');
			estimatedNumber_ = new Number();
			estimatedNumber_.FirstNumber =  int.Parse(sNumber.Substring(0,1)); 
			estimatedNumber_.SecondNumber = int.Parse(sNumber.Substring(1,1)); 
			estimatedNumber_.ThirdNumber = int.Parse(sNumber.Substring(2,1));
			estimatedNumber_.FourthNumber = int.Parse(sNumber.Substring(3,1));
			estimatedNumber_.PozitiveResult = positive;
			estimatedNumber_.NegativeResult = negative;
		}

		if (didLocalPlayerWin) {

			if(iStarted){
				string title = "Game Over";
				string message = "Unfortunatelly you lost! Would you like to play again? \n";
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showConfirmDialog(title,message,10,true);
			}
//			else{
//				string message = "Unfortunatelly you lost! \n Your opponent may invite you to rematch soon.. Please wait.. \n";
//				dialogueManager = DialogueManager.Instance();
//				dialogueManager.showDialog("",message,10);
//			}
		} else {
			if(iStarted){
				string title = "Game Over";
				string message = string.Format("You won! \n Opponent's number:{0} \n Would you like to play again? \n",number);
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showConfirmDialog(title,message,10,true);
			}
//			else{
//				string message = string.Format("You won! \n Opponent's number:{0}  \n Your opponent may invite you to rematch soon.. Please wait.. \n",number);
//				dialogueManager = DialogueManager.Instance();
//				dialogueManager.showDialog("",message,10);
//			}
		}

		setOppenentsNumber(NumberGameAI.Instance.opponentNumber);
		
		if (resultNumbers.Count > 0) {
			ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
			if(tmpResultNumber.myNumber == null){
				tmpResultNumber.myNumber = estimatedNumber_;
				resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
			}
			else{
				ResultNumber resultNumber = new ResultNumber();
				resultNumber.myNumber = estimatedNumber_;
				// add mine estimated number to my numbers array
				resultNumbers.Add(resultNumber);
			}
			
		}else{
			ResultNumber resultNumber = new ResultNumber();
			resultNumber.myNumber = estimatedNumber_;
			// add mine estimated number to my numbers array
			resultNumbers.Add(resultNumber);
		}
		
		reloadTableView ();
	}
	
	public void gameOverWithDraws(int number, int positive, int negative){

		calculateRankingPoints(kDrawPoints ,kGameDuration);
		// local oyuncu kazandi puanlarini ekle
		calculateScoreTablesForMulti(true);

		string sNumber = string.Empty;
		Number estimatedNumber_ = new Number();
		if (GameData.Instance.appModeType == 3) { 
			sNumber = number.ToString ().PadLeft (3, '0');
			estimatedNumber_.FirstNumber =  int.Parse(sNumber.Substring(0,1)); 
			estimatedNumber_.SecondNumber = int.Parse(sNumber.Substring(1,1)); 
			estimatedNumber_.ThirdNumber = int.Parse(sNumber.Substring(2,1)); 
			estimatedNumber_.PozitiveResult = positive;
			estimatedNumber_.NegativeResult = negative;
		} else {
			sNumber = number.ToString ().PadLeft (4, '0');
			estimatedNumber_.FirstNumber =  int.Parse(sNumber.Substring(0,1)); 
			estimatedNumber_.SecondNumber = int.Parse(sNumber.Substring(1,1)); 
			estimatedNumber_.ThirdNumber = int.Parse(sNumber.Substring(2,1)); 
			estimatedNumber_.FourthNumber = int.Parse(sNumber.Substring(3,1)); 
			estimatedNumber_.PozitiveResult = positive;
			estimatedNumber_.NegativeResult = negative;
		}

		setOppenentsNumber (NumberGameAI.Instance.opponentNumber);

		if (iStarted) {
			string message = string.Format ("Opponent's number:{0} \n  Draw! \n Would you like to play again? \n", number);
			string title = "Game Over";
			dialogueManager = DialogueManager.Instance ();
			dialogueManager.showConfirmDialog (title,message, 10, true);
		} 
//		else {
//
//			string message = string.Format("Draw! \n Your opponent may invite you to rematch soon.. Please wait.. \n");
//			dialogueManager = DialogueManager.Instance();
//			dialogueManager.showDialog("",message,10);
//		}

		if (resultNumbers.Count > 0) {
			ResultNumber tmpResultNumber = resultNumbers[resultNumbers.Count-1];
			if(tmpResultNumber.myNumber == null){
				tmpResultNumber.myNumber = estimatedNumber_;
				resultNumbers[resultNumbers.Count-1] = tmpResultNumber;
			}
			else{
				ResultNumber resultNumber = new ResultNumber();
				resultNumber.myNumber = estimatedNumber_;
				// add mine estimated number to my numbers array
				resultNumbers.Add(resultNumber);
			}
			
		}else{
			ResultNumber resultNumber = new ResultNumber();
			resultNumber.myNumber = estimatedNumber_;

			// add mine estimated number to my numbers array
			resultNumbers.Add(resultNumber);
		}
		
		reloadTableView ();

	}

}

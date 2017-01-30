using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;


public class MultiplayerController : RealTimeMultiplayerListener 
{
	enum GameState 
	{
		kGameStateWaitingForMatch = 0,
		kGameStateWaitingForRandomNumber,
		kGameStateWaitingForStart,
		kGameStateActive,
		kGameStateDone
	};

	public bool showingWaitingRoom = false;
	public MPUpdateListener updateListener;
	public MPLobbyListener lobbyListener;

	private bool _receivedAllRandomNumbers;
	private  GameState _gameState;
	private uint _ourRandomNumber;
	private bool _isPlayer1;
	const int QuickGameOpponents = 1;
	const uint minimumOpponents = 1;
	const uint maximumOpponents = 1;
	static uint gameVariation = 0;
	private byte _protocolVersion = 1;

	private int _chooseMessageLength = 6;
	private List<byte> _chooseMessage;

	private int _beginMessageLength = 3;
	private List<byte> _beginMessage;

	private int _moveMessageLength = 14;
	private List<byte> _moveMessage;

	private int _resultMessageLength = 14;
	private List<byte> _resultMoveMessage;

	private int _gameOverMessageLength = 15;
	private List<byte> _gameOverMessage;

	private int _gameDrawMessageLength = 15;
	private List<byte> _gameDrawMessage;
	

	private GameController gameLogic;
	ArrayList _orderOfPlayers;

	private static MultiplayerController _instance = null;

	private MultiplayerController() {

		_chooseMessage = new List<byte>(_chooseMessageLength);
		_beginMessage = new List<byte>(_beginMessageLength);
		_moveMessage = new List<byte>(_moveMessageLength);
		_resultMoveMessage = new List<byte>(_resultMessageLength);
		_gameOverMessage = new List<byte>(_gameOverMessageLength);
		_gameDrawMessage = new List<byte>(_gameDrawMessageLength);
		
		_ourRandomNumber = (uint)Random.Range (0, 2147480000);
		_gameState = GameState.kGameStateWaitingForMatch;
		_orderOfPlayers = new ArrayList ();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		
		dictionary.Add("playerIdKey", Social.localUser.id);
		dictionary.Add("randomNumberKey", _ourRandomNumber.ToString());
		
		_orderOfPlayers.Add(dictionary);
		gameVariation = (uint) GameData.Instance.appModeType;
		_isPlayer1 = false;
		_receivedAllRandomNumbers = false;

//		PlayGamesPlatform.DebugLogEnabled = true;
//		PlayGamesPlatform.Activate ();
	}
	
	public static void CreateQuickGame()
	{
		gameVariation = (uint)GameData.Instance.appModeType;
		_instance = new MultiplayerController();
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame(QuickGameOpponents, QuickGameOpponents,
		                                                    gameVariation, _instance);
	}
	
	public static void CreateWithInvitationScreen()
	{
		gameVariation = (uint)GameData.Instance.appModeType;
		_instance = new MultiplayerController();
		PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(minimumOpponents, maximumOpponents,
		                                                               gameVariation, _instance);
	}
	
	public static void AcceptFromInbox()
	{
		_instance = new MultiplayerController();
		PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(_instance);
	}
	
	public static void AcceptInvitation(string invitationId)
	{
		_instance = new MultiplayerController();
		PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitationId, _instance);
	}


	public string GetMyParticipantId() {
		return PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
	}

	public List<Participant> GetAllPlayers() {
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants ();
	}

	private void StartMatchMaking() {
		_instance = new MultiplayerController();
		gameVariation = (uint)GameData.Instance.appModeType;
//		PlayGamesPlatform.Instance.RealTime.CreateQuickGame (minimumOpponents, maximumOpponents, gameVariation, this);
		PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(minimumOpponents, maximumOpponents, gameVariation, _instance);
	}

	private void StartMatchMaking(Invitation invitation) {
		_instance = new MultiplayerController();
		Debug.Log ("StartMatchMaking invitation "+ invitation);
		//		PlayGamesPlatform.Instance.RealTime.CreateQuickGame (minimumOpponents, maximumOpponents, gameVariation, this);
		PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, _instance);
	}

	public void DeclineInvitation(Invitation invitation){
		PlayGamesPlatform.Instance.RealTime.DeclineInvitation(invitation.InvitationId);
	}

	private void ShowMPStatus(string message) {
		Debug.Log(message);
		if (lobbyListener != null) {
			lobbyListener.SetLobbyStatusMessage(message);
		}
	}

	private bool isLocalPlayerPlayer1()
	{

//		Dictionary<string, string> myDdictionary = (Dictionary<string, string>)_orderOfPlayers[0];
//
//		string localUserId = myDdictionary ["playerIdKey"];
//		if(localUserId.Equals(Social.localUser.id))
//		{
//			Debug.Log ("I'm player 1");
//			return true;
//		}
//
//		return false;


		Dictionary<string, string> myDdictionary = (Dictionary<string, string>)_orderOfPlayers[0];
		int myNumber;
		int.TryParse (myDdictionary["randomNumberKey"],out myNumber);

		Debug.Log ("my number "+ myNumber);

		Dictionary<string, string> oppDdictionary = (Dictionary<string, string>)_orderOfPlayers[1];
		int oppNumber;
		int.TryParse (oppDdictionary["randomNumberKey"],out oppNumber);

		Debug.Log ("opp number "+ oppNumber);

		if (myNumber < oppNumber) {
			Debug.Log("I'm player 1");
			_orderOfPlayers.RemoveAt (1);
			return true;
		}

		_orderOfPlayers.RemoveAt (1);
		return false;
	}

	private void tryStartGame()
	{
		Debug.Log ("local oyuncu " + _isPlayer1 + " ");
		if (_isPlayer1 && _gameState == GameState.kGameStateWaitingForStart) {
			_gameState = GameState.kGameStateActive;
			
//			BOOL invite = [GameKitHelper sharedGameKitHelper].invitedGame;

//			string number = NumberGameAI.Instance.myNumber.ToString()+NumberGameAI.Instance.myNumber.ToString()+NumberGameAI.Instance.myNumber.ToString();
//			int myNumber = int.Parse(number);
//			Loom.RunAsync(()=>{
				//Loop through the vertices
				Debug.Log ("send game begin");
				SendGameBegin(false);
				
//			});


			Thread.Sleep (1000);
//			[self sendGameBeginWithInvite:invite andNumber:[RWGameData sharedGameData].myNumber];
			
			//first player

			Debug.Log ("showStartGameMessage once");
//			Loom.QueueOnMainThread (() => {
				Debug.Log ("showStartGameMessage icinde");
				gameLogic = GameController.Instance(); 
				gameLogic.showStartGameMessage(false);
//			});

//			[GameKitHelper sharedGameKitHelper].invitedGame = NO;
		}
	}

	private void tryRestartGame()
	{
		Debug.Log ("local oyuncu " + _isPlayer1 + " ");
//		if (_isPlayer1) {
			_gameState = GameState.kGameStateActive;
			
			Debug.Log ("send game begin");

		SendGameBegin(false);

		Thread.Sleep (1000);

		gameLogic = GameController.Instance(); 
		gameLogic.showStartGameMessage(false);

//		}
	}

	public void matchStarted() {
		Debug.Log(@"Match has started successfully");
		if (_receivedAllRandomNumbers) {
			_gameState = GameState.kGameStateWaitingForStart;
		} else {
			_gameState = GameState.kGameStateWaitingForRandomNumber;
		}
		SendRandomNumber((int)_ourRandomNumber);
		tryStartGame();
	}

	public void matchRestarted() {
		Debug.Log(@"Match has started successfully");
		if (_receivedAllRandomNumbers) {
			_gameState = GameState.kGameStateWaitingForStart;
		} else {
			_gameState = GameState.kGameStateWaitingForRandomNumber;
		}
		SendRandomNumber((int)_ourRandomNumber);
		tryRestartGame();
	}


	public void SendRandomNumber(int number) {
		_chooseMessage.Clear ();
		_chooseMessage.Add (_protocolVersion);
		_chooseMessage.Add ((byte)'N');
		_chooseMessage.AddRange (System.BitConverter.GetBytes (number));  
		
		byte[] messageToSend = _chooseMessage.ToArray(); 
		Debug.Log ("Sending my SendRandomNumber  " + number + " to all players in the room");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}

	public void SendChooseNumber(int number) {
		_chooseMessage.Clear ();
		_chooseMessage.Add (_protocolVersion);
		_chooseMessage.Add ((byte)'C');
		_chooseMessage.AddRange (System.BitConverter.GetBytes (number));  

		byte[] messageToSend = _chooseMessage.ToArray(); 
		Debug.Log ("Sending my SendChooseNumber  " + messageToSend + " to all players in the room");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}

	public void SendGameBegin(bool invite) {
		_beginMessage.Clear ();
		_beginMessage.Add (_protocolVersion);
		_beginMessage.Add ((byte)'B');
		_beginMessage.AddRange (System.BitConverter.GetBytes (invite));  

		byte[] messageToSend = _beginMessage.ToArray(); 
		Debug.Log ("Sending my SendGameBegin  " + messageToSend + " to all players in the room");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}

	public void SendRematch(bool rematch) {
		_beginMessage.Clear ();
		_beginMessage.Add (_protocolVersion);
		_beginMessage.Add ((byte)'E');
		_beginMessage.AddRange (System.BitConverter.GetBytes (rematch));  
		
		byte[] messageToSend = _beginMessage.ToArray(); 
		Debug.Log ("Sending my Sendrematch  " + messageToSend + " to all players in the room");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}

	public void SendAcceptedRematch(bool rematch) {
		_beginMessage.Clear ();
		_beginMessage.Add (_protocolVersion);
		_beginMessage.Add ((byte)'A');
		_beginMessage.AddRange (System.BitConverter.GetBytes (rematch));  
		
		byte[] messageToSend = _beginMessage.ToArray(); 
		Debug.Log ("Sending my Sendrematch  " + messageToSend + " to all players in the room");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}

	public void SendEstimatingNumber(int number,int positiveResult, int negativeResult) {
		_moveMessage.Clear ();
		_moveMessage.Add (_protocolVersion);
		_moveMessage.Add ((byte)'M');
		_moveMessage.AddRange (System.BitConverter.GetBytes (number));  
		_moveMessage.AddRange (System.BitConverter.GetBytes (positiveResult));  
		_moveMessage.AddRange (System.BitConverter.GetBytes (negativeResult));

		byte[] messageToSend = _moveMessage.ToArray(); 
		Debug.Log ("Sending my SendEstimatingNumber  " + messageToSend + " to all players in the room");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}

	public void SendEstimatingResult(int number,int positiveResult, int negativeResult) {
		_resultMoveMessage.Clear ();
		_resultMoveMessage.Add (_protocolVersion);
		_resultMoveMessage.Add ((byte)'R');
		_resultMoveMessage.AddRange (System.BitConverter.GetBytes (number));  
		_resultMoveMessage.AddRange (System.BitConverter.GetBytes (positiveResult));  
		_resultMoveMessage.AddRange (System.BitConverter.GetBytes (negativeResult));
		
		byte[] messageToSend = _resultMoveMessage.ToArray(); 
		Debug.Log ("Sending my estimating result  " + messageToSend + " to all players in the room");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}

	public void SendGameOver(int number, int positiveResult, int negativeResult, bool player1Won) {
		_gameOverMessage.Clear ();
		_gameOverMessage.Add (_protocolVersion);
		_gameOverMessage.Add ((byte)'O');
		_gameOverMessage.AddRange (System.BitConverter.GetBytes (number));  
		_gameOverMessage.AddRange (System.BitConverter.GetBytes (positiveResult));  
		_gameOverMessage.AddRange (System.BitConverter.GetBytes (negativeResult));
		_gameOverMessage.AddRange (System.BitConverter.GetBytes (player1Won));

		byte[] messageToSend = _gameOverMessage.ToArray(); 
		Debug.Log ("Sending my SendGameOver  " + messageToSend + " to all players in the room");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}

	public void SendGameDraws(int number, int positiveResult, int negativeResult, bool playersDraws) {
		_gameDrawMessage.Clear ();
		_gameDrawMessage.Add (_protocolVersion);
		_gameDrawMessage.Add ((byte)'D');
		_gameDrawMessage.AddRange (System.BitConverter.GetBytes (number));  
		_gameDrawMessage.AddRange (System.BitConverter.GetBytes (positiveResult));  
		_gameDrawMessage.AddRange (System.BitConverter.GetBytes (negativeResult));
		_gameDrawMessage.AddRange (System.BitConverter.GetBytes (playersDraws));

		byte[] messageToSend = _gameDrawMessage.ToArray(); 
		Debug.Log ("Sending my SendGameDrawse  " + messageToSend + " to all players in the room");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}


	public void SignInAndStartMPGame() {
		if (!Social.localUser.authenticated) {
			Social.localUser.Authenticate((bool success) => {
				if (success) {
					Debug.Log ("We're signed in! Welcome " + Social.localUser.userName);
					GameData.Instance.loggedIn = 1;
					GameData.Instance.Save();
					StartMatchMaking();
				} else {
					Debug.Log ("Oh... we're not signed in.");
				}
			});
		} else {
			Debug.Log ("You're already signed in.");
			GameData.Instance.loggedIn = 1;
			GameData.Instance.Save();
			StartMatchMaking();
		}
	}


	public void StartMPGame() {
		Debug.Log ("You're already signed in.");
		StartMatchMaking();
	}

	public void SignInAndStartMPGame(Invitation invitation) {
		Debug.Log ("SignInAndStartMPGame "+ invitation);
		if (!Social.localUser.authenticated) {
			Social.localUser.Authenticate((bool success) => {
				if (success) {
					Debug.Log ("We're signed in! Welcome " + Social.localUser.userName);
					GameData.Instance.loggedIn = 1;
					GameData.Instance.Save();
					StartMatchMaking(invitation);
				} else {
					Debug.Log ("Oh... we're not signed in.");
				}
			});
		} else {
			Debug.Log ("You're already signed in.");
			GameData.Instance.loggedIn = 1;
			GameData.Instance.Save();
			StartMatchMaking(invitation);
		}
	}

	public void OnParticipantLeft (Participant participant)
	{
		throw new System.NotImplementedException ();
	}


	public void SignOut() {
		PlayGamesPlatform.Instance.SignOut ();
	}
	
	public bool IsAuthenticated() {
		return Social.localUser.authenticated;
	}

	public void TrySilentSignIn() {
		if (!Social.localUser.authenticated) {
			Social.localUser.Authenticate((bool success) => {
				if (success) {
					GameData.Instance.loggedIn = 1;
					GameData.Instance.Save();
					Debug.Log ("Silently signed in! Welcome " + Social.localUser.userName);
				} else {
					Debug.Log ("Oh... we're not signed in.");
				}
			});
		} else {
			GameData.Instance.loggedIn = 1;
			GameData.Instance.Save();
			Debug.Log ("You're already signed in.");
		}

	}

	public static MultiplayerController Instance {
		get {
			if (_instance == null) {
				_instance = new MultiplayerController();
			}
			return _instance;
		}
	}

	public void OnRoomSetupProgress (float percent)
	{
//		ShowMPStatus ("Finding players..");
		if (!showingWaitingRoom) {
			showingWaitingRoom = true;
			PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI (); 

		}

	}

	public void OnRoomConnected (bool success)
	{
		if (success) {
//			ShowMPStatus ("We are connected to the room!");
			SceneManager.LoadScene("GameScene");

		} else {
		
//			ShowMPStatus ("No players found!");
//
//			lobbyListener.HideLobby();
//			lobbyListener = null;
		}
	}



	public void OnLeftRoom ()
	{
//		ShowMPStatus("We have left the room.");
//		lobbyListener.HideLobby();
//		lobbyListener = null;
		if (updateListener != null) {
			updateListener.LeftRoomConfirmed();
		}
	}

	public void OnPeersConnected (string[] participantIds)
	{
		foreach (string participantID in participantIds) {
			ShowMPStatus ("Player " + participantID + " has joined.");
		}
	}

	public void OnPeersDisconnected (string[] participantIds)
	{
		foreach (string participantID in participantIds) {
			ShowMPStatus ("Player " + participantID + " has left.");
		}
		lobbyListener.HideLobby();
		lobbyListener = null;
	}

	public void LeaveGame() {
		PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
	}

	private void processReceivedRandomNumber(Dictionary<string, string> randomNumberDetails){

		_orderOfPlayers.Add (randomNumberDetails);

		if (_orderOfPlayers.Count == 2) {
			_receivedAllRandomNumbers = true;
		}
	}

	public void OnRealTimeMessageReceived (bool isReliable, string senderId, byte[] data)
	{
		// We'll be doing more with this later...
		byte messageVersion = (byte)data[0];
		// Let's figure out what type of message this is.
		char messageType = (char)data[1];

		if (messageType == 'N' && data.Length ==_chooseMessageLength) { 
			int number = System.BitConverter.ToInt32(data, 2);
			bool tie = false;
			if (number == _ourRandomNumber) {
				//2
				Debug.Log("Tie");
				tie = true;
				_ourRandomNumber = (uint)Random.Range (0, 2147480000);
				SendRandomNumber((int)_ourRandomNumber);
			} else {
				//3

				Debug.Log ("Random number alindi " + senderId + " choose number " + number);

				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("playerIdKey", senderId);
				dictionary.Add("randomNumberKey", number.ToString());
				
//
//				NSDictionary *dictionary = @{playerIdKey : playerID,
//				randomNumberKey : @(messageRandomNumber->randomNumber)};
//
				_orderOfPlayers.Add (dictionary);

				if (_orderOfPlayers.Count == 2) {
					_receivedAllRandomNumbers = true;

				}
//				processReceivedRandomNumber(dictionary);
			}


			//4
			if (_receivedAllRandomNumbers) {
				_isPlayer1 = isLocalPlayerPlayer1();
			}
			
			if (!tie && _receivedAllRandomNumbers) {

				Debug.Log ("tum numaralar alindi maci baslat " + senderId + " choose number " + number);

				if (_gameState == GameState.kGameStateWaitingForRandomNumber) {
					_gameState = GameState.kGameStateWaitingForStart;
				}
				tryStartGame();
			}
			
		}else if (messageType == 'C' && data.Length ==_chooseMessageLength) { 
			int number = System.BitConverter.ToInt32(data, 2);

			Debug.Log ("Player " + senderId + " choose number " + number);
			gameLogic = GameController.Instance(); 
			gameLogic.chooseNumber(number);

		}else if (messageType == 'B' && data.Length ==_beginMessageLength) { 
			bool invite = System.BitConverter.ToBoolean(data, 2);

			Debug.Log ("Begin Player " + senderId + " sending number ");
			gameLogic = GameController.Instance(); 
			gameLogic.matchFirstStarted(invite);

		}else if (messageType == 'E' && data.Length ==_beginMessageLength) { 
			bool rematch = System.BitConverter.ToBoolean(data, 2);
			
			Debug.Log ("Begin Player " + senderId + " sending number ");
			gameLogic = GameController.Instance(); 
			gameLogic.rematchFirstStarted(rematch);
			
		}else if (messageType == 'A' && data.Length ==_beginMessageLength) { 
			bool rematch = System.BitConverter.ToBoolean(data, 2);
			
//			Debug.Log ("Begin Player " + senderId + " sending number ");
//			gameLogic = GameController.Instance(); 
//			gameLogic.restartMultiGame(rematch);
			
		}else if (messageType == 'M' && data.Length ==_moveMessageLength) { 
			int number = System.BitConverter.ToInt32(data, 2);
			int positiveResult = System.BitConverter.ToInt32(data, 6);
			int negativeResult = System.BitConverter.ToInt32(data, 10);

			Debug.Log ("Move Player " + senderId + " sending number " + number + "pozitive:" + positiveResult + "negative:" + negativeResult);

			gameLogic = GameController.Instance(); 
			gameLogic.movePlayerNumber(number);

		}else if (messageType == 'R' && data.Length ==_resultMessageLength) { 
			int number = System.BitConverter.ToInt32(data, 2);
			int positiveResult = System.BitConverter.ToInt32(data, 6);
			int negativeResult = System.BitConverter.ToInt32(data, 10);
			
			Debug.Log ("Result Player " + senderId + " sending number " + number + "pozitive:" + positiveResult + "negative:" + negativeResult);
			gameLogic = GameController.Instance(); 
			gameLogic.showPlayerResult(number,positiveResult,negativeResult);
			
		}else if (messageType == 'D' && data.Length ==_gameDrawMessageLength) { 
			int number = System.BitConverter.ToInt32(data, 2);
			int positiveResult = System.BitConverter.ToInt32(data, 6);
			int negativeResult = System.BitConverter.ToInt32(data, 10);
			bool draw = System.BitConverter.ToBoolean(data, 14);

			Debug.Log ("Draw Player " + senderId + " sending number " + number + "pozitive:" + positiveResult + "negative:" + negativeResult);
			gameLogic = GameController.Instance(); 
			gameLogic.gameOverWithDraws(number,positiveResult,negativeResult); 


		}else if (messageType == 'O' && data.Length ==_gameOverMessageLength) { 
			int number = System.BitConverter.ToInt32(data, 2);
			int positiveResult = System.BitConverter.ToInt32(data, 6);
			int negativeResult = System.BitConverter.ToInt32(data, 10);
			bool win = System.BitConverter.ToBoolean(data, 14);

			Debug.Log ("Game Over Player " + senderId + " sending number " + number + "pozitive:" + positiveResult + "negative:" + negativeResult);
			gameLogic = GameController.Instance(); 
			gameLogic.gameOver(win,number,positiveResult,negativeResult); 


		}
	}


}

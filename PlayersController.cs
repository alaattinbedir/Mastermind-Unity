using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayersController : MonoBehaviour {

	public Toggle master;
	public Toggle rand;
	public Toggle lui;
	public Toggle alice;
	public Toggle jack;
	public Toggle marcia;
	public Toggle sally;
	public Toggle tanya;

	// Use this for initialization
	void Start () {
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MasterChanged(){
		if (master.isOn)
			GameData.Instance.playerId = 1;

	}

	public void RandChanged(){
		if (rand.isOn)
			GameData.Instance.playerId = 2;
		
	}

	public void LuiChanged(){
		if (lui.isOn)
			GameData.Instance.playerId = 3;
		
	}

	public void AliceChanged(){
		if (alice.isOn)
			GameData.Instance.playerId = 4;
		
	}

	public void JackChanged(){
		if (jack.isOn)
			GameData.Instance.playerId = 5;
		
	}

	public void MarciaChanged(){
		if (marcia.isOn)
			GameData.Instance.playerId = 6;
		
	}

	public void SallyChanged(){
		if (sally.isOn)
			GameData.Instance.playerId = 7;
		
	}

	public void TanyaChanged(){
		if (tanya.isOn)
			GameData.Instance.playerId = 8;		
	}

	public void BackButtonClicked()
	{
		Debug.Log("Back button Clicked");
		
		Application.LoadLevel("MainMenu");
		
	}


}

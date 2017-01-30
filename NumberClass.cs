using UnityEngine;
using System.Collections;
using DG.Tweening;




public class NumberClass : MonoBehaviour {

	public GameObject CurrentNumber;
	public GameObject BaseNumber;
	public GameObject NumberIcon;
	public bool BeingDragged = false;
	public bool CollisionDetected = false;
	public bool ExitedFromPanel = false;
	public GameObject TempPanelToMoveTo;
	public bool isFirstNumber = false;
	public bool isSecondNumber = false;
	public bool isThirdNumber = false;
	public bool isFourthNumber = false;
	private float tweenTime = 0.6f;

	// Use this for initialization
	void Start () {
		DOTween.Init(false, false, LogBehaviour.Default);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void snapToPanel()
	{
		float x = CurrentNumber.transform.position.x;
		float y = CurrentNumber.transform.position.y;

		float z = -1.0f;
		Vector3 newMyVector = new Vector3(x, y, z);
		Debug.Log("Fleet Snap");
		NumberIcon.transform.DOMove(newMyVector, tweenTime).SetEase(Ease.OutQuint);
	}

	public void snapToBase()
	{
		float x = BaseNumber.transform.position.x;
		float y = BaseNumber.transform.position.y;
		
		float z = -1.0f;
		Vector3 newMyVector = new Vector3(x, y, z);
		Debug.Log("Base Snap");

		NumberIcon.transform.DOMove(newMyVector, tweenTime).SetEase(Ease.OutQuint);
	}

	public void MoveFleetTo(GameObject newPlanetToMoveTo)
	{
		if (this.gameObject.GetComponent<NumberClass> ().ExitedFromPanel) {
			snapToBase();
		} else {
			CurrentNumber = newPlanetToMoveTo;
			snapToPanel();
		}

	}
	
	public bool checkPosition(Vector3 objectToCheck)
	{
		float x = CurrentNumber.transform.position.x + 1.5f;
		float y = CurrentNumber.transform.position.y + 1.0f;
		float z = -1.0f;
		Vector3 hostNumber = new Vector3(x, y, z);
		if (hostNumber != objectToCheck)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

}

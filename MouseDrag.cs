using UnityEngine;
using System.Collections;

public class MouseDrag : MonoBehaviour {

	float x;
	float y;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update()
	{
		x = Input.mousePosition.x;
		y = Input.mousePosition.y;
	}

	void OnMouseDown()
	{
		Debug.Log("MouseDown");
		this.gameObject.GetComponent<NumberClass>().BeingDragged = true;
	}

	void OnMouseDrag()
	{
		transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 7.0f));//The 7.0f z seems to be the only way to keep my object from flying off its z position when dragged. Not sure why as all my objects are at -1. So I guess the difference from the camera or the mouse height??
		Debug.Log("MouseDrag");
	}

	void OnMouseUp()
	{ 
		Debug.Log("MouseUP");//let go of me!
		this.gameObject.GetComponent<NumberClass>().BeingDragged = false ;
		this.gameObject.GetComponent<NumberClass>().MoveFleetTo(this.gameObject.GetComponent<NumberClass>().TempPanelToMoveTo); 

	}
}

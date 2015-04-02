using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	public GameObject CornerTab;
	public GameObject CornerTabArrow;

	private Vector3 origPos;
	public Vector3 menuPos;
	private Vector3 slerpPos;
	private bool toMenu = true;
	private bool doSlerp = false;

	// Use this for initialization
	void Awake() {
		HexColor.SetColor(CornerTab,GameColors.selected);
		origPos = Camera.main.transform.position;
		//Vector3 camSize = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth*1.5f,Camera.main.pixelHeight*1.5f,0));
		//menuPos = new Vector3(origPos.x+camSize.x,origPos.y+camSize.y,origPos.z); 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (doSlerp) {
			Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, slerpPos, Time.deltaTime*4.5f);
		}
	}

	void OnMouseDown() {
		if (toMenu) {
			slerpPos = menuPos;
			toMenu = false;
		}
		else {
			slerpPos = origPos;
			toMenu = true;
		}
		doSlerp = true;
	}
}

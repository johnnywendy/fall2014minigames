using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	private Camera myCam;
	private Vector3 screenPos;
	private float   angleOffset;
	
	void Start () {
		myCam = Camera.main;
	}
	
	void FixedUpdate () {

		//This fires only on the frame the button is clicked
		if(Input.GetMouseButtonDown(0)) {
			screenPos = myCam.WorldToScreenPoint (transform.position);
			Vector3 v3 = Input.mousePosition - screenPos;
			angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(v3.y, v3.x))  * Mathf.Rad2Deg;
		}

		//This fires while the button is pressed down
		if(Input.GetMouseButton(0)) {
			Vector3 v3 = Input.mousePosition - screenPos;
			float angle = Mathf.Atan2(v3.y, v3.x) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0, 0, angle + angleOffset); 
		}
	}
}

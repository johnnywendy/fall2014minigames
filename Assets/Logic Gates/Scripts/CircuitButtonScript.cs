using UnityEngine;
using System.Collections;

public class CircuitButtonScript : MonoBehaviour {

	public bool mouseDown = false;

	void OnMouseDown() {
		mouseDown = true;
	}

	void OnMouseUp() {
		mouseDown = false;
	}

	void OnMouseExit() {
		if (mouseDown)
			Camera.main.GetComponent<LGManager_1>().ButtonClicked(transform.name);
		mouseDown = false;
	}
}

using UnityEngine;
using System.Collections;

public class ButtonBehavior : MonoBehaviour {

	Vector3 defaultScale;
	private bool mouseClicked = false;

	// Use this for initialization
	void Start () {
		defaultScale = transform.localScale;
	}

	void OnMouseOver() {
		if (!mouseClicked) {
			transform.localScale = new Vector3(1.4f,1.4f,1f);
		}
	}

	void OnMouseExit() {
		transform.localScale = defaultScale;
	}

	void OnMouseDown() {
		mouseClicked = true;
		transform.localScale = new Vector3(1.1f,1.1f,1f);
	}

	void OnMouseUp() {
		mouseClicked = false;
		transform.localScale = defaultScale;
	}
}

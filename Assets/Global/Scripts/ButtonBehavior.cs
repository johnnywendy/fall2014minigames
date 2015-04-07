using UnityEngine;
using System.Collections;

public class ButtonBehavior : MonoBehaviour {

	Vector3 defaultScale;
	private bool mouseClicked = false;
	public float hoverExpandAmnt = 0.4f;
	public float clickExpandAmnt = 0.1f;

	// Use this for initialization
	void Start () {
		defaultScale = transform.localScale;
	}

	void OnMouseOver() {
		if (!mouseClicked) {
			transform.localScale = new Vector3(defaultScale.x+hoverExpandAmnt,defaultScale.y+hoverExpandAmnt,1f);
		}
	}

	void OnMouseExit() {
		transform.localScale = defaultScale;
	}

	void OnMouseDown() {
		mouseClicked = true;
		transform.localScale = new Vector3(defaultScale.x+clickExpandAmnt,defaultScale.y+clickExpandAmnt,1f);
	}

	void OnMouseUp() {
		mouseClicked = false;
		transform.localScale = defaultScale;
	}
}

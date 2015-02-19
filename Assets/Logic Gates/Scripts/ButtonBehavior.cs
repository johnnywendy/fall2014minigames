using UnityEngine;
using System.Collections;

public class ButtonBehavior : MonoBehaviour {

	Vector3 defaultScale;

	// Use this for initialization
	void Start () {
		defaultScale = transform.localScale;
	}

	void OnMouseOver() {
		transform.localScale = new Vector3(1.4f,1.4f,1f);
	}

	void OnMouseExit() {
		transform.localScale = defaultScale;
	}

	void OnMouseDown() {
		transform.localScale = new Vector3(0.8f,0.8f,1f);
	}

	void OnMouseUp() {
		transform.localScale = defaultScale;
	}
}

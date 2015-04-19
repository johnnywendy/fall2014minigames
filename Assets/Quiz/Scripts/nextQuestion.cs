using UnityEngine;
using System.Collections;

public class nextQuestion : MonoBehaviour {

	GameObject obj;

	bool press = false;

	// Use this for initialization
	void Start () {
		obj = GameObject.Find ("question");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		press = true;
	}
	
	void OnMouseUp() {
		if (press) {
			obj.GetComponent<listQuestions>().newSet();
		}
	}
	
	void OnMouseExit() {
		press = false;
	}
}

using UnityEngine;
using System.Collections;

public class CircuitButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Debug.Log("sadfsd");
		Camera.main.GetComponent<LGManager_1>().ButtonClicked(transform.name);
	}

	void OnMouseUp() {

	}
}

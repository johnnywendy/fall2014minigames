﻿using UnityEngine;
using System.Collections;

public class clickScene : MonoBehaviour {

	GameObject background;
	public string treasureIsIn;

	// Use this for initialization
	void Start () {
		background = GameObject.FindWithTag("backgroundNotChest");
		background.renderer.material.color = new Color(2f,2f,2f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit = new RaycastHit ();
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.transform.tag == treasureIsIn) {
					Debug.Log ("This is where the treasure is!");
					treasureClick();
				} else if(hit.collider.transform.tag != "backgroundNotChest") {
					Debug.Log ("This is not where the treasure is!");
					notTreasureClick();
				}
			}
		}
	}
	
	void treasureClick() {
		background.renderer.material.color = new Color(10f,10f,10f);
	}

	void notTreasureClick() {
		background.renderer.material.color = new Color(0f,0f,0f);
	}
}

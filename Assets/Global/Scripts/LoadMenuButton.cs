using UnityEngine;
using System.Collections;

public class LoadMenuButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		HexColor.SetColor(this.gameObject,GameColors.selected);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Application.LoadLevel("Main");
	}
}

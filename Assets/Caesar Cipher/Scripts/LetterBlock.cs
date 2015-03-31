using UnityEngine;
using System.Collections;

public class LetterBlock : MonoBehaviour {

	private TextMesh letter;
	public string startText = "";
	public bool generator = false;

	// Use this for initialization
	void Start () {
		letter = GetComponentsInChildren<TextMesh>()[0];
		letter.text = startText;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetText(string newText) {
		letter.text = newText;
	}
}

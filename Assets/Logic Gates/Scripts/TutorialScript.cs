using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialScript : MonoBehaviour {

	public GameObject tutorialCover;
	public TextMesh tutorialText;
	public List<Vector3> coverPositions;
	public List<string> tutorialTexts = new List<string>() {};
	public List<Vector3> textPositions;

	private int tutIndex = 0;
	private bool pressing = false;

	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown() {
		pressing = true;
	}

	void OnMouseUp() {
		if (pressing) {
			tutIndex++;
			if (tutIndex < coverPositions.Count) {
				tutorialCover.transform.position = coverPositions[tutIndex];
				tutorialText.text = tutorialTexts[tutIndex];
				tutorialText.transform.position = textPositions[tutIndex];
			}
			else {
				tutorialCover.gameObject.SetActive(false);
				tutorialText.gameObject.SetActive(false);
			}
		}
		pressing = false;
	}

}

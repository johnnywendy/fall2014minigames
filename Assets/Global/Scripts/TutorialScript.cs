using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialScript : MonoBehaviour {

	public GameObject tutorialCover;
	public TextMesh tutorialText;
	public List<Vector3> coverPositions;
	private List<string> tutorialTexts;
	private List<string> circuitTexts = new List<string>() {"This is a logic gate. You will be\nusing them to create circuits.",
															"This is an Input block. You will be\nplugging them up to your Logic Gates.",
															"This is the Output Block. You will be plugging\nyour Logic Gates into it to finish a circuit.",
															"This is the Truth Table. Your goal is to make a\ncircuit that satisfies the Truth Table.",
															"The truth table shows you all of the\npossible combinations of green (True)\nand orange (False) Input blocks.",
															"Your goal is to get the Output block\nto match the Truth Table for\nall of those scenarios at once.",
															"You can create a Logic Gate by\ndragging it onto the board.",
															"You can wire it up to other gates by clicking\nthe small circle (Output) on its front and\ndragging it to the other small circles (Inputs)\non the back of other Logic Gates.",
															"Once you have satisfied the Truth Table,\nall of the X’s will change to check\nmarks and you can Test your circuit.",
															"Good Luck!"};
	private List<string> circuitTexts2 = new List<string>() {"Tap with two fingers to switch between\nedit mode. In edit mode you can reset\nconnections and remove Logic Gates.",
															 "You can also tap an Input block to switch its\noutput between True (green) and False (orange).",
															 "Good Luck!"};
	private List<string> cipherTexts = new List<string>() {"To decode a message you need to shift each \ncharacter of the encrypted message by a certain amount.",
															 "For instance, If we wanted to shift “A” two spaces",
															 "We would drag “C” into the next decrypted slot…",
															 "Which happens to be right here for now.",
															 "Give it a shot!"};
	public List<Vector3> textPositions;

	private int tutIndex = 0;
	private bool pressing = false;
	private bool ready = true;

	private bool doSlerp = false;
	private Vector3 slerpPos;
	private Vector3 slerpPos2;

	// Use this for initialization
	void Awake () {
		tutorialText.gameObject.SetActive(false);
		tutorialCover.gameObject.SetActive(false);
		GetComponent<BoxCollider2D>().enabled = false;
		ready = false;
	}

	public void CancelTutorial() {
		GameObject.Destroy(this.gameObject);
	}

	public void StartTutorial() {
		if (GameData.GetCurrentGame() == 0) {
			if (GameData.GetCurrentLevel() == 1)
				tutorialTexts = circuitTexts;
			if (GameData.GetCurrentLevel() == 2)
				tutorialTexts = circuitTexts2;

		}
		if (GameData.GetCurrentGame() == 1) {
			tutorialTexts = cipherTexts;
			
		}
		GetComponent<BoxCollider2D>().enabled = true;
		tutorialText.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Top";
		tutorialCover.transform.position = coverPositions[tutIndex];
		tutorialText.transform.position = textPositions[tutIndex];
		tutorialText.text = tutorialTexts[tutIndex];
		tutorialText.gameObject.SetActive(true);
		tutorialCover.gameObject.SetActive(true);
		ready = true;
	}

	void FixedUpdate() {
		if (doSlerp) {
			if (Vector3.Distance(tutorialCover.transform.position,slerpPos) > 0.01) {
				tutorialCover.transform.position = Vector3.Slerp(tutorialCover.transform.position, slerpPos, Time.deltaTime*4.5f);
				tutorialText.transform.position = Vector3.Slerp(tutorialText.transform.position, slerpPos2, Time.deltaTime*4.5f);
			}
			else {
				tutorialCover.transform.position = slerpPos;
				tutorialText.transform.position = slerpPos2;
				doSlerp = false;
				ready = true;
			}
		}
	}

	void UpdatePositions() {
		ready = false;
		slerpPos = coverPositions[tutIndex];
		slerpPos2 = textPositions[tutIndex]; 
		tutorialText.text = tutorialTexts[tutIndex];
		doSlerp = true;
	}

	void OnMouseDown() {
		pressing = true;
	}

	void OnMouseUp() {
		if (ready && pressing) {
			tutIndex++;
			if (tutIndex < coverPositions.Count) {
				UpdatePositions();
			}
			else {
				GameObject.Destroy(this.gameObject);
			}
		}
		pressing = false;
	}

}

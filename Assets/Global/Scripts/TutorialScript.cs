using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialScript : MonoBehaviour {

	public GameObject tutorialCover;
	public TextMesh tutorialText;
	public List<Vector3> coverPositions;
	private List<string> tutorialTexts = new List<string>() {"This is a logic gate.\nYou will be using it to create circuits.","Hey Listen!"};
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

	void StartTutorial() {
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

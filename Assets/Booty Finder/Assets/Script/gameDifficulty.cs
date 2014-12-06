using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class gameDifficulty : MonoBehaviour
{
		bool modeStarted;
		string trueAnswer;
		string answer;
		GameObject easy;
		GameObject medium;
		GameObject hard;
		GameObject buttonNeither;
		GameObject buttonBoth;
		GameObject cubeLeft;
		GameObject cubeRight;
		GameObject spotlightLeft;
		GameObject spotlightRight;
		GameObject speechInstructions;
		GameObject textLeft;
		GameObject textRight;

// Use this for initialization
		void Start ()
		{
				modeStarted = false;
				trueAnswer = "";
				answer = "";
				easy = GameObject.FindGameObjectWithTag ("buttonEasy");
				medium = GameObject.FindGameObjectWithTag ("buttonMedium");
				hard = GameObject.FindGameObjectWithTag ("buttonHard");
				cubeLeft = GameObject.FindGameObjectWithTag ("characterLeft");
				cubeRight = GameObject.FindGameObjectWithTag ("characterRight");
				buttonNeither = GameObject.FindGameObjectWithTag ("buttonNeither");
				buttonBoth = GameObject.FindGameObjectWithTag ("buttonBoth");	
				spotlightLeft = GameObject.FindGameObjectWithTag ("spotlightLeft");
				spotlightRight = GameObject.FindGameObjectWithTag ("spotlightRight");
				speechInstructions = GameObject.FindGameObjectWithTag ("speechInstructions");
				textLeft = GameObject.FindGameObjectWithTag ("speechLeft");
				textRight = GameObject.FindGameObjectWithTag ("speechRight");
				cubeLeft.SetActive (false);
				cubeRight.SetActive (false);
				buttonNeither.SetActive (false);
				buttonBoth.SetActive (false);
				spotlightLeft.SetActive (false);
				spotlightRight.SetActive (false);
				speechInstructions.SetActive (false);
				textLeft.SetActive (false);
				textRight.SetActive (false);
		}

// Update is called once per frame
		void Update ()
		{
				if (Input.GetMouseButtonDown (0) && modeStarted) {
						RaycastHit hit = new RaycastHit ();
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						if (Physics.Raycast (ray, out hit)) {
								if (hit.collider.transform.tag == "characterLeft") {
										Debug.Log ("Left Character");
										answer = "left";
										checkWin ();
								}
								if (hit.collider.transform.tag == "characterRight") {
										Debug.Log ("Right Character");
										answer = "right";
										checkWin ();
								}
						}
				}
		}
		
		public void buttonNeitherFunction ()
		{
				answer = "neither";
				if(modeStarted)
					checkWin ();
		}

		public void buttonBothFunction ()
		{
			answer = "both";
			if(modeStarted)
				checkWin ();
		}
		
		public void checkWin ()
		{
				string message = "";
				if (trueAnswer == "left" && answer == "left") {
						spotlightRight.SetActive (false);
						message = "You win! The knight is on the left!";
				} else if (trueAnswer == "right" && answer == "right") {
						spotlightLeft.SetActive (false);
						message = "You win! The knight is on the right!";
				} else if(trueAnswer == "both" && answer == "both") {
						spotlightLeft.SetActive(true);
						spotlightRight.SetActive(true);
						message = "You win! There is a knight is on both sides!";
				} else if(trueAnswer == "neither" && answer == "neither") {
						spotlightLeft.SetActive(false);
						spotlightRight.SetActive(false);
						message = "You win! The knight is on neither side!";
				} else {
						message = "You lose!";
				}
						speechInstructions.GetComponent<Text> ().text = message;
		}

		IEnumerator timeDelay (int seconds, string left, string right)
		{
				speechInstructions.SetActive (true);
				cubeLeft.SetActive (true);
				cubeRight.SetActive (true);
				yield return new WaitForSeconds (seconds);
				spotlightLeft.SetActive (true);
				spotlightRight.SetActive (true);
				yield return new WaitForSeconds (seconds);
				textLeft.GetComponent<Text> ().text = left;
				textRight.GetComponent<Text> ().text = right;
				textLeft.SetActive (true);
				yield return new WaitForSeconds (seconds);
				textRight.SetActive (true);
				buttonNeither.SetActive (true);
				buttonBoth.SetActive (true);
				modeStarted = true;
		}

		public void easyMode ()
		{
				trueAnswer = "both"; 
				Debug.Log ("Easy mode started");
				medium.SetActive (false);
				hard.SetActive (false);
				StartCoroutine (timeDelay (1, "We are the same type and I am a knight", "We are the same type and I am a knight"));
		}

		public void mediumMode ()
		{
				trueAnswer = "neither";
				Debug.Log ("Medium mode started");
				easy.SetActive (false);
				hard.SetActive (false);
				StartCoroutine (timeDelay (1, "We are different", "We are different"));
		}

		public void hardMode ()
		{
				trueAnswer = "left";
				Debug.Log ("Hard mode started");
				easy.SetActive (false);
				medium.SetActive (false);
				StartCoroutine (timeDelay (1, "I am a knight", "We are different"));
		}
}

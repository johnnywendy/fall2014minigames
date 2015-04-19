using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class listQuestions : MonoBehaviour {

	public string[] questions;
	public GameObject answerA;
	public GameObject answerB;
	public GameObject answerC;
	public GameObject answerD;

	public int messageWidth;

	// Use this for initialization
	void Start () {
		newQuestion ();
		HexColor.SetColor (answerA.transform.FindChild ("AnswerBlockA").gameObject, GameColors.off2);
		HexColor.SetColor (answerB.transform.FindChild ("AnswerBlockB").gameObject, GameColors.on2);
		HexColor.SetColor (answerC.transform.FindChild ("AnswerBlockC").gameObject, GameColors.on);
		HexColor.SetColor (answerD.transform.FindChild ("AnswerBlockD").gameObject, GameColors.selected);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void newSet() {
		newQuestion ();
		newA ();
		newB ();
		newC ();
		newD ();
	}

	void newQuestion() {
		string temp = questions [(int)Random.Range (0.0F, questions.Length)];
		string pattern = ".{1,"+messageWidth+"}(\\s+|$)";
		string rep = "$&\n";
		string newTemp = Regex.Replace(temp, pattern, rep);
		GetComponent<TextMesh> ().text = newTemp;
	}

	void newA() {

	}

	void newB() {
		
	}

	void newC() {
		
	}

	void newD() {
		
	}
}

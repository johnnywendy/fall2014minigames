using UnityEngine;
using System.Collections;

public class AnswerBlock : MonoBehaviour {
	
	public TextMesh letter;
	public string startText = "";

	public bool isAnswer = false;
	public bool press = false;

	// Use this for initialization
	void Start () {
		letter.text = startText;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		press = true;
	}

	void OnMouseUp() {
		if (press) {
			//puts alert if right or wrong answer was chosen
			GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			MessageBox alertBox = alert.GetComponent<MessageBox>();
			if(isAnswer)
				alertBox.message = "You picked the right answer.";
			else
				alertBox.message = "You picked the wrong answer.";
			alertBox.rightButtonText = "OK";
			alertBox.leftButtonText = "ALRIGHT";
			GameObject obj = GameObject.Find ("question");
			alertBox.SetLeftAction(obj,"listQuestions","newSet");
			alertBox.SetRightAction(obj,"listQuestions","newSet");
		}
	}

	void OnMouseExit() {
		press = false;
	}
}

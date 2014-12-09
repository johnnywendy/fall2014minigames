using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonTest : MonoBehaviour {
	public Slider Slider;
	public Button Button;
	public bool rightAnswer;
	//public GUIText guiText;
	public Text hintTextA;
	public Text hintTextB;
	public Text hinttextC;
	public Text hinttextD;
	//public AudioSource audioSound;

	//public Tex
	public int score = 0;

	//Must be public. Must be void. Must have 0 to 1 parameters.
	public void DoSomething() {
		Debug.Log (Slider.value.ToString ());
	}

	public void DoSomethingWithASlider (Slider slider) {
		Debug.Log (slider.value.ToString ());
	}
	
	public void LoadScene(string sceneName) {
		//audioSound.Play();
		Application.LoadLevel (sceneName);
	}

	public void RightAnswer(Button button) {
		score = score + 30;
		Debug.Log ("Correct! Score = " + score);
	}

	public void WrongAnswer(Button button) {
		score = score - 25;
		Debug.Log ("Score = " + score );
		//guiText.text = "This is okay";
	}

	public void DisplayHint(string msg) {
		hintTextA.text = msg;
		StartCoroutine (TimeDelay(5)); 
		//hintTextA.text = "";
	}

	public IEnumerator TimeDelay (int seconds) {
		yield return new WaitForSeconds(seconds);
		hintTextA.text = "";
	}

//	void Awake() {
//		DontDestroyOnLoad(score);
//	}
}

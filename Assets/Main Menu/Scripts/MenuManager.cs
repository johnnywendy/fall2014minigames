using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

	public List<MenuStrip> menuOptions;
	public List<CircleButton> circles;
	private List<string> menuColors = new List<string>() {"F2385A","F5A503","94E376","32BDAB","4AD9D9","36B1BF","825DC2"};
	private List<string> darkerColors;
	public bool canInteract = true;

	void Awake() {
		GameData.RegisterGame(0,10);
		GameData.RegisterGame(1,8);
		GameData.RegisterGame(2,6);
		GameData.RegisterGame(3,8);
		GameData.RegisterGame(4,6);
		GameData.CompletedLevel(0,1);
		GameData.CompletedLevel(0,2);
		GameData.CompletedLevel(0,3);
		GameData.CompletedLevel(0,4);
		GameData.CompletedLevel(0,5);
	}

	// Use this for initialization
	void Start () {
		ResetStripColors();
		SetPercentage();
	}

	// Update is called once per frame
	void Update () {

	}

	public void ResetStripColors() {
		for (int i = 0; i < menuOptions.Count; i++) {
			menuOptions[i].SetColor(menuColors[i]);
		}
	}

	public void LoadMenuForGame(int gameNumber) {
		if (canInteract) {
			GameData.SetCurrentGame(gameNumber);
			bool[] levels = GameData.GetLevelStatuses(gameNumber);
			bool last = true;
			bool foundNext = false;
			for (int i = 0; i < levels.Length; i++) {
				if ((last != levels[i]) && !foundNext) {
					circles[i].SetAsNext();
					foundNext = true;
				}
				else if (levels[i])
					circles[i].SetAsCompleted();
				else if (!levels[i])
					circles[i].SetAsIncomplete();
				last = levels[i];
			}
			SlideUp(gameNumber);
		}
	}

	void SetPercentage() {
		float total = 0; float completed = 0;
		for (int i = 0; i < 4; i++) {
			total += GameData.GetLevelCount(i);
			completed += GameData.GetCompletedCount(i);
		}
		GameObject.Find("PercentCompleted").GetComponent<TextMesh>().text = (System.Convert.ToInt32((completed/total)*100)).ToString()+"%";
	}

	public void SlideUp(int gameNumber) {
		Debug.Log(gameNumber);
		if (gameNumber == 6) return;
		float delay = 0.02f;
		int count = 1;
		for (int i = 0; i < menuOptions.Count; i++) {
			StartCoroutine(WaitAndSlideUp(menuOptions[i],delay));
			delay += 0.06f;
		}
		if (gameNumber == 5) {
			GameObject.Find ("Settings").GetComponent<ShrinkExpand>().Expand();
			StartCoroutine(WaitAndFadeBackButton());
		}
		else {
			foreach (CircleButton circle in circles)
				circle.gameObject.SetActive(false);
			for (int i = 0; i < GameData.GetLevelCount(gameNumber); i++) {
				circles[i].gameObject.SetActive(true);
				StartCoroutine(WaitAndExpandCircle(circles[i],delay));
				circles[i].SetLevel(count,GameData.GetLevelCount(gameNumber),"");
				count++;
				delay += 0.06f;
			}
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ShrinkExpand1")) {
				if (obj.name != "CircleButton" && obj.name != "LevelSelectionText")
					obj.GetComponent<ShrinkExpand>().Shrink();
				else if (obj.name == "LevelSelectionText")
					obj.GetComponent<ShrinkExpand>().Expand();
			}
			string completed = GameData.GetCompletedCount(gameNumber).ToString() + " of " + GameData.GetLevelCount(gameNumber).ToString();
			GameObject.Find ("LevelSelectionText").transform.FindChild("LevelsCompleted").GetComponent<TextMesh>().text = completed;
			StartCoroutine(WaitAndFadeBackButton());
		}
	}

	public void SlideUpFromBottom() {
		ResetStripColors();
		float delay = 0.02f;
		for (int i = 0; i < menuOptions.Count; i++) {
			menuOptions[i].transform.position = new Vector3(-11,-11,1);
			StartCoroutine(WaitAndSlideUpFromBottom(menuOptions[i],delay));
			delay += 0.06f;
		}
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ShrinkExpand1")) {
			if (obj.name != "CircleButton" && obj.name != "LevelSelectionText" && obj.name != "Settings")
				obj.GetComponent<ShrinkExpand>().Expand();
			else
				obj.GetComponent<ShrinkExpand>().Shrink();
		}
		canInteract = false;
		StartCoroutine(InteractOn());
	}

	IEnumerator InteractOn() {
		yield return new WaitForSeconds(0.6f);
		foreach (CircleButton circle in circles)
			circle.gameObject.SetActive(true);
		canInteract = true;
		foreach (MenuStrip strip in menuOptions)
			strip.ResetSensitiveVars();
	}

	IEnumerator WaitAndFadeBackButton() {
		yield return new WaitForSeconds(0.1f);
		GameObject.Find("BackButton").GetComponent<BackButton>().FadeIn();
	}

	IEnumerator WaitAndSlideUp(MenuStrip strip,float delay) {
		yield return new WaitForSeconds(delay);
		strip.SlideToHeight(11,1f);
	}

	IEnumerator WaitAndSlideUpFromBottom(MenuStrip strip,float delay) {
		yield return new WaitForSeconds(delay);
		strip.SlideToHeight(0,1f);
	}

	IEnumerator WaitAndExpandCircle(CircleButton circle,float delay) {
		yield return new WaitForSeconds(delay);
		circle.GetComponent<ShrinkExpand>().Expand();
	}
}

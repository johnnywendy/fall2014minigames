using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CCManager : MonoBehaviour {

	private int level = 0;
	private List<string> words;
	private List<string> words0 = new List<string>() {"Test"};
	private List<string> words1 = new List<string>() {"Caesar","Cipher"};
	private List<string> words2 = new List<string>() {"Circuits","Circuit"};
	private List<string> words3 = new List<string>() {"Binary"};
	private List<string> words4 = new List<string>() {"Launch"};
	private List<string> words5 = new List<string>() {"Success"};
	int min = 0; int max = 0;
	bool reshuffle = false;
	public List<LetterBlock> encrypted;
	public List<LetterBlock> decrypted;
	public List<LetterBlock> options;
	public List<TextMesh> shiftedAmounts;
	public TextMesh shiftAmount;
	public TextMesh shiftLetter;
	public GameObject arrow;
	public GameObject heldObj;
	private int _shiftAmount = 0;
	private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	private string activeWord = "";
	private int _activeIndex = 0;
	public int activeIndex {
		get {
			return _activeIndex;
		}
		set {
			_activeIndex = value;
			if (_activeIndex > 0) {
				if (_activeIndex < activeWord.Length) 
					SetupNextLetter();
				else
					GameOver();
			}
			if (_activeIndex == activeWord.Length && activeWord != "") {
				return;
			}
			HexColor.SetColor(encrypted[encrypted.Count-1-_activeIndex].gameObject,GameColors.off);
			HexColor.SetColor(decrypted[decrypted.Count-1-_activeIndex].gameObject,GameColors.on2);
		}
	}

	// Use this for initialization
	void Start () {
		level = GameData.GetCurrentLevel();
		Debug.Log(level);
		HexColor.SetColor(arrow,GameColors.inactive);
		shiftLetter.color = HexColor.HexToColor(GameColors.inactive);
		shiftAmount.color = HexColor.HexToColor(GameColors.inactive);
		Random.seed = (int)System.DateTime.Now.Ticks;
		foreach (LetterBlock block in options)
			HexColor.SetColor(block.gameObject,GameColors.inactive);
		StartNewGame();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			if (heldObj != null && !heldObj.GetComponent<LetterBlock>().isHittingCurrent) { 
				GameObject.Destroy(heldObj);
				heldObj = null;
			}
			else if (heldObj != null) {
				heldObj.transform.localScale = heldObj.GetComponent<LetterBlock>().startScale;
				IsCorrect(heldObj.GetComponent<LetterBlock>());
			}
		}
	}

	public void StartNewGame() {
		foreach (LetterBlock block in encrypted) {
			HexColor.SetColor(block.gameObject,GameColors.inactive);
			block.gameObject.SetActive(false);
		}
		foreach (LetterBlock block in decrypted) {
			HexColor.SetColor(block.gameObject,GameColors.inactive2);
			block.gameObject.SetActive(false);
		}
		foreach (TextMesh amnt in shiftedAmounts) {
			amnt.color = HexColor.HexToColor(GameColors.inactive2);
			amnt.gameObject.SetActive(false);
		}
		if (level == 1) {
			words = words0;
			min = -2; max = 2;
			reshuffle = false;
			GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			MessageBox alertBox = alert.GetComponent<MessageBox>();
			alertBox.message = "Hey soldier! We need you to try\nout this new system to decode\nmesssages for us. Get to it!";
			alertBox.leftButtonText = "ALRIGHT";
			GameObject tut = GameObject.Find ("Tutorial");
			alertBox.SetLeftAction(tut,"TutorialScript","StartTutorial");
			alertBox.SetRightAction(tut,"TutorialScript","StartTutorial");
		}
		if (level == 2) {
			words = words1;
			min = -3; max = 3;
			reshuffle = false;
			GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			MessageBox alertBox = alert.GetComponent<MessageBox>();
			alertBox.message = "Great, it works!\nSee what the next message is.";
			alertBox.leftButtonText = "ALRIGHT";
			alertBox.SetLeftAction("destroy");
			alertBox.SetRightAction("destroy");
		}
		if (level == 3) {
			words = words2;
			min = -4; max = 4;
			reshuffle = false;
			GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			MessageBox alertBox = alert.GetComponent<MessageBox>();
			alertBox.message = "Exactly! This method of\nencryption is called a Caesar Cipher\nLook, a new incoming message!";
			alertBox.leftButtonText = "ALRIGHT";
			alertBox.SetLeftAction("destroy");
			alertBox.SetRightAction("destroy");
		}
		if (level == 4) {
			words = words3;
			min = -2; max = 2;
			reshuffle = true;
			GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			MessageBox alertBox = alert.GetComponent<MessageBox>();
			alertBox.message = "Ah we need to use the circuits\nto wire up the launch console.\nBut what is next?";
			alertBox.leftButtonText = "ALRIGHT";
			alertBox.SetLeftAction("destroy");
			alertBox.SetRightAction("destroy");
		}
		if (level == 5) {
			words = words4;
			min = -4; max = 4;
			reshuffle = true;
			GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			MessageBox alertBox = alert.GetComponent<MessageBox>();
			alertBox.message = "Of course! We need to program\nthe console with binary. Let's see\nwhat the next messge is.";
			alertBox.leftButtonText = "ALRIGHT";
			alertBox.SetLeftAction("destroy");
			alertBox.SetRightAction("destroy");
		}
		if (level == 6) {
			words = words5;
			min = -6; max = 6;
			reshuffle = true;
			GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			MessageBox alertBox = alert.GetComponent<MessageBox>();
			alertBox.message = "Everything is a go, Launch\nthe payload! Did it work?";
			alertBox.leftButtonText = "ALRIGHT";
			alertBox.SetLeftAction("destroy");
			alertBox.SetRightAction("destroy");
		}
		activeIndex = 0;
		Random.seed = (int)System.DateTime.Now.Ticks;
		activeWord = words[Random.Range(0,words.Count)].ToUpper();
		if (level == 0) {
			min = -1; max = 1;
			reshuffle = false;
		}
		if (reshuffle) {
			_shiftAmount = ((int)Random.Range (min, max));
			if (_shiftAmount == 0) {
				int neg = Random.Range(1,2);
				int add = Random.Range(1,max);
				_shiftAmount += (neg == 1) ? (-add) : add;
			}
		}
		else {
			_shiftAmount = ((int)Random.Range (1, 2)) == 1 ? min : max;
		}
		shiftAmount.text = (-_shiftAmount).ToString();
		encrypted[encrypted.Count-1-activeIndex].SetText(ShiftChar(activeWord[activeIndex].ToString(),_shiftAmount));
		shiftLetter.text = ShiftChar(activeWord[activeIndex].ToString(),_shiftAmount);
		for (int i = 0; i < activeWord.Length; i++) {
			encrypted[encrypted.Count-1-i].gameObject.SetActive(true);
			decrypted[decrypted.Count-1-i].gameObject.SetActive(true);
		}
	}

	void SetupNextLetter() {
		if (reshuffle) {
			_shiftAmount = ((int)Random.Range (min, max));
			if (_shiftAmount == 0) {
				int neg = Random.Range(1,2);
				int add = Random.Range(1,max);
				_shiftAmount += (neg == 1) ? (-add) : add;
			}
		}
		shiftAmount.text = (-_shiftAmount).ToString();
		encrypted[encrypted.Count-1-activeIndex].SetText(ShiftChar(activeWord[activeIndex].ToString(),_shiftAmount));
		shiftLetter.text = ShiftChar(activeWord[activeIndex].ToString(),_shiftAmount);
	}

	void GameOver() {
		GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
		MessageBox alertBox = alert.GetComponent<MessageBox>();
		alertBox.message = "Great Job, you\ndecoded the message";
		GameData.CompletedLevel(GameData.GetCurrentGame(),level);
		GameData.SetCurrentLevel(level+1);
		if (level != GameData.GetLevelCount(GameData.GetCurrentGame())) {
			GameData.SetCurrentLevel(level+1);
			alertBox.SetLeftAction("loadscene","iPadCaesar");
			alertBox.SetRightAction("loadscene","iPadCaesar");
		}
		else {
			alertBox.message = "Success! You've completed the\nmission. Try out another game!";
			alertBox.SetLeftAction("loadscene","Main");
			alertBox.SetRightAction("loadscene","Main");
		}
	}

	string ShiftChar(string chr, int amount) {
		int index = alphabet.IndexOf(((char)(chr.ToUpper()[0]+amount)).ToString());
		if (index < 0) index += 26;
		if (index > 26) index -= 26;
		return alphabet[index].ToString();
	}

	public void IsCorrect(LetterBlock block) {
		if (block.isHittingCurrent && block.letter.text.ToUpper() == activeWord[activeIndex].ToString().ToUpper()) {
			decrypted[decrypted.Count-1-activeIndex].GetComponent<LetterBlock>().SetText(block.letter.text);
			if (reshuffle) {
				shiftedAmounts[activeIndex].gameObject.SetActive(true);
				shiftedAmounts[activeIndex].text = shiftAmount.text;
			}
			HexColor.SetColor(decrypted[decrypted.Count-1-_activeIndex].gameObject,GameColors.on);
			activeIndex++;
		}
		else {
			HexColor.SetColor(decrypted[decrypted.Count-1-_activeIndex].gameObject,GameColors.off2);
		}
		GameObject.Destroy(block.gameObject);
		heldObj = null;
	}
}

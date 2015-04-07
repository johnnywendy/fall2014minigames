using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CCManager : MonoBehaviour {

	private int level = 0;
	private List<string> words0 = new List<string>() {"test","hello","world"};
	private List<string> words1 = new List<string>() {"test","hello","world"};
	private List<string> words2 = new List<string>() {"test","hello","world"};
	private List<string> words3 = new List<string>() {"test","hello","world"};
	private List<string> words4 = new List<string>() {"test","hello","world"};
	private List<string> words5 = new List<string>() {"test","hello","world"};
	private List<string> words6 = new List<string>() {"test","hello","world"};
	private List<string> words7 = new List<string>() {"test","hello","world"};

	public List<LetterBlock> encrypted;
	public List<LetterBlock> decrypted;
	public List<LetterBlock> options;
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
		activeIndex = 0;
		Random.seed = (int)System.DateTime.Now.Ticks;
		activeWord = words0[Random.Range(0,words0.Count)].ToUpper();
		_shiftAmount = ((int)Random.Range (-4, 4));
		if (_shiftAmount == 0) {
			int neg = Random.Range(1,2);
			int add = Random.Range(1,5);
			_shiftAmount += (neg == 1) ? (-add) : add;
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
		_shiftAmount = ((int)Random.Range (-4, 4));
		shiftAmount.text = (-_shiftAmount).ToString();
		encrypted[encrypted.Count-1-activeIndex].SetText(ShiftChar(activeWord[activeIndex].ToString(),_shiftAmount));
		shiftLetter.text = ShiftChar(activeWord[activeIndex].ToString(),_shiftAmount);
	}

	void GameOver() {
		GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
		MessageBox alertBox = alert.GetComponent<MessageBox>();
		alertBox.message = "Great Job, you\ndecoded the message";
		GameData.CompletedLevel(GameData.GetCurrentGame(),level);
		if (level+1 != GameData.GetLevelCount(GameData.GetCurrentGame())) {
			GameData.SetCurrentLevel(level+1);
			alertBox.SetLeftAction("loadscene","iPadCaesar");
			alertBox.SetRightAction("loadscene","iPadCaesar");
		}
		else {
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

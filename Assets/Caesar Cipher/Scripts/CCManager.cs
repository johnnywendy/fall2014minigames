using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CCManager : MonoBehaviour {
	
	private List<string> words = new List<string>() {"test","hello","world"};
	public List<LetterBlock> encrypted;
	public List<LetterBlock> decrypted;
	public List<LetterBlock> options;
	public TextMesh shiftAmount;
	public GameObject heldObj;
	private int _shiftAmount = 0;
	private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	private string activeWord;
	private int _activeIndex = 0;
	public int activeIndex {
		get {
			return _activeIndex;
		}
		set {
			_activeIndex = value;
			HexColor.SetColor(encrypted[encrypted.Count-1-_activeIndex].gameObject,GameColors.off);
			HexColor.SetColor(decrypted[decrypted.Count-1-_activeIndex].gameObject,GameColors.on2);
			if (_activeIndex > 0) {
				if (_activeIndex < activeWord.Length) 
					SetupNextLetter();
				else
					GameOver();
			}
		}
	}

	// Use this for initialization
	void Start () {
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
			else {
				heldObj.transform.localScale = heldObj.GetComponent<LetterBlock>().startScale;
				IsCorrect(heldObj.GetComponent<LetterBlock>());
			}
		}
	}

	public void StartNewGame() {
		foreach (LetterBlock block in encrypted)
			HexColor.SetColor(block.gameObject,GameColors.inactive);
		foreach (LetterBlock block in decrypted)
			HexColor.SetColor(block.gameObject,GameColors.inactive2);
		activeIndex = 0;
		Random.seed = (int)System.DateTime.Now.Ticks;
		activeWord = words[Random.Range(0,words.Count)].ToUpper();
		_shiftAmount = ((int)Random.Range (-5, 5));
		shiftAmount.text = (-_shiftAmount).ToString();
		encrypted[encrypted.Count-1-activeIndex].SetText(ShiftChar(activeWord[activeIndex].ToString(),_shiftAmount));
	}

	void SetupNextLetter() {
		_shiftAmount = ((int)Random.Range (-5, 5));
		shiftAmount.text = (-_shiftAmount).ToString();
		encrypted[encrypted.Count-1-activeIndex].SetText(ShiftChar(activeWord[activeIndex].ToString(),_shiftAmount));
	}

	void GameOver() {

	}

	string ShiftChar(string chr, int amount) {
		int index = alphabet.IndexOf(((char)(chr.ToUpper()[0]+amount)).ToString());
		if (index < 0) index += 26;
		if (index > 26) index -= 26;
		return alphabet[index].ToString();
	}

	void NextLetter() {

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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CCManager : MonoBehaviour {

	public List<string> words = new List<string>() {"test"};
	public List<LetterBlock> encrypted;
	public List<LetterBlock> decrypted;
	public List<LetterBlock> options;
	private int _activeIndex = 0;
	public int activeIndex {
		get {
			return _activeIndex;
		}
		set {
			_activeIndex = value;
			HexColor.SetColor(encrypted[encrypted.Count-1-_activeIndex].gameObject,GameColors.off);
			HexColor.SetColor(decrypted[decrypted.Count-1-_activeIndex].gameObject,GameColors.on2);
		}
	}

	// Use this for initialization
	void Start () {
		foreach (LetterBlock block in encrypted) {
			HexColor.SetColor(block.gameObject,GameColors.inactive);
		}
		foreach (LetterBlock block in decrypted) {
			HexColor.SetColor(block.gameObject,GameColors.inactive2);
		}
		activeIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

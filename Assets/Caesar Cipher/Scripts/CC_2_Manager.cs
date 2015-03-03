using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CC_2_Manager : MonoBehaviour {

	public GameObject correct;
	public GameObject incorrect;
	public GameObject bandOverlay;
	public Text shiftAmount;
	public List<Text> originWord;
	public List<Text> shiftedWord;

	private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	private List<string> words = new List<string> {"HELLO","WORLD","TEST","PICKLES","COMPUTER","PHONE","OREO"};

	private string word;
	private int shiftAmnt;
	private int shiftMaxBound;
	private int shiftMinBound;

	// Use this for initialization
	void Start () {
		correct.SetActive (false);
		incorrect.SetActive (false);
		bandOverlay.SetActive (false);
		Random.seed = (int)System.DateTime.Now.Ticks;
		word = words [Random.Range (0, words.Count)];
		SetOriginWord(word);
		SetShiftedWord(word);
		SetOptimalShiftAmounts(word);
		shiftAmount.text = shiftAmnt.ToString();
		ShiftOriginWord();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {

		}
		if (Input.GetMouseButtonUp (0)) {

		}

		RaycastHit hitTemp = new RaycastHit();
		Ray rayTemp = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(rayTemp, out hitTemp)) {

		}
		else {

		}
	}

	void SetOriginWord(string newWord) {
		int count = newWord.Length;
		for (int i = 0; i < count; i++) {
			originWord[i].text = word[i].ToString();
		}
		for (int i = count; i < originWord.Count; i++) {
			originWord[i].text = "";
		}
	}

	void SetShiftedWord(string newWord) {
		int count = newWord.Length;
		for (int i = 0; i < count; i++) {
			shiftedWord[i].text = word[i].ToString();
		}
		for (int i = count; i < shiftedWord.Count; i++) {
			shiftedWord[i].text = "";
			shiftedWord[i].rectTransform.parent.GetComponent<CC_Selector>().enabled = false;
		}
	}

	void ShiftOriginWord() {
		int count = word.Length;
		for (int i = 0; i < count; i++) {
			originWord[i].text = alphabet[(int)word.ToCharArray()[i]+shiftAmnt-65].ToString();
		}
	}

	void SetOptimalShiftAmounts(string newWord) {
		shiftMaxBound = 27;
		shiftMinBound = 27;
		for (int i = 0; i < newWord.Length; i++) {
			int val = (int)newWord.ToCharArray()[i]-65;
			if (shiftMaxBound > (26 - val))
				shiftMaxBound = 26 - val;
			if (shiftMinBound > val)
				shiftMinBound = val;
		}
		shiftMinBound = shiftMinBound * (-1);
		shiftAmnt = 0;
		while (shiftAmnt == 0) {
			shiftAmnt = ((int)Random.Range (shiftMinBound, shiftMaxBound));
		}
		for (int i = 0; i < newWord.Length; i++) {
			int charShiftAmnt = 0;
			if (shiftAmnt > 0)
				charShiftAmnt = ((int)Random.Range (shiftMinBound/3, shiftMaxBound/2));
			else
				charShiftAmnt = ((int)Random.Range (shiftMinBound/2, shiftMaxBound/3));
			shiftedWord[i].text = alphabet[(int)word.ToCharArray()[i]+charShiftAmnt-65].ToString();
		}
	}

	public void ValueChanged() {
		bool completed = true;
		for (int i = 0; i < word.Length; i++) {
			if (shiftedWord[i].text != word[i].ToString())
				completed = false;
		}
		if (completed) {
			bandOverlay.SetActive(true);
			correct.SetActive(true);
			StartCoroutine(RestartScene(2.0f));
		}
		else {
			Debug.Log("nope");
		}
	}

	public IEnumerator RestartScene(float delay) {
		yield return new WaitForSeconds(delay);
		Application.LoadLevel (Application.loadedLevelName);
	}

	public Color HexToColor(string hex) {
		string rs = hex[0].ToString() + hex[1].ToString();
		string gs = hex[2].ToString() + hex[3].ToString();
		string bs = hex[4].ToString() + hex[5].ToString();
		int r = System.Convert.ToInt32(rs,16);
		int g = System.Convert.ToInt32(gs,16);
		int b = System.Convert.ToInt32(bs,16);
		return new Color(r/255.0f, g/255.0f, b/255.0f, 1.0f);
	}

	public Color HexToColorWithAlpha(string hex,float alpha) {
		string rs = hex[0].ToString() + hex[1].ToString();
		string gs = hex[2].ToString() + hex[3].ToString();
		string bs = hex[4].ToString() + hex[5].ToString();
		int r = System.Convert.ToInt32(rs,16);
		int g = System.Convert.ToInt32(gs,16);
		int b = System.Convert.ToInt32(bs,16);
		return new Color(r/255.0f, g/255.0f, b/255.0f, alpha);
	}
}

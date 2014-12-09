using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CC_1_Manager : MonoBehaviour {

	public Text shiftAmount;
	public Text button;
	public Text minus3;
	public Text minus2;
	public Text minus1;
	public Text zero;
	public Text plus1;
	public Text plus2;
	public Text plus3;
	public Text instructions;
	public GameObject correct;
	public GameObject incorrect;
	public GameObject bandOverlay;
	private RectTransform target;
	private bool firstUp = false;
	private bool isDragging = false;
	private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	private string letter;
	private Vector3 startPos;

	// Use this for initialization
	void Start () {
		correct.SetActive (false);
		incorrect.SetActive (false);
		bandOverlay.SetActive (false);
		shiftAmount = GameObject.Find ("ShiftAmount").GetComponent<Text>();
		Random.seed = (int)System.DateTime.Now.Ticks;
		shiftAmount.text = ((int)Random.Range (-3, 3)).ToString();
		int alphaIndex = (int)Random.Range(3, 22);
		letter = alphabet[alphaIndex].ToString();
		button.text = letter;
		minus3.text = alphabet[alphaIndex-3].ToString();
		minus2.text = alphabet[alphaIndex-2].ToString();
		minus1.text = alphabet[alphaIndex-1].ToString();
		zero.text = alphabet[alphaIndex].ToString();
		plus1.text = alphabet[alphaIndex+1].ToString();
		plus2.text = alphabet[alphaIndex+2].ToString();
		plus3.text = alphabet[alphaIndex+3].ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (isDragging && firstUp) {
			target.localPosition = Input.mousePosition - new Vector3(640,360,1);
		}
		if (Input.GetMouseButtonDown (0)) {
			if (isDragging && firstUp) {
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if (Physics.Raycast(ray, out hit)) {
					target.gameObject.SetActive(false);
					if (hit.collider.gameObject.name == shiftAmount.text) {
						hit.collider.gameObject.GetComponent<Selectable>().interactable = false;
						correct.SetActive(true);
						bandOverlay.SetActive(true);
						StartCoroutine(RestartScene(2.0f));
					}
					else {
						ColorBlock temp = hit.collider.gameObject.GetComponent<Selectable>().colors;
						temp.disabledColor = HexToColor("cc3f3f");
						hit.collider.gameObject.GetComponent<Selectable>().colors = temp;
						hit.collider.gameObject.GetComponent<Selectable>().interactable = false;
						bandOverlay.SetActive(true);
						incorrect.SetActive(true);
						StartCoroutine(RestartScene(2.0f));
					}
				}
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			if (firstUp) {
				isDragging = false;
				target.GetComponent<Selectable>().interactable = true;
			}
			else
				firstUp = true;
		}

		RaycastHit hitTemp = new RaycastHit();
		Ray rayTemp = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(rayTemp, out hitTemp)) {
			if (hitTemp.collider.gameObject.name == "Button") {
				instructions.color = HexToColor("c7c7c7");
			}
			if (hitTemp.collider.gameObject.tag == "Option") {
				button.text = hitTemp.collider.gameObject.GetComponentInChildren<Text>().text;
				ColorBlock tempColorBlock = button.rectTransform.parent.GetComponent<Selectable>().colors;
				tempColorBlock.disabledColor = HexToColorWithAlpha("60A0B4",0.75f);
				button.rectTransform.parent.GetComponent<Selectable>().colors = tempColorBlock;
			}
		}
		else {
			ColorBlock tempColorBlock = button.rectTransform.parent.GetComponent<Selectable>().colors;
			tempColorBlock.disabledColor = HexToColorWithAlpha("515151",0.5f);
			button.rectTransform.parent.GetComponent<Selectable>().colors = tempColorBlock;
			instructions.color = HexToColor("595959");
			button.text = letter;
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

	public void dragObject(GameObject obj) {
		firstUp = false;
		isDragging = true;
		instructions.gameObject.SetActive (false);
		obj.GetComponent<BoxCollider> ().enabled = false;
		target = obj.GetComponent<RectTransform>();
		target.GetComponent<Selectable>().interactable = false;
	}
}

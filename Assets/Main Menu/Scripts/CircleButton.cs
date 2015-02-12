using UnityEngine;
using System.Collections;

public class CircleButton : MonoBehaviour {
	
	private bool isClicking = false;
	private bool expand = false;
	private Vector3 startScale;
	private Vector3 selectedScale;

	enum states {complete,incomplete,next};
	private states levelState = states.incomplete;

	void Awake() {
		startScale = transform.localScale;
		selectedScale = startScale - new Vector3(0.2f,0.2f,0.2f);
	}

	// Use this for initialization
	void Start () {
	
	}

	void Update() {
		if (Input.GetMouseButtonDown(0))
			isClicking = true;
		if (Input.GetMouseButtonUp(0))
			isClicking = false;
		if (expand)
			transform.localScale = Vector3.Slerp(transform.localScale,startScale,Time.deltaTime*4);
	}

	void FixedUpdate () {

	}

	void OnMouseOver() {
		if (isClicking)
			transform.localScale = Vector3.Slerp(transform.localScale,selectedScale,Time.deltaTime*8);
		else
			expand = true;
	}

	void OnMouseExit() {
		if (isClicking)
			expand = true;
	}

	void OnMouseUp() {
		if (levelState != states.incomplete)
			Application.LoadLevel(GameData.GetCurrentGame()+1);
	}

	public void SetLevel(int number, int maxNumber, string scene) {

		float radius = 3f;
		float step = ((Mathf.PI * 2f)/maxNumber);
		float x = Mathf.Sin((number-1)*step) * radius;
		float y = Mathf.Cos((number-1)*step) * radius;
		transform.position = new Vector3(x,y,transform.position.z);

		gameObject.GetComponentInChildren<TextMesh>().text = number.ToString();
		expand = false;
	}

	public void SetAsIncomplete() {
		levelState = states.incomplete;
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("circle");
		GetComponentsInChildren<TextMesh>()[0].color = HexColor.HexToColor("8E8E8E");
	}

	public void SetAsCompleted() {
		levelState = states.complete;
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("circle-complete");
		GetComponentsInChildren<TextMesh>()[0].color = HexColor.HexToColor("32BDAB");
	}

	public void SetAsNext() {
		levelState = states.next;
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("circle-next");
		GetComponentsInChildren<TextMesh>()[0].color = HexColor.HexToColor("F5A503");
	}
}

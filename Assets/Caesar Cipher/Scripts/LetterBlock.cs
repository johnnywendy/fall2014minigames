using UnityEngine;
using System.Collections;

public class LetterBlock : MonoBehaviour {

	public TextMesh letter;
	public string startText = "";
	public bool generator = false;
	public bool isHeld = true;
	public bool isStatic;
	public bool isHittingCurrent = false;

	public Vector3 startScale;
	private Vector3 heldScale;
	private CCManager gm;

	// Use this for initialization
	void Awake () {
		startScale = transform.localScale;
		heldScale = new Vector3 (startScale.x*1.1f,startScale.y*1.1f,startScale.z);
		letter = GetComponentsInChildren<TextMesh>()[0];
		letter.text = startText;
		gm = Camera.main.GetComponent<CCManager>();
		if (isStatic) isHeld = false; 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isHeld) {
			transform.localScale = heldScale;
			transform.position = VisibleMousePosition();
		}
	}

	public Vector3 VisibleMousePosition() {
		Vector3 mousPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		return new Vector3 (mousPos.x, mousPos.y, -2f);
	}

	public Vector3 RayCheck() {
		Vector3 mousPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		return new Vector3 (mousPos.x, mousPos.y, -1f);
	}

	public void SetText(string newText) {
		letter.text = newText;
	}

	void OnMouseDown() {
		if (generator) {
			GameObject newBlock = (GameObject)Instantiate(Resources.Load("LetterBlock", typeof(GameObject)),Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity);
			HexColor.SetColor(newBlock,GameColors.selected);
			newBlock.GetComponent<LetterBlock>().generator = false;
			newBlock.GetComponent<LetterBlock>().isHeld = true;
			newBlock.GetComponent<LetterBlock>().SetText(letter.text);
			newBlock.AddComponent<Rigidbody2D>();
			newBlock.GetComponent<Rigidbody2D>().gravityScale = 0;
			newBlock.GetComponent<Rigidbody2D>().fixedAngle = true;
			newBlock.GetComponent<Rigidbody2D>().isKinematic = true;
			gm.heldObj = newBlock;
		}
	}

	void OnMouseEnter() {
		if (generator)
			HexColor.SetColor(this.gameObject,GameColors.on2);
	}

	void OnMouseExit() {
		if (generator)
			HexColor.SetColor(this.gameObject,GameColors.inactive);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (isHeld && !isHittingCurrent) {
			if (other.gameObject == gm.decrypted[gm.decrypted.Count-1-gm.activeIndex].gameObject) {
				HexColor.SetColor(this.gameObject,GameColors.on);
				isHittingCurrent = true;
			}
			else {
				HexColor.SetColor(this.gameObject,GameColors.off);
				isHittingCurrent = false;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (isHeld && !isHittingCurrent) {
			if (other.gameObject == gm.decrypted[gm.decrypted.Count-1-gm.activeIndex].gameObject) {
				HexColor.SetColor(this.gameObject,GameColors.on);
				isHittingCurrent = true;
			}
			else {
				HexColor.SetColor(this.gameObject,GameColors.off);
				isHittingCurrent = false;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject == gm.decrypted[gm.decrypted.Count-1-gm.activeIndex].gameObject) {
			isHittingCurrent = false;
		}
		else if (isHeld) {
			HexColor.SetColor(this.gameObject,GameColors.selected);
		}
	}
}

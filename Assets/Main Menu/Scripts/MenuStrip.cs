using UnityEngine;
using System.Collections;

public class MenuStrip : MonoBehaviour {

	public bool allowDraggging = false;
	public int gameNumber = 0;

	//private float dragSlope = -2.857142857142857f;
	private float dragSlope = -3.65f;
	private bool shouldTrack = false;
	private bool isAnimating = false;
	private bool acceptClick = false;
	private bool isClicking = false;
	public bool canInteract = true;
	private float anchorX = 0f;
	private Vector3 startPos;
	private Vector3 offsetPos;
	private Vector3 mousePos;
	private float targetHeight;
	private float animateSpeed = 0;
	private string hexColor;
	private float startingZ;

	private SpriteRenderer myRenderer;
	private Shader shaderGUItext;
	private Shader shaderSpritesDefault;

	private float _position = 0;
	public float position {
		set {
			_position = value;
			transform.position = new Vector3(value/dragSlope+anchorX,value,startingZ);
		}
		get {
			return  _position;
		}
	}

	// Use this for initialization
	void Awake () {
		anchorX = transform.position.x;
		startingZ = transform.position.z;
		myRenderer = gameObject.GetComponent<SpriteRenderer>();
		shaderGUItext = Shader.Find("GUI/Text Shader");
		shaderSpritesDefault = Shader.Find("Sprites/Default");
	}
	
	// Update is called once per frame
	void Update () {
		if (allowDraggging) {
			if (shouldTrack) {
				mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				position = (offsetPos.y + mousePos.y - startPos.y);
			}
		}
		canInteract = Camera.main.GetComponent<MenuManager>().canInteract;
	}

	void FixedUpdate() {
		if (isAnimating) {
			if (transform.position.y < targetHeight-animateSpeed) {
				position = transform.position.y+animateSpeed;
			}
			else {
				position = targetHeight;
				isAnimating = false;
				StartCoroutine(MakeInteractable());
			}
		}
	}

	void OnMouseEnter() {
			acceptClick = true;
			if (isClicking)
				myRenderer.color = HexColor.AdjustHexToColor(hexColor,-40);
	}

	void OnMouseExit() {
			acceptClick = false;
			if (isClicking)
				myRenderer.color = HexColor.HexToColor(hexColor);
	}

	void OnMouseDown() {
			if (allowDraggging) {
				startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				offsetPos =  transform.position;
				shouldTrack = true;
			}
			if (!isClicking)
				myRenderer.color = HexColor.AdjustHexToColor(hexColor,-40);
			isClicking = true;
	}

	void OnMouseUp() {
			if (allowDraggging)
				shouldTrack = false;
			else if (acceptClick) {
				GameObject.Find("Main Camera").GetComponent<MenuManager>().LoadMenuForGame(gameNumber);
			}
			isClicking = false;
			myRenderer.color = HexColor.HexToColor(hexColor);
	}

	public void ResetSensitiveVars() {
		isAnimating = false;
		acceptClick = false;
		isClicking = false;
		canInteract = true;
	}

	public void SetColor(string hexCode) {
		hexColor = hexCode;
		myRenderer.material.shader = shaderGUItext;
		myRenderer.color = HexColor.HexToColor(hexCode);
	}

	public void SetColor(string hexCode, float alpha) {
		hexColor = hexCode;
		myRenderer.material.shader = shaderGUItext;
		myRenderer.color = HexColor.HexToColorWithAlpha(hexCode, alpha);
	}

	public void SlideToHeight(float height, float speed) {
		canInteract = false;
		isAnimating = true;
		targetHeight = height;
		animateSpeed = speed;
	}

	IEnumerator MakeInteractable() {
		yield return new WaitForSeconds(0.3f);
		canInteract = true;
	}

}

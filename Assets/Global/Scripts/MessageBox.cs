using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour {

	private string _message = "Message";
	public string message {
		set {
			_message = value;
		}
		get {
			return _message;
		}
	}
	
	private string _lbt = "CANCEL";
	public string leftButtonText {
		set {
			_lbt = value;
		}
		get {
			return _lbt;
		}
	}
	
	private string _rbt = "OK";
	public string rightButtonText {
		set {
			_rbt = value;
		}
		get {
			return _rbt;
		}
	}

	private TextMesh messageText;
	private TextMesh rightButton;
	private TextMesh leftButton;

	private GameObject shadow;
	
	private GameObject _target1;
	private string _component1;
	private string _action1;
	private object _parameter1;
	private string _scene1;
	private bool passParams1 = false;
	private bool loadScene1 = false;
	private bool destroy1 = false;
	
	private GameObject _target2;
	private string _component2;
	private string _action2;
	private object _parameter2;
	private string _scene2;
	private bool passParams2 = false;
	private bool loadScene2 = false;
	private bool destroy2 = false;
	
	private Vector3 slerpPos;
	private bool doSlerp = false;
	
	// Use this for initialization
	void Awake () {
		float temp = Camera.main.transform.position.y-(Camera.main.ScreenToWorldPoint(new Vector3(0,Camera.main.pixelHeight*1.5f,0)).y);
		transform.position = new Vector3(Camera.main.transform.position.x,temp,0f);
		rightButton = transform.FindChild("LeftButton").transform.FindChild("Text").GetComponent<TextMesh>() as TextMesh;
		leftButton = transform.FindChild("RightButton").transform.FindChild("Text").GetComponent<TextMesh>() as TextMesh;
		messageText = transform.FindChild("Message").GetComponent<TextMesh>() as TextMesh;
		shadow = transform.FindChild("Shadow").gameObject as GameObject;
		messageText.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Top";
		rightButton.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Top";
		leftButton.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Top";
	}

	void Start() {
		HexColor.SetColorWithAlpha(shadow,"1f1f1f",0.15f);
		SetButtonColors(GameColors.off,GameColors.selected);
		doSlerp = true;
		slerpPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (messageText.text != message)
			messageText.text = message;
		if (rightButton.text != _rbt)
			rightButton.text = _rbt;
		if (leftButton.text != _lbt)
			leftButton.text = _lbt;
	}

	void FixedUpdate() {
		if (doSlerp) {
			if (Vector3.Distance(Camera.main.transform.position,slerpPos) > 0.01)
				transform.position = Vector3.Lerp(transform.position, slerpPos, Time.deltaTime*12f);
			else {
				transform.position = slerpPos;
				doSlerp = false;
			}
		}
	}

	public void SetButtonColors(string leftColor, string rightColor) {
		HexColor.SetColor(rightButton.transform.parent.gameObject,leftColor);
		HexColor.SetColor(leftButton.transform.parent.gameObject,rightColor);
	}

	public void SetColors(string boxColor, string leftColor, string rightColor) {
		HexColor.SetColor(this.gameObject,boxColor);
		HexColor.SetColor(rightButton.transform.parent.gameObject,leftColor);
		HexColor.SetColor(leftButton.transform.parent.gameObject,rightColor);
	}

	public void SetLeftAction(GameObject target, string component, string action, object parameter) {
		_target1 = target;
		_component1 = component;
		_action1 = action;
		_parameter1 = parameter;
		passParams1 = true;
	}
	
	public void SetLeftAction(GameObject target, string component, string action) {
		_target1 = target;
		_component1 = component;
		_action1 = action;
		passParams1 = false;
	}
	
	public void SetLeftAction(string command) {
		if (command == "destroy") {
			destroy1 = true;
		}
	}
	
	public void SetLeftAction(string command, string sceneName) {
		if (command == "loadscene") {
			loadScene1 = true;
			_scene1 = sceneName;
		}
	}
	
	public void InvokeLeftAction() {
		if (destroy1) {
			GameObject.Destroy(gameObject);
			return;
		}
		if (loadScene1) {
			Application.LoadLevel(_scene1);
			return;
		}
		if (passParams1) {
			GameObject.Destroy(gameObject);
			_target1.GetComponent(_component1).SendMessage(_action1, _parameter1);
		}
		else {
			GameObject.Destroy(gameObject);
			_target1.GetComponent(_component1).SendMessage(_action1);
		}
	}
	
	public void SetRightAction(GameObject target, string component, string action, object parameter) {
		_target2 = target;
		_component2 = component;
		_action2 = action;
		_parameter2 = parameter;
		passParams2 = true;
	}
	
	public void SetRightAction(GameObject target, string component, string action) {
		_target2 = target;
		_component2 = component;
		_action2 = action;
		passParams2 = false;
	}
	
	public void SetRightAction(string command) {
		if (command == "destroy") {
			destroy2 = true;
		}
	}
	
	public void SetRightAction(string command, string sceneName) {
		if (command == "loadscene") {
			loadScene2 = true;
			_scene2 = sceneName;
		}
	}
	
	public void InvokeRightAction() {
		if (destroy2) {
			GameObject.Destroy(gameObject);
			return;
		}
		if (loadScene2) {
			Application.LoadLevel(_scene2);
			return;
		}
		if (passParams2) {
			GameObject.Destroy(gameObject);
			_target2.GetComponent(_component2).SendMessage(_action2, _parameter2);
		}
		else {
			GameObject.Destroy(gameObject);
			_target2.GetComponent(_component2).SendMessage(_action2);
		}
	}

}

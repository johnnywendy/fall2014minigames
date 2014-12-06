using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlertBox : MonoBehaviour {

	private string _title = "Title";
	public string title {
		set {
			_title = value;
		}
		get {
			return _title;
		}
	}

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

	private Text titleText;
	private Text messageText;
	private Text rightButton;
	private Text leftButton;

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

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		Vector3 scale = transform.localScale;
		Quaternion rotation = transform.localRotation;
		transform.parent = GameObject.Find ("Canvas").transform;
		transform.position = pos;
		transform.localScale = scale;
		transform.localRotation = rotation;
		rightButton = transform.FindChild("RightButton").transform.FindChild("Text").GetComponent<Text>() as Text;
		leftButton = transform.FindChild("LeftButton").transform.FindChild("Text").GetComponent<Text>() as Text;
		titleText = transform.FindChild("Title").GetComponent<Text>() as Text;
		messageText = transform.FindChild("Message").GetComponent<Text>() as Text;
	}
	
	// Update is called once per frame
	void Update () {
		if (titleText.text != title)
			titleText.text = title;
		if (messageText.text != message)
			messageText.text = message;
		if (rightButton.text != _rbt)
			rightButton.text = _rbt;
		if (leftButton.text != _lbt)
			leftButton.text = _lbt;
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
		if (passParams2)
			_target1.GetComponent(_component1).SendMessage(_action1, _parameter1);
		else
			_target1.GetComponent(_component1).SendMessage(_action1);
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
		if (passParams2)
			_target2.GetComponent(_component2).SendMessage(_action2, _parameter2);
		else
			_target2.GetComponent(_component2).SendMessage(_action2);
	}

}

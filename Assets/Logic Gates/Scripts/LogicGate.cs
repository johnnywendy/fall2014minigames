using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicGate : MonoBehaviour {

	public int mode = 0;
	public bool rotateMode = false;
	public bool preferredEditMode = false;
	public bool disableOnStart = false;

	private string[] labels = new string[] {"AND","OR","NOT","NAND","NOR","XOR","XNOR"};
	private string[] colors = new string[] {"36b1bf","ffcc00","f2385a","55eecc","825dc2","f5a503","4cd963"};
	private TextMesh label;
	private List<LogicGate> pluggedGates = new List<LogicGate>();
	private List<string> pluggedSides = new List<string>();
	private LogicGate rightConnectedGate = null;
	private LogicGate leftConnectedGate = null;
	private PowerSource rightConnectedPower = null;
	private PowerSource leftConnectedPower = null;
	private GoalGate goalGate = null;
	private List<Cable> cables = new List<Cable>();
	private Cable newCable = null;
	private GameObject Input1;
	private GameObject Input2;
	private GameObject Output;
	private GameObject Destroy;
	private GameObject Reset;
	private bool _input1 = false;
	private bool _input2 = false;
	private bool power1 = false;
	private bool power2 = false;
	private bool disableAfterStart = false;
	private bool editMode = false;

	private string hexColor;
	private SpriteRenderer myRenderer;
	private Shader shaderGUItext;
	private Shader shaderSpritesDefault;

	private int _logicMode = 0;
	public int logicMode {
		set {
			_logicMode = value;
			label.text = labels[_logicMode];
			if (_logicMode == 2)
				Input2.SetActive(false);
			else
				Input2.SetActive(true);
			transform.FindChild("Text").GetComponent<TextMesh>().text = labels[_logicMode];
			myRenderer.sprite = Resources.Load<Sprite>(labels[_logicMode].ToLower()+"-gate");
			SetColor(colors[_logicMode]);
			updateOutput();
		}
		get {
			return _logicMode;
		}
	}
	
	public bool power_output {
		get {
			if (power1 | power2) {
				return true;
			}
			else
				return false;
			return false;
			return true;
		}
	}
	
	public bool input1 {
		set {
			_input1 = value;
			updateOutput();
			if (_input1)
				HexColor.SetColor(Input1,GameColors.on);
			else
				HexColor.SetColor(Input1,GameColors.off);
		}
		get {
			return _input1;
		}
	}
	
	public bool input2 {
		set {
			_input2 = value;
			updateOutput();
			if (_input2)
				HexColor.SetColor(Input2,GameColors.on);
			else
				HexColor.SetColor(Input2,GameColors.off);
		}
		get {
			return _input2;
		}
	}
	
	public bool output {
		get {
			switch (this.logicMode) {
			case 0: // AND
				return input1 & input2;
			case 1: // OR
				return input1 | input2;
			case 2: // NOT
				return !input1;
			case 3: // NAND
				return !(input1 & input2);
			case 4: // NOR
				return !(input1 | input2);
			case 5: // XOR
				return input1 ^ input2;
			case 6: // XNOR
				return !(input1 ^ input2);
			default:
				return false;
			}
		}
	}
	private Color defaultColor;

	public void shouldDisable(bool val) {
		disableAfterStart = val;
	}

	void Awake() {
		myRenderer = gameObject.GetComponent<SpriteRenderer>();
		shaderGUItext = Shader.Find("GUI/Text Shader");
		shaderSpritesDefault = Shader.Find("Sprites/Default");
	}

	// Use this for initialization
	void Start () {
		Input1 = transform.FindChild("Input1").gameObject;
		Input2 = transform.FindChild("Input2").gameObject;
		Output = transform.FindChild("Output").gameObject;
		Destroy = transform.FindChild("Destroy").gameObject;
		Reset = transform.FindChild("Reset").gameObject;
		label = transform.FindChild ("Text").GetComponent<TextMesh> ();

		HexColor.SetColor(Input1,GameColors.inactive);
		HexColor.SetColor(Input2,GameColors.inactive);

		if (mode != -1)
			logicMode = mode;

		updateOutput();
		SetEditMode(preferredEditMode);

		if (disableAfterStart) {
			this.enabled = false;
		}

		if (disableOnStart) {
			gameObject.tag = "Untagged";
			Input1.tag = "Untagged";
			Input2.tag = "Untagged";
			Output.tag = "Untagged";
			gameObject.name = "ButtonIcon";
			Input1.name = "ButtonIcon";
			Input2.name = "ButtonIcon";
			Output.name = "ButtonIcon";
			this.enabled = false;
		}
	}
	
	void Update () {
		if (rotateMode == true) {
			if (logicMode < 5)
				logicMode++;
			else
				logicMode = 0;
			rotateMode = false;
		}
	}

	public void SetColor(string hexCode) {
		hexColor = hexCode;
		myRenderer.material.shader = shaderGUItext;
		myRenderer.color = HexColor.HexToColor(hexCode);
	}

	public void SetEditMode(bool val) {
		editMode = val;
		editMode = val;
		Destroy.SetActive(val);
		Reset.SetActive(val);
	}

	public void NeedNewCable() {
		GameObject tempCable = (GameObject)Instantiate(Resources.Load("Cable", typeof(GameObject)),Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity);
		newCable = tempCable.GetComponent<Cable>();
		newCable.transform.parent = transform;
		newCable.SetupNewCable(this);
	}
	
	public void DestroyNewCable() {
		GameObject.Destroy(newCable.gameObject);
		newCable = null;
	}

	public void WasPickedUp() {
		transform.localScale = new Vector3 (1f, 1f, 1f);
		CablesShouldFollowTargets(true);
		if (rightConnectedGate != null)
			rightConnectedGate.CablesShouldFollowTargets(true);
		if (leftConnectedGate != null)
			leftConnectedGate.CablesShouldFollowTargets(true);
		if (rightConnectedPower != null)
			rightConnectedPower.CablesShouldFollowTargets(true);
		if (leftConnectedPower != null)
			leftConnectedPower.CablesShouldFollowTargets(true);
	}

	public void WasPutDown() {
		transform.localScale = new Vector3 (0.8f, 0.8f, 1f);
		CablesShouldFollowTargets(false);
		if (rightConnectedGate != null)
			rightConnectedGate.CablesShouldFollowTargets(false);
		if (leftConnectedGate != null)
			leftConnectedGate.CablesShouldFollowTargets(false);
		if (rightConnectedPower != null)
			rightConnectedPower.CablesShouldFollowTargets(false);
		if (leftConnectedPower != null)
			leftConnectedPower.CablesShouldFollowTargets(false);
	}

	public void CablesShouldFollowTargets(bool value) {
		foreach (Cable cable in cables) {
			cable.cableShouldFollowTargets = value;
			cable.shouldAnimate = !value;
			cable.shouldReset = true;
		}
	}

	void updateOutput() {
		foreach (Cable cable in cables) {
			cable.power = power_output;
			cable.signal = output;
		}
		if (power_output) {
			if (output) {
				Debug.Log(">> True!");
				HexColor.SetColor(Output,GameColors.on);
			} 
			else {
				Debug.Log(">> False.");
				HexColor.SetColor(Output,GameColors.off);
			}
		}
		else {
			HexColor.SetColor(Output,GameColors.inactive);
		}
		if (pluggedGates.Count > 0) {
			for (int i = 0; i < pluggedGates.Count; i++) {
				if (pluggedSides[i] == "left") {
					pluggedGates[i].power1 = power_output;
					pluggedGates[i].input1 = output;
				}
				if (pluggedSides[i] == "right") {
					pluggedGates[i].power2 = power_output;
					pluggedGates[i].input2 = output;
				}
			}
		}
		if (goalGate != null) {
			goalGate.input = output;
		}
	}

	public void HookUpPowerSource(PowerSource source, string side) {
		if (side == "left") {
			power1 = source.power;
			input1 = source.output;
			leftConnectedPower = source;
		} else if (side == "right") {
			power2 = source.power;
			input2 = source.output;
			rightConnectedPower = source;
		}
		updateOutput();
	}

	public void PlugIntoGate(LogicGate otherGate, string side) {
		pluggedGates.Add(otherGate);
		if (side == "left") {
			pluggedSides.Add("left");
			otherGate.power1 = power_output;
			otherGate.input1 = output;
			otherGate.leftConnectedGate = this;
		}
		else if (side == "right") {
			pluggedSides.Add("right");
			otherGate.power2 = power_output;
			otherGate.input2 = output;
			otherGate.rightConnectedGate = this;
		}
		newCable.Connect(this,otherGate,side);
		newCable.shouldAnimate = true;
		cables.Add(newCable);
		newCable = null;
	}

	public void PlugIntoGoal(GoalGate goal) {
		goalGate = goal;
		goalGate.plugged = true;
		goalGate.input = output;
		newCable.Connect(this,goal);
		newCable.shouldAnimate = true;
		cables.Add(newCable);
		newCable = null;
	}

	public void UnplugFromGate(LogicGate gate) {
		pluggedGates.RemoveAt(PrepUnplugFromGate(gate));
	}

	public int PrepUnplugFromGate(LogicGate gate) {
		int index = pluggedGates.IndexOf(gate);
		if (pluggedSides[index] == "right") {
			pluggedGates[index].power2 = false;
			pluggedGates[index].input2 = false;
			pluggedGates[index].rightConnectedGate = null;
			HexColor.SetColor(pluggedGates[index].Input2,GameColors.inactive);
		}
		if (pluggedSides[index] == "left") {
			pluggedGates[index].power1 = false;
			pluggedGates[index].input1 = false;
			pluggedGates[index].leftConnectedGate = null;
			HexColor.SetColor(pluggedGates[index].Input1,GameColors.inactive);
		}
		pluggedSides.RemoveAt(index);
		List<Cable> shouldRemove = new List<Cable>();
		foreach (Cable cable in cables) {
			if (cable.GateB == gate && cable.GateA == this) {
				shouldRemove.Add(cable);
			}
			else if (cable.GoalB == gate && cable.GateA == this) {
				shouldRemove.Add(cable);
			}
		}
		foreach (Cable cable in shouldRemove) {
			cables.Remove(cable);
			GameObject.Destroy(cable.gameObject);
		}
		return index;
	}

	public void powerReset(string side) {
		if (side == "left") {
			power1 = false;
			input1 = false;
		}
		if (side == "right") {
			power2 = false;
			input2 = false;
		}
	}

	public void resetConnections() {
		if (rightConnectedPower != null)
			rightConnectedPower.UnplugFromGate(this);
		if (leftConnectedPower != null)
			leftConnectedPower.UnplugFromGate(this);
		if (goalGate != null)
			goalGate.resetConnection();
		List<int> shouldRemoveGates = new List<int>();
		foreach (LogicGate gate in pluggedGates)
			shouldRemoveGates.Add(PrepUnplugFromGate(gate));
		foreach (int index in shouldRemoveGates)
			pluggedGates.RemoveAt(index);
		foreach (Cable acable in cables)
			GameObject.Destroy(acable.gameObject);
		cables = new List<Cable>();
		power1 = false;
		power2 = false;
		input1 = false;
		input2 = false;
		HexColor.SetColor(Input1,GameColors.inactive);
		HexColor.SetColor(Input2,GameColors.inactive);
		for (int i = 0; i < pluggedGates.Count; i++) {
			if (pluggedSides[i] == "left") {
				pluggedGates[i].power1 = false;
				pluggedGates[i].input1 = output;
				pluggedGates[i].leftConnectedGate = null;
			}
			if (pluggedSides[i] == "right") {
				pluggedGates[i].power2 = false;
				pluggedGates[i].input2 = output;
				pluggedGates[i].rightConnectedGate = null;
			}
		}
		if (rightConnectedGate != null) {
			rightConnectedGate.UnplugFromGate(this);
			rightConnectedGate = null;
		}
		if (leftConnectedGate != null) {
			leftConnectedGate.UnplugFromGate(this);
			leftConnectedGate = null;
		}
	}

	public Vector3 GetInputPos(string side) {
		if (side == "left")
			return new Vector3 (Input1.transform.position.x+0.075f, Input1.transform.position.y-0.01f, Input1.transform.position.z + 10);
		if (side == "right")
			return new Vector3 (Input2.transform.position.x+0.075f, Input2.transform.position.y-0.01f, Input2.transform.position.z + 10);
		return Vector3.zero;
	}

	public Vector3 GetOutputPos() {
		return new Vector3 (Output.transform.position.x+0.05f, Output.transform.position.y-0.02f, Output.transform.position.z + 10);
	}
}

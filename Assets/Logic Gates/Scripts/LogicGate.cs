using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicGate : MonoBehaviour {

	public int mode = 0;
	public bool rotateMode = false;
	public float physicalRotateAmt = -90;
	public bool preferredEditMode = false;
	public bool disableOnStart = false;

	private string[] labels = new string[] {"AND","OR","NOT","NAND","NOR","XOR","XNOR"};
	private string[] colors = new string[] {"e07793","c477e0","778ce0","77d2e0","77e093","e0ac77","d0bd45"};
	private TextMesh label;
	private LogicGate pluggedGate = null;
	private string pluggedSide = "none";
	private LogicGate rightConnectedGate = null;
	private LogicGate leftConnectedGate = null;
	private PowerSource rightConnectedPower = null;
	private PowerSource leftConnectedPower = null;
	private GoalGate goalGate = null;
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
	private bool editMode = true;
	private bool cableShouldFollowMouse = false;
	private LineRenderer cable;
	private GameObject sparks = null;
	private bool shouldRenderSparks = true;
	private bool shouldResetSparks = true;
	public bool cableShouldFollowTarget = false;
	private LogicGate target;

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
			transform.renderer.material.color = HexToColor(colors[_logicMode]);
			updateOutput();
		}
		get {
			return _logicMode;
		}
	}
	
	private bool power_output {
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
		}
		get {
			return _input1;
		}
	}
	
	public bool input2 {
		set {
			_input2 = value;
			updateOutput();
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

	// Use this for initialization
	void Start () {
		Input1 = transform.FindChild("Input1").gameObject;
		Input2 = transform.FindChild("Input2").gameObject;
		Output = transform.FindChild("Output").gameObject;
		Destroy = transform.FindChild("Destroy").gameObject;
		Reset = transform.FindChild("Reset").gameObject;
		sparks = transform.FindChild ("Sparks").gameObject;
		label = transform.FindChild ("Text").GetComponent<TextMesh> ();
		cable = GetComponent<LineRenderer> ();

		Input1.transform.renderer.material.color = HexToColor ("1dc6e0");
		Input2.transform.renderer.material.color = HexToColor ("5b1de0");

		transform.rotation = Quaternion.Euler (0, 0, physicalRotateAmt);

		cable.SetVertexCount(2);
		cable.SetColors(HexToColor("424242"),HexToColor("424242"));
		cable.material.color = HexToColor ("424242");
		cable.SetWidth (0.04f, 0.04f);
		cable.SetPosition (0, new Vector3 (Output.transform.position.x,Output.transform.position.y,Output.transform.position.z+1));
		cable.SetPosition (1, Vector3.zero);
		cable.enabled = false;
		sparks.particleSystem.enableEmission = false;

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
		if (cableShouldFollowMouse) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			cable.SetPosition (1, new Vector3 (mousePos.x,mousePos.y,Output.transform.position.z-1));

			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out hit)) {
				if (hit.collider.transform.gameObject.name == "Input1" || hit.collider.transform.gameObject.name == "Input2") {
					cable.material.color = Color.green;
				}
			}
			else {
				cable.material.color = Color.red;
			}
		}
		if (cableShouldFollowTarget) {
			if (pluggedSide == "left")
				cable.SetPosition(1,pluggedGate.getInputPos(pluggedSide));
			if (pluggedSide == "right")
				cable.SetPosition(1,pluggedGate.getInputPos(pluggedSide));
			cable.SetPosition (0, new Vector3 (Output.transform.position.x,Output.transform.position.y,Output.transform.position.z+1));
		}
		if (shouldRenderSparks && power_output && (pluggedGate != null || goalGate != null)) {
			if (shouldResetSparks) {
				sparks.transform.position = new Vector3 (Output.transform.position.x,Output.transform.position.y,Output.transform.position.z+1);
				shouldResetSparks = false;
			}
			if (sparks != null)
				sparks.particleSystem.enableEmission = true;
			if (pluggedGate != null) {
				if (Vector3.Distance(sparks.transform.position,pluggedGate.getInputPos(pluggedSide)) > 0.14f)
					sparks.transform.position = Vector3.Lerp(sparks.transform.position,pluggedGate.getInputPos(pluggedSide),Time.deltaTime);
				else
					shouldRenderSparks = false;
			}
			if (goalGate != null) {
				if (Vector3.Distance(sparks.transform.position,goalGate.getInputPos()) > 0.14f)
					sparks.transform.position = Vector3.Lerp(sparks.transform.position,goalGate.getInputPos(),Time.deltaTime);
				else
					shouldRenderSparks = false;
			}
		}
		else if (sparks != null && sparks.particleSystem.enableEmission) {
			shouldResetSparks = true;
			sparks.particleSystem.enableEmission = false;
		}
	}

	public void RenderSparks(bool val) {
		shouldRenderSparks = val;
		if (rightConnectedGate != null)
			rightConnectedGate.shouldRenderSparks = val;
		if (leftConnectedGate != null)
			leftConnectedGate.shouldRenderSparks = val;
		if (leftConnectedPower != null)
			leftConnectedPower.shouldRenderSparks = val;
		if (rightConnectedPower != null)
			rightConnectedPower.shouldRenderSparks = val;
	}

	IEnumerator PauseSparks(float delay) {
		yield return new WaitForSeconds(delay);
		shouldRenderSparks = true;
	}

	public void SetEditMode(bool val) {
		editMode = val;
		editMode = val;
		Destroy.SetActive(val);
		Reset.SetActive(val);
	}

	public void WasPickedUp() {
		transform.localScale = new Vector3 (1.2f, 1.2f, 1f);
		cableShouldFollowTarget = true;
		if (rightConnectedGate != null)
			rightConnectedGate.cableShouldFollowTarget = true;
		if (leftConnectedGate != null)
			leftConnectedGate.cableShouldFollowTarget = true;
		if (rightConnectedPower != null)
			rightConnectedPower.cableShouldFollowTarget = true;
		if (leftConnectedPower != null)
			leftConnectedPower.cableShouldFollowTarget = true;
	}

	public void WasPutDown() {
		transform.localScale = new Vector3 (1, 1, 1f);
		cableShouldFollowTarget = false;
		if (rightConnectedGate != null)
			rightConnectedGate.cableShouldFollowTarget = false;
		if (leftConnectedGate != null)
			leftConnectedGate.cableShouldFollowTarget = false;
		if (rightConnectedPower != null)
			rightConnectedPower.cableShouldFollowTarget = false;
		if (leftConnectedPower != null)
			leftConnectedPower.cableShouldFollowTarget = false;
	}

	void updateOutput() {
		if (power_output) {
			if (output) {
				Debug.Log(">> True!");
				Output.transform.renderer.material.color = HexToColor("1a991c");
				cable.material.color = Color.cyan;
			} 
			else {
				Debug.Log(">> False.");
				Output.transform.renderer.material.color = HexToColor("f00c0c");
				cable.material.color = Color.magenta;
			}
		}
		else {
			Output.transform.renderer.material.color = HexToColor("9d9d9d");
			cable.material.color = HexToColor ("424242");
		}
		if (pluggedGate != null) {
			if (pluggedSide == "left") {
				pluggedGate.power1 = power_output;
				pluggedGate.input1 = output;
			}
			if (pluggedSide == "right") {
				pluggedGate.power2 = power_output;
				pluggedGate.input2 = output;
			}
		}
	}

	public void hookUpPowerSource(PowerSource source, string side) {
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

	public void plugIntoGate(LogicGate otherGate, string side) {
		plugCableInGate (otherGate, side);
		pluggedGate = otherGate;
		if (side == "left") {
			pluggedSide = "left";
			otherGate.power1 = power_output;
			otherGate.input1 = output;
			otherGate.leftConnectedGate = this;
		}
		else if (side == "right") {
			pluggedSide = "right";
			otherGate.power2 = power_output;
			otherGate.input2 = output;
			otherGate.rightConnectedGate = this;
		}
	}

	public void plugIntoGoal(GoalGate goal) {
		goalGate = goal;
		goalGate.plugged = true;
		cableShouldFollowMouse = false;
		cable.material.color = HexToColor ("424242");
		cable.SetPosition(1,goalGate.getInputPos());
		goalGate.input = output;
	}
	
	public void unplugFromGate() {
		resetCable ();
		if (pluggedSide == "right") {
			pluggedGate.power2 = false;
			pluggedGate.input2 = false;
			pluggedGate.rightConnectedGate = null;
		}
		if (pluggedSide == "left") {
			pluggedGate.power1 = false;
			pluggedGate.input1 = false;
			pluggedGate.leftConnectedGate = null;
		}
		pluggedGate = null;
		pluggedSide = "none";
		cableShouldFollowMouse = false;
		cable.enabled = false;
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
		resetCable();
		unplugFromGate ();
		if (rightConnectedPower != null)
			rightConnectedPower.unplugFromGate();
		if (leftConnectedPower != null)
			leftConnectedPower.unplugFromGate();
		if (goalGate != null)
			goalGate.resetConnection();
		power1 = false;
		power2 = false;
		input1 = false;
		input2 = false;
		if (pluggedSide == "left") {
			pluggedGate.power1 = false;
			pluggedGate.input1 = output;
			pluggedGate.leftConnectedGate = null;
		}
		if (pluggedSide == "right") {
			pluggedGate.power2 = false;
			pluggedGate.input2 = output;
			pluggedGate.rightConnectedGate = null;
		}
		if (rightConnectedGate != null) {
			rightConnectedGate.unplugFromGate();
			rightConnectedGate = null;
		}
		if (leftConnectedGate != null) {
			leftConnectedGate.unplugFromGate();
			leftConnectedGate = null;
		}
	}

	public void enableCable() {
		cableShouldFollowMouse = true;
		cable.SetPosition (0, new Vector3 (Output.transform.position.x,Output.transform.position.y,Output.transform.position.z+1));
		cable.SetPosition (1, Vector3.zero);
		cable.enabled = true;
	}

	public void plugCableInGate(LogicGate otherGate, string side) {
		cableShouldFollowMouse = false;
		cable.material.color = HexToColor ("424242");
		if (side == "left")
			cable.SetPosition(1,otherGate.getInputPos(side));
		if (side == "right")
			cable.SetPosition(1,otherGate.getInputPos(side));
	}

	public void resetCable() {
		cableShouldFollowMouse = false;
		cable.enabled = false;
	}

	public Vector3 getInputPos(string side) {
		if (side == "left")
			return new Vector3 (Input1.transform.position.x, Input1.transform.position.y-0.02f, Input1.transform.position.z + 1);
		if (side == "right")
			return new Vector3 (Input2.transform.position.x, Input2.transform.position.y-0.02f, Input2.transform.position.z + 1);
		return Vector3.zero;
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

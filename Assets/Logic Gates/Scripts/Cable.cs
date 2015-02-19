using UnityEngine;
using System.Collections;

public class Cable : MonoBehaviour {

	public PowerSource PowerA;
	public GameObject sparks = null;
	public LogicGate GateA;
	public LogicGate GateB;
	public GoalGate GoalB;
	public string side = null;
	private bool _visible = false;
	private bool connectionEstablished = false;
	private string connectionType = "";
	public bool power = false;
	private bool _signal = false;
	public bool signal {
		set {
			_signal = value;
			if (connectionEstablished) {
				shouldAnimate = true;
				shouldReset = true;
			}
			if (power && value)
				cable.material.color = HexColor.HexToColor(GameColors.on2);
			if (power && !value)
				cable.material.color = HexColor.HexToColor(GameColors.off2);
			if (!power) {
				shouldAnimate = false;
				cable.SetColors(HexColor.HexToColor(GameColors.inactive),HexColor.HexToColor(GameColors.inactive));
			}
		}
		get {
			return _signal;
		}
	}
	public bool sparksVisible {
		set {
			_visible = value;
			particleSystem.enableEmission = value;
			shouldAnimate = value;
		}
		get {
			return _visible;
		}
	}
	public bool shouldAnimate = false;
	public bool shouldReset = false;

	private LineRenderer cable;
	private bool cableShouldFollowMouse = false;
	public bool cableShouldFollowTargets = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (shouldReset) {
			if (connectionType == "powertogate") {
				cable.SetPosition (0, PowerA.GetOutputPos());
				sparks.transform.position = PowerA.GetOutputPos();
			}
			if (connectionType == "gatetogate") {
				cable.SetPosition (0, GateA.GetOutputPos());
				sparks.transform.position = GateA.GetOutputPos();
			}
			if (connectionType == "gatetogoal") {
				cable.SetPosition (0, GateA.GetOutputPos());
				sparks.transform.position = GateA.GetOutputPos();
			}
			shouldReset = false;
		}
		if (shouldAnimate) {
			sparks.particleSystem.enableEmission = true;
			if (connectionType == "powertogate") {
				if (Vector3.Distance(sparks.transform.position,GateB.GetInputPos(side)) > 0.14f)
					sparks.transform.position = Vector3.Lerp(sparks.transform.position,GateB.GetInputPos(side),Time.deltaTime);
				else
					shouldReset = true;
			}
			if (connectionType == "gatetogate") {
				if (Vector3.Distance(sparks.transform.position,GateB.GetInputPos(side)) > 0.14f)
					sparks.transform.position = Vector3.Lerp(sparks.transform.position,GateB.GetInputPos(side),Time.deltaTime);
				else
					shouldReset = true;
			}
			if (connectionType == "gatetogoal") {
				if (Vector3.Distance(sparks.transform.position,GoalB.GetInputPos()) > 0.14f)
					sparks.transform.position = Vector3.Lerp(sparks.transform.position,GoalB.GetInputPos(),Time.deltaTime);
				else
					shouldReset = true;
			}
		}
		else if (sparks != null) {
			sparks.particleSystem.enableEmission = false;
		}
		if (cableShouldFollowMouse) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (connectionType == "powertogate")
				cable.SetPosition (1, new Vector3 (mousePos.x,mousePos.y,PowerA.transform.position.z-1));
			else
				cable.SetPosition (1, new Vector3 (mousePos.x,mousePos.y,GateA.transform.position.z-1));
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
		if (cableShouldFollowTargets) {
			if (connectionType == "powertogate") {
				cable.SetPosition (0, PowerA.GetOutputPos());
				cable.SetPosition (1, GateB.GetInputPos(side));
			}
			if (connectionType == "gatetogate") {
				cable.SetPosition (0, GateA.GetOutputPos());
				cable.SetPosition (1, GateB.GetInputPos(side));
			}
			if (connectionType == "gatetogoal") {
				cable.SetPosition (0, GateA.GetOutputPos());
				cable.SetPosition (1, GoalB.GetInputPos());
			}
		}
	}

	public void SetupNewCable(PowerSource targetA) {
		sparks = transform.FindChild("Sparks").gameObject;
		cable = GetComponent<LineRenderer>();
		cable.SetVertexCount(2);
		cable.SetPosition (0, targetA.GetOutputPos());
		cable.SetPosition (1, Input.mousePosition);
		cable.SetColors(HexColor.HexToColor("FFFFFF"),HexColor.HexToColor("FFFFFF"));
		cable.SetWidth (0.04f, 0.04f);
		cable.material.color = HexColor.HexToColor(GameColors.on2);
		shouldReset = true;
		PowerA = targetA;
		cableShouldFollowMouse = true;
		cable.enabled = true;
		connectionType = "powertogate";
	}

	public void SetupNewCable(LogicGate targetA) {
		sparks = transform.FindChild("Sparks").gameObject;
		cable = GetComponent<LineRenderer>();
		cable.SetVertexCount(2);
		cable.SetPosition (0, targetA.GetOutputPos());
		cable.SetPosition (1, Input.mousePosition);
		cable.SetColors(HexColor.HexToColor("FFFFFF"),HexColor.HexToColor("FFFFFF"));
		cable.SetWidth (0.04f, 0.04f);
		cable.material.color = HexColor.HexToColor(GameColors.on2);
		GateA = targetA;
		cableShouldFollowMouse = true;
		cable.enabled = true;
		connectionType = "gatetogate";
	}

	public void Connect(PowerSource targetA, LogicGate targetB, string newSide) {
		connectionEstablished = true;
		cableShouldFollowMouse = false;
		PowerA = targetA;
		GateB = targetB;
		side = newSide;
		shouldReset = true;
		shouldAnimate = true;
		connectionType = "powertogate";
		transform.position = targetA.GetOutputPos();
		cable.SetPosition (0, targetA.GetOutputPos());
		cable.SetPosition (1, targetB.GetInputPos(newSide));
		power = targetA.power;
		signal = targetA.output;
	}

	public void Connect(LogicGate targetA, LogicGate targetB, string newSide) {
		connectionEstablished = true;
		cableShouldFollowMouse = false;
		GateA = targetA;
		GateB = targetB;
		side = newSide;
		shouldReset = true;
		shouldAnimate = true;
		connectionType = "gatetogate";
		transform.position = targetA.GetOutputPos();
		cable.SetPosition (0, targetA.GetOutputPos());
		cable.SetPosition (1, targetB.GetInputPos(newSide));
		power = targetA.power_output;
		signal = targetA.output;
	}

	public void Connect(LogicGate targetA, GoalGate targetB) {
		connectionEstablished = true;
		cableShouldFollowMouse = false;
		GateA = targetA;
		GoalB = targetB;
		shouldReset = true;
		shouldAnimate = true;
		connectionType = "gatetogoal";
		transform.position = targetA.GetOutputPos();
		cable.SetPosition (0, targetA.GetOutputPos());
		cable.SetPosition (1, targetB.GetInputPos());
		power = targetA.power_output;
		signal = targetA.output;
	}
}

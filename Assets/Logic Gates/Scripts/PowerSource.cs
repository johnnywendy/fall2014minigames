using UnityEngine;
using System.Collections;

public class PowerSource : MonoBehaviour {

	public bool power = true;
	public bool pluggedIn = false;
	public LogicGate pluggedGate = null;
	public bool flipOutput = false;

	private GameObject sparks = null;
	private bool shouldResetSparks = true;
	private bool _shouldRenderSparks = true;
	public bool shouldRenderSparks {
		set {
			_shouldRenderSparks = value;
		}
		get {
			return _shouldRenderSparks;
		}
	}

	private GameObject Output;
	public bool cableShouldFollowMouse = false;
	private LineRenderer cable;
	private string side = "none";
	public bool cableShouldFollowTarget = false;
	private LogicGate target;
	private bool _output = true;
	public bool output {
		set {
			_output = value;
			updatePluggedGates();
		}
		get {
			return _output; 
		}
	}

	void Start() {
		Output = transform.FindChild("Output").gameObject;
		cable = GetComponent<LineRenderer> ();
		cable.SetVertexCount(2);
		cable.SetColors(HexToColor("424242"),HexToColor("424242"));
		cable.SetWidth (0.04f, 0.04f);
		cable.SetPosition (0, new Vector3 (Output.transform.position.x,Output.transform.position.y,Output.transform.position.z+1));
		cable.SetPosition (1, Vector3.zero);
		cable.enabled = false;
		cable.material.color = Color.cyan;

		sparks = transform.FindChild ("Sparks").gameObject;
		sparks.particleSystem.enableEmission = false;
	}

	void Update() {
		if (flipOutput) {
			if (output)
				cable.material.color = Color.cyan;
			else
				cable.material.color = Color.magenta;
			output = !output;
			flipOutput = false;
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
			if (side == "left")
				cable.SetPosition(1,target.getInputPos(side));
			if (side == "right")
				cable.SetPosition(1,target.getInputPos(side));
		}
		if (shouldRenderSparks && pluggedGate != null) {
			if (shouldResetSparks) {
				sparks.transform.position = new Vector3 (Output.transform.position.x,Output.transform.position.y,Output.transform.position.z+1);
				shouldResetSparks = false;
			}
			if (sparks != null)
				sparks.particleSystem.enableEmission = true;
			if (pluggedGate != null) {
				if (Vector3.Distance(sparks.transform.position,pluggedGate.getInputPos(side)) > 0.14f)
					sparks.transform.position = Vector3.Lerp(sparks.transform.position,pluggedGate.getInputPos(side),Time.deltaTime);
				else
					sparks.transform.position = new Vector3 (Output.transform.position.x,Output.transform.position.y,Output.transform.position.z+1);
			}
		}
		else if (sparks != null && sparks.particleSystem.enableEmission) {
			shouldResetSparks = true;
			sparks.particleSystem.enableEmission = false;
		}
	}

	void updatePluggedGates() {
		if (side == "left")
			pluggedGate.input1 = output;
		if (side == "right")
			pluggedGate.input2 = output;
	}
	
	public void plugIntoGate(LogicGate gate, string newSide) {
		enableCable ();
		plugCableInGate(gate, newSide);
		pluggedGate = gate;
		side = newSide;
	}
	
	public void unplugFromGate() {
		resetCable();
		pluggedGate = null;
		side = "none";
	}

	public void forceUnplugFromGate() {
		if (pluggedGate != null) {
			pluggedGate.powerReset(side);
		}
		pluggedGate = null;
		side = "none";
		resetCable();
	}

	public void enableCable() {
		cableShouldFollowMouse = true;
		cable.SetPosition (1, Vector3.zero);
		cable.enabled = true;
	}
	
	public void plugCableInGate(LogicGate otherGate, string side) {
		cableShouldFollowMouse = false;
		if (output)
			cable.material.color = Color.cyan;
		else
			cable.material.color = Color.magenta;
		target = otherGate;
		if (side == "left")
			cable.SetPosition(1,otherGate.getInputPos(side));
		if (side == "right")
			cable.SetPosition(1,otherGate.getInputPos(side));
	}
	
	public void resetCable() {
		cableShouldFollowMouse = false;
		cable.enabled = false;
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

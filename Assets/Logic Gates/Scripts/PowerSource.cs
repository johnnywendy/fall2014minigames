using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerSource : MonoBehaviour {

	public bool power = true;
	public List<LogicGate> pluggedGates = new List<LogicGate>();
	public List<string> pluggedSides = new List<string>();
	private List<Cable> cables = new List<Cable>();
	private Cable newCable = null;

	private GameObject Output;
	private bool _output = true;
	public bool output {
		set {
			_output = value;
			UpdatePluggedGates();
		}
		get {
			return _output; 
		}
	}

	void Start() {
		Output = transform.FindChild("Output").gameObject;
	}

	void Update() {

	}

	public void CablesShouldFollowTargets(bool value) {
		foreach (Cable cable in cables) {
			cable.cableShouldFollowTargets = value;
			cable.shouldAnimate = !value;
			cable.shouldReset = true;
		}
	}

	void UpdatePluggedGates() {
		for (int i = 0; i < cables.Count; i++) {
			if (pluggedSides[i] == "left")
				pluggedGates[i].input1 = output;
			if (pluggedSides[i] == "right")
				pluggedGates[i].input2 = output;
		}
	}

	public void FlipOutput() {
		output = !output;
		foreach (Cable cable in cables)
			cable.signal = output;
	}

	public void SetOutput(bool value) {
		output = value;
		foreach (Cable cable in cables)
			cable.signal = output;
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

	public void PlugIntoGate(LogicGate gate, string newSide) {
		pluggedGates.Add(gate);
		pluggedSides.Add(newSide);
		newCable.Connect(this,gate,newSide);
		newCable.shouldAnimate = true;
		cables.Add(newCable);
		newCable = null;
	}
	
	public void UnplugFromGate(LogicGate gate) {
		int index = pluggedGates.IndexOf(gate);
		pluggedGates.RemoveAt(index);
		pluggedSides.RemoveAt(index);
		List<Cable> shouldRemove = new List<Cable>();
		foreach (Cable cable in cables) {
			if (cable.GateB == gate) {
				shouldRemove.Add(cable);
			}
		}
		foreach (Cable cable in shouldRemove) {
			cables.Remove(cable);
			GameObject.Destroy(cable.gameObject);
		}
		shouldRemove = null;
	}

	public void ForceUnplugFromGate(LogicGate gate) {
		int index = pluggedGates.IndexOf(gate);
		pluggedGates.RemoveAt(index);
		pluggedSides.RemoveAt(index);
		foreach (Cable cable in cables) {
			if (cable.GateB == gate) {
				cables.Remove(cable);
				GameObject.Destroy(cable);
			}
		}
	}

	public void RemoveCable(LogicGate gate) {
		foreach (Cable cable in cables) {
			if (cable.GateB == gate) {
				cables.Remove(cable);
				GameObject.Destroy(cable);
			}
		}
	}

	public Vector3 GetOutputPos() {
		return new Vector3 (Output.transform.position.x, Output.transform.position.y-0.02f, Output.transform.position.z + 1);
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

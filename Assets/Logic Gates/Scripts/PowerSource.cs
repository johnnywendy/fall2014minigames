﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerSource : MonoBehaviour {

	public bool power = true;
	public List<LogicGate> pluggedGates = new List<LogicGate>();
	public List<string> pluggedSides = new List<string>();
	public List<Cable> cables = new List<Cable>();
	private Cable newCable = null;

	private GameObject Output;
	private bool _output = true;
	public bool output {
		set {
			_output = value;
			UpdatePluggedGates();
			if (_output) {
				HexColor.SetColor(gameObject,GameColors.on);
				HexColor.SetColor(Output,GameColors.on2);
			}
			else {
				HexColor.SetColor(gameObject,GameColors.off);
				HexColor.SetColor(Output,GameColors.off2);
			}
		}
		get {
			return _output; 
		}
	}

	void Start() {
		Output = transform.FindChild("Output").gameObject;
		output = true;
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
		if (index > -1) {
			pluggedGates.RemoveAt(index);
			pluggedSides.RemoveAt(index);
		}
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
		return new Vector3 (Output.transform.position.x+0.1f, Output.transform.position.y-0.02f, Output.transform.position.z + 10);
	}

}

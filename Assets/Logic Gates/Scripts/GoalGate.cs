using UnityEngine;
using System.Collections;

public class GoalGate : MonoBehaviour {

	private GameObject Input1;
	public bool input = false;
	public bool plugged = false;

	void Start() {
		Input1 = transform.FindChild("Input1").gameObject;
	}

	public void resetConnection() {
		input = false;
		plugged = false;
	}

	public Vector3 GetInputPos() {
		return new Vector3 (Input1.transform.position.x, Input1.transform.position.y-0.02f, Input1.transform.position.z+2);
	}

}

using UnityEngine;
using System.Collections;

public class GoalGate : MonoBehaviour {

	private GameObject Input1;
	private bool _input = false;
	public bool plugged = false;
	public bool input {
		set {
			_input = value;
			checkWinCondition();
		}
	}

	void Start() {
		Input1 = transform.FindChild("Input1").gameObject;
	}

	public void resetConnection() {
		input = false;
		plugged = false;
	}

	public Vector3 getInputPos() {
		return new Vector3 (Input1.transform.position.x, Input1.transform.position.y-0.02f, Input1.transform.position.z+2);
	}

	void checkWinCondition() {

	}

}

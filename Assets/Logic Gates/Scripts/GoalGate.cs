using UnityEngine;
using System.Collections;

public class GoalGate : MonoBehaviour {

	private GameObject Input1;
	private Shader shaderGUItext;
	private bool _input = false;
	public bool input {
		set {
			_input = value;
			if (plugged) {
				if (_input) {
					SetColor(gameObject,GameColors.on);
					SetColor(Input1,GameColors.on2);
				}
				else {
					SetColor(gameObject,GameColors.off);
					SetColor(Input1,GameColors.off2);
				}
			}
			else {
				SetColor(gameObject,GameColors.inactive);
				SetColor(Input1,GameColors.inactive2);
			}
		}
		get {
			return _input;
		}
	}
	public bool plugged = false;

	void Start() {
		Input1 = transform.FindChild("Input1").gameObject;
		shaderGUItext = Shader.Find("GUI/Text Shader");
		input = false;
	}

	public void SetColor(GameObject obj, string hexCode) {
		obj.GetComponent<SpriteRenderer>().material.shader = shaderGUItext;
		obj.GetComponent<SpriteRenderer>().color = HexColor.HexToColor(hexCode);
	}

	public void resetConnection() {
		input = false;
		plugged = false;
	}

	public Vector3 GetInputPos() {
		return new Vector3 (Input1.transform.position.x-0.1f, Input1.transform.position.y-0.02f, Input1.transform.position.z+10);
	}

}

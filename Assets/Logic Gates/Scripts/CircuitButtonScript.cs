using UnityEngine;
using System.Collections;

public class CircuitButtonScript : MonoBehaviour {

	void OnMouseDown() {
		Camera.main.GetComponent<LGManager_1>().ButtonClicked(transform.name);
	}

	void OnMouseUp() {

	}
}

using UnityEngine;
using System.Collections;

public class MessageBoxButton : MonoBehaviour {

	public bool RightButton = true;

	void OnMouseDown() {
		if (RightButton)
			transform.parent.GetComponent<MessageBox>().InvokeRightAction();
		else
			transform.parent.GetComponent<MessageBox>().InvokeLeftAction();
	}

}

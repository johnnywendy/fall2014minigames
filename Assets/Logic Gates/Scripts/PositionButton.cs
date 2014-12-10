using UnityEngine;
using System.Collections;

public class PositionButton : MonoBehaviour {

	public float amount = 40f;

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.localPosition;
		transform.localPosition = new Vector3(pos.x,(-Camera.main.pixelHeight/2+amount),pos.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

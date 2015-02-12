using UnityEngine;
using System.Collections;

public class ShrinkExpand : MonoBehaviour {

	public bool startExpanded = true;
	private bool shouldExpand = false;
	private bool shouldShrink = false;
	private bool expanded = true;
	
	private Vector3 startScaleVec;
	private float startScale;
	private float shrunkScale;
	private Vector3 selectedScale;
	
	void Start() {
		startScaleVec = transform.localScale;
		startScale = startScaleVec.x/60f;
		shrunkScale = startScaleVec.x/98f;
		if (!startExpanded) {
			expanded = false;
			transform.localScale = Vector3.zero;
		}
	}

	void FixedUpdate () {
		if (shouldExpand) {
			transform.localScale = Vector3.Slerp(transform.localScale,startScaleVec,Time.deltaTime*6);
			if (Vector3.Distance(transform.localScale,startScaleVec) < startScale) {
				transform.localScale = startScaleVec;
				shouldExpand = false;
			}
		}
		else if (shouldShrink) {
			transform.localScale = Vector3.Slerp(transform.localScale,new Vector3(0,0,1),Time.deltaTime*8);
			if (Vector3.Distance(transform.localScale,new Vector3(0,0,1)) < shrunkScale) {
				shouldShrink = false;
			}
		}
	}

	public void Flip() {
		if (expanded)
			Shrink();
		else
			Expand();
	}

	public void Expand() {
		transform.localScale = Vector3.zero;
		expanded = true;
		shouldExpand = true;
	}
	
	public void Shrink() {
		transform.localScale = startScaleVec;
		expanded = false;
		shouldShrink = true;
	}
}

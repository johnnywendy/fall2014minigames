using UnityEngine;
using System.Collections;

public class CheckOrX : MonoBehaviour {

	private SpriteRenderer sRenderer;
	private bool _val = false;
	public bool val {
		set {
			if (value == true) {
				sRenderer.sprite = Resources.Load<Sprite>("check");
			}
			else {
				sRenderer.sprite = Resources.Load<Sprite>("times");
			}
		}
		get {
			return _val;
		}
	}

	void Awake() {
		sRenderer = gameObject.GetComponent<SpriteRenderer>();
	}

}

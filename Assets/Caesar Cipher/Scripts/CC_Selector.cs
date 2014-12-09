using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CC_Selector : MonoBehaviour {

	private CC_2_Manager manager;
	private Button upButton;
	private Button downButton;

	public Text character;
	public string val {
		get {
			return character.text;
		}
		set {
			character.text = value;
		}
	}
	public bool enabled {
		set {
			upButton.enabled = value;
			downButton.enabled = value;
			character.enabled = value;
		}
	}

	private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	// Use this for initialization
	void Start () {
		character.text = val;
		downButton = transform.FindChild ("Down").GetComponent<Button> ();
		upButton = transform.FindChild ("Up").GetComponent<Button> ();
		manager = GameObject.Find ("UIManager").GetComponent<CC_2_Manager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DownClicked() {
		if ((int)val.ToCharArray()[0]-65 != 25)
			val = alphabet[(int)val.ToCharArray()[0]-65+1].ToString();
		manager.ValueChanged();
	}

	public void UpClicked() {
		if ((int)val.ToCharArray()[0]-65 != 0)
			val = alphabet [(int)val.ToCharArray()[0]-65-1].ToString();
		manager.ValueChanged();
	}
}

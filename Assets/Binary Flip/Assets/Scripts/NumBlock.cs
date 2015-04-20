using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumBlock : MonoBehaviour
{
	private bool buttonOn = false;
	private TextMesh txtMesh;
	private SpriteRenderer spr;
	private Color onColor, offColor, solvedColor;
	private bool solved = false;
	private static Shader shaderGUItext = Shader.Find ("GUI/Text Shader");
	void Start ()
	{
		spr = gameObject.GetComponent<SpriteRenderer> ();
		spr.material.shader = shaderGUItext;
		txtMesh = gameObject.GetComponentInChildren<TextMesh> ();
		onColor = HexColor.HexToColor (GameColors.selected);
		offColor = HexColor.HexToColor (GameColors.inactive);
		solvedColor = HexColor.HexToColor (GameColors.on2);
		spr.color = offColor;
	}

	// Update is called once per frame
	void Update ()
	{
		if (buttonOn)
			txtMesh.text = "1";
		else
			txtMesh.text = "0";
	}

	void FixedUpdate ()
	{
	}

	public void opacity (float timeleft)
	{
		if (txtMesh != null && spr != null) {
			Color tempc = txtMesh.color;
			txtMesh.color = new Vector4 (tempc.r, tempc.g, tempc.b, ((5f - timeleft) / 5f));
			tempc = spr.color;
			spr.color = new Vector4 (tempc.r, tempc.g, tempc.b, ((5f - timeleft) / 5f));
		}
	}

	private void changeValue ()
	{
		buttonOn = !buttonOn;
		if (buttonOn) {
			spr.color = onColor;
		} else {
			spr.color = offColor;
		}
	}

	public int getValue ()
	{
		if (buttonOn)
			return 1;
		else
			return 0;
	}

	public bool Solved {
		get {
			return solved;
		}
		set {
			solved = value;
			if (value)
				spr.color = solvedColor;
		}
	}

	void OnMouseDown ()
	{
		changeValue ();
	}

}

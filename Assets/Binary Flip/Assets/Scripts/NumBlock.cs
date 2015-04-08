using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumBlock : MonoBehaviour
{
	bool value = false;
	TextMesh txtMesh;
	Image img;
	void Start ()
	{
		img = gameObject.GetComponent<Image> ();
		txtMesh = gameObject.GetComponentInChildren<TextMesh> ();
		//txtMesh = gameObject.GetComponent<TextMesh> ();
	}
	

	// Update is called once per frame
	void Update ()
	{
		if (value)
			txtMesh.text = "1";
		else
			txtMesh.text = "0";
	}

	void FixedUpdate ()
	{
	}

	public void opacity (float timeleft)
	{
		if (txtMesh != null && img != null) {
			Color tempc = txtMesh.color;
			txtMesh.color = new Vector4 (tempc.r, tempc.g, tempc.b, ((5f - timeleft) / 5f));
			tempc = img.color;
			img.color = new Vector4 (tempc.r, tempc.g, tempc.b, ((5f - timeleft) / 5f) * (150f / 255f));
		}
	}

	public void changeValue ()
	{
		value = !value;
	}

	public int getValue ()
	{
		if (value)
			return 1;
		else
			return 0;
	}

	void OnMouseDown ()
	{
		changeValue ();
	}

}

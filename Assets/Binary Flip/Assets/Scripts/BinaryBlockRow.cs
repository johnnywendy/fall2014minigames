using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BinaryBlockRow : MonoBehaviour
{
	NumBlock[] blocks = new NumBlock[8];
	private TextMesh currentNumTextMesh;
	private TextMesh goalTextMesh;
	public bool difficult;
	Transform rowtransform;
	GameObject bbr;
	bool solved = false;
	bool goalNumSetByParent = false;
	private int currentNum = 0;
	private int goalnum = 1;
	private bool currentNumBeingSet = false;

	//public GameObject currentNumGO;
	//public GameObject goalNumGO;
	
	void Start ()
	{	
		bbr = Instantiate (Resources.Load ("BinaryBlockRow", typeof(GameObject))) as GameObject;
		rowtransform = bbr.GetComponent<Transform> ();
		//rowtransform.localScale = new Vector3 (1, 1, 1);
		//rowtransform.localPosition = new Vector3 (rowtransform.localPosition.x - 100, rowtransform.localPosition.y, rowtransform.localPosition.z);
				
		//c = gameObject.GetComponentInParent<Canvas> ();
		  
		currentNumTextMesh = bbr.GetComponentsInChildren<TextMesh> () [8];
		if (difficult) {
			//txt = Instantiate (Resources.Load ("txt", typeof(GameObject))) as GameObject;
			//txt.GetComponent<RectTransform> ().parent = c.transform;
			//txtrt = txt.GetComponent<RectTransform> ();
			//string temp = "rowtransform x : " + rowtransform.localPosition.x.ToString () + "rowtransform width : " + rowtransform.rect.width.ToString () + "txtrw width : " + txtrt.rect.width.ToString ();
			//Debug.Log (temp);
			//txtrt.localPosition = new Vector3 (rowtransform.localPosition.x + rowtransform.rect.width / 2 + txtrt.rect.width / 2 + 5, rowtransform.localPosition.y - 5, 0);
			//txtrt.localScale = new Vector3 (1, 1, 1);
			Destroy (currentNumTextMesh);
		}
		blocks = bbr.GetComponentsInChildren<NumBlock> ();


		//goaltxt = Instantiate (Resources.Load ("txt", typeof(GameObject))) as GameObject;
		//goaltxt.GetComponent<RectTransform> ().parent = c.transform;
		//goaltxtrt = goaltxt.GetComponent<Transform> ();
		
		//goaltxtrt.localPosition = new Vector3 (txtrt.localPosition.x + txtrt.rect.width + 5, txtrt.localPosition.y, 0);
		//goaltxtrt.localScale = new Vector3 (1, 1, 1);
		if (!goalNumSetByParent)
			goalnum = Random.Range (1, 255);
		goalTextMesh = bbr.GetComponentsInChildren<TextMesh> () [9];
		goalTextMesh.text = goalnum.ToString ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentNumBeingSet = true;
		currentNum = 0;
		for (int a =0; a<8; a++) {
			if (blocks [a].getValue () == 1) {
				currentNum += (int)System.Math.Pow (2, a);
			}
		}
		currentNumBeingSet = false;
		if (!difficult) {
			currentNumTextMesh.text = currentNum.ToString ();
		}

			
	}
	public void rowSolved (float t)
	{
		for (int a =0; a<8; ++a) {
			if (blocks [a] != null)
				blocks [a].GetComponent<SpriteRenderer> ().color = new Color (0f, 255f, 45f);
		}
		Invoke ("delete", t);
	}
	private void delete ()
	{

		Destroy (bbr);
		Destroy (this);
	}

	public void updatePos (Vector3 newpos)
	{
		if (rowtransform != null) {
			rowtransform.position = newpos;
		}
		/*	if (rowtransform != null)
			rowtransform.localPosition = newpos;
		if (txtrt != null)		
			txtrt.localPosition = new Vector3 (rowtransform.localPosition.x + rowtransform.rect.width / 2 + txtrt.rect.width / 2 + 5, rowtransform.localPosition.y - 5, 0);
		if (goaltxtrt != null)		
			goaltxtrt.localPosition = new Vector3 (txtrt.localPosition.x + txtrt.rect.width + 5, txtrt.localPosition.y, 0);
			*/
	}

	public void opacity (float timeleft)
	{
		for (int a =0; a<8; ++a) {
			if (blocks [a] != null)
				blocks [a].opacity (timeleft);
		}
		if (currentNumTextMesh != null && goalTextMesh != null) {
			Color tempcolor = new Vector4 (currentNumTextMesh.color.r, currentNumTextMesh.color.g, currentNumTextMesh.color.b, ((5f - timeleft) / 5f));
			currentNumTextMesh.color = tempcolor;
			goalTextMesh.color = tempcolor;
		}
	}

	public void updateDifficulty (bool d)
	{
		difficult = d;

	}

	public int Goalnum {
		get {
			return goalnum;
		}

		set {
			goalnum = value;
			goalNumSetByParent = true;
		}
	}

	public int CurrentVal {
		get {
			if (currentNumBeingSet) {
				return 257;
			} else {
				return currentNum;
			}
		}
	}

	public Transform Rowtransform {
		get {
			return rowtransform;
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BinaryBlockRow : MonoBehaviour
{
		NumBlock[] blockarr = new NumBlock[8];
		Canvas c;
		GameObject goaltxt;
		GameObject txt;
		RectTransform txtrt;
		private Text rowtotal;
		public bool difficult;
		RectTransform rowtransform;
		RectTransform goaltxtrt;
		GameObject bbr;

		
		int currentVal = 0;
		int goalnum;
		void Start ()
		{	
				bbr = Instantiate (Resources.Load ("BinaryBlockRow", typeof(GameObject))) as GameObject;
				bbr.transform.parent = GameObject.Find ("Canvas").transform;	
				rowtransform = bbr.GetComponent<RectTransform> ();
				rowtransform.localScale = new Vector3 (1, 1, 1);
				rowtransform.localPosition = new Vector3 (rowtransform.localPosition.x - 100, rowtransform.localPosition.y, rowtransform.localPosition.z);

				
				c = gameObject.GetComponentInParent<Canvas> ();
				if (!difficult) {
						txt = Instantiate (Resources.Load ("txt", typeof(GameObject))) as GameObject;
						txt.GetComponent<RectTransform> ().parent = c.transform;
						txtrt = txt.GetComponent<RectTransform> ();
						//string temp = "rowtransform x : " + rowtransform.localPosition.x.ToString () + "rowtransform width : " + rowtransform.rect.width.ToString () + "txtrw width : " + txtrt.rect.width.ToString ();
						//Debug.Log (temp);
						txtrt.localPosition = new Vector3 (rowtransform.localPosition.x + rowtransform.rect.width / 2 + txtrt.rect.width / 2 + 5, rowtransform.localPosition.y - 5, 0);
						txtrt.localScale = new Vector3 (1, 1, 1);
			
						rowtotal = txt.GetComponent<Text> ();
				}

				blockarr = bbr.GetComponentsInChildren<NumBlock> ();
		
		
				goaltxt = Instantiate (Resources.Load ("txt", typeof(GameObject))) as GameObject;
				goaltxt.GetComponent<RectTransform> ().parent = c.transform;
		
				goaltxtrt = goaltxt.GetComponent<RectTransform> ();
		
				goaltxtrt.localPosition = new Vector3 (txtrt.localPosition.x + txtrt.rect.width + 5, txtrt.localPosition.y, 0);
				goaltxtrt.localScale = new Vector3 (1, 1, 1);

				goalnum = Random.Range (1, 255);


		}
	

		// Update is called once per frame
		void Update ()
		{
				Text goaltxttemp = goaltxt.GetComponent<Text> ();
				goaltxttemp.text = goalnum.ToString ();
			
				currentVal = 0;
				for (int a =0; a<8; ++a) {
						if (blockarr [a].getValue () == 1) {
								currentVal += (int)System.Math.Pow (2, a);
						}
				}

				if (!difficult) {
						rowtotal.enabled = true;
						rowtotal.fontStyle = FontStyle.Normal;
						rowtotal.text = currentVal.ToString ();
				} else {
						rowtotal.enabled = false;
				}
			
		}
		public void rowSolved (float t)
		{
				for (int a =0; a<8; ++a) {
						blockarr [a].GetComponent<Image> ().color = new Color (0f, 255f, 45f);
				}
				Invoke ("delete", t);
		}
		private void delete ()
		{
				Destroy (txt);
				Destroy (goaltxt);
				Destroy (bbr);
				Destroy (this);
		}

		public void updatePos (Vector3 newpos)
		{
				if (rowtransform != null)
						rowtransform.localPosition = newpos;
				if (txtrt != null)		
						txtrt.localPosition = new Vector3 (rowtransform.localPosition.x + rowtransform.rect.width / 2 + txtrt.rect.width / 2 + 5, rowtransform.localPosition.y - 5, 0);
				if (goaltxtrt != null)		
						goaltxtrt.localPosition = new Vector3 (txtrt.localPosition.x + txtrt.rect.width + 5, txtrt.localPosition.y, 0);
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
				}
		}

		public int CurrentVal {
				get {
						return currentVal;
				}
		}

		

		public RectTransform Rowtransform {
				get {
						return rowtransform;
				}
		}

		public RectTransform Goaltxtrt {
				get {
						return goaltxtrt;
				}
		}

		public RectTransform Txtrt {
				get {
						return txtrt;
				}
		}
}

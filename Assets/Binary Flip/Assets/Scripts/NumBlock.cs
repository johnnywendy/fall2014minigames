using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumBlock : MonoBehaviour
{
		bool value = false;
		Text txt;
		Image img;
		void Start ()
		{
				img = gameObject.GetComponent<Image> ();
				txt = gameObject.GetComponentInChildren<Text> ();
		}
	

		// Update is called once per frame
		void Update ()
		{
				if (value)
						txt.text = "1";
				else
						txt.text = "0";
		}

		void FixedUpdate ()
		{
		}

		public void opacity (float timeleft)
		{
				if (txt != null && img != null) {
						Color tempc = txt.color;
						txt.color = new Vector4 (tempc.r, tempc.g, tempc.b, ((5f - timeleft) / 5f));
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

}

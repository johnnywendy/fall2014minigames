using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NumBlock : MonoBehaviour
{
		bool value = false;
		Text txt;
		void Start ()
		{
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

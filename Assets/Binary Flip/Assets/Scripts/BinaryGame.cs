using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BinaryGame : MonoBehaviour
{
		public bool hard = false;
		public bool medium = false;
		BinaryBlockRow[] blockrows = new BinaryBlockRow[5];

		bool init = false;
		private bool gameoverbool = false;
		public GameObject score;
		public GameObject time;
		Text scoretxt;
		Text timetxt;
		float scorenum = 0;
		float timenum = 100;
		
		
		
		void Start ()
		{
				BinaryBlockRow temp;
				for (int a =0; a<blockrows.Length; ++a) {
						temp = gameObject.AddComponent<BinaryBlockRow> ();
						blockrows.SetValue (temp, a);
				}

				scoretxt = score.GetComponent<Text> ();
				timetxt = time.GetComponent<Text> ();

		}
		void FixedUpdate ()
		{
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (!gameoverbool) {
						timenum -= Time.deltaTime;
				

						if (!init) {
								init = true;
								for (int a =0; a< blockrows.Length; ++a) {
										if (blockrows [a] != null) {
												Vector3 tempv = new Vector3 (-100, -220 + a * 100, 0);
												blockrows [a].updatePos (tempv);
										}
								}
						}
			
						scoretxt.text = scorenum.ToString ("0000");
						timetxt.text = timenum.ToString ("00");


						for (int a =0; a<blockrows.Length; ++a) {
								if (blockrows [a] != null) {
										Vector3 temp = new Vector3 (-100, -220 + a * 100, 0);
										//blockrows [a].updatePos (temp);

										blockrows [a].Rowtransform.localPosition = Vector3.Lerp (blockrows [a].Rowtransform.localPosition, temp, Time.deltaTime);
										blockrows [a].Txtrt.localPosition = Vector3.Lerp (blockrows [a].Txtrt.localPosition, new Vector3 (temp.x + blockrows [a].Rowtransform.rect.width / 2 + blockrows [a].Txtrt.rect.width / 2 + 5, temp.y - 5, 0), Time.deltaTime);
										blockrows [a].Goaltxtrt.localPosition = Vector3.Lerp (blockrows [a].Goaltxtrt.localPosition, new Vector3 (blockrows [a].Txtrt.localPosition.x + blockrows [a].Txtrt.rect.width + 5, temp.y, 0), Time.deltaTime);
										blockrows [a].updateDifficulty (medium);
								}
						}
				
						for (int a =0; a<blockrows.Length; ++a) {
								//Debug.Log (a.ToString () + " val: " + blockrows [a].CurrentVal.ToString ());
								if (blockrows [a] != null) {
										if (blockrows [a].CurrentVal == blockrows [a].Goalnum) {
												rowSolved (a);
										}
								}
						}

						if (hard) {
								if (blockrows.Length <= 0) {
										gameOver ();					
								}
						} else if (timenum <= 0 || blockrows.Length <= 0) {
								gameOver ();
						}
				} else {

				}
		}

		private void rowSolved (int a)
		{
				float time = 1f;
				if (blockrows [a] != null)
						blockrows [a].rowSolved (time);
				blockrows [a] = null;
				Invoke ("shiftRows", time);
				scorenum += timenum;

		}

		private void gameOver(){
			gameoverbool = true;
			
		GameObject alert = (GameObject)Instantiate(Resources.Load("Alert", typeof(GameObject)),Vector3.zero,Quaternion.identity);
		AlertBox alertBox = alert.GetComponent<AlertBox>();
		alertBox.title = "Game Over";
		alertBox.message = " ";
		alertBox.leftButtonText = "Main Menu";
		alertBox.rightButtonText = "Keep Playing";
		alertBox.SetLeftAction("loadscene","MainMenu");
		alertBox.SetRightAction("loadscene","binarylevel");
		}

		private void shiftRows ()
		{
				bool flag = true;
				int num = 0;
				for (int a =0; a<blockrows.Length && flag; ++a) {
						if (blockrows [a] == null) {
								flag = false;
								num = a;
						}
				}

				for (int i =num; i<blockrows.Length-1; ++i) {
						blockrows [i] = blockrows [i + 1];
				}
				blockrows [blockrows.Length - 1] = null;

		}

		
}

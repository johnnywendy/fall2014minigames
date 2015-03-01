using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BinaryGame : MonoBehaviour
{
	public bool hard = false;
	public bool medium = false;
	//BinaryBlockRow[] blockrows = new BinaryBlockRow[6];
	System.Collections.Generic.List<BinaryBlockRow> blockrows = new List<BinaryBlockRow> ();

	bool init = false;
	bool nextRowFlag = true;
	private bool gameoverbool = false;
	public GameObject score;
	public GameObject time;
	Text scoretxt;
	Text timetxt;
	float scorenum = 0;
	float timeTillNextRow = 20;
	float totalTimePassed = 0;
	int lastnumofRows = 6;
	int rowxpos = -145;
	int rowypos = -300;
	BinaryBlockRow temp;
	void Start ()
	{
		for (int a =0; a<6; ++a) {
			temp = gameObject.AddComponent<BinaryBlockRow> ();
			blockrows.Add (temp);
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
			incrementTime ();
			if (timeTillNextRow <= 0 && nextRowFlag) {
				nextRowFlag = false;
				addNextRow ();
			
			} else {
				nextRowFlag = true;
			}

			if (!init) {
				initializeRowPlacement ();
			}

			if (timeTillNextRow <= 4) {
				blockrows [blockrows.Count - 1].opacity (timeTillNextRow);
			} else {
				blockrows [blockrows.Count - 1].opacity (5);
			}

			updateNumbers ();
			animateRows ();	
			if (lastnumofRows != blockrows.Count) {
				lastnumofRows = blockrows.Count;
			}
			checkForSolvedRows ();
						
		} else {

		}
	}
	private void checkForSolvedRows ()
	{
		for (int a =0; a<blockrows.Count; ++a) {
			//Debug.Log (a.ToString () + " val: " + blockrows [a].CurrentVal.ToString ());
			if (blockrows [a] != null) {
				if (blockrows [a].CurrentVal == blockrows [a].Goalnum) {
					rowSolved (a);
				}
			}
		}
	}

	private void animateRows ()
	{
		for (int a = 0; a < blockrows.Count; ++a) {
			if (blockrows [a].Rowtransform != null) {
				Vector3 temp = new Vector3 (rowxpos, rowypos + a * 100, 0);
				blockrows [a].Rowtransform.localPosition = Vector3.Lerp (blockrows [a].Rowtransform.localPosition, temp, Time.deltaTime);
				blockrows [a].Txtrt.localPosition = Vector3.Lerp (blockrows [a].Txtrt.localPosition, new Vector3 (temp.x + blockrows [a].Rowtransform.rect.width / 2 + blockrows [a].Txtrt.rect.width / 2 + 5, temp.y - 5, 0), Time.deltaTime);
				blockrows [a].Goaltxtrt.localPosition = Vector3.Lerp (blockrows [a].Goaltxtrt.localPosition, new Vector3 (blockrows [a].Txtrt.localPosition.x + blockrows [a].Txtrt.rect.width + 5, temp.y, 0), Time.deltaTime);

			}
		}
	}

	void updateNumbers ()
	{
		scoretxt.text = scorenum.ToString ("0000");
		timetxt.text = timeTillNextRow.ToString ("00");
	}

	private void incrementTime ()
	{
		timeTillNextRow -= Time.deltaTime;
		totalTimePassed += Time.deltaTime;
	}
	private void addNextRow ()
	{
		if (blockrows.Count >= 6) {
			gameOver ();
		} else {
			timeTillNextRow = 20;
			temp = gameObject.AddComponent<BinaryBlockRow> ();
			temp.opacity (5);
			blockrows.Add (temp);
		}
	}
	private void initializeRowPlacement ()
	{
		init = true;
		for (int a =0; a< blockrows.Count; ++a) {
			Vector3 tempv = new Vector3 (rowxpos, rowypos + a * 100, 0);
			if (blockrows [a] != null) {
				blockrows [a].updatePos (tempv);
				blockrows [a].updateDifficulty (medium);
			}
		}

	}

	private void rowSolved (int a)
	{
		float time = .1f;

		if (blockrows [a] != null)
			blockrows [a].rowSolved (time);
		scorenum += (100 - totalTimePassed);
		blockrows.RemoveAt (a);
	
	}

	private void gameOver ()
	{
		gameoverbool = true;
		init = false;
		GameObject alert = (GameObject)Instantiate (Resources.Load ("Alert", typeof(GameObject)), Vector3.zero, Quaternion.identity);
		AlertBox alertBox = alert.GetComponent<AlertBox> ();
		alertBox.title = "Game Over";
		alertBox.message = " ";
		alertBox.leftButtonText = "Main Menu";
		alertBox.rightButtonText = "Keep Playing";
		alertBox.SetLeftAction ("loadscene", "MainMenu");
		alertBox.SetRightAction ("loadscene", "binarylevel");
	}
			
}

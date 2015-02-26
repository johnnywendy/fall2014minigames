using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BinaryGame : MonoBehaviour
{
	public bool hard = false;
	public bool medium = false;
	BinaryBlockRow[] blockrows = new BinaryBlockRow[6];
	int nextRowPos = 5;

	bool init = false;
	private bool gameoverbool = false;
	public GameObject score;
	public GameObject time;
	Text scoretxt;
	Text timetxt;
	float scorenum = 0;
	float timeTillNextRow = 20;
	float totalTimePassed = 0;

	int rowxpos = -145;
	int rowypos = -300;
		
	void Start ()
	{
		BinaryBlockRow temp;
		for (int a =0; a<6; ++a) {
			temp = gameObject.AddComponent<BinaryBlockRow> ();
			if (a != 5) {			
				blockrows.SetValue (temp, a);
			} else {
				blockrows.SetValue (temp, a);
			}
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
			if (timeTillNextRow <= 0) {

				addNextRow ();
			
			}

			if (!init) {
				initializeRowPlacement ();
			}

			if (timeTillNextRow <= 4) {
				Debug.Log (nextRowPos);
				blockrows [nextRowPos].opacity (timeTillNextRow);
			} else {
				Debug.Log (nextRowPos);
				Debug.Log (blockrows [nextRowPos]);
				blockrows [nextRowPos].opacity (5);
			}

			updateNumbers ();
			animateRows ();	
			checkForSolvedRows ();
						
		} else {

		}
	}
	private void checkForSolvedRows ()
	{
		for (int a =0; a<blockrows.Length; ++a) {
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
		int l = nextAvailableRowPos ();
		for (int a = 0; a < l; ++a) {
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

	private int nextAvailableRowPos ()
	{
		for (int a =0; a<blockrows.Length; ++a) {
			if (blockrows [a] == null)
				return a;
		}
		return -1;
	}
	private void incrementTime ()
	{
		timeTillNextRow -= Time.deltaTime;
		totalTimePassed += Time.deltaTime;
	}
	private void addNextRow ()
	{
		if (nextAvailableRowPos () == -1) {
			gameOver ();
		} else {
			timeTillNextRow = 20;
			BinaryBlockRow tempbbr;
			tempbbr = gameObject.AddComponent<BinaryBlockRow> ();
			nextRowPos = nextAvailableRowPos ();				
			blockrows.SetValue (tempbbr, nextRowPos);
			initializeRowPlacement ();
		}
	}
	private void initializeRowPlacement ()
	{
		init = true;
		for (int a =0; a< blockrows.Length; ++a) {
			Vector3 tempv = new Vector3 (rowxpos, rowypos + a * 100, 0);
			if (blockrows [a] != null) {
				blockrows [a].updatePos (tempv);
				blockrows [a].updateDifficulty (medium);
			}
		}

	}

	private void rowSolved (int a)
	{
		float time = 1f;
		if (blockrows [a] != null)
			blockrows [a].rowSolved (time);
		blockrows [a] = null;
		Invoke ("shiftRows", time);
		scorenum += (100 - totalTimePassed);


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

	private void shiftRows ()
	{
		nextRowPos--;
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

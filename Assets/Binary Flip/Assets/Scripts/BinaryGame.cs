using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class BinaryGame : MonoBehaviour
{
	public bool hard = false;
	public bool medium = false;
	//BinaryBlockRow[] blockrows = new BinaryBlockRow[6];
	System.Collections.Generic.List<BinaryBlockRow> blockrows = new List<BinaryBlockRow> ();
	List<int> rowsToRemove = new List<int> ();
	bool init = false;
	bool nextRowFlag = true;
	private bool gameoverbool = false;
	public GameObject score;
	public GameObject time;
	TextMesh scoreTextMesh;
	TextMesh timeTextMesh;
	float scorenum = 0;
	float timeTillNextRow = 20;
	float maxTimeTillNextRow = 5;
	float totalTimePassed = 0;
	int lastnumofRows = 6;
	int rowxpos = -5;
	int rowypos = -4;
	BinaryBlockRow temp;
	int level = -1;
	/*int[,] numberProblems = new int[,] {{1,2,4,8,16,32,64,128},
										{3,6,12,24,48,65,129,17,33,13,10,130,192,40},
										{131,7,28,67,42,35,224,112,14,161},
										{1}	};
	*/
	List<List<int> > numberProblems = new List<List<int> > {
		new List<int> {1, 2, 4, 8, 16, 32, 64, 128},
		new List<int> {3, 6, 12, 24, 48, 65, 129, 17, 33, 13, 10, 130, 192, 40},
		new List<int> {131, 7, 28, 67, 42, 35, 224, 112, 14, 161},
		new List<int> {1},
		new List<int> {1},
		new List<int> {1},
		new List<int> {1},
		new List<int> {1}
	};
	int[] rowTimesPerLevel = new int[] {9,10,10,10,10,10,10,10};
	int playerOnProblemNum = 0;
	int numberProblemsLength = 1;

	void Start ()
	{
		level = GameData.GetCurrentLevel () - 1;
		maxTimeTillNextRow = rowTimesPerLevel [level];
		timeTillNextRow = maxTimeTillNextRow;
		//numberProblemsLength = numberProblems.GetLength (level);
		numberProblemsLength = numberProblems [level].Count;
		for (int a =0; a<6; ++a) {
			temp = gameObject.AddComponent<BinaryBlockRow> ();
			temp.Goalnum = numberProblems [level] [playerOnProblemNum++];

			blockrows.Add (temp);
			//Debug.LogError (numberProblems [level, playerOnNum]);
		}

		scoreTextMesh = score.GetComponent<TextMesh> ();
		timeTextMesh = time.GetComponent<TextMesh> ();



	}
	void FixedUpdate ()
	{
	}

	// Update is called once per frame
	void Update ()
	{

		if (!gameoverbool) {
			incrementTime ();
			if (playerOnProblemNum + 1 < numberProblemsLength && timeTillNextRow <= 0f && nextRowFlag) {
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
				if (!rowsToRemove.Contains (a) && blockrows [a].CurrentVal == blockrows [a].Goalnum) {
					rowSolved (a);
				}
			}
		}
	}

	private void animateRows ()
	{
		for (int a = 0; a < blockrows.Count; ++a) {
			if (blockrows [a].Rowtransform != null) {
				Vector3 temp = new Vector3 (rowxpos, rowypos + a * 1.05f, 0);
				blockrows [a].Rowtransform.localPosition = Vector3.Lerp (blockrows [a].Rowtransform.localPosition, temp, Time.deltaTime);
				//blockrows [a].Txtrt.localPosition = Vector3.Lerp (blockrows [a].Txtrt.localPosition, new Vector3 (temp.x + blockrows [a].Rowtransform.rect.width / 2 + blockrows [a].Txtrt.rect.width / 2 + 5, temp.y - 5, 0), Time.deltaTime);
				//blockrows [a].Goaltxtrt.localPosition = Vector3.Lerp (blockrows [a].Goaltxtrt.localPosition, new Vector3 (blockrows [a].Txtrt.localPosition.x + blockrows [a].Txtrt.rect.width + 5, temp.y, 0), Time.deltaTime);

			}
		}
	}

	void updateNumbers ()
	{
		scoreTextMesh.text = scorenum.ToString ("0000");
		timeTextMesh.text = timeTillNextRow.ToString ("00");
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
			timeTillNextRow = maxTimeTillNextRow;
			temp = gameObject.AddComponent<BinaryBlockRow> ();
			temp.Goalnum = numberProblems [level] [playerOnProblemNum++];
			temp.opacity (5);
			blockrows.Add (temp);
		}
	}
	private void initializeRowPlacement ()
	{
		init = true;
		for (int a =0; a< blockrows.Count; ++a) {
			Vector3 tempv = new Vector3 (rowxpos, rowypos + a * 1.05f, 0);
			if (blockrows [a] != null) {
				blockrows [a].updatePos (tempv);
				blockrows [a].updateDifficulty (medium);
			}
		}

	}

	private void removeRowsSolved ()
	{
		for (int a =0; a<rowsToRemove.Count; ++a) {
			blockrows.RemoveAt (rowsToRemove [a]);

		}
		rowsToRemove.Clear ();
	}

	private void rowSolved (int a)
	{
		float time = .5f;

		if (blockrows [a] != null)
			blockrows [a].rowSolved (time);
		scorenum += (100 - totalTimePassed);
		rowsToRemove.Add (a);
		Invoke ("removeRowsSolved", time);
		if (playerOnProblemNum + 1 >= numberProblemsLength) {
			levelbeaten ();
		}
	
	}

	private void levelbeaten ()
	{
		gameoverbool = true;
		init = false;
		GameData.CompletedLevel (GameData.GetCurrentGame (), level + 1);
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

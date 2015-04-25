using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class BinaryGame : MonoBehaviour
{
	//BinaryBlockRow[] blockrows = new BinaryBlockRow[6];
	System.Collections.Generic.List<BinaryBlockRow> blockrows = new List<BinaryBlockRow> ();
	List<int> rowsToRemove = new List<int> ();
	bool init = false;
	bool nextRowFlag = true;
	private bool gameoverbool = false;
	public GameObject score;
	public GameObject time;
	public GameObject timeText;
	TextMesh scoreTextMesh;
	TextMesh timeTextMesh;
	float scorenum = 0;
	float timeTillNextRow = 20;
	float maxTimeTillNextRow = 5;
	float totalTimePassed = 0;
	int lastnumofRows = 6;
	int rowxpos = -5;
	int rowypos = -4;
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
		new List<int> {-1},
		new List<int> {-1},
		new List<int> {-1},
		new List<int> {-1},
		new List<int> {-1}
	};
	int[] rowTimesPerLevel = new int[] {9,10,10,10,10,10,10,10};
	int playerOnProblemNum = 0;
	int numberProblemsLength = 1;
	bool generateRandomProblemNums = false;
	bool timeLimitOnThisLevel = false;

	void Start ()
	{
		level = GameData.GetCurrentLevel () - 1;
		level = 3;
		if (numberProblems [level] [0] == -1) {
			generateRandomProblemNums = true;
		} else {
			generateRandomProblemNums = false;
		}
		if (level == 7) {
			timeLimitOnThisLevel = true;
		} else {
			timeLimitOnThisLevel = false;
		}
		if (timeLimitOnThisLevel) {
			maxTimeTillNextRow = 15;
		} else {
			maxTimeTillNextRow = 4;
			Destroy (time);
			Destroy (timeText);
		}
		timeTillNextRow = maxTimeTillNextRow;
		switch (level) {
		case 0:
			numberProblemsLength = numberProblems [level].Count;
			break;
		case 1:
			numberProblemsLength = numberProblems [level].Count;
			break;
		case 2:
			numberProblemsLength = numberProblems [level].Count;
			break;
		case 3:
			numberProblemsLength = 17;
			break;
		case 4:
			numberProblemsLength = 24;
			break;
		case 5:
			numberProblemsLength = 30;
			break;
		case 6:
			numberProblemsLength = 40;
			break;
		case 7:
			numberProblemsLength = 1000;
			break;
		default:
			numberProblemsLength = 5;
			break;
		}
		BinaryBlockRow temp;
		for (int a =0; a<5; ++a) {
			temp = gameObject.AddComponent<BinaryBlockRow> ();
			if (generateRandomProblemNums) {
				temp.Goalnum = randomProblemNumber ();
			} else {
				temp.Goalnum = numberProblems [level] [playerOnProblemNum++];
			}
			blockrows.Add (temp);
			//Debug.LogError (numberProblems [level, playerOnNum]);
		}

		scoreTextMesh = score.GetComponent<TextMesh> ();
		if (timeLimitOnThisLevel) {
			timeTextMesh = time.GetComponent<TextMesh> ();
		}



	}
	void FixedUpdate ()
	{
	}

	// Update is called once per frame
	void Update ()
	{

		if (!gameoverbool) {
			incrementTime ();
			if (nextRowFlag && playerOnProblemNum + 1 < numberProblemsLength && ((!timeLimitOnThisLevel && blockrows.Count < 5) || (timeLimitOnThisLevel && timeTillNextRow <= 0f))) {
				nextRowFlag = false;
				addNextRow ();
			}

			if (!init) {
				initializeRowPlacement ();
			}
			if (timeLimitOnThisLevel) {
				if (timeTillNextRow <= 4) {
					blockrows [blockrows.Count - 1].opacity (timeTillNextRow);
				} else {
					blockrows [blockrows.Count - 1].opacity (5);
				}
			} else {
				blockrows [blockrows.Count - 1].opacity (timeTillNextRow);
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
		if (timeLimitOnThisLevel) {
			timeTextMesh.text = timeTillNextRow.ToString ("00");
		}
	}

	private void incrementTime ()
	{
		if (timeTillNextRow > 0) {
			timeTillNextRow -= Time.deltaTime;
		} else {
			timeTillNextRow = 0;
		}
		totalTimePassed += Time.deltaTime;
	}
	private void addNextRow ()
	{
		BinaryBlockRow newRow;
		if (timeLimitOnThisLevel && blockrows.Count >= 6) {
			gameOver ();
		} else {
			timeTillNextRow = maxTimeTillNextRow;
			newRow = gameObject.AddComponent<BinaryBlockRow> ();
			if (generateRandomProblemNums) {
				newRow.Goalnum = randomProblemNumber ();
			} else {
				newRow.Goalnum = numberProblems [level] [playerOnProblemNum++];
			}
			Vector3 tempv = new Vector3 (rowxpos, rowypos + blockrows.Count + 1 * 1.05f, 0);
			newRow.updatePos (tempv);
			newRow.opacity (5);
			blockrows [blockrows.Count - 1].opacity (0);
			blockrows.Add (newRow);
			nextRowFlag = true;
		}
	}
	private int randomProblemNumber ()
	{
		int numOfBitsToUse, ret = 0;
		char[] numtext = "00000000".ToCharArray ();
		switch (level) {
		case 3:
			numOfBitsToUse = 3;
			break;
		case 4:
			numOfBitsToUse = 3;
			break;
		case 5:
			numOfBitsToUse = 4;
			break;
		case 6:
			numOfBitsToUse = 5;
			break;
		case 7:
			numOfBitsToUse = Random.Range (1, 8);
			break;
		default:
			numOfBitsToUse = 1;
			break;
		}
		int temp;
		for (int a =0; a<numOfBitsToUse; ++a) {
			while (numtext[(temp = Random.Range(0,8))] != '1') {
				numtext [temp] = '1';
			}

		}
		for (int a =7; a>=0; a--) {
			if (numtext [a] == '1') {
				ret += (int)System.Math.Pow (2, a);
			}
		}

		return ret;
	}
	private void initializeRowPlacement ()
	{
		init = true;
		for (int a =0; a< blockrows.Count; ++a) {
			Vector3 tempv = new Vector3 (rowxpos, rowypos + a * 1.05f, 0);
			if (blockrows [a] != null) {
				blockrows [a].updatePos (tempv);
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
		scorenum += Mathf.Max ((100 - totalTimePassed), 50);
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

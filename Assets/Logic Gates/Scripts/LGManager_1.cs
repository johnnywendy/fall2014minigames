using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LGManager_1 : MonoBehaviour {

	private bool isHoldingObj = false;
	private bool isHoldingPow = false;
	private bool isPositioningObj = false;
	private bool isConnectingObj = false;
	private bool isEditing = false;

	private GameObject plugObj;
	private GameObject socketObj;
	private GameObject heldObj;

	// Inventory Stuff
	public List<GameObject> buttons;
	public int[] InvAmounts = new int[] {1,1,1,1,1,1,1};
	public bool[] InvEnabled = new bool[] {true,true,true,true,true,true,true};

	// Level Management
	public int level = 1;
	private List<List<bool>> truthTable = null;
	private List<PowerSource> powerSources = new List<PowerSource> {};
	private GoalGate goalGate = null;

	List<List<bool>> level1;
	int[] inv1 = new int[] {0,0,0,2,0,0,0};
	string message1 = "Congratulations, you just\ncreated a NOT gate.";
	string hint1 = "Make a Not gate:\n1 NAND";
	List<List<bool>> level2;
	int[] inv2 = new int[] {0,0,2,2,0,0,0};
	string message2 = "Congratulations, you just\ncreated an AND gate.";
	string hint2 = "Make an AND gate:\n1 NAND, 1 NOT";
	List<List<bool>> level3;
	int[] inv3 = new int[] {2,0,2,3,0,0,0};
	string message3 = "Congratulations, you just\ncreated an OR gate.";
	string hint3 = "Make an OR gate:\n3 NAND";
	List<List<bool>> level4;
	int[] inv4 = new int[] {2,2,2,2,0,0,0};
	string message4 = "Congratulations, you just\ncreated an NOR gate.";
	string hint4 = "Make a NOR gate:\n2 NOT, 1 AND";
	List<List<bool>> level5;
	int[] inv5 = new int[] {2,2,2,2,2,0,0};
	string message5 = "Congratulations, you just\ncreated an XOR gate.";
	string hint5 = "Make a XOR gate:\n1 OR, 1 NAND, 1 AND";
	List<List<bool>> level6;
	int[] inv6 = new int[] {2,2,2,2,2,2,0};
	string message6 = "Congratulations, you just\ncreated an XNOR gate.";
	string hint6 = "Make an XNOR gate:\n1 XOR, 1 NOT";

	// Use this for initialization
	void Start () {
		goalGate = GameObject.Find("GoalBlock").GetComponent<GoalGate>() as GoalGate;

		for (int i = 0; i < InvAmounts.Length; i++) {
			buttons[i].SetActive(InvEnabled[i]);
			buttons[i].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[i].ToString();
		}

		List<bool> row1 = new List<bool>() {false,true};
		List<bool> row2 = new List<bool>() {true,false};
		level1 = new List<List<bool>>() {row1,row2};

		row1 = new List<bool>() {false,false,false};
		row2 = new List<bool>() {true,false,false};
		List<bool> row3 = new List<bool>() {true,true,true};
		List<bool> row4 = new List<bool>() {false,true,false};
		level2 = new List<List<bool>>() {row1,row2,row3,row4};

		row1 = new List<bool>() {false,false,false};
		row2 = new List<bool>() {true,false,true};
		row3 = new List<bool>() {true,true,true};
		row4 = new List<bool>() {false,true,true};
		level3 = new List<List<bool>>() {row1,row2,row3,row4};

		row1 = new List<bool>() {false,false,true};
		row2 = new List<bool>() {true,false,false};
		row3 = new List<bool>() {true,true,false};
		row4 = new List<bool>() {false,true,false};
		level4 = new List<List<bool>>() {row1,row2,row3,row4};

		row1 = new List<bool>() {false,false,false};
		row2 = new List<bool>() {true,false,true};
		row3 = new List<bool>() {true,true,false};
		row4 = new List<bool>() {false,true,true};
		level5 = new List<List<bool>>() {row1,row2,row3,row4};

		row1 = new List<bool>() {false,false,true};
		row2 = new List<bool>() {true,false,false};
		row3 = new List<bool>() {true,true,true};
		row4 = new List<bool>() {false,true,false};
		level6 = new List<List<bool>>() {row1,row2,row3,row4};

		level = GameData.GetCurrentLevel();
		SetupNewLevel(level);
		HexColor.SetColor(GameObject.Find ("CheckAnswer"),GameColors.selected);
		HexColor.SetColor(GameObject.Find ("Hint"),GameColors.selected);
	}

	// Update is called once per frame
	void Update () {
		if (isHoldingObj && heldObj != null) {
			heldObj.transform.position = VisibleMousePosition();
		}

		// On Left Click
		if (Input.GetMouseButtonDown (0)) {
			if (isHoldingObj || isEditing) {
				if (isEditing) {
					RaycastHit hit = new RaycastHit();
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					
					if (Physics.Raycast(ray, out hit)) {
						if (hit.collider.transform.gameObject.name == "Reset") {
							hit.collider.transform.parent.gameObject.GetComponent<LogicGate>().resetConnections();
						}
						if (hit.collider.transform.gameObject.name == "Destroy") {
							hit.collider.transform.parent.gameObject.GetComponent<LogicGate>().resetConnections();
							int index = hit.collider.transform.parent.gameObject.GetComponent<LogicGate>().logicMode;
							InvAmounts[index]++;
							buttons[index].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[index].ToString();
							Destroy(hit.collider.transform.parent.gameObject);
						}
					}
				}
			}
			else {
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if (Physics.Raycast(ray, out hit)) {
					if (hit.collider.transform.gameObject.name == "CheckAnswer") {
						CheckAnswer();
					}
					if (hit.collider.transform.gameObject.name == "Hint") {
						DisplayHint();
					}
					if (!isConnectingObj) {
						if (hit.collider.transform.gameObject.name == "Output") {
							plugObj = hit.collider.transform.parent.gameObject;
							if (hit.collider.tag == "IO") {
								plugObj.GetComponent<LogicGate>().NeedNewCable();
							}
							else if (hit.collider.tag == "Power") {
								isHoldingPow = true;
								plugObj.GetComponent<PowerSource>().NeedNewCable();
							}
							isConnectingObj = true;
						}
						else if (hit.collider.tag == "Power") {
							hit.collider.gameObject.GetComponent<PowerSource>().FlipOutput();
						}
						if (hit.collider.transform.gameObject.tag == "Gate") {
							HoldObject(hit.collider.transform.gameObject);
						}
					}
				}
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			if (isHoldingObj) {
				isHoldingObj = false;
				LogicGate gate = heldObj.GetComponent<LogicGate> ();
				if (gate != null) {
					gate.enabled = true;
					gate.WasPutDown();
					heldObj = null;
				}
			}
			else {
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if (Physics.Raycast(ray, out hit)) {
					if (isConnectingObj && (hit.collider.transform.gameObject.name == "Input1" || hit.collider.transform.gameObject.name == "Input2")) {
						if (plugObj.tag == "Power") {
							socketObj = hit.collider.transform.gameObject;
							isConnectingObj = false;
							isHoldingPow = false;
							PowerSource pow = plugObj.GetComponent<PowerSource>();
							
							if (hit.collider.transform.gameObject.name == "Input1") {
								if (socketObj.transform.parent.GetComponent<LogicGate>() != null) {
									socketObj.transform.parent.GetComponent<LogicGate>().HookUpPowerSource(pow,"left");
									pow.PlugIntoGate(socketObj.transform.parent.GetComponent<LogicGate>(), "left");
								}
								else {
									plugObj.GetComponent<PowerSource>().DestroyNewCable();
									isConnectingObj = false;
									isHoldingPow = false;
								}
							}
							if (hit.collider.transform.gameObject.name == "Input2") {
								socketObj.transform.parent.GetComponent<LogicGate>().HookUpPowerSource(pow,"right");
								pow.PlugIntoGate(socketObj.transform.parent.GetComponent<LogicGate>(), "right");
							}
						}
						else if (plugObj.tag == "Gate" && hit.collider.tag != "Goal") {
							socketObj = hit.collider.transform.gameObject;
							isConnectingObj = false;
							if (hit.collider.transform.gameObject.name == "Input1")
								plugObj.GetComponent<LogicGate>().PlugIntoGate(socketObj.transform.parent.GetComponent<LogicGate>(),"left");
							if (hit.collider.transform.gameObject.name == "Input2")
								plugObj.GetComponent<LogicGate>().PlugIntoGate(socketObj.transform.parent.GetComponent<LogicGate>(),"right");
						}
						else if (hit.collider.tag == "Goal") {
							socketObj = hit.collider.transform.gameObject;
							isConnectingObj = false;
							plugObj.GetComponent<LogicGate>().PlugIntoGoal(socketObj.transform.parent.GetComponent<GoalGate>());
						}
					}
				}
				else if (isConnectingObj) {
					if (isHoldingPow)
						plugObj.GetComponent<PowerSource>().DestroyNewCable();
					else
						plugObj.GetComponent<LogicGate>().DestroyNewCable();
					isConnectingObj = false;
					isHoldingPow = false;
				}
			}
			GameObject.Find ("TruthTable").GetComponent<TruthTable>().UpdateTable(GetCurrentTable());
		}
		// On Right Click
		if (Input.GetMouseButtonDown (1)) {
			if (!isHoldingObj) {
				if (isEditing)
					isEditing = false;
				else
					isEditing = true;
				foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Gate")) {
					obj.GetComponent<LogicGate>().SetEditMode(isEditing);
				}
			}
		}
	}

	public Vector3 VisibleMousePosition() {
		Vector3 mousPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		return new Vector3 (mousPos.x, mousPos.y, -0.11636f);
	}

	public bool CanHoldObj() {
		if (isHoldingObj || isConnectingObj || isHoldingPow || isPositioningObj)
			return false;
		else
			return true;
	}

	public void HoldObject(GameObject obj) {
		isHoldingObj = true;
		LogicGate gate = obj.GetComponent<LogicGate> ();
		if (gate != null) {
			gate.WasPickedUp();
			gate.shouldDisable(true);
			heldObj = obj;
		}
	}

	public void ButtonClicked(string newObject) {
		isEditing = false;
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Gate")) {
			obj.GetComponent<LogicGate>().SetEditMode(isEditing);
		}
		if (CanHoldObj()) {
			GameObject newGate = (GameObject)Instantiate(Resources.Load("LogicGate", typeof(GameObject)),Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity);
			if (newObject == "AndButton" && InvAmounts[0] > 0) {
				InvAmounts[0] -= 1;
				newGate.GetComponent<LogicGate>().mode = 0;
				buttons[0].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[0].ToString();
			}
			else if (newObject == "OrButton" && InvAmounts[1] > 0) {
				InvAmounts[1] -= 1;
				newGate.GetComponent<LogicGate>().mode = 1;
				buttons[1].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[1].ToString();
			}
			else if (newObject == "NotButton" && InvAmounts[2] > 0) {
				InvAmounts[2] -= 1;
				newGate.GetComponent<LogicGate>().mode = 2;
				buttons[2].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[2].ToString();
			}
			else if (newObject == "NandButton" && InvAmounts[3] > 0) {
				InvAmounts[3] -= 1;
				newGate.GetComponent<LogicGate>().mode = 3;
				buttons[3].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[3].ToString();
			}
			else if (newObject == "NorButton" && InvAmounts[4] > 0) {
				InvAmounts[4] -= 1;
				newGate.GetComponent<LogicGate>().mode = 4;
				buttons[4].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[4].ToString();
			}
			else if (newObject == "XorButton" && InvAmounts[5] > 0) {
				InvAmounts[5] -= 1;
				newGate.GetComponent<LogicGate>().mode = 5;
				buttons[5].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[5].ToString();
			}
			else if (newObject == "XnorButton" && InvAmounts[6] > 0) {
				InvAmounts[6] -= 1;
				newGate.GetComponent<LogicGate>().mode = 6;
				buttons[6].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[6].ToString();
			}
			else {
				Destroy(newGate);
				return;
			}
			newGate.GetComponent<LogicGate>().preferredEditMode = isEditing;
			HoldObject(newGate);
		}
	}

	void ClearFloor() {
		foreach (GameObject gate in GameObject.FindGameObjectsWithTag("Gate")) {
			LogicGate lGate = gate.GetComponent<LogicGate>();
			lGate.resetConnections();
			int index = lGate.logicMode;
			InvAmounts[index]++;
			buttons[index].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[index].ToString();
			Destroy(gate);
		}
		foreach (PowerSource pow in powerSources) {
			foreach (Cable cable in pow.cables) {
				GameObject.Destroy(cable.gameObject);
			}
			GameObject.Destroy(pow.gameObject);
		}
		powerSources = new List<PowerSource>();
	}

	public void SetupNextLevel(int newLevel) {
		level = newLevel;
		SetupNewLevel(level);
	}

	public void SetupNewLevel(int level) {
		ClearFloor ();
		if (level == 1) {
			truthTable = level1;
			InvAmounts = inv1;
		}
		if (level == 2) {
			truthTable = level2;
			InvAmounts = inv2;
		}
		if (level == 3) {
			truthTable = level3;
			InvAmounts = inv3;
		}
		if (level == 4) {
			truthTable = level4;
			InvAmounts = inv4;
		}
		if (level == 5) {
			truthTable = level5;
			InvAmounts = inv5;
		}
		if (level == 6) {
			truthTable = level6;
			InvAmounts = inv6;
		}
		for (int i = 0; i < InvAmounts.Length; i++) {
			buttons[i].SetActive(true);
			if (InvAmounts[i] > 0)
				buttons[i].transform.FindChild("ButtonIcon").GetComponent<LogicGate>().enabledInButtonMode = true;
			else
				buttons[i].transform.FindChild("ButtonIcon").GetComponent<LogicGate>().enabledInButtonMode = false;
			buttons[i].transform.FindChild("Amount").GetComponent<TextMesh>().text = InvAmounts[i].ToString();
		}
		int numberOfSources = truthTable[0].Count-1;
		if (numberOfSources == 1) {
			GameObject powerSource1 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,1.7f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource1.GetComponent<PowerSource>());
		}
		if (numberOfSources == 2) {
			GameObject powerSource1 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,2.85f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource1.GetComponent<PowerSource>());
			GameObject powerSource2 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,0.55f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource2.GetComponent<PowerSource>());
		}
		if (numberOfSources == 3) {
			GameObject powerSource1 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,4f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource1.GetComponent<PowerSource>());
			GameObject powerSource2 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,1.7f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource2.GetComponent<PowerSource>());
			GameObject powerSource3 = (GameObject)Instantiate(Resources.Load("PowerSource", typeof(GameObject)),new Vector3(-4.8f,-0.6f,-0.5f),Quaternion.identity);
			powerSources.Add(powerSource3.GetComponent<PowerSource>());
		}
		GameObject.Find ("TruthTable").GetComponent<TruthTable>().SetTable(truthTable);
	}

	public List<List<bool>> GetCurrentTable() {
		List<List<bool>> currentTable = new List<List<bool>>();
		List<bool> originalOutputs = new List<bool>();
		foreach (PowerSource input in powerSources) {
			originalOutputs.Add(input.output);
			input.SetOutput(false);
		}
		if (powerSources.Count == 1) {
			powerSources[0].output = false;
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
		}
		if (powerSources.Count == 2) {
			powerSources[0].output = false;
			powerSources[1].output = false;
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[1].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
		}
		if (powerSources.Count == 3) {
			powerSources[0].output = false;
			powerSources[1].output = false;
			powerSources[2].output = false;
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[1].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[2].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[1].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			currentTable.Add(GetRow());
			powerSources[0].FlipOutput();
			powerSources[1].FlipOutput();
			powerSources[2].FlipOutput();
			currentTable.Add(GetRow());
		}
		for (int i = 0; i < powerSources.Count; i++)
			powerSources[i].SetOutput(originalOutputs[i]);
		return currentTable;
	}

	public void CheckAnswer() {
		CheckCompletion(GetCurrentTable());
	}

	List<bool> GetRow() {
		List<bool> currentScenario = new List<bool>();
		foreach (PowerSource input in powerSources)
			currentScenario.Add (input.output);
		currentScenario.Add(goalGate.input);
		return currentScenario;
	}

	public void UpdateTruthTable() {
		GameObject.Find ("TruthTable").GetComponent<TruthTable>().UpdateTable(GetCurrentTable());
	}

	void CheckCompletion(List<List<bool>> currentAnswer) {
		bool correct = true;
		for (int i = 0; i < currentAnswer.Count; i++)
			for (int j = 0; j < currentAnswer[i].Count; j++)
				if (truthTable[i][j] != currentAnswer[i][j])
					correct = false;
		if (correct) {
			Debug.Log("CORRECT");
			GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			MessageBox alertBox = alert.GetComponent<MessageBox>();
			if (level == 1)
				alertBox.message = message1;
			if (level == 2)
				alertBox.message = message2;
			if (level == 3)
				alertBox.message = message3;
			if (level == 4)
				alertBox.message = message4;
			if (level == 5)
				alertBox.message = message5;
			if (level == 6)
				alertBox.message = message6;
			alertBox.SetLeftAction("destroy");
			if (level == 6) {
				alertBox.rightButtonText = "MENU";
				alertBox.SetRightAction("loadscene","Main");
			}
			else {
				alertBox.rightButtonText = "NEXT";
				alertBox.SetRightAction(gameObject,"LGManager_1","SetupNextLevel",(level+1));
			}
		}
		else {
			Debug.Log("INCORRECT");
			GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
			MessageBox alertBox = alert.GetComponent<MessageBox>();
			alertBox.message = "Hmm, this doesn't look\ncorrect, try again.";
			alertBox.SetLeftAction("destroy");
			alertBox.SetRightAction("destroy");
		}
	}

	void DisplayHint() {
		GameObject alert = (GameObject)Instantiate(Resources.Load("MsgSmall", typeof(GameObject)),Vector3.zero,Quaternion.identity);
		MessageBox alertBox = alert.GetComponent<MessageBox>();
		if (level == 1)
			alertBox.message = hint1;
		if (level == 2)
			alertBox.message = hint2;
		if (level == 3)
			alertBox.message = hint3;
		if (level == 4)
			alertBox.message = hint4;
		if (level == 5)
			alertBox.message = hint5;
		if (level == 6)
			alertBox.message = hint6;
		alertBox.SetLeftAction("destroy");
		alertBox.SetRightAction("destroy");
	}
}
